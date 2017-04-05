//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

if ( isObject( moveMap ) )
   moveMap.delete();
new ActionMap(moveMap);

GlobalActionMap.bindCmd(keyboard, "alt enter", "", "Canvas.toggleFullScreen();");
//------------------------------------------------------------------------------
// Non-remapable binds
//-----------------------------------------------------------------------------

function escapeFromGame()
{
   if ( $Server::ServerType $= "SinglePlayer" )
      MessageBoxYesNoOld( "Exit", "Exit from this Mission?", "disconnect();", "");
   else
      MessageBoxYesNoOld( "Disconnect", "Disconnect from the server?", "disconnect();", "");
}

function showPlayerList(%val)
{
   if (%val)
      PlayerListGui.toggle();
}

function showControlsHelp(%val)
{
   if (%val)
      ControlsHelpDlg.toggle();
}

moveMap.bind(keyboard, h, showControlsHelp);

function hideHUDs(%val)
{
   if (%val)
      HudlessPlayGui.toggle();
}

function doScreenShotHudless(%val)
{
   if(%val)
   {
      canvas.setContent(HudlessPlayGui);
      //doScreenshot(%val);
      schedule(10, 0, "doScreenShot", %val);
   }
   else
      canvas.setContent(PlayGui);
}

//------------------------------------------------------------------------------
// Movement Keys
//------------------------------------------------------------------------------

$movementSpeed = 1; // m/s

function setSpeed(%speed)
{
   if(%speed)
      $movementSpeed = %speed;
}

function moveleft(%val)
{
   $mvLeftAction = %val * $movementSpeed;
}

function moveright(%val)
{
   $mvRightAction = %val * $movementSpeed;
}

function moveforward(%val)
{
   $mvForwardAction = %val * $movementSpeed;
}

function movebackward(%val)
{
   $mvBackwardAction = %val * $movementSpeed;
}

function moveup(%val)
{
   $mvUpAction = %val * $movementSpeed;
}

function movedown(%val)
{
   $mvDownAction = %val * $movementSpeed;
}

function turnLeft( %val )
{
   $mvYawRightSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function turnRight( %val )
{
   $mvYawLeftSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panUp( %val )
{
   $mvPitchDownSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function panDown( %val )
{
   $mvPitchUpSpeed = %val ? $Pref::Input::KeyboardTurnSpeed : 0;
}

function getMouseAdjustAmount(%val)
{
   // based on a default camera FOV of 90'
   return(%val * ($cameraFov / 90) * 0.01) * $pref::Input::LinkMouseSensitivity;
}

function getGamepadAdjustAmount(%val)
{
   // based on a default camera FOV of 90'
   return(%val * ($cameraFov / 90) * 0.01) * 5.0;
}

function yaw(%val)
{
   %yawAdj = getMouseAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %yawAdj *= 0.5;
   }

   $mvYaw += %yawAdj;
}

function pitch(%val)
{
   %pitchAdj = getMouseAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %pitchAdj *= 0.5;
   }

   $mvPitch += %pitchAdj;
}

function jump(%val)
{
   $mvTriggerCount2++;
}

function gamePadMoveX( %val )
{
   if(%val > 0)
   {
      $mvRightAction = %val * $movementSpeed;
      $mvLeftAction = 0;
   }
   else
   {
      $mvRightAction = 0;
      $mvLeftAction = -%val * $movementSpeed;
   }
   //$mvXAxis_L = %val;
}

function gamePadMoveY( %val )
{
   if(%val > 0)
   {
      $mvForwardAction = %val * $movementSpeed;
      $mvBackwardAction = 0;
   }
   else
   {
      $mvForwardAction = 0;
      $mvBackwardAction = -%val * $movementSpeed;
   }
   //$mvYAxis_L = %val;
}

function gamepadYaw(%val)
{
   %yawAdj = getGamepadAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %yawAdj = mClamp(%yawAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %yawAdj *= 0.5;
   }

   $mvYaw += %yawAdj;
}

function gamepadPitch(%val)
{
   %pitchAdj = getGamepadAdjustAmount(%val);
   if(ServerConnection.isControlObjectRotDampedCamera())
   {
      // Clamp and scale
      %pitchAdj = mClamp(%pitchAdj, -m2Pi()+0.01, m2Pi()-0.01);
      %pitchAdj *= 0.5;
   }

   $mvPitch += %pitchAdj;
}

moveMap.bind( keyboard, a, moveleft );
moveMap.bind( keyboard, d, moveright );
moveMap.bind( keyboard, left, moveleft );
moveMap.bind( keyboard, right, moveright );

moveMap.bind( keyboard, w, moveforward );
moveMap.bind( keyboard, s, movebackward );
moveMap.bind( keyboard, up, moveforward );
moveMap.bind( keyboard, down, movebackward );

moveMap.bind( keyboard, e, moveup );
moveMap.bind( keyboard, c, movedown );

moveMap.bind( keyboard, space, jump );
moveMap.bind( mouse, xaxis, yaw );
moveMap.bind( mouse, yaxis, pitch );

moveMap.bind( gamepad, thumbrx, "D", "-0.23 0.23", gamepadYaw );
moveMap.bind( gamepad, thumbry, "D", "-0.23 0.23", gamepadPitch );
moveMap.bind( gamepad, thumblx, "D", "-0.23 0.23", gamePadMoveX );
moveMap.bind( gamepad, thumbly, "D", "-0.23 0.23", gamePadMoveY );

moveMap.bind( gamepad, btn_a, jump );
moveMap.bindCmd( gamepad, btn_back, "disconnect();", "" );

moveMap.bindCmd(gamepad, dpadl, "toggleLightColorViz();", "");
moveMap.bindCmd(gamepad, dpadu, "toggleDepthViz();", "");
moveMap.bindCmd(gamepad, dpadd, "toggleNormalsViz();", "");
moveMap.bindCmd(gamepad, dpadr, "toggleLightSpecularViz();", "");

// ----------------------------------------------------------------------------
// Stance/pose
// ----------------------------------------------------------------------------

function doCrouch(%val)
{
   $mvTriggerCount3++;
}

//------------------------------------------------------------------------------
// Mouse Trigger
//------------------------------------------------------------------------------

$testFireCount = 0;
function mouseFire(%val)
{//mouseFireP2(%val);return;
	if ($EnterGameset)
	{
		if(%val)
		{
			$PlayerIsOnFire = true;
			
			if( $BtnIndex == 4 && adjustBitmap.isVisible( ) && adjustBitmap.bitmap !$= "" )
			{
				schedule(40, 0, getCoordAndSet );
				
				if( $CoordIndex == 5 )
				{
					$CoordIndex = 1;
					adjustBitmap.setVisible( false );
					adjustBitmap.bitmap = "";
				}
			}
			return;
		}
		else
		{
			$PlayerIsOnFire = false;
		}
		
		return;
	}
	
	if( $pathCameraFly 
		|| $WaitContinue 
		|| $playerDead 
		|| $GameOver
		|| ($BossErGuanDead && $ZongBossDead)
		|| !$StartGame
		|| $player1State != 1)
	{
		return;
	}
	
	if(%val)
	{
		$PlayerIsOnFire = true;
		commandToServer( 'playerFire');
		return;
	}
	else
	{
		$PlayerIsOnFire = false;
	}
}

$testFireCountP2 = 0;
function mouseFireP2(%val)
{
	if ($EnterGameset)
	{
		if(%val)
		{
			$PlayerIsOnFireP2 = true;
			
			if( $BtnIndex == 5 && adjustBitmap.isVisible( ) && adjustBitmap.bitmap !$= "" )
			{
				schedule(40, 0, getCoordAndSetP2 );
				
				if( $CoordIndex == 5 )
				{
					$CoordIndex = 1;
					adjustBitmap.setVisible( false );
					adjustBitmap.bitmap = "";
				}
			}
			return;
		}
		else
		{
			$PlayerIsOnFireP2 = false;
		}
		
		return;
	}
	
	if( $pathCameraFly 
		|| $WaitContinue 
		|| $playerDead 
		|| $GameOver
		|| ($BossErGuanDead && $ZongBossDead)
		|| !$StartGame
		|| $player2State != 1)
	{
		return;
	}
	
	if(%val)
	{
		$PlayerIsOnFireP2 = true;
		onFireP2();
		return;
	}
	else
	{
		$PlayerIsOnFireP2 = false;
	}
}

//r键发射导�?
function RocketLauncherAmmo( %val )
{//RocketLauncherAmmoP2(%val);return;
	if ($player1State != 1)
	{
		return;
	}
	
	if( %val )
	{
		if( $PlayerDaoDanNum > 0 )
		{
			if( (isObject(aimFlyShapeBitmap.getBitmapFollowObj()) || DaoDanIsEmpty()) )
			{
				// Weapon projectile doesn't have a spread factor so we fire it using
				// the straight ahead aiming point of the gun
				%muzzleVector = $PlayerP1.getAttack1Vector();		
				// Get the player's velocity, we'll then add it to that of the projectile
				%objectVelocity = $PlayerP1.getVelocity();
				%muzzleVelocity = VectorAdd(
				VectorScale(%muzzleVector, zhujiaodaodan.muzzleVelocity),
				VectorScale(%objectVelocity, zhujiaodaodan.velInheritFactor));
				%npc = aimFlyShapeBitmap.getBitmapFollowObj();
				%dandaoNpc = findDaoDan();
				
				if( isObject(%dandaoNpc) )
				{
					%npc = %dandaoNpc;
					%dandaoNpc.lock = true;
				}
				//error("导弹数量�?= "@$DaoDanNum);	
				//error("跟踪对象�?@%npc.getClassName());
				%p = new projectile()
				{
					dataBlock = "zhujiaodaodan";
					initialVelocity = %muzzleVelocity;
					initialPosition = $PlayerP1.getAttackPoint1();
					sourceObject = $PlayerP1;
					aimObject = %npc;
					sourceSlot = 0;
					client = $PlayerP1.client;
				};
				
				if (isObject(%p))
				{
					MissionCleanup.add(%p);
				}
				else
				{
					return;
				}
				
				%p.setIsDaodan( true );
				%p.sourceObj = $PlayerP1;
				$PlayerDaoDanNum--;
				sfxPlayOnce(zhujiaoZJ_Sound_ZJdaoDan01);
				if( $PlayerDaoDanNum == 0 )
				{
					MyShine.setVisible(true);
				}
				commandToClient($PlayerP1.client, 'showDaoDanNum' );
			}			
		}
		else if( (isObject(aimFlyShapeBitmap.getBitmapFollowObj()) || DaoDanIsEmpty()) )
		{
			if (!emptyMissile.isVisible())
			{
				emptyMissile.setVisible( true );
				emptyMissile.schedule(1000, setVisible, false);
			}
		}
	}
	else
	{
		emptyMissile.setVisible( false );
	}
}

function RocketLauncherAmmoP2( %val )
{
	if ($player2State != 1)
	{
		return;
	}
	
	if( %val )
	{
		if( $PlayerDaoDanNumP2 > 0 )
		{
			if( (isObject(aimFlyShapeBitmap.getBitmapFollowObj()) || DaoDanIsEmpty()) )
			{
				%muzzleVector = $PlayerP1.getAttack1Vector();
				%objectVelocity = $PlayerP1.getVelocity();
				%muzzleVelocity = VectorAdd(
				VectorScale(%muzzleVector, zhujiaodaodan.muzzleVelocity),
				VectorScale(%objectVelocity, zhujiaodaodan.velInheritFactor));
				%npc = aimFlyShapeBitmap.getBitmapFollowObj();
				%dandaoNpc = findDaoDan();
				
				if( isObject(%dandaoNpc) )
				{
					%npc = %dandaoNpc;
					%dandaoNpc.lock = true;
				}
				
				%p = new projectile()
				{
					dataBlock = "zhujiaodaodan";
					initialVelocity = %muzzleVelocity;
					initialPosition = $PlayerP1.getAttackPoint1();
					sourceObject = $PlayerP1;
					aimObject = %npc;
					sourceSlot = 0;
					client = $PlayerP1.client;
				};
				
				if (isObject(%p))
				{
					MissionCleanup.add(%p);
				}
				else
				{
					return;
				}
				
				%p.setIsDaodan( true );
				%p.sourceObj = $PlayerP1;
				$PlayerDaoDanNumP2--;
				sfxPlayOnce(zhujiaoZJ_Sound_ZJdaoDan01);
				if( $PlayerDaoDanNumP2 == 0 )
				{
					MyShineP2.setVisible(true);
				}
				commandToClient($PlayerP1.client, 'showDaoDanNumP2' );
			}			
		}
		else if( (isObject(aimFlyShapeBitmap.getBitmapFollowObj()) || DaoDanIsEmpty()) )
		{
			if (!emptyMissile.isVisible())
			{
				emptyMissile.setVisible( true );
				emptyMissile.schedule(1000, setVisible, false);
			}
		}
	}
	else
	{
		emptyMissile.setVisible( false );
	}
}

//p键复�?
function reSpawnPlayer()
{echo("reSpawnPlayerreSpawnPlayerreSpawnPlayerreSpawnPlayer");
	if( $WaitContinue )
	{
		$player1State = 1;
		playerCross.setVisible(true);
		
		$PlayerP1.setDamageLevel( 0 );
		$playerDamageLevel = 0;
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		$PlayerP1.wudi = true;
		$Playerdead = false;
		
		SetRespawnState(1, true);
		schedule(100, 0, setGunShakeState, 1, true);
		
		schedule( 5000, 0, setWudi );
		if( $PlayerP1.currentNode.mustDestroy !$= "true" || !$PlayerP1.isOnNode )
		{
			if( $BOSSjuwuba1 || $BOSSjuwuba2 || $BOSSjuwuba3 || $BOSSjuwuba4
			|| $langren01 || $BOSSlangren || $BOSSlangren02
			|| $BOSSfeiJi01 || $BOSSfeiJi03 )
			{
				//echo("boss not dead, not move");
			}
			else
			{				
				$PlayerP1.continueMove();	
				echo("reSpawn       $PlayerP1.continueMove()");
			}
		}
		commandToClient( $PlayerP1.client,'drawPlayerHealth' );
		$WaitContinue = false;
		PCVRSubCoin( $RequestCoinNum );
		$CurrentCoinNum -= $RequestCoinNum;
		echo("sub coin successp111");
		changeCoinNum();
		$PlayerDaoDanNum = 20;
		emptyMissile.setVisible( false );
		commandToClient($PlayerP1.client, 'showDaoDanNum' );
		continueGame.setVisible(false);
		willContinue.setVisible( false );
		continueNumber.setVisible( false );
	}
	else if( $player1State == 0 )
	{
		$player1State = 1;
		if ($player2State > 0)
		{
			//$damagePercent = 0.5;
			$attackPercent = 0.7;
		}
		playerCross.setVisible(true);
		
		$PlayerP1.setDamageLevel( 0 );
		$playerDamageLevel = 0;
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		
		SetRespawnState(1, true);
		schedule(100, 0, setGunShakeState, 1, true);
		
		commandToClient( $PlayerP1.client,'drawPlayerHealth' );
		$WaitContinue = false;
		PCVRSubCoin( $RequestCoinNum );
		$CurrentCoinNum -= $RequestCoinNum;
		echo("sub coin success p1");
		changeCoinNum();
        $PlayerDaoDanNum = 20;
		commandToClient($PlayerP1.client, 'showDaoDanNum' );
	}
	else if ($player1State < 0)
	{
		if ($player2State > 0)
		{
			$player1State = 1;
			//$damagePercent = 0.5;
			$attackPercent = 0.7;
		}
		
		$PlayerP1.setDamageLevel( 0 );
		$playerDamageLevel = 0;
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		
		SetRespawnState(1, true);
		schedule(100, 0, setGunShakeState, 1, true);
		
		commandToClient( $PlayerP1.client,'drawPlayerHealth' );
		$WaitContinue = false;
		PCVRSubCoin( $RequestCoinNum );
		$CurrentCoinNum -= $RequestCoinNum;
		echo("sub coin success p1");
		changeCoinNum();
        $PlayerDaoDanNum = 20;
		commandToClient($PlayerP1.client, 'showDaoDanNum' );
	}
	
	return;
}

function reSpawnPlayerP2()
{echo("reSpawnPlayerP2reSpawnPlayerP2reSpawnPlayerP2reSpawnPlayerP2reSpawnPlayerP2");
	if( $WaitContinue )
	{
		$player2State = 1;
		playerCrossP2.setVisible(true);
		
		$playerDamageLevelP2 = 0;
		changeBloodP2();
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		$PlayerP1.wudi = true;
		$Playerdead = false;
		
		SetRespawnState(2, true);
		schedule(100, 0, setGunShakeState, 2, true);
		
		schedule( 5000, 0, setWudi ); 
		if( $PlayerP1.currentNode.mustDestroy !$= "true" || !$PlayerP1.isOnNode )
		{
			if( $BOSSjuwuba1 || $BOSSjuwuba2 || $BOSSjuwuba3 || $BOSSjuwuba4
			   || $langren01 || $BOSSlangren || $BOSSlangren02
			   || $BOSSfeiJi01 || $BOSSfeiJi03 )
			{
				//echo("boss not dead, not move");
			}
			else
			{				
				$PlayerP1.continueMove();	
				echo("reSpawn       $PlayerP1.continueMove()");
			}
		}
		
		$WaitContinue = false;
		PCVRSubCoinP2( $RequestCoinNum );
		$CurrentCoinNumP2 -= $RequestCoinNum;
		echo("sub coin success p2");
		changeCoinNump2();
        $PlayerDaoDanNumP2 = 20;
		emptyMissile.setVisible( false );
		commandToClient($PlayerP1.client, 'showDaoDanNumP2' );
		continueGame.setVisible(false);
		willContinue.setVisible( false );
		continueNumber.setVisible( false );
	}
	else if( $player2State == 0 )
	{
		$player2State = 1;
		if ($player1State > 0)
		{
			//$damagePercent = 0.5;
			$attackPercent = 0.7;
		}
		playerCrossP2.setVisible(true);
		
		$playerDamageLevelP2 = 0;
		changeBloodP2();
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		
		SetRespawnState(2, true);
		schedule(100, 0, setGunShakeState, 2, true);
		
		$WaitContinue = false;
		PCVRSubCoinP2( $RequestCoinNum );
		$CurrentCoinNumP2 -= $RequestCoinNum;
		echo("sub coin success p2");
		changeCoinNump2();
        $PlayerDaoDanNumP2 = 20;
		commandToClient($PlayerP1.client, 'showDaoDanNumP2' );
	}
	else if ($player2State < 0)
	{
		if ($player1State > 0)
		{
			$player2State = 1;
			//$damagePercent = 0.5;
			$attackPercent = 0.7;
		}
		
		$JiJiaPercent50IsShow  = false;
		$JiJiaPercent70IsShow  = false;
		$JiJiaPercent90IsShow  = false;
		
		$playerDamageLevelP2 = 0;
		changeBloodP2();
		
		SetRespawnState(2, true);
		schedule(100, 0, setGunShakeState, 2, true);
		
		$WaitContinue = false;
		PCVRSubCoinP2( $RequestCoinNum );
		$CurrentCoinNumP2 -= $RequestCoinNum;
		echo("sub coin success p2");
		changeCoinNump2();
        $PlayerDaoDanNumP2 = 20;
		commandToClient($PlayerP1.client, 'showDaoDanNumP2' );
	}
	return;
}

function setWudi()
{
	$PlayerP1.wudi = false;
}

//设置玩家币数
function setPlayerCoinNum( %num )
{
	if($CurrentCoinNum !$= "" )
	{
		recordCoinInfor( );//this function is below, and it is about the game setting panel
	}
	
    sfxPlayOnce(panel_Sound_touBi01);
	$CurrentCoinNum += %num; 
	
	changeCoinNum();
	%RecordCounts = GetIniValueNum("RecordCounts", "Counts", "./Detail.hnb");
	%currentStateCoin = GetIniValueNum("Record"@%RecordCounts, "TotalCoinNum", "./Detail.hnb" );
	%currentStateCoin++;
	WriteIniValueString("Record"@%RecordCounts, "TotalCoinNum", %currentStateCoin, "./Detail.hnb");
	
	%allCoins = getIniValueNum( "b99d53", "b99d53", "./shaders/procedural/b99d53c5c5c2d0b_V.hlsl" );
	%allCoins++;
	WriteIniValueString("b99d53", "b99d53", %allCoins, "./shaders/procedural/b99d53c5c5c2d0b_V.hlsl" );
}

//设置玩家币数
function setPlayerCoinNumP2( %num )
{	
    sfxPlayOnce(panel_Sound_touBi01);
	$CurrentCoinNumP2 += %num; 
	
	changeCoinNump2();
}
function altTrigger(%val)
{
   $mvTriggerCount1++;
}

//insert coin, then will be recorded the infor
function recordCoinInfor()
{
	//return;
	%getDaysNum = GetIniValueNum( totalDays, "days", "./calculateCoin.ini" );
	
	if( %getDaysNum == -1 )
	{
		%getDaysNum = 0;
	}
	
	%indexMy = "days"@( %getDaysNum + 1 );
	%getYear1 = GetIniValueNum( %indexMy, "date", "./calculateCoin.ini" );
	
	%myCurrentDateTime = getCurrentTime( );
	%myCurrentDayTime = getWord( %myCurrentDateTime, 0 );	
	%getYearNow = GetIniValueNum( %getDaysNum, "date", "./calculateCoin.ini" );
	
	if( %myCurrentDayTime != %getYearNow )
	{
		createCurrentDayInfor( );
	}
	
	//to record the total operating days
	%getDaysNum = GetIniValueNum( totalDays, "days", "./calculateCoin.ini" );
	
	%dayIndex = "days"@%getDaysNum;
	
	//to record the total coins number
	%getTotalCoinNumber = GetIniValueNum( totalCoinNumber, "totalCoin", "./calculateCoin.ini" );	
	%totalCoinsNum = %getTotalCoinNumber + 1;
	WriteIniValueNum( totalCoinNumber, "totalCoin", %totalCoinsNum, "./calculateCoin.ini" );	
	
	//to record the coin number everday
	%coinNumBefore = GetIniValueNum( %dayIndex, "coin", "./calculateCoin.ini" );	
	%coinNumNow = %coinNumBefore + 1;
	WriteIniValueNum( %dayIndex, "coin", %coinNumNow, "./calculateCoin.ini" );
}

moveMap.bind( mouse, button0, mouseFire );
moveMap.bind( mouse, button1, altTrigger );

moveMap.bind( keyboard, "r", RocketLauncherAmmo );
//------------------------------------------------------------------------------
// Zoom and FOV functions
//------------------------------------------------------------------------------

if($Pref::player::CurrentFOV $= "")
   $Pref::player::CurrentFOV = $pref::Player::DefaultFOV / 2;

// toggleZoomFOV() works by dividing the CurrentFOV by 2.  Each time that this
// toggle is hit it simply divides the CurrentFOV by 2 once again.  If the
// FOV is reduced below a certain threshold then it resets to equal half of the
// DefaultFOV value.  This gives us 4 zoom levels to cycle through.

function toggleZoomFOV()
{
    $Pref::Player::CurrentFOV = $Pref::Player::CurrentFOV / 2;

    if($Pref::Player::CurrentFOV < 5)
        $Pref::Player::CurrentFOV = $Pref::Player::DefaultFov / 2;

    if($ZoomOn)
      setFOV($Pref::Player::CurrentFOV);
    else
      setFOV($Pref::Player::DefaultFOV);
}

function setZoomFOV(%val)
{
   if(%val)
      toggleZoomFOV();
}

$ZoomOn = false;

function toggleZoom(%val)
{
   if (%val)
   {
      if (!$ZoomOn)
         commandToServer('getZoomReticle');
   }
   else
   {
      $ZoomOn = false;
      setFOV($Pref::Player::defaultFOV);
      zoomReticle.setVisible(false);
   }
}

moveMap.bind(keyboard, f, setZoomFOV); // f for field of view
moveMap.bind(keyboard, z, toggleZoom); // z for zoom

//------------------------------------------------------------------------------
// Camera & View functions
//------------------------------------------------------------------------------

function toggleFreeLook( %val )
{
   if ( %val )
      $mvFreeLook = true;
   else
      $mvFreeLook = false;
}

function toggleFirstPerson(%val)
{
   if (%val)
   {
      ServerConnection.setFirstPerson(!ServerConnection.isFirstPerson() );
   }
}

function toggleCamera(%val)
{
   if (%val)
      commandToServer('ToggleCamera');
}

moveMap.bind( keyboard, v, toggleFreeLook ); // v for vanity
moveMap.bind(keyboard, tab, toggleFirstPerson );
moveMap.bind(keyboard, "alt c", toggleCamera);

moveMap.bind( gamepad, btn_start, toggleCamera );
moveMap.bind( gamepad, btn_x, toggleFirstPerson );

// ----------------------------------------------------------------------------
// Misc. Player stuff
// ----------------------------------------------------------------------------

// Gideon does not have these animations, so the player does not need access to
// them.  Commenting instead of removing so as to retain an example for those
// who will want to use a player model that has these animations and wishes to
// use them.

//------------------------------------------------------------------------------
// Item manipulation
//------------------------------------------------------------------------------
moveMap.bindCmd(keyboard, "1", "commandToServer('use',\"RocketLauncher\");", "");

function unmountWeapon(%val)
{
   if (%val)
      commandToServer('unmountWeapon');
}

moveMap.bind(keyboard, 0, unmountWeapon);

function throwWeapon(%val)
{
   if (%val)
      commandToServer('Throw', "Weapon");
}
function tossAmmo(%val)
{
   if (%val)
      commandToServer('Throw', "Ammo");
}

function nextWeapon(%val)
{
   if (%val)
      commandToServer('cycleWeapon', "next");
}

function prevWeapon(%val)
{
   if (%val)
      commandToServer('cycleWeapon', "prev");
}

function mouseWheelWeaponCycle(%val)
{
   if (%val < 0)
      commandToServer('cycleWeapon', "next");
   else if (%val > 0)
      commandToServer('cycleWeapon', "prev");
}

//------------------------------------------------------------------------------
// Message HUD functions
//------------------------------------------------------------------------------

function pageMessageHudUp( %val )
{
   if ( %val )
      pageUpMessageHud();
}

function pageMessageHudDown( %val )
{
   if ( %val )
      pageDownMessageHud();
}

function resizeMessageHud( %val )
{
   if ( %val )
      cycleMessageHudSize();
}

//------------------------------------------------------------------------------
// Demo recording functions
//------------------------------------------------------------------------------

function startRecordingDemo( %val )
{
   if ( %val )
      startDemoRecord();
}

function stopRecordingDemo( %val )
{
   if ( %val )
      stopDemoRecord();
}


//------------------------------------------------------------------------------
// Helper Functions
//------------------------------------------------------------------------------

function dropCameraAtPlayer(%val)
{
   if (%val)
      commandToServer('dropCameraAtPlayer');
}

function dropPlayerAtCamera(%val)
{
   if (%val)
      commandToServer('DropPlayerAtCamera');
}

function bringUpOptions(%val)
{
   if (%val)
      Canvas.pushDialog(OptionsDlg);
}


//------------------------------------------------------------------------------
// Debugging Functions
//------------------------------------------------------------------------------

$MFDebugRenderMode = 0;
function cycleDebugRenderMode(%val)
{
   if (!%val)
      return;

   $MFDebugRenderMode++;

   if ($MFDebugRenderMode > 16)
      $MFDebugRenderMode = 0;
   if ($MFDebugRenderMode == 15)
      $MFDebugRenderMode = 16;

   setInteriorRenderMode($MFDebugRenderMode);

   if (isObject(ChatHud))
   {
      %message = "Setting Interior debug render mode to ";
      %debugMode = "Unknown";

      switch($MFDebugRenderMode)
      {
         case 0:
            %debugMode = "NormalRender";
         case 1:
            %debugMode = "NormalRenderLines";
         case 2:
            %debugMode = "ShowDetail";
         case 3:
            %debugMode = "ShowAmbiguous";
         case 4:
            %debugMode = "ShowOrphan";
         case 5:
            %debugMode = "ShowLightmaps";
         case 6:
            %debugMode = "ShowTexturesOnly";
         case 7:
            %debugMode = "ShowPortalZones";
         case 8:
            %debugMode = "ShowOutsideVisible";
         case 9:
            %debugMode = "ShowCollisionFans";
         case 10:
            %debugMode = "ShowStrips";
         case 11:
            %debugMode = "ShowNullSurfaces";
         case 12:
            %debugMode = "ShowLargeTextures";
         case 13:
            %debugMode = "ShowHullSurfaces";
         case 14:
            %debugMode = "ShowVehicleHullSurfaces";
         // Depreciated
         //case 15:
         //   %debugMode = "ShowVertexColors";
         case 16:
            %debugMode = "ShowDetailLevel";
      }

      ChatHud.addLine(%message @ %debugMode);
   }
}

//------------------------------------------------------------------------------
//
// Start profiler by pressing ctrl f3
// ctrl f3 - starts profile that will dump to console and file
//
function doProfile(%val)
{
   if (%val)
   {
      // key down -- start profile
      //echo("Starting profile session...");
      profilerReset();
      profilerEnable(true);
   }
   else
   {
      // key up -- finish off profile
      //echo("Ending profile session...");

      profilerDumpToFile("profilerDumpToFile" @ getSimTime() @ ".txt");
      profilerEnable(false);
   }
}

//------------------------------------------------------------------------------
// Misc.
//------------------------------------------------------------------------------

// ----------------------------------------------------------------------------
// Useful vehicle stuff
// ----------------------------------------------------------------------------

// Trace a line along the direction the crosshair is pointing
// If you find a car with a player in it...eject them
function carjack()
{
   %player = LocalClientConnection.getControlObject();

   if (%player.getClassName() $= "Player")
   {
      %eyeVec = %player.getEyeVector();

      %startPos = %player.getEyePoint();
      %endPos = VectorAdd(%startPos, VectorScale(%eyeVec, 1000));

      %target = ContainerRayCast(%startPos, %endPos, $TypeMasks::VehicleObjectType);

      if (%target)
      {
         // See if anyone is mounted in the car's driver seat
         %mount = %target.getMountNodeObject(0);

         // Can only carjack bots
         // remove '&& %mount.getClassName() $= "AIPlayer"' to allow you
         // to carjack anyone/anything
         if (%mount && %mount.getClassName() $= "AIPlayer")
         {
            commandToServer('carUnmountObj', %mount);
         }
      }
   }
}

function getOut()
{
   commandToServer('setPlayerControl');
   schedule(500,0,"jump","");
   schedule(600,0,"jump","");
}
