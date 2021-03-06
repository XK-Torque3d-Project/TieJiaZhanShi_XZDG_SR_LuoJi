//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Timeouts for corpse deletion.
$CorpseTimeoutValue = 45 * 1000;

// // Damage Rate for entering Liquid
// $DamageLava = 0.01;
// $DamageHotLava = 0.01;
// $DamageCrustyLava = 0.01;

// Death Animations
$PlayerDeathAnim::TorsoFrontFallForward = 1;
$PlayerDeathAnim::TorsoFrontFallBack = 2;
$PlayerDeathAnim::TorsoBackFallForward = 3;
$PlayerDeathAnim::TorsoLeftSpinDeath = 4;
$PlayerDeathAnim::TorsoRightSpinDeath = 5;
$PlayerDeathAnim::LegsLeftGimp = 6;
$PlayerDeathAnim::LegsRightGimp = 7;
$PlayerDeathAnim::TorsoBackFallForward = 8;
$PlayerDeathAnim::HeadFrontDirect = 9;
$PlayerDeathAnim::HeadBackFallForward = 10;
$PlayerDeathAnim::ExplosionBlowBack = 11;

$StartGame = false;
$PlayerScore = 0;
$PlayerKillBossNum = 0;
$PlayerKillBossScore = 0;
$PlayerAllScore = 0;

$PlayerKillNpcNum = 0;
$PlayerKillNpcScore = 0;

$PlayerUseAmmoCount = 0;

//策划设定值
$MingZhongJiangLi = 1000;
$DaoDanJiangLi = 1000;
$ShengMingZhiJiangLi = 1000;
$PlayerDaoDanNum = 20;
$PlayerDaoDanNumP2 = 20;

$PlayerTimeNumber = 0;
$GameOver = false;
$WaitContinue = false;

$JiJiaPercent50IsShow = false;
$JiJiaPercent70IsShow = false;
$JiJiaPercent90IsShow = false;
$JiJiaPercentShowOver = false;

$IsDaoDanTiShiShow = false;
$JiFenKuangMaxNum = 0;

$BossYiGuanDead = false;
$BossErGuanDead = false;
$ZongBossDead = false;

//----------------------------------------------------------------------------
// Armor Datablock methods
//----------------------------------------------------------------------------

function Armor::onAdd(%this, %obj)
{
   // Vehicle timeout
   %obj.mountVehicle = true;

   // Default dynamic armor stats
   %obj.setRechargeRate(%this.rechargeRate);
   %obj.setRepairRate(0);

   // Set the numerical Health HUD
   //%obj.updateHealth();

   // Calling updateHealth() must be delayed now... for some reason
   %obj.schedule(50, "updateHealth");
}

function Armor::onRemove(%this, %obj)
{
   if (%obj.client.player == %obj)
      %obj.client.player = 0;
}

function Armor::onNewDataBlock(%this, %obj)
{
}

//----------------------------------------------------------------------------

function Armor::onMount(%this, %obj, %vehicle, %node)
{
   // Node 0 is the pilot's position, we need to dismount his weapon.
   if (%node == 0)
   {
      %obj.setTransform("0 0 0 0 0 1 0");
      %obj.setActionThread(%vehicle.getDatablock().mountPose[%node], true, true);

      %obj.lastWeapon = %obj.getMountedImage($WeaponSlot);
      %obj.unmountImage($WeaponSlot);

      %obj.setControlObject(%vehicle);
      //%obj.client.setObjectActiveImage(%vehicle, 2);
   }
   else
   {
      if (%vehicle.getDataBlock().mountPose[%node] !$= "")
         %obj.setActionThread(%vehicle.getDatablock().mountPose[%node]);
      else
         %obj.setActionThread("root", true);
   }
}

function Armor::onUnmount(%this, %obj, %vehicle, %node)
{
   %obj.setActionThread("run", true, true);

   if (%node == 0)
   {
      %obj.mountImage(%obj.lastWeapon, $WeaponSlot);
      %obj.setControlObject("");
   }
}

function Armor::doDismount(%this, %obj, %forced)
{
   //echo("\c4Armor::doDismount(" @ %this @", "@ %obj.client.nameBase @", "@ %forced @")");

   // This function is called by player.cc when the jump trigger
   // is true while mounted
   %vehicle = %obj.mVehicle;
   if (!%obj.isMounted() || !isObject(%vehicle))
      return;

   // Vehicle must be at rest!
   if ((VectorLen(%vehicle.getVelocity()) <= %vehicle.getDataBlock().maxDismountSpeed ) || %forced)
   {
      // Position above dismount point
      %pos = getWords(%obj.getTransform(), 0, 2);
      %rot = getWords(%obj.getTransform(), 3, 6);
      %oldPos = %pos;
      %vec[0] = " -1 0 0";
      %vec[1] = " 0 0 1";
      %vec[2] = " 0 0 -1";
      %vec[3] = " 1 0 0";
      %vec[4] = "0 -1 0";
      %impulseVec = "0 0 0";
      %vec[0] = MatrixMulVector(%obj.getTransform(), %vec[0]);

      // Make sure the point is valid
      %pos = "0 0 0";
      %numAttempts = 5;
      %success = -1;
      for (%i = 0; %i < %numAttempts; %i++)
      {
         %pos = VectorAdd(%oldPos, VectorScale(%vec[%i], 3));
         if (%obj.checkDismountPoint(%oldPos, %pos))
         {
            %success = %i;
            %impulseVec = %vec[%i];
            break;
         }
      }
      if (%forced && %success == -1)
         %pos = %oldPos;

      %obj.mountVehicle = false;
      %obj.schedule(4000, "mountVehicles", true);

      // Position above dismount point
      %obj.unmount();
      %obj.setTransform(%pos SPC %rot);//%obj.setTransform(%pos);
      //%obj.playAudio(0, UnmountVehicleSound);
      %obj.applyImpulse(%pos, VectorScale(%impulseVec, %obj.getDataBlock().mass));

      // Set player velocity when ejecting
      %vel = %obj.getVelocity();
      %vec = vectorDot( %vel, vectorNormalize(%vel));
      if(%vec > 50)
      {
         %scale = 50 / %vec;
         %obj.setVelocity(VectorScale(%vel, %scale));
      }

      //%obj.vehicleTurret = "";
   }
   else
      messageClient(%obj.client, 'msgUnmount', '\c2Cannot exit %1 while moving.', %vehicle.getDataBlock().nameTag);
}

//----------------------------------------------------------------------------

function Armor::onCollision(%this, %obj, %col)
{
   return;
   if (!isObject(%col) || %obj.getState() $= "Dead")
      return;

   // Try and pickup all items
   if (%col.getClassName() $= "Item")
   {
      %obj.pickup(%col);
      return;
   }

   // Mount vehicles
   if (%col.getType() & $TypeMasks::GameBaseObjectType)
   {
      %db = %col.getDataBlock();
      if ((%db.getClassName() $= "WheeledVehicleData" ) && %obj.mountVehicle && %obj.getState() $= "Move" && %col.mountable)
      {
         // Only mount drivers for now.
         %node = 0;
         %col.mountObject(%obj, %node);
         %obj.mVehicle = %col;
      }
   }
}

function Armor::onImpact(%this, %obj, %collidedObject, %vec, %vecLen)
{
   %obj.damage(0, VectorAdd(%obj.getPosition(), %vec), %vecLen * %this.speedDamageScale, "Impact");
}

//----------------------------------------------------------------------------

function Armor::damage(%this, %obj, %sourceObject, %position, %damage, %damageType)
{
   if (!isObject(%obj) || %obj.getState() $= "Dead")
      return;
   if( ( %sourceObject == $PlayerP1 && %obj != $PlayerP1 ) || ( %sourceObject != $PlayerP1 && %obj == $PlayerP1 ) )
   {
		if( %obj.getDataBlock().getName() $= "BossDianJu" )
		{
//			%obj.DianJuGetDamage( %damage );
		}
		else
		{
			if( %obj == $PlayerP1 )
			{
				if( $PlayerP1.wudi )
					return;
				
				//$playerDamageLevel += (%damage * $damagePercent);
				//%curHealth = %obj.getMaxDamage() - %obj.getDamageLevel();
				%curHealth = %obj.getMaxDamage() - $playerDamageLevel;
				//commandToClient( %client,'drawPlayerHealth' );	
				judgeHurtPlayerOnly(true, (%damage * $damagePercent), %obj);
				
				return;
			}
			else
   			%obj.applyDamage(%damage);
		}
   }

   %location = "Body";

   // Update the numerical Health HUD
   %obj.updateHealth();

   // Deal with client callbacks here because we don't have this
   // information in the onDamage or onDisable methods
   %client = %obj.client;
   %sourceClient = %sourceObject ? %sourceObject.client : 0;

   if ( %obj.getState() $= "Dead" && isObject( %client ) )
   	{
      	%client.onDeath(%sourceObject, %sourceClient, %damageType, %location);
   	}
	return;
}

$FirstPlay = false;
$ThirdPlay = false;

function PlaySecondSound()
{
	if(!isObject($PlayerP1))
	return;
	
	if($FirstPlay == true)
	{
		sfxPlayOnce(panel_Sound_tiShi01);
		schedule(800, 0, PlaySecondSound );
			return;
	}
	else
		return;
}

function PlayThirdSound()
{
	if(!isObject($PlayerP1))
	return;
	
	if( $ThirdPlay == true )
	{
		sfxPlayOnce(panel_Sound_tiShi01);
		schedule(400, 0, PlayThirdSound);
		return;
	}
	else
		return;
}

function Armor::onDamage(%this, %obj, %delta)
{return;
   // This method is invoked by the ShapeBase code whenever the
   // object's damage level changes.
   if (%delta > 0 && %obj.getState() !$= "Dead")
   {
      // If the pain is excessive, let's hear about it.
      if (%delta > 10)
         %obj.playPain();
   }
}

// ----------------------------------------------------------------------------
// The player object sets the "disabled" state when damage exceeds it's
// maxDamage value. This is method is invoked by ShapeBase state mangement code.

// If we want to deal with the damage information that actually caused this
// death, then we would have to move this code into the script "damage" method.

function Armor::onDisabled(%this, %obj, %state)
{
   // Release the main weapon trigger
   %obj.setDamageState(Destroyed);
   %obj.setImageTrigger(0, false);

   %obj.playDeathAnimation();

   // Schedule corpse removal. Just keeping the place clean.
   if( %obj == $PlayerP1 )
	{
	}
	else
	{
		$PlayerScore += %obj.aiPlayerDb.score;
		$PlayerKillNpcScore += %obj.aiPlayerDb.score;
		$PlayerKillNpcNum++;
		commandToClient( $PlayerP1.client, 'addPlayerScore', $PlayerScore );
		commandToClient( $PlayerP1.client, 'addKillNpcNum', $PlayerKillNpcNum );
		showScore(%obj,%obj.aiPlayerDb.score);
		if( $PlayerP1.firstDaXieBao )
		{
			$PlayerP1.firstDaXieBao = 0;
			//error("主角小于%10产生物品");
			spwanTool( %obj, 1 );
		}
		if( %obj.tool !$= "" )
		{
			//error("掉落物品"@%obj.tool);
			spwanTool( %obj, %obj.tool );
		}
		%obj.schedule(100, "delete");
	}
}

function Armor::onDestroyed(%this, %obj, %state)
{
   	// Release the main weapon trigger
	%expData = %this.explosion;
  	if( isObject(%expData) 
		&& %expData.getClassName() $= "ExplosionData" 
		&& isObject(%obj))
	{
  		%exp = new Explosion()
		{
			datablock = %expData;
			position = %obj.getPosition();
		};
		if (isObject(%exp))
		{
			MissionCleanup.add(%exp);
		}
	}
	else
	{
		%exp = "";
	}
	
	return %exp;
}

//-----------------------------------------------------------------------------

function Armor::onLeaveMissionArea(%this, %obj)
{
   //echo("\c4Leaving Mission Area at POS:"@ %obj.getPosition());

   // Inform the client
   %obj.client.onLeaveMissionArea();

   // Damage over time and kill the coward!
   //%obj.setDamageDt(0.2, "MissionAreaDamage");
}

function Armor::onEnterMissionArea(%this, %obj)
{
   //echo("\c4Entering Mission Area at POS:"@ %obj.getPosition());

   // Inform the client
   %obj.client.onEnterMissionArea();

   // Stop the punishment
   //%obj.clearDamageDt();
}

//-----------------------------------------------------------------------------

function Armor::onEnterLiquid(%this, %obj, %coverage, %type)
{
   //echo("\c4this:"@ %this @" object:"@ %obj @" just entered water of type:"@ %type @" for "@ %coverage @"coverage");
}

function Armor::onLeaveLiquid(%this, %obj, %type)
{
   //
}

//-----------------------------------------------------------------------------

function Armor::onTrigger(%this, %obj, %triggerNum, %val)
{
   // This method is invoked when the player receives a trigger move event.
   // The player automatically triggers slot 0 and slot one off of triggers #
   // 0 & 1.  Trigger # 2 is also used as the jump key.
}

//-----------------------------------------------------------------------------
// Player methods
//-----------------------------------------------------------------------------

//----------------------------------------------------------------------------

function Player::kill(%this, %damageType)
{
   %this.damage(0, %this.getPosition(), 10000, %damageType);
}

//----------------------------------------------------------------------------

function Player::mountVehicles(%this, %bool)
{
   // If set to false, this variable disables vehicle mounting.
   %this.mountVehicle = %bool;
}

function Player::isPilot(%this)
{
   %vehicle = %this.getObjectMount();
   // There are two "if" statements to avoid a script warning.
   if (%vehicle)
      if (%vehicle.getMountNodeObject(0) == %this)
         return true;
   return false;
}

//----------------------------------------------------------------------------

function Player::playDeathAnimation(%this)
{
   if (%this.client.deathIdx++ > 11)
      %this.client.deathIdx = 1;
   %this.setActionThread("Death" @ %this.client.deathIdx);
}

function Player::playCelAnimation(%this, %anim)
{
   if (%this.getState() !$= "Dead")
      %this.setActionThread("cel"@%anim);
}


//----------------------------------------------------------------------------

function Player::playDeathCry(%this)
{
   //%this.playAudio(0, DeathCrySound);
}

function Player::playPain(%this)
{
   //%this.playAudio(0, PainCrySound);
}

// ----------------------------------------------------------------------------
// Numerical Health Counter
// ----------------------------------------------------------------------------

function Player::updateHealth(%player)
{
   //echo("\c4Player::updateHealth() -> Player Health changed, updating HUD!");

   // Calcualte player health
   %maxDamage = %player.getDatablock().maxDamage;
   %damageLevel = %player.getDamageLevel();
   %curHealth = %maxDamage - %damageLevel;
   %curHealth = mceil(%curHealth);

   // Send the player object's current health level to the client, where it
   // will Update the numericalHealth HUD.
   commandToClient(%player.client, 'setNumericalHealthHUD', %curHealth);
}

function Player::use(%player, %data)
{
   // No mounting/using weapons when you're driving!
   if (%player.isPilot())
      return(false);

   Parent::use(%player, %data);
}

function spwanTool( %obj, %index )
{
	if( %index $= 1 )
	{
		%name = "daXueBao";
	}
	else if( %index $= 2 )
	{
		%name = "xiaoXueBao";
	}
	else
	{
		%name = "daoDanBao";
	}
	%pos = %obj.getPosition();
	%pos.z += 3;
	%t = new Item() {
	  collideable = "0";
      static = "1";
      rotate = "1";
      rotate2 = "0";
      dataBlock = %name;
      position = %pos;
      scale = "2 2 2";
	  pitchAngle = "0";
      canSaveDynamicFields = "1";
   };
	MissionCleanup.add( %t );
	%t.wudi = false;
	schedule( 900, 0, toolWudi, %t );
	//error("掉落物品"@%t.dataBlock);
}

function toolWudi( %obj )
{
	%obj.wudi = true;
}
