//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// PlayGui is the main TSControl through which the game is viewed.
// The PlayGui also contains the hud controls.
//-----------------------------------------------------------------------------
function PlayGui::onWake(%this)
{
	//playerCross.setVisible(false);
	//playerCrossP2.setVisible(false);
	echo("here PlayGui::onWake");
	//checkAnquandai(false);
	schedule(1000, 0, intoPlaygui, %this);
}

function intoPlaygui(%this)
{
	echo(" here intoPlaygui");
   danliang.setvisible( true );
   // Turn off any shell sounds...
   // sfxStop( ... );

   $enableDirectInput = "1";
   //activateDirectInput();

   // Message hud dialog
   //Canvas.pushDialog( MainChatHud );
   //chatHud.attach(HudMessageVector);
   if( $LoadingLevel == 1)
   {
	   $BossFeiJiDead = false;
  	   sfxPlayOnce(Back_Sound_yiGuanBeiJing);
   }
   if( $LoadingLevel == 2)
   { 
	   schedule(3000, 0, setBossWeaponBitmap, 2 );
	   sfxStopAll();
	   sfxPlayOnce(back_Sound_siGuanBeiJing);
   }
   if( $LoadingLevel == 3)
   {
	   schedule(3000, 0, setBossWeaponBitmap, 4 );
	   sfxStopAll();
	   sfxPlayOnce(back_Sound_boss3BeiJing);
   }

   commandToClient( $PlayerP1.client, 'bossGetDamage', 0, $PlayerP1.GetMaxDamage() );
   commandToClient( $PlayerP1.client, 'BossGetDamageTotal', 0, $PlayerP1.GetMaxDamage() );
   // just update the action map here

	if($showOtherInofr)
	{
		moveMap.push();
	}
  
    GroundFadeInBlack.setVisible( false );
	if( $LoadingLevel != 1 )
	{
		playGuiContainer.setVisible( true );
		aimFlyShapeBitmap.setRootCtrl( %this );
		commandToClient( $PlayerP1.client, 'aimBitmapCheck' );
		commandToClient( $PlayerP1.client, 'showDaoDanNum' );
		commandToClient( $PlayerP1.client, 'showDaoDanNumP2' );
    	commandToClient( $PlayerP1.client, 'startFlashYiBiaoPan' );
		commandToClient( $PlayerP1.client, 'PlaySound');
		changeCoinNum();
		changeCoinNump2();
		getMissionInfo();
	}
	schedule( 4000, 0, danliangguodu, 0 );
	$PlayerP1.reset();
	$PlayerP1.ContinueMove();
    $PlayerP1.getStayTime(0);
	$PlayerP1.setKillAllState(false);
    $PlayerP1.schedule( 3000, OnPlayerStartGame );
	%this.initFollowNpcCtrl();
	if( $HardIndex == 1 )
	{
		$PlayerP1.setInvincible(true);
	}
	else
	{
		$PlayerP1.setInvincible(false);
	}
	if( $HardIndex == 3)
	{
		%maxDamage = $PlayerP1.getMaxDamage();
		//%maxDamage = mFloor( ( %maxDamage *2 )/3 );
		$PlayerP1.setMaxDamage( %maxDamage);
	}
	if( $PlayerTimeNumber == 0 )
	{
		//echo("\c4start Timer!!\n\n\n");
		$GameOver = false;
		%this.setTimeCtrl();
		aimFlyShapeBitmap.setRootCtrl( %this );
		commandToClient( $PlayerP1.client, 'aimBitmapCheck' );
		commandToClient( $PlayerP1.client, 'showDaoDanNum' );
		commandToClient( $PlayerP1.client, 'showDaoDanNumP2' );
    	commandToClient( $PlayerP1.client, 'startFlashYiBiaoPan' );
		commandToClient( $PlayerP1.client, 'PlaySound');
		commandToClient( $PlayerP1.client, 'addPlayerScore', 0 );
		commandToClient( $PlayerP1.client, 'addKillBossNum', 0 );
		changeCoinNum();
		changeCoinNump2();
	}
	followObj1.setVisible( false );
	followObj2.setVisible( false );
	followObj3.setVisible( false );
	followObj4.setVisible( false );
	reStartShowGui();
	$playerDamageLevel = 0;
	clientCmddrawPlayerHealth();
	$playerDamageLevelP2 = 0;
	changeBloodP2();
	
	if ($SafeBeltIndex == 2)
	{
		anquandai2.setVisible( false );
		anquandai2P2.setVisible( false );
	}
	else
	checkAnquandai(false);
	
	playGuiInitSomeInfor();
}

function PlayGui::onSleep(%this)
{
	//playerCross.setVisible(false);
	//playerCrossP2.setVisible(false);
	setGunShakeState(0, false);
	Canvas.popDialog( MainChatHud  );
	setStartGameState(false);
	$StartGame = false;
	$PlayerCanshoot = false;
	
	// pop the keymaps
	moveMap.pop();
}

function PlayGui::initFollowNpcCtrl( %this )
{
	followObj1.setRootCtrl( PlayGui.getId() );
	followObj2.setRootCtrl( PlayGui.getId() );
	followObj3.setRootCtrl( PlayGui.getId() );
	followObj4.setRootCtrl( PlayGui.getId() );
	
	followObj1.setControlObj( $PlayerP1 );
	followObj2.setControlObj( $PlayerP1 );
	followObj3.setControlObj( $PlayerP1 );
	followObj4.setControlObj( $PlayerP1 );
}
function PlayGui::clearHud( %this )
{
   Canvas.popDialog( MainChatHud );

   while ( %this.getCount() > 0 )
      %this.getObject( 0 ).delete();
}

function PlayGui::setTimeCtrl( %this )
{
	if( PlayGui.isAwake() )
	{
		$PlayerTimeNumber++;
		commandToClient( $PlayerP1.client, 'setPlayerTimeNumber', $PlayerTimeNumber );
	}
	if( !$GameOver )
	{
		%this.schedule( 1000, setTimeCtrl );
	}
	return;
}
//-----------------------------------------------------------------------------

function refreshBottomTextCtrl()
{
   BottomPrintText.position = "0 0";
}

function refreshCenterTextCtrl()
{
   CenterPrintText.position = "0 0";
}

function spawnDangerCtrlFollowBoss( %obj, %lockSpeed, %changeSpeed, %width, %adjustExtent )
{
	%obj = %obj.getId();
	
	if( !isObject( %obj ) )
		return;
	
	if( !isObject( $PlayerP1 ) )
		return;
		
	%ctrObj = %obj.alarmControl;
	
	if( isObject( %ctrObj ) )
		%ctrObj.delete();
		
	%dangerControl = new guiDangerAlarmCtrl(  ) {
		bitmap = "art/gui/dangerObject/alarmControlBoss1.png";
		wrap = "1";
		lockSpeed = "30";
		changeSpeed = "30";
		controlType = "1";
		showPart = "1";
		reversal_X = "0";
		isContainer = "0";
		Profile = "GuiDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = "0 0";
		Extent = "8 8";
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		tooltipprofile = "GuiToolTipProfile";
		hovertime = "1000";
		canSaveDynamicFields = "0";
	};
   	MissionCleanup.add( %dangerControl );
	
	if( !isObject( %dangerControl ) )
		return;
	
	%changeSpeed = mfloor( ( %changeSpeed - 180 - 360 ) / 40 );
	if(%changeSpeed <= 10)
	{
		%changeSpeed = 10;
	}
	
	%lockSpeed = %lockSpeed * 10;
	%dangerControl.parentGroup = "PlayGui";
	%dangerControl.setState( false );
	%dangerControl.setLockSpeed( %lockSpeed );
	%dangerControl.setChangeSpeed( %changeSpeed );
	%dangerControl.getFollowObj( %obj );
	%dangerControl.setSendObj( $PlayerP1 );
	%dangerControl.setTHeight( 0 );
	%dangerControl.setCanAdjustExtent( %adjustExtent );
	%dangerControl.setExtentWidth( %width );
	%dangerControl.setIsFlicker( true );
	%dangerControl.setNeedFlickerTime( 50 );
	%dangerControl.setIsLargerDegree( true );
	%dangerControl.setRootCtrl( PlayGui.getId( ) );
	%obj.alarmControl = %dangerControl;
}

function spawnDangerCtrlFlickFragment( %obj, %width, %adjustExtent, %isFragmentate, %fragmentateType )
{
	%obj = %obj.getId();
	
	if( !isObject( %obj ) )
		return;
	
	if( !isObject( $PlayerP1 ) )
		return;
		
	if( %isFragmentate )
	{
		sfxPlayOnce( bossQuanBao );
		spawnDangerCtrlFragmentOut(%obj, %width);
		return;
	}
		
	%ctrObj = %obj.alarmFlickFragmentControl;
	
	if( isObject( %ctrObj ) )
		return;
	
	%dangerControl = new guiDangerAlarmCtrl(  ) {
		bitmap = "art/gui/dangerObject/fragmentedFrame.png";
		wrap = "1";
		lockSpeed = "30";
		changeSpeed = "30";
		showPart = "1";
		reversal_X = "0";
		isContainer = "0";
		Profile = "GuiDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = "0 0";
		Extent = "8 8";
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		tooltipprofile = "GuiToolTipProfile";
		hovertime = "1000";
		canSaveDynamicFields = "0";
	};
   	MissionCleanup.add( %dangerControl );
	
	if( !isObject( %dangerControl ) )
		return;
		
	%dangerControl.parentGroup = "PlayGui";
	%dangerControl.setState( false );
	%dangerControl.setLockSpeed( 10 );
	%dangerControl.setChangeSpeed( 60 );
	%dangerControl.getFollowObj( %obj );
	%dangerControl.setSendObj( $PlayerP1 );
	%dangerControl.setTHeight( 0 );
	%dangerControl.setCanAdjustExtent( %adjustExtent );
	%dangerControl.setExtentWidth( %width );
	%dangerControl.setRootCtrl( PlayGui.getId( ) );
	%dangerControl.setIsNeedFlickerFollow( true );
	%dangerControl.setIsFlickerFollow( false );
	%dangerControl.setIsFragmentate( %isFragmentate );
	%dangerControl.setFragmentateType( %fragmentateType );
	%obj.alarmFlickFragmentControl = %dangerControl;
	
	if( %isFragmentate )
	{
		sfxPlayOnce( bossQuanBao );
		schedule( 5000, 0, deleteObj, %dangerControl );
	}
}

function spawnDangerCtrlFragmentOut(%followObj, %width)
{
	%spawnObj = new GuiBitmapDrawPartCtrl() {
		bitmap = "art/gui/dangerObject/fragmentedFrameSpecialTwo.png";
		wrap = "0";
		showPart = "0";
		reversal_X = "0";
		isContainer = "0";
		Profile = "GuiDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = "26 60";
		Extent = "21 34";
		MinExtent = "0 0";
		canSave = "1";
		Visible = "1";
		tooltipprofile = "GuiToolTipProfile";
		hovertime = "1000";
		canSaveDynamicFields = "0";
	};
   	MissionCleanup.add( %spawnObj );
	
	//%width = %width * 2;
	%width = 600;
	%extentX = %width;
	%extentY = %width;
	%pos = %followObj.getPosition();
	%pos = PlayGui.project( %pos );
	%posX = %pos.x - %extentX / 2;
	%posY = %pos.y - %extentY / 2;
	
	%spawnObj.setPosition(%posX, %posY);
	%spawnObj.setExtent(%extentX, %extentY);
	
	%spawnObj.parentGroup = "PlayGui";
	
	spawnDangerCtrlFragmentOutAfter(%spawnObj, 0);
	schedule( 5000, 0, deleteObj, %spawnObj );
}

function spawnDangerCtrlFragmentOutAfter(%spawnObj, %val)
{
	if(!isObject(%spawnObj))
	{
		return;
	}
		
	%spawnObj.setDrawPartOfBitmap( 1, 0, 0, 600 *  %val, 600, 600, 600 );
	%val++;
	if(%val < 9)
	{
		schedule(120, 0, spawnDangerCtrlFragmentOutAfter, %spawnObj, %val);
	}
	else
	{
		if(isObject(%spawnObj))
		{
			%spawnObj.delete();
		}
	}
}

function spawnFlickFragmentSpecialOne( %obj, %width, %adjustExtent )
{
	%obj = %obj.getId();
	
	if( !isObject( %obj ) )
		return;
	
	if( !isObject( $PlayerP1 ) )
		return;
	
	%ctrObj = %obj.SepcialOneControl;
	
	if( isObject( %ctrObj ) )
		return;
		
	%dangerControl = new guiDangerAlarmCtrl(  ) {
		bitmap = "art/gui/dangerObject/fragmentedFrameSpecialOne.png";
		wrap = "1";
		lockSpeed = "30";
		changeSpeed = "30";
		showPart = "1";
		reversal_X = "0";
		isContainer = "0";
		Profile = "GuiDefaultProfile";
		HorizSizing = "right";
		VertSizing = "bottom";
		position = "0 0";
		Extent = "8 8";
		MinExtent = "8 2";
		canSave = "1";
		Visible = "1";
		tooltipprofile = "GuiToolTipProfile";
		hovertime = "1000";
		canSaveDynamicFields = "0";
	};
   	MissionCleanup.add( %dangerControl );
	
	if( !isObject( %dangerControl ) )
		return;
		
	%dangerControl.parentGroup = "PlayGui";
	%dangerControl.setState( false );
	%dangerControl.setLockSpeed( 10 );
	%dangerControl.setChangeSpeed( 75 );
	%dangerControl.getFollowObj( %obj );
	%dangerControl.setSendObj( $PlayerP1 );
	%dangerControl.setTHeight( 0 );
	%dangerControl.setCanAdjustExtent( %adjustExtent );
	%dangerControl.setExtentWidth( %width );
	%dangerControl.setRootCtrl( PlayGui.getId( ) );
	%dangerControl.setIsNeedFlickerFollow( true );
	%dangerControl.setIsFlickerFollow( true );
	%obj.SepcialOneControl = %dangerControl;
}

function danliangguodu(%val)
{
	if( %val >= 48 )
	{
		danliang.setvisible( false );
		return;
	}
	if( %val == 0 )
	{
		danliang.setvisible( true );
		PlayGuiBlackBackGround.setVisible( false );
	}

	danliang.setDrawPartOfBitmap(1, 0, 0, %val*16, 0, 16, 16);
	schedule( 300, 0, danliangguodu, %val++ );
}

function deleteObj( %obj )
{
	if( !isObject( %obj ) )
		return;
	%obj.delete( );
}
