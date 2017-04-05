//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// This file contains Weapon and Ammo Class/"namespace" helper methods as well
// as hooks into the inventory system. These functions are not attached to a
// specific C++ class or datablock, but define a set of methods which are part
// of dynamic namespaces "class". The Items include these namespaces into their
// scope using the  ItemData and ItemImageData "className" variable.
// ----------------------------------------------------------------------------

// All ShapeBase images are mounted into one of 8 slots on a shape.  This weapon
// system assumes all primary weapons are mounted into this specified slot:
$WeaponSlot = 0;

// ----------------------------------------------------------------------------
// Weapon Order
// ----------------------------------------------------------------------------

// This is a simple means of handling easy adding & removal of weapons to the
// cycleWeapon function and still maintain a constant cycle order.
function WeaponOrder(%weapon, %slot)
{
   if ($lastWeaponOrderSlot $= "")
      $lastWeaponOrderSlot = -1;

   // the order# slot to name index
   $weaponOrderIndex[%slot] = %weapon;

   // the weaponName to order# slot index
   $weaponNameIndex[%weapon] = %slot;

   // the last slot in the array
   $lastWeaponOrderSlot++;
}

// Now create the Index/array by passing a name and order# for each weapon.
// NOTE:  the first weapon needs to be 0.
WeaponOrder(RocketLauncher, 0);

//-----------------------------------------------------------------------------
// Weapon Class
//-----------------------------------------------------------------------------

function Weapon::onUse(%data, %obj)
{
   // Default behavior for all weapons is to mount it into the object's weapon
   // slot, which is currently assumed to be slot 0
   if (%obj.getMountedImage($WeaponSlot) != %data.image.getId())
   {
      serverPlay3D(WeaponUseSound, %obj.getTransform());

      %obj.mountImage(%data.image, $WeaponSlot);
      if (%obj.client)
      {
         if (%data.description !$= "")
            messageClient(%obj.client, 'MsgWeaponUsed', '\c0%1 selected.', %data.description);
         else
            messageClient(%obj.client, 'MsgWeaponUsed', '\c0Weapon selected');
      }
   }
}

function Weapon::onPickup(%this, %obj, %shape, %amount)
{
   // The parent Item method performs the actual pickup.
   // For player's we automatically use the weapon if the
   // player does not already have one in hand.
   if (Parent::onPickup(%this, %obj, %shape, %amount))
   {
      serverPlay3D(WeaponPickupSound, %shape.getTransform());
      if (%shape.getClassName() $= "Player" && %shape.getMountedImage($WeaponSlot) == 0)
         %shape.use(%this);
   }
}

function Weapon::onInventory(%this, %obj, %amount)
{
   // Weapon inventory has changed, make sure there are no weapons
   // of this type mounted if there are none left in inventory.
   if (!%amount && (%slot = %obj.getMountSlot(%this.image)) != -1)
      %obj.unmountImage(%slot);
}

//-----------------------------------------------------------------------------
// Weapon Image Class
//-----------------------------------------------------------------------------

function WeaponImage::onMount(%this, %obj, %slot)
{
   // Images assume a false ammo state on load.  We need to
   // set the state according to the current inventory.
   if(%this.ammo !$= "")
   {
      if (%obj.getInventory(%this.ammo))
      {
         %obj.setImageAmmo(%slot, true);
         %currentAmmo = %obj.getInventory(%this.ammo);
      }
      else
         %currentAmmo = 0;
	  if( isObject( %obj.client ) )
	  {
         %obj.client.RefreshWeaponHud(%currentAmmo, %this.item.previewImage, %this.item.reticle);
	  }
   }
}

function WeaponImage::onUnmount(%this, %obj, %slot)
{
   if( isObject( %obj.client ) )
   {
   	   %obj.client.RefreshWeaponHud(0, "", "");
   }
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onFire handler for most weapons.  Can be overridden
// with an appropriate namespace method for any weapon that requires a custom
// firing solution.

// projectileSpread is a dynamic property declared in the weaponImage datablock
// for those weapons in which bullet skew is desired.  Must be greater than 0,
// otherwise the projectile goes straight ahead as normal.  lower values give
// greater accuracy, higher values increase the spread pattern.
// ----------------------------------------------------------------------------

function WeaponImage::onFire(%this, %obj, %slot)
{
	%dataBlock = %this.projectile;
	
	if (!isObject(%obj)
		|| !isObject(%dataBlock) 
		|| %dataBlock $= "" 
		|| %dataBlock $= "0" 
		|| !isDefined(%dataBlock))
	{
		$testIsFireStart = false;
		return;
	}
	
	%clientObj = %obj.getClientObject();
	if (%clientObj $= "" || !isObject(%clientObj))
	{	
		$testIsFireStart = false;
		return;
	}
	
   //echo("\c4WeaponImage::onFire( "@%this.getName()@", "@%obj.client.nameBase@", "@%slot@" )");

   // Decrement inventory ammo. The image's ammo state is updated
   // automatically by the ammo inventory hooks.
	if (!isObject(%this.projectile) 
		|| %this.projectile.getClassName() !$= "ProjectileData")
	{	
		$testIsFireStart = false;
		return;
	}

	if ($ActiveQianMingGui || $ActiveChooseGui)
	{
		$testIsFireStart = false;
		return;
	}

   //if ( !%this.infiniteAmmo )
      //%obj.decInventory(%this.ammo, 1);
   if (%this.projectileSpread)
   {
      // We'll need to "skew" this projectile a little bit.  We start by
      // getting the straight ahead aiming point of the gun
	if( %obj == $PlayerP1 )
	{
      %vec = %obj.getAttack1Vector();
	}
	else
	{
		//%vec = %obj.getMuzzleVector();
		%damagePos1 = $PlayerP1.getPosition();
		%damagePos1.z += 5;
		//%damagePos1.x += 0.6;
		
		%vec = VectorSub( %damagePos1,%obj.getAttackPoint1() );
	}

      // Then we'll create a spread matrix by randomly generating x, y, and z
      // points in a circle
      for(%i = 0; %i < 3; %i++)
         %matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.projectileSpread @ " ";
      %mat = MatrixCreateFromEuler(%matrix);

      // Which we'll use to alter the projectile's initial vector with
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      // Weapon projectile doesn't have a spread factor so we fire it using
      // the straight ahead aiming point of the gun
	  if( %obj == $PlayerP1 )
	  {
      	%muzzleVector1 = %obj.getAttack1Vector();
		$PlayerUseAmmoCount++;
	  }
	  else
	  {
		//%muzzleVector = %obj.getMuzzleVector();
		%damagePos1 = $PlayerP1.getPosition();
		%damagePos1.z += 5;
		//%damagePos1.x += 0.6;
		%damagePos2 = %damagePos1;
		
		if( %obj.aiPlayerDb $= "jiqiangTa" || %obj.aiPlayerDb $= "jiQiangTou" )
		{
			%randomZ1 = getRandom(0,20)/2 - 5;
			%randomX1 = getRandom(0,10)-5;
			%damagePos1.z += %randomZ1;
			%damagePos1.x += %randomX1;
			
			%randomZ2 = getRandom(0,20)/2 - 5;
			%randomX2 = getRandom(0,10)-5;
			%damagePos2.z += %randomZ2;
			%damagePos2.x += %randomX2;
		}
		%muzzleVector1 = VectorSub( %damagePos1,%clientObj.getAttackPoint1() );
		%muzzleVector2 = VectorSub( %damagePos2,%clientObj.getAttackPoint2() );
	  }
   }
   
   	// Get the player's velocity, we'll then add it to that of the projectile
   	%objectVelocity = %obj.getVelocity();
	
   	%muzzleVelocity1 = VectorAdd(
      VectorScale(%muzzleVector1, %this.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.projectile.velInheritFactor));
	
	%muzzleVelocity2 = VectorAdd(
      VectorScale(%muzzleVector2, %this.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.projectile.velInheritFactor));
	
	if( %obj.aiPlayerDb $= "jiqiangTa" || %obj.aiPlayerDb $= "fangkongdaodanDongHua" ||
		%obj.aiPlayerDb $= "GuangNengRen" || %obj.aiPlayerDb $= "shuangqiang" || %obj.aiPlayerDb $= "tietou" )
	{
		%p1 = new (%this.projectileType )()
   		{
      		dataBlock = %dataBlock;
      		initialVelocity = %muzzleVelocity2;
      		initialPosition = %clientObj.getAttackPoint2();
      		sourceObject = %obj;
      		sourceSlot = %slot;
      		client = %obj.client;
   		};
		
		if (isObject(%p1))
		{
			MissionCleanup.add(%p1);
		}
		
   		%p1.sourceObj = %obj;   //sourceObject在projectile类中在一段时间之后会置为�?
	}
	
   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %dataBlock;
      initialVelocity = %muzzleVelocity1;
      initialPosition = %clientObj.getAttackPoint1();
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	
	//echo( "dataBlock is:"@%dataBlock@"   id is:  "@%p );
	%p.sourceObj = %obj;   //sourceObject在projectile类中在一段时间之后会置为�?
	
	if( %obj == $PlayerP1 )
	{
		if ($testFireCount > 10)
		{
			$testFireCount = 0;
		}
		
		$testIsFireStart = true;
		%this.schedule( 100, onFireLoop, %obj, %slot,  $testFireCount++);
	}
	
	return %p;
}

function WeaponImage::onFireLoop(%this, %obj, %slot, %num)
{
	%dataBlock = %this.projectile;
	%clientObj = %obj.getClientObject();
	
	if (%num != $testFireCount || $ActiveQianMingGui || $ActiveChooseGui)
	{
		$testIsFireStart = false;
		return;
	}
	
	if( $pathCameraFly || $WaitContinue || $gameEndNow )
	{
		$testIsFireStart = false;
		return;
	}
	
	if(!$PlayerIsOnFire || ($BossErGuanDead && $ZongBossDead))
	{
		$testIsFireStart = false;
		return;
	}
	
	if( $PlayerP1.isHidden() )
	{
		$testIsFireStart = false;
		return;
	}
	
	if (%this.projectileSpread)
	{
		%vec = %obj.getAttack1Vector();
		
		for(%i = 0; %i < 3; %i++)
		%matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.projectileSpread @ " ";
		%mat = MatrixCreateFromEuler(%matrix);
		
		// Which we'll use to alter the projectile's initial vector with
		%muzzleVector = MatrixMulVector(%mat, %vec);
	}
	else
	{
		%muzzleVector1 = %obj.getAttack1Vector();
		$PlayerUseAmmoCount++;
	}
   
   	// Get the player's velocity, we'll then add it to that of the projectile
   	%objectVelocity = %obj.getVelocity();
	
   	%muzzleVelocity1 = VectorAdd(
      VectorScale(%muzzleVector1, %this.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.projectile.velInheritFactor));
		
   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %dataBlock;
      initialVelocity = %muzzleVelocity1;
      initialPosition = %clientObj.getAttackPoint1();
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	
	%p.sourceObj = %obj;
	
	%this.schedule( 130, onFireLoop, %obj, %slot, %num );
}
function onFireP2()
{
	%obj = $PlayerP1;
	%thisP2 = "RocketLauncherImage";
	%dataBlock = "RocketLauncherProjectile";
	%clientObj = %obj.getClientObject();
	%slot = 0;
	
	if (!isObject(%obj)
		|| !isObject(%dataBlock) 
		|| %dataBlock $= "" 
		|| %dataBlock $= "0" 
		|| !isDefined(%dataBlock))
	{
		$testIsFireStartP2 = false;
		return;
	}

	if ($ActiveQianMingGui || $ActiveChooseGui)
	{
		$testIsFireStartP2 = false;
		return;
	}
   
  	%muzzleVector1 = %obj.getAttack1VectorP2();
	$PlayerUseAmmoCountP2++;
   	%objectVelocity = %obj.getVelocity();
	
   	%muzzleVelocity1 = VectorAdd(
      VectorScale(%muzzleVector1, %thisP2.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %thisP2.projectile.velInheritFactor));
	
	%muzzleVelocity2 = VectorAdd(
      VectorScale(%muzzleVector2, %thisP2.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %thisP2.projectile.velInheritFactor));
	
	%p = new (%thisP2.projectileType)()
	{
		dataBlock = %dataBlock;
		initialVelocity = %muzzleVelocity1;
		initialPosition = %clientObj.getAttackPoint1();
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	
	%p.sourceObj = %obj;
	%p.sourceObjP2 = "p2Obj";
	
	if ($testFireCountP2 > 10)
	{
		$testFireCountP2 = 0;
	}
	
	$testIsFireStartP2 = true;
	schedule( 100, 0, onFireLoopP2, $PlayerP1, 0,  $testFireCountP2++);
	
	return %p;
}

function onFireLoopP2(%obj, %slot, %num)
{
	%dataBlock = "RocketLauncherProjectile";
	%clientObj = %obj.getClientObject();
	%thisP2 = "RocketLauncherImage";
	
	if (%num != $testFireCountP2 || $ActiveQianMingGui || $ActiveChooseGui)
	{
		$testIsFireStartP2 = false;
		return;
	}
	
	if( $pathCameraFly || $WaitContinue || $gameEndNow )
	{
		$testIsFireStartP2 = false;
		return;
	}
	
	if(!$PlayerIsOnFireP2 || ($BossErGuanDead && $ZongBossDead))
	{
		$testIsFireStartP2 = false;
		return;
	}
	
	if( $PlayerP1.isHidden() )
	{echo("hidennnnnnnnnnn         return");
		$testIsFireStartP2 = false;
		return;
	}
	
	%muzzleVector1 = %obj.getAttack1VectorP2();
	$PlayerUseAmmoCountP2++;
   
   	// Get the player's velocity, we'll then add it to that of the projectile
   	%objectVelocity = %obj.getVelocity();
	
   	%muzzleVelocity1 = VectorAdd(
      VectorScale(%muzzleVector1, %thisP2.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %thisP2.projectile.velInheritFactor));
	
	%muzzleVelocity2 = VectorAdd(
      VectorScale(%muzzleVector2, %thisP2.projectile.muzzleVelocity),
      VectorScale(%objectVelocity, %thisP2.projectile.velInheritFactor));
		
   // Create the projectile object
	%p = new (%thisP2.projectileType)()
	{
		dataBlock = %dataBlock;
		initialVelocity = %muzzleVelocity1;
		initialPosition = %clientObj.getAttackPoint1();
		sourceObject = %obj;
		sourceSlot = %slot;
		client = %obj.client;
	};
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	
	%p.sourceObj = %obj;
	%p.sourceObjP2 = "p2Obj";
	
	schedule( 130, 0, onFireLoopP2, %obj, %slot, %num );
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onAltFire handler for most weapons.  Can be
// overridden with an appropriate namespace method for any weapon that requires
// a custom firing solution.
// ----------------------------------------------------------------------------

function WeaponImage::onAltFire(%this, %obj, %slot)
{
   //echo("\c4WeaponImage::onAltFire("@%this.getName()@", "@%obj.client.nameBase@", "@%slot@")");

   // Decrement inventory ammo. The image's ammo state is updated
   // automatically by the ammo inventory hooks.
   %obj.decInventory(%this.ammo, 1);

   if (%this.altProjectileSpread)
   {
      // We'll need to "skew" this projectile a little bit.  We start by
      // getting the straight ahead aiming point of the gun
      %vec = %obj.getMuzzleVector(%slot);

      // Then we'll create a spread matrix by randomly generating x, y, and z
      // points in a circle
      for(%i = 0; %i < 3; %i++)
         %matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.altProjectileSpread @ " ";
      %mat = MatrixCreateFromEuler(%matrix);

      // Which we'll use to alter the projectile's initial vector with
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      // Weapon projectile doesn't have a spread factor so we fire it using
      // the straight ahead aiming point of the gun.
      %muzzleVector = %obj.getMuzzleVector(%slot);
   }

   // Get the player's velocity, we'll then add it to that of the projectile
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
      VectorScale(%muzzleVector, %this.altProjectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.altProjectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %this.altProjectile;
      initialVelocity = %muzzleVelocity;
      initialPosition = %obj.getMuzzlePoint(%slot);
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}

// ----------------------------------------------------------------------------
// A "generic" weaponimage onWetFire handler for most weapons.  Can be
// overridden with an appropriate namespace method for any weapon that requires
// a custom firing solution.
// ----------------------------------------------------------------------------

function WeaponImage::onWetFire(%this, %obj, %slot)
{
   //echo("\c4WeaponImage::onWetFire("@%this.getName()@", "@%obj.client.nameBase@", "@%slot@")");

   // Decrement inventory ammo. The image's ammo state is updated
   // automatically by the ammo inventory hooks.
   %obj.decInventory(%this.ammo, 1);

   if (%this.wetProjectileSpread)
   {
      // We'll need to "skew" this projectile a little bit.  We start by
      // getting the straight ahead aiming point of the gun
      %vec = %obj.getMuzzleVector(%slot);

      // Then we'll create a spread matrix by randomly generating x, y, and z
      // points in a circle
      for(%i = 0; %i < 3; %i++)
      %matrix = %matrix @ (getRandom() - 0.5) * 2 * 3.1415926 * %this.wetProjectileSpread @ " ";
      %mat = MatrixCreateFromEuler(%matrix);

      // Which we'll use to alter the projectile's initial vector with
      %muzzleVector = MatrixMulVector(%mat, %vec);
   }
   else
   {
      // Weapon projectile doesn't have a spread factor so we fire it using
      // the straight ahead aiming point of the gun.
      %muzzleVector = %obj.getMuzzleVector(%slot);
   }

   // Get the player's velocity, we'll then add it to that of the projectile
   %objectVelocity = %obj.getVelocity();
   %muzzleVelocity = VectorAdd(
      VectorScale(%muzzleVector, %this.wetProjectile.muzzleVelocity),
      VectorScale(%objectVelocity, %this.wetProjectile.velInheritFactor));

   // Create the projectile object
   %p = new (%this.projectileType)()
   {
      dataBlock = %this.wetProjectile;
      initialVelocity = %muzzleVelocity;
      initialPosition = %obj.getMuzzlePoint(%slot);
      sourceObject = %obj;
      sourceSlot = %slot;
      client = %obj.client;
   };
   MissionCleanup.add(%p);
   return %p;
}

//-----------------------------------------------------------------------------
// Ammmo Class
//-----------------------------------------------------------------------------

function Ammo::onPickup(%this, %obj, %shape, %amount)
{
   // The parent Item method performs the actual pickup.
   if (Parent::onPickup(%this, %obj, %shape, %amount))
      serverPlay3D(AmmoPickupSound, %shape.getTransform());
}

function Ammo::onInventory(%this, %obj, %amount)
{
   // The ammo inventory state has changed, we need to update any
   // mounted images using this ammo to reflect the new state.
   for (%i = 0; %i < 8; %i++)
   {
      if ((%image = %obj.getMountedImage(%i)) > 0)
         if (isObject(%image.ammo) && %image.ammo.getId() == %this.getId())
         {
            %obj.setImageAmmo(%i, %amount != 0);
            %currentAmmo = %obj.getInventory(%this);
            %obj.client.setAmmoAmountHud(%currentAmmo);
         }
   }
}

// ----------------------------------------------------------------------------
// Weapon cycling
// ----------------------------------------------------------------------------

// Could make this player namespace only or even a vehicle namespace method,
// but for the time being....

function ShapeBase::cycleWeapon(%this, %direction)
{
   %slot = -1;
   if (%this.getMountedImage($WeaponSlot) != 0)
   {
      %curWeapon = %this.getMountedImage($WeaponSlot).item.getName();
      %slot = $weaponNameIndex[%curWeapon];
   }

   if (%direction $= "prev")
   {
      // Previous weapon...
      if (%slot == 0 || %slot == -1)
      {
         %requestedSlot = $lastWeaponOrderSlot;
         %slot = 0;
      }
      else
         %requestedSlot = %slot - 1;
   }
   else
   {
      // Next weapon...
      if (%slot == $lastWeaponOrderSlot || %slot == -1)
      {
         %requestedSlot = 0;
         %slot = $lastWeaponOrderSlot;
      }
      else
         %requestedSlot = %slot + 1;
   }

   %newSlot = -1;
   while (%requestedSlot != %slot)
   {
      if ($weaponOrderIndex[%requestedSlot] !$= "" && %this.hasInventory($weaponOrderIndex[%requestedSlot]) && %this.hasAmmo($weaponOrderIndex[%requestedSlot]))
      {
         // player has this weapon and it has ammo or doesn't need ammo
         %newSlot = %requestedSlot;
         break;
      }
      if (%direction $= "prev")
      {
         if (%requestedSlot == 0)
            %requestedSlot = $lastWeaponOrderSlot;
         else
            %requestedSlot--;
      }
      else
      {
         if (%requestedSlot == $lastWeaponOrderSlot)
            %requestedSlot = 0;
         else
            %requestedSlot++;
      }
   }
   if (%newSlot != -1)
      %this.use($weaponOrderIndex[%newSlot]);
}
