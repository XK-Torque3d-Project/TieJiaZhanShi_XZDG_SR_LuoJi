//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Global movement speed that affects all cameras.  This should be moved
// into the camera datablock.
$Camera::movementSpeed = 30;

function Observer::onTrigger(%this,%obj,%trigger,%state)
{
   // state = 0 means that a trigger key was released
   if (%state == 0)
      return;

   // Default player triggers: 0=fire 1=altFire 2=jump
   %client = %obj.getControllingClient();
   switch$ (%obj.mode)
   {
      case "Observer":
         // Do something interesting.

      case "Corpse":
         // Viewing dead corpse, so we probably want to respawn.
         %client.spawnPlayer();

         // Set the camera back into observer mode, since in
         // debug mode we like to switch to it.
         %this.setMode(%obj,"Observer");
   }
}

function Observer::setMode(%this,%obj,%mode,%arg1,%arg2,%arg3)
{
   switch$ (%mode)
   {
      case "Observer":
         // Let the player fly around
         %obj.setFlyMode();

      case "Corpse":
         // Lock the camera down in orbit around the corpse,
         // which should be arg1
         %transform = %arg1.getTransform();
         %obj.setOrbitMode(%arg1, %transform, 0.5, 4.5, 4.5);

   }
   %obj.mode = %mode;
}


//-----------------------------------------------------------------------------
// Camera methods
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------

function PathCamera::onAdd(%this,%obj)
{
   // Default start mode
   %this.setMode(%this.mode);
}

function PathCamera::setMode(%this,%mode,%arg1,%arg2,%arg3)
{
   // Punt this one over to our datablock
   %this.getDatablock().setMode(%this,%mode,%arg1,%arg2,%arg3);
}

function GameCameraData::onNode(%this,%camera,%nodeId )   
{
	if( $GameOver )
		return;
   %nodeObj = %camera.path.getObject( %nodeId );
   if( %nodeObj.spawnNPC !$= "" )
	{
		$PlayerP1.cameraSpawnNodeNPC( %nodeObj );
	}
   if( %nodeObj.ActiveExplosion !$= "" )
    {
		$PlayerP1.cameraActiveNodeExplosion( %nodeObj );
	}
   if( %nodeId == ( %camera.path.getCount()-1 ) )
	{
		%stayTime = 0;
		if( %nodeObj.stayTime !$= "" )
		{
			%stayTime = %nodeObj.stayTime;
		}
		%this.schedule( %stayTime, findNxetPath, %camera );
	}
	return;
}   
function GameCameraData::findNxetPath( %this, %camera )
{
	%camera.pathId += 1;
	%pathName = getWord( %camera.pathString, %camera.pathId );
	if( %pathName !$= "" )
	{
		%path = getVariable( %pathName );
			
   		%camera.reset();
   		%camera.path = %pathName;   
  		%camera.pushPath(%pathName);      
   		%camera.popFront();
	}
	else
	{
		schedule(100, 0, setGunShakeState, 0, true);
		SetRespawnState(0, true);
		%this.mountPlayer( %camera );
	}
	return;
}
function GameCameraData::mountPlayer( %this,%camera )
{
	$pathCameraFly = false;
	$PlayerP1.setScale( "1 1 1" );//显示主角
	%camera.mountToPlayer();
	return;
}
function PathCamera::mountToPlayer( %this )
{
	$PlayerP1.client.setControlObject($PlayerP1);
	playGuiContainer.setVisible( true );
	hideNpcShuoMing();
	$PlayerP1.setActionThread("pao");
	return;
}
function PathCamera::setPathNode( %this, %node )
{
   %this.followNode = %node;
   %pathString = %node.cameraPath;
   %this.pathString = %pathString;
   %this.pathId = 0;
   %curPathName = getWord( %pathString, %this.pathId );

   %this.reset();
   %this.path = %curPathName;   
   %this.pushPath(%curPathName);      
   %this.popFront();
   if( PlayGuiBlackBackGround.isVisible() )
	{
		PlayGuiBlackBackGround.setVisible( false );
	}
   return;
}

function PathCamera::pushPath(%this,%path)   
{   
   //echo("PathCamera::pushPath was called!!!");
   for (%i = 0; %i < %path.getCount(); %i++)   
      %this.pushNode(%path.getObject(%i));
   return;
}   
  
function PathCamera::pushNode(%this,%node)   
{   
   //echo("PathCamera::pushNode was called!!!");
   %speed = %node.msToNext * 5;   
   if ((%type = %node.type) $= "")   
      %type = "Normal";   
   if ((%smoothing = %node.smoothing ) $= "")   
      %smoothing = "Linear";
   %this.pushBack(%node.getTransform(),%speed,%type,%smoothing);
   return;
}  