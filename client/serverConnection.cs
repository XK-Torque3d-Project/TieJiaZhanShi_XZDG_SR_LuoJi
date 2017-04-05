//-----------------------------------------------------------------------------
// Torque Game Engine 
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// Functions dealing with connecting to a server


//-----------------------------------------------------------------------------
// Server connection error
//-----------------------------------------------------------------------------

addMessageCallback( 'MsgConnectionError', handleConnectionErrorMessage );

function handleConnectionErrorMessage(%msgType, %msgString, %msgError)
{
   // On connect the server transmits a message to display if there
   // are any problems with the connection.  Most connection errors
   // are game version differences, so hopefully the server message
   // will tell us where to get the latest version of the game.
   $ServerConnectionErrorMessage = %msgError;
}



//----------------------------------------------------------------------------
// GameConnection client callbacks
//----------------------------------------------------------------------------

function GameConnection::initialControlSet(%this)
{
   echo ("*** Initial Control Object");

   // The first control object has been set by the server
   // and we are now ready to go.
   
   // first check if the editor is active
   if (!isToolBuild() || !Editor::checkActiveLoadDone())
   {
		PCVREnablePcvrProcess(true);
      	if (Canvas.getContent() != PlayGui.getId())
		{
			echo("loading over", $LoadingLevel);
			if( $LoadingLevel == 1 )
			{
				//Canvas.setContent(touBiWindow);
				Canvas.schedule(1000, setContent, touBiWindow);
			}
			else
			{
				//Canvas.setContent(PlayGui);
				Canvas.schedule(1000, setContent, PlayGui);
				//schedule(5000, 0, setPlayerCanshoot, true);
			}
		}
    }
}

function GameConnection::setLagIcon(%this, %state)
{
   if (%this.getAddress() $= "local")
      return;
   LagIcon.setVisible(%state $= "true");
}

function GameConnection::onConnectionAccepted(%this)
{
   // Called on the new connection object after connect() succeeds.
   //LagIcon.setVisible(false);
   
   // Startup the physX world on the client before any
   // datablocks and objects are ghosted over.
   physicsInitWorld( "client" );   
}

function GameConnection::onConnectionTimedOut(%this)
{
   // Called when an established connection times out
   disconnectedCleanup();
   MessageBoxOK( "TIMED OUT", "The server connection has timed out.");
}

function GameConnection::onConnectionDropped(%this, %msg)
{
   // Established connection was dropped by the server
   disconnectedCleanup();
   MessageBoxOK( "DISCONNECT", "The server has dropped the connection: " @ %msg);
}

function GameConnection::onConnectionError(%this, %msg)
{
   // General connection error, usually raised by ghosted objects
   // initialization problems, such as missing files.  We'll display
   // the server's connection error message.
   disconnectedCleanup();
   MessageBoxOK( "DISCONNECT", $ServerConnectionErrorMessage @ " (" @ %msg @ ")" );
}


//----------------------------------------------------------------------------
// Connection Failed Events
//----------------------------------------------------------------------------

function GameConnection::onConnectRequestRejected( %this, %msg )
{
   switch$(%msg)
   {
      case "CR_INVALID_PROTOCOL_VERSION":
         %error = "Incompatible protocol version: Your game version is not compatible with this server.";
      case "CR_INVALID_CONNECT_PACKET":
         %error = "Internal Error: badly formed network packet";
      case "CR_YOUAREBANNED":
         %error = "You are not allowed to play on this server.";
      case "CR_SERVERFULL":
         %error = "This server is full.";
      case "CHR_PASSWORD":
         // XXX Should put up a password-entry dialog.
         if ($Client::Password $= "")
            MessageBoxOK( "REJECTED", "That server requires a password.");
         else {
            $Client::Password = "";
            MessageBoxOK( "REJECTED", "That password is incorrect.");
         }
         return;
      case "CHR_PROTOCOL":
         %error = "Incompatible protocol version: Your game version is not compatible with this server.";
      case "CHR_CLASSCRC":
         %error = "Incompatible game classes: Your game version is not compatible with this server.";
      case "CHR_INVALID_CHALLENGE_PACKET":
         %error = "Internal Error: Invalid server response packet";
      default:
         %error = "Connection error.  Please try another server.  Error code: (" @ %msg @ ")";
   }
   disconnectedCleanup();
   MessageBoxOK( "REJECTED", %error);
}

function GameConnection::onConnectRequestTimedOut(%this)
{
   disconnectedCleanup();
   MessageBoxOK( "TIMED OUT", "Your connection to the server timed out." );
}


//-----------------------------------------------------------------------------
// Disconnect
//-----------------------------------------------------------------------------

function disconnect()
{
	saveMissionInfo();
//   // We need to stop the client side simulation
//   // else physics resources will not cleanup properly.
//   physicsStopSimulation( "client" );
//      
//   // Delete the connection if it's still there.
//   if (isObject(ServerConnection))
//      ServerConnection.delete();
//   disconnectedCleanup();
//
//   // Call destroyServer in case we're hosting
//   destroyServer();
    
	stopMission();
}

//结束游戏关卡
function stopMission()
{
   physicsStopSimulation( "client" );
    if (isObject(ServerConnection))
	{
      	ServerConnection.delete();
	}
   disconnectedCleanup();
   destroyServer();
   startMission();
}

//关卡结束时清理
function stopMissionCleanup()
{
	// Clear misc script stuff
   HudMessageVector.clear();

   // Terminate all playing sounds
   sfxStopAll($SimAudioType);
   if (isObject(MusicPlayer))
      MusicPlayer.stop();

   //
   LagIcon.setVisible(false);
   PlayerListGui.clear();
   
   // Clear all print messages
   clientCmdclearBottomPrint();
   clientCmdClearCenterPrint();

   purgeResources();
}

function disconnectedCleanup()
{
   // Clear misc script stuff
   HudMessageVector.clear();

   // Terminate all playing sounds
   sfxStopAll($SimAudioType);
   if (isObject(MusicPlayer))
      MusicPlayer.stop();

   //
   //LagIcon.setVisible(false);
   PlayerListGui.clear();
   
   // Clear all print messages
   clientCmdclearBottomPrint();
   clientCmdClearCenterPrint();

   // Back to the launch screen
//   if (isObject( MainMenuGui ))
//      Canvas.setContent( MainMenuGui );
//   else if (isObject( UnifiedMainMenuGui ))
//      Canvas.setContent( UnifiedMainMenuGui );
   
   // We can now delete the client physics simulation.
   physicsDestroyWorld( "client" );                 
}

//启动新关卡
function startMission()
{
	if( $GameOver || ( $BossErGuanDead && $ZongBossDead ) )  //游戏通关
	{
		$LoadingLevel = 1;
		cleanMissionInfo();
		StartLevel( $LoadingLevel );
	}
    else if( $BossYiGuanDead && !$BossErGuanDead && !$ZongBossDead )
	{
		Canvas.setContent("ChooseLevelGui");
	}
	else if( $BossErGuanDead && !$ZongBossDead )
	{
		$LoadingLevel = 3;
		StartLevel( $LoadingLevel );
	}
	else if( !$BossErGuanDead && $ZongBossDead )
	{
		$LoadingLevel = 2; 
		StartLevel( $LoadingLevel );
	}
}

function  saveMissionInfo()
{
	WriteIniValueString( "PlayerTimeNumber", "Score", $PlayerTimeNumber, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerDaoDanNum", "Score", $PlayerDaoDanNum, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerDaoDanNumP2", "Score", $PlayerDaoDanNumP2, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerScore", "Score", $PlayerScore, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillBossNum", "Score", $PlayerKillBossNum, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillBossScore", "Score", $PlayerKillBossScore, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillNpcNum", "Score", $PlayerKillNpcNum, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillNpcScore", "Score", $PlayerKillNpcScore, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerUseAmmoCount", "Score", $PlayerUseAmmoCount, "./missionInfo.hnb" );
	//WriteIniValueString( "CurrentCoinNum", "Score", $CurrentCoinNum, "./missionInfo.hnb" );
	WriteIniValueString( "RequestCoinNum", "Score", $RequestCoinNum, "./missionInfo.hnb" );
	WriteIniValueString( "LoadingLevel", "Score", $LoadingLevel, "./missionInfo.hnb" );
	WriteIniValueString( "BossYiGuanDead", "Score", $BossYiGuanDead, "./missionInfo.hnb" );
	WriteIniValueString( "BossErGuanDead", "Score", $BossErGuanDead, "./missionInfo.hnb" );
	WriteIniValueString( "ZongBossDead", "Score", $ZongBossDead, "./missionInfo.hnb" );
}

function getMissionInfo()
{
	$PlayerScore = GetIniValueNum( "PlayerScore", "Score", "./missionInfo.hnb" );
	$PlayerKillBossNum = GetIniValueNum( "PlayerKillBossNum", "Score", "./missionInfo.hnb" );
	$PlayerKillBossScore = GetIniValueNum( "PlayerKillBossScore", "Score", "./missionInfo.hnb" );
	$PlayerKillNpcNum = GetIniValueNum( "PlayerKillNpcNum", "Score", "./missionInfo.hnb" );
	$PlayerKillNpcScore = GetIniValueNum( "PlayerKillNpcScore", "Score", "./missionInfo.hnb" );
	$PlayerUseAmmoCount = GetIniValueNum( "PlayerUseAmmoCount", "Score", "./missionInfo.hnb" );
	$PlayerDaoDanNum = GetIniValueNum( "PlayerDaoDanNum", "Score", "./missionInfo.hnb" );
	$PlayerDaoDanNumP2 = GetIniValueNum( "PlayerDaoDanNumP2", "Score", "./missionInfo.hnb" );
	$PlayerTimeNumber = GetIniValueNum( "PlayerTimeNumber", "Score", "./missionInfo.hnb" );
	//$CurrentCoinNum = GetIniValueNum( "CurrentCoinNum", "Score", "./missionInfo.hnb" );	
	//$RequestCoinNum = GetIniValueNum( "RequestCoinNum", "Score", "./missionInfo.hnb" );
	//$LoadingLevel = GetIniValueNum("LoadingLevel","Score","missionInfo.hnb");
	$BossYiGuanDead = GetIniValueNum( "BossYiGuanDead", "Score", "./missionInfo.hnb" );
	$BossErGuanDead = GetIniValueNum( "BossErGuanDead", "Score", "./missionInfo.hnb" );
	$ZongBossDead = GetIniValueNum( "ZongBossDead", "Score", "./missionInfo.hnb" );
}

function cleanMissionInfo()
{
	WriteIniValueString( "PlayerTimeNumber", "Score",0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerDaoDanNum", "Score", 20, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerScore", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillBossNum", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillBossScore", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillNpcNum", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerKillNpcScore", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "PlayerUseAmmoCount", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "CurrentCoinNum", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "RequestCoinNum", "Score", 2, "./missionInfo.hnb" );
	WriteIniValueString( "LoadingLevel", "Score", 1, "./missionInfo.hnb" );
	WriteIniValueString( "BossYiGuanDead", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "BossErGuanDead", "Score", 0, "./missionInfo.hnb" );
	WriteIniValueString( "ZongBossDead", "Score", 0, "./missionInfo.hnb" );
}
