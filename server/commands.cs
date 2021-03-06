//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Misc server commands avialable to clients
//-----------------------------------------------------------------------------
$PlayerIsOnFire = false;
$PlayerIsOnFireP2 = false;
function serverCmdSuicide(%client)
{
   if (isObject(%client.player))
      %client.player.kill("Suicide");
}

function serverCmdPlayCel(%client,%anim)
{
   if (isObject(%client.player))
      %client.player.playCelAnimation(%anim);
}

function serverCmdTestAnimation(%client, %anim)
{
   if (isObject(%client.player))
      %client.player.playTestAnimation(%anim);
}

function serverCmdPlayDeath(%client)
{
   if (isObject(%client.player))
      %client.player.playDeathAnimation();
}

// ----------------------------------------------------------------------------
// Throw/Toss
// ----------------------------------------------------------------------------

function serverCmdThrow(%client, %data)
{
   %player = %client.player;
   if(!isObject(%player) || %player.getState() $= "Dead" || !$Game::Running)
      return;
   switch$ (%data)
   {
      case "Weapon":
         %item = (%player.getMountedImage($WeaponSlot) == 0) ? "" : %player.getMountedImage($WeaponSlot).item;
         if (%item !$="")
            %player.throw(%item);
      case "Ammo":
         %weapon = (%player.getMountedImage($WeaponSlot) == 0) ? "" : %player.getMountedImage($WeaponSlot);
         if (%weapon !$= "")
         {
            if(%weapon.ammo !$= "")
               %player.throw(%weapon.ammo);
         }
      default:
         if(%player.hasInventory(%data.getName()))
            %player.throw(%data);
   }
}

// ----------------------------------------------------------------------------
// Force game end and cycle
// ----------------------------------------------------------------------------

function serverCmdFinishGame()
{
   cycleGame();
}

// ----------------------------------------------------------------------------
// Cycle weapons
// ----------------------------------------------------------------------------

function serverCmdCycleWeapon(%client, %data)
{
   %client.getControlObject().cycleWeapon(%data);
}

// ----------------------------------------------------------------------------
// Unmount current weapon
// ----------------------------------------------------------------------------

function serverCmdUnmountWeapon(%client)
{
   %client.getControlObject().unmountImage($WeaponSlot);
}

// ----------------------------------------------------------------------------
// Zoom reticle
// ----------------------------------------------------------------------------

function serverCmdgetZoomReticle(%client)
{
   %player = %client.player;
   %weapon = (%player.getMountedImage($WeaponSlot) == 0) ? "" : %player.getMountedImage($WeaponSlot).item;
   %reticle = %weapon $= "" ? 'bino.png' : %weapon.zoomReticle;
   commandToClient(%client, 'setZoom', %reticle);
}

function serverCmdplayerFire(%client)
{
	if( %client.getControlObject() != $PlayerP1 )
	{
		return;
	}
	
	if(!$testIsFireStart && $PlayerCanshoot)
	{
		$PlayerP1.setImageTrigger( 0, true);
		$PlayerP1.setImageTrigger(0, false);
	}
	return;
}
