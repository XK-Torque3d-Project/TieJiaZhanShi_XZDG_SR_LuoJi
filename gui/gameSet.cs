// ============================================================
// Project            :  game
// File               :  .\scripts\client\gamesetGui.cs
// Copyright          :  
// Author             :  diantianen
// Created on         :  2010年10月15日 16:18
//
// Editor             :  Codeweaver v. 1.2.2594.2497
//
// Description        :  
//                    :  
//                    :  
// ============================================================
$BtnIndex = 0;
$TextIndex = 0;
$HardIndex = 2;
$ModeIndex = 2;
$SafeBeltIndex = 1;	//1-open; 2-close	beltSelect1	beltSelect2
$ShakeIndexP1 = 0;
$ShakeIndexP2 = 0;
$ZhuandongIndexP1 = 0;
$ZhuandongIndexP2 = 0;
$HurtShakeIndex = 1;	//1-50;2-100;3-150;----;10-500
$HurtShakeValue = 50;
$CoordIndex = 1;

$TimeIndex = 0;
$ChangeItem = true;
$EnterGameset = false;

$PosLeftTopX = 659;
$PosLeftTopY = 652;
$PosRightTopX = 125;
$PosRightTopY = 652;
$PosRightBottomX = 125;
$PosRightBottomY = 277;
$PosLeftBottomX = 657;
$PosLeftBottomY = 277;

$PosLeftTopXP2 = 659;
$PosLeftTopYP2 = 652;
$PosRightTopXP2 = 125;
$PosRightTopYP2 = 652;
$PosRightBottomXP2 = 125;
$PosRightBottomYP2 = 277;
$PosLeftBottomXP2 = 657;
$PosLeftBottomYP2 = 277;

$STime = "";

//$CurrentCoinNum = 10;   	//目前游戏币数量
$RequestCoinNum = 2;	//目前要求游戏币数量

$initializeResult = false;
$urgentStopRotate = false;
$gameEndNow = false;

function gamesetGui::onWake(%this)
{
	$ChangeItem = true;
	gamesetCrossP1.setVisible(false);
	gamesetCrossP2.setVisible(false);
	//%test = $PlayerP1.GetMaxDamage();
	
	%TextIndex = GetIniValueNum("Record", "CoinCount", "./Detail.hnb");
	%HardIndex = GetIniValueNum("Record", "HardIndex", "./Detail.hnb");
	%ModeIndex = GetIniValueNum("Record", "ModeIndex", "./Detail.hnb");
	%SafeBeltIndex = GetIniValueNum("Record", "SafeBeltIndex", "./Detail.hnb");
	%ShakeIndexP1 = GetIniValueNum("Record", "ShakeIndexP1", "./Detail.hnb");
	%ShakeIndexP2 = GetIniValueNum("Record", "ShakeIndexP2", "./Detail.hnb");
	%ZhuandongIndexP1 = GetIniValueNum("Record", "ZhuandongIndexP1", "./Detail.hnb");
	%ZhuandongIndexP2 = GetIniValueNum("Record", "ZhuandongIndexP2", "./Detail.hnb");
	%HurtShakeIndex = GetIniValueNum("Record", "HurtShakeIndex", "./Detail.hnb");
	%CoordIndex = GetIniValueNum("Position", "CoordIndex", "./Pos.hnb");	
	
	%PosLeftTopX = GetIniValueNum("Position", "PosLeftTopX", "./Pos.hnb");
	%PosLeftTopY = GetIniValueNum("Position", "PosLeftTopY", "./Pos.hnb");
	%PosRightTopX = GetIniValueNum("Position", "PosRightTopX", "./Pos.hnb");
	%PosRightTopY = GetIniValueNum("Position", "PosRightTopY", "./Pos.hnb");
	%PosRightBottomX = GetIniValueNum("Position", "PosRightBottomX", "./Pos.hnb");
	%PosRightBottomY = GetIniValueNum("Position", "PosRightBottomY", "./Pos.hnb");
	%PosLeftBottomX = GetIniValueNum("Position", "PosLeftBottomX", "./Pos.hnb");
	%PosLeftBottomY = GetIniValueNum("Position", "PosLeftBottomY", "./Pos.hnb");
	
	%PosLeftTopXP2 = GetIniValueNum("Position", "PosLeftTopXP2", "./Pos.hnb");
	%PosLeftTopYP2 = GetIniValueNum("Position", "PosLeftTopYP2", "./Pos.hnb");
	%PosRightTopXP2 = GetIniValueNum("Position", "PosRightTopXP2", "./Pos.hnb");
	%PosRightTopYP2 = GetIniValueNum("Position", "PosRightTopYP2", "./Pos.hnb");
	%PosRightBottomXP2 = GetIniValueNum("Position", "PosRightBottomXP2", "./Pos.hnb");
	%PosRightBottomYP2 = GetIniValueNum("Position", "PosRightBottomYP2", "./Pos.hnb");
	%PosLeftBottomXP2 = GetIniValueNum("Position", "PosLeftBottomXP2", "./Pos.hnb");
	%PosLeftBottomYP2 = GetIniValueNum("Position", "PosLeftBottomYP2", "./Pos.hnb");
	
	if( %TextIndex > 0 )
	{
		$TextIndex = %TextIndex;
	}
	else
	{
		$TextIndex = $RequestCoinNum;
	}
	
	if( %HardIndex != -1 )
		$HardIndex = %HardIndex;
	
	if( %ModeIndex != -1 )
		$ModeIndex = %ModeIndex;
	
	if( %SafeBeltIndex != -1 )
		$SafeBeltIndex = %SafeBeltIndex;
		
	if( %ShakeIndexP1 != -1 )
		$ShakeIndexP1 = %ShakeIndexP1;
		
	if( %ShakeIndexP2 != -1 )
		$ShakeIndexP2 = %ShakeIndexP2;
		
	if( %ZhuandongIndexP1 != -1 )
		$ZhuandongIndexP1 = %ZhuandongIndexP1;
		
	if( %ZhuandongIndexP2 != -1 )
		$ZhuandongIndexP2 = %ZhuandongIndexP2;
		
	if( %HurtShakeIndex != -1 )
		$HurtShakeIndex = %HurtShakeIndex;
		
	$HurtShakeValue = $HurtShakeIndex * 50;
	
	if( %CoordIndex != -1 )
		$CoordIndex = %CoordIndex;
		
	if( %PosLeftTopX != -1 )
		$PosLeftTopX = %PosLeftTopX;
		
	if( %PosLeftTopY != -1 )
		$PosLeftTopY = %PosLeftTopY;
		
	if( %PosRightTopX != -1 )
		$PosRightTopX = %PosRightTopX;
		
	if( %PosRightTopY != -1 )
		$PosRightTopY = %PosRightTopY;
		
	if( %PosRightBottomX != -1 )
		$PosRightBottomX = %PosRightBottomX;
		
	if( %PosRightBottomY != -1 )
		$PosRightBottomY = %PosRightBottomY;
		
	if( %PosLeftBottomX != -1 )
		$PosLeftBottomX = %PosLeftBottomX;
		
	if( %PosLeftBottomY != -1 )
		$PosLeftBottomY = %PosLeftBottomY;
	//p2	
	if( %PosLeftTopXP2 != -1 )
		$PosLeftTopXP2 = %PosLeftTopXP2;
		
	if( %PosLeftTopYP2 != -1 )
		$PosLeftTopYP2 = %PosLeftTopYP2;
		
	if( %PosRightTopXP2 != -1 )
		$PosRightTopXP2 = %PosRightTopXP2;
		
	if( %PosRightTopYP2 != -1 )
		$PosRightTopYP2 = %PosRightTopYP2;
		
	if( %PosRightBottomXP2 != -1 )
		$PosRightBottomXP2 = %PosRightBottomXP2;
		
	if( %PosRightBottomYP2 != -1 )
		$PosRightBottomYP2 = %PosRightBottomYP2;
		
	if( %PosLeftBottomXP2 != -1 )
		$PosLeftBottomXP2 = %PosLeftBottomXP2;
		
	if( %PosLeftBottomYP2 != -1 )
		$PosLeftBottomYP2 = %PosLeftBottomYP2;
	
	$TimeIndex = 0;
	
	Btn0.setProfile(GuiMenuButtonProfile);
	
	%HardIndex = "Hard" @ $HardIndex;
	%HardIndex.setStateOn(1);
	%ModeIndex = "Mode" @ $ModeIndex;
	%ModeIndex.setStateOn(1);
	%SafeBeltIndex = "beltSelect" @ $SafeBeltIndex;
	%SafeBeltIndex.setStateOn(1);
	$Time = getCurrentTime();
	$STime = getWord($Time, 0);
	TodayText.setText($STime);
	setGunShakeStateGSet(0, false);
	setGunShakeLevel(1, $ShakeIndexP1);
	setGunShakeLevel(2, $ShakeIndexP2);
	setGunZhuandongLevel(1, $ZhuandongIndexP1);
	setGunZhuandongLevel(2, $ZhuandongIndexP2);

 	$BeforeTime = GetIniValueNum("Record", "BeforeTime", "./Detail.hnb");
	TextCoin.setText($TextIndex);
	shakeLevelP1.setText($ShakeIndexP1);
	shakeLevelP2.setText($ShakeIndexP2);
	doudongLevel.setText($HurtShakeIndex);
	zhuandongLevelP1.setText($ZhuandongIndexP1);
	zhuandongLevelP2.setText($ZhuandongIndexP2);
    JiTaiZhuanSu.setText($XuanZhuanDengJi);
}

function gamesetGui::onSleep(%this)
{
	setGunShakeStateGSet(0, false);
}

//Exit the gameSetGui, record the information
function Btn6::onAction(%this)
{
	$BeforeTime1 = GetCurrentSecond();
	$RunDays = mfloor(($BeforeTime1 - $BeforeTime)/24/60/60);
	
	$RequestCoinNum = $TextIndex;
    WriteIniValueString("Record", "XuanZhuanDengJi", $XuanZhuanDengJi, "./Detail.hnb");
	WriteIniValueString("Record", "CoinCount", $TextIndex, "./Detail.hnb");
	WriteIniValueString("Record", "HardIndex", $HardIndex, "./Detail.hnb");
	WriteIniValueString("Record", "ModeIndex", $ModeIndex, "./Detail.hnb");
	WriteIniValueString("Record", "SafeBeltIndex", $SafeBeltIndex, "./Detail.hnb");
	WriteIniValueString("Record", "ShakeIndexP1", $ShakeIndexP1, "./Detail.hnb");
	WriteIniValueString("Record", "ShakeIndexP2", $ShakeIndexP2, "./Detail.hnb");
	WriteIniValueString("Record", "ZhuandongIndexP1", $ZhuandongIndexP1, "./Detail.hnb");
	WriteIniValueString("Record", "ZhuandongIndexP2", $ZhuandongIndexP2, "./Detail.hnb");
	WriteIniValueString("Record", "HurtShakeIndex", $HurtShakeIndex, "./Detail.hnb");
	WriteIniValueString("Record", "BeforeTime1",$BeforeTime1, "./Detail.hnb");
	
	$RecordCounts++;
	%Section = "Record" @ $RecordCounts;
	WriteIniValueString(%Section, "CurrentTime",  $STime, "./Detail.hnb");
	WriteIniValueString(%Section, "CoinCount",    $TextIndex, "./Detail.hnb");
	WriteIniValueString(%Section, "HardIndex",    $HardIndex, "./Detail.hnb");
	WriteIniValueString(%Section, "ModeIndex",    $ModeIndex, "./Detail.hnb");
	WriteIniValueString(%Section, "SafeBeltIndex",    $SafeBeltIndex, "./Detail.hnb");
	WriteIniValueString(%Section, "ShakeIndexP1",   $ShakeIndexP1, "./Detail.hnb");
	WriteIniValueString(%Section, "ShakeIndexP2",   $ShakeIndexP2, "./Detail.hnb");
	WriteIniValueString(%Section, "ZhuandongIndexP1",   $ZhuandongIndexP1, "./Detail.hnb");
	WriteIniValueString(%Section, "ZhuandongIndexP2",   $ZhuandongIndexP2, "./Detail.hnb");
	WriteIniValueString(%Section, "HurtShakeIndex",   $HurtShakeIndex, "./Detail.hnb");
	WriteIniValueString(%Section, "RunDays",      $RunDays,   "./Detail.hnb");
	WriteIniValueString("RecordCounts", "Counts", $RecordCounts, "./Detail.hnb");
	
	WriteIniValueString("Position", "PosLeftTopX",     $PosLeftTopX, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftTopY",     $PosLeftTopY, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightTopX",    $PosRightTopX, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightTopY",    $PosRightTopY, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightBottomX", $PosRightBottomX, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightBottomY", $PosRightBottomY, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftBottomX",  $PosLeftBottomX, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftBottomY",  $PosLeftBottomY, "./Pos.hnb");
	WriteIniValueString("Position", "CoordIndex",      1, "./Pos.hnb");
	
	WriteIniValueString("Position", "PosLeftTopXP2",     $PosLeftTopXP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftTopYP2",     $PosLeftTopYP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightTopXP2",    $PosRightTopXP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightTopYP2",    $PosRightTopYP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightBottomXP2", $PosRightBottomXP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosRightBottomYP2", $PosRightBottomYP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftBottomXP2",  $PosLeftBottomXP2, "./Pos.hnb");
	WriteIniValueString("Position", "PosLeftBottomYP2",  $PosLeftBottomYP2, "./Pos.hnb");
	
	if ($HurtShakeValue < 50)
	{
		$HurtShakeValue = 50;
	}
	
	PCVRResetBasePoint();
	PCVRResetBasePointP2();
	Btn6.setProfile(GuiMenuBarProfile);
}

//adjust, position
function getCoordAndSet()
{
	%pos = PCVRGetHardWareCoord();
	if( $CoordIndex == 1 )
	{		
		$PosLeftTopX = %pos.x;
		$PosLeftBottomX = %pos.x;
		WriteIniValueString("Position", "PosLeftTopX",     $PosLeftTopX, "./Pos.hnb");
		WriteIniValueString("Position", "PosLeftBottomX",  $PosLeftBottomX, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN2");
		PCVRResetBasePoint();
		return;
	}
	if( $CoordIndex == 2 )
	{		
		$PosRightTopX = %pos.x;
		$PosRightBottomX = %pos.x;
		WriteIniValueString("Position", "PosRightTopX",    $PosRightTopX, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightBottomX", $PosRightBottomX, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN3");
		PCVRResetBasePoint();
		return;
	}
	if( $CoordIndex == 3 )
	{		
		$PosLeftBottomY = %pos.y;
		$PosRightBottomY = %pos.y;
		WriteIniValueString("Position", "PosLeftBottomY",  $PosLeftBottomY, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightBottomY", $PosRightBottomY, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN4");
		PCVRResetBasePoint();
		return;
	}
	if( $CoordIndex == 4 )
	{		
		$PosLeftTopY = %pos.y;
		$PosRightTopY = %pos.y;
		WriteIniValueString("Position", "PosLeftTopY",     $PosLeftTopY, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightTopY",    $PosRightTopY, "./Pos.hnb");
		$CoordIndex++;
		PCVRResetBasePoint();
		
		if( $CoordIndex == 5 )
		{
			$CoordIndex = 1;
			adjustBitmap.setVisible( false );
			adjustBitmap.bitmap = "";
			$ChangeItem = !$ChangeItem;
		}
		return;
	}
}

function getCoordAndSetP2()
{
	%pos = PCVRGetHardWareCoordP2();
	if( $CoordIndex == 1 )
	{		
		$PosLeftTopXP2 = %pos.x;
		$PosLeftBottomXP2 = %pos.x;
		WriteIniValueString("Position", "PosLeftTopXP2",     $PosLeftTopXP2, "./Pos.hnb");
		WriteIniValueString("Position", "PosLeftBottomXP2",  $PosLeftBottomXP2, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN2");
		PCVRResetBasePointP2();
		return;
	}
	if( $CoordIndex == 2 )
	{		
		$PosRightTopXP2 = %pos.x;
		$PosRightBottomXP2 = %pos.x;
		WriteIniValueString("Position", "PosRightTopXP2",    $PosRightTopXP2, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightBottomXP2", $PosRightBottomXP2, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN3");
		PCVRResetBasePointP2();
		return;
	}
	if( $CoordIndex == 3 )
	{		
		$PosLeftBottomYP2 = %pos.y;
		$PosRightBottomYP2 = %pos.y;
		WriteIniValueString("Position", "PosLeftBottomYP2",  $PosLeftBottomYP2, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightBottomYP2", $PosRightBottomYP2, "./Pos.hnb");
		$CoordIndex++;
		adjustBitmap.setBitmap("scripts/gui/GUN4");
		PCVRResetBasePointP2();
		return;
	}
	if( $CoordIndex == 4 )
	{		
		$PosLeftTopYP2 = %pos.y;
		$PosRightTopYP2 = %pos.y;
		WriteIniValueString("Position", "PosLeftTopYP2",     $PosLeftTopYP2, "./Pos.hnb");
		WriteIniValueString("Position", "PosRightTopYP2",    $PosRightTopYP2, "./Pos.hnb");
		$CoordIndex++;
		PCVRResetBasePointP2();
		
		if( $CoordIndex == 5 )
		{
			$CoordIndex = 1;
			adjustBitmap.setVisible( false );
			adjustBitmap.bitmap = "";
			$ChangeItem = !$ChangeItem;
		}
		return;
	}
}

//enter the gamesetgui or select one itme
function GamesetEnter(%make)
{
   if(%make)
   {
		if(!$EnterGameset)
		{
			//open the gameSetgui
			if( !touBiWindow.isAwake() )
			{
				return;
			}
			
			if( (theoraPlayCtrl.isPlaying() || paiHangBackGround.isVisible()) && isObject(gameSetGui) )
			{
				$EnterGameset = true;
				$BtnIndex = 0;
				
				theoraPlayCtrl.stop();
				paiHangBackGround.setVisible( false );
				theoraPlayCtrl.setVisible( false );
				touBiFadeInWhite.setVisible( false );
				
				Canvas.schedule( 1000, setContent, "gameSetGui" );
				return;
			}
		}
		else if( $BtnIndex == 4 )
		{
			gamesetCrossP1.setVisible(true);
			//adjust p1
			if( !adjustBitmap.isVisible( ) || adjustBitmap.bitmap $= "" )
			{
				$CoordIndex = 1;
				adjustBitmap.setBitmap("scripts/gui/GUN1");
				adjustBitmap.setVisible( true );
			}
			else if( adjustBitmap.isVisible( ) && adjustBitmap.bitmap !$= "" )
			{
				return;
			}
		}
		else if( $BtnIndex == 5 )
		{
			gamesetCrossP1.setVisible(false);
			gamesetCrossP2.setVisible(true);
			//adjust p2
			if( !adjustBitmap.isVisible( ) || adjustBitmap.bitmap $= "" )
			{
				$CoordIndex = 1;
				adjustBitmap.setBitmap("scripts/gui/GUN1");
				adjustBitmap.setVisible( true );
			}
			else if( adjustBitmap.isVisible( ) && adjustBitmap.bitmap !$= "" )
			{
				return;
			}
		}
		else if ($BtnIndex == 6 && $ChangeItem)
		{
			gamesetCrossP1.setVisible(false);
			gamesetCrossP2.setVisible(false);
			setGunShakeStateGSet(1, true);
		}
		else if ($BtnIndex == 7 && $ChangeItem)
		{
			setGunShakeStateGSet(2, true);
			setGunShakeStateGSet(1, false);
		}
		else if ($BtnIndex == 8 && $ChangeItem)
		{
			setGunShakeStateGSet(1, true);
			setGunShakeStateGSet(2, false);
		}
		else if ($BtnIndex == 9 && $ChangeItem)
		{
			setGunShakeStateGSet(2, true);
			setGunShakeStateGSet(1, false);
		}
		else if ($BtnIndex == 10 && !$ChangeItem)
		{
			setGunShakeStateGSet(1, false);
			setGunShakeStateGSet(2, false);
		}
		else if( $BtnIndex == 12 )
		{
			//clear infor
			$TextIndex = 3;
			$RequestCoinNum = $TextIndex;
			TextCoin.setText($TextIndex);
            $XuanZhuanDengJi = 5;
            JiTaiZhuanSu.setText($XuanZhuanDengJi);
            WriteIniValueString("Record", "XuanZhuanDengJi", $XuanZhuanDengJi, "./Detail.hnb");
			
			$HardIndex = 2;
			%RadioName = "Hard" @ $HardIndex;
			%RadioName.setStateOn(1);
			
			$ModeIndex = 2;
			%RadioMode = "Mode" @ $ModeIndex;
			%RadioMode.setStateOn(1);
			
			$SafeBeltIndex = 1;
			%RadioMode = "beltSelect" @ $SafeBeltIndex;
			%RadioMode.setStateOn(1);
			
			$ShakeIndexP1 = 3;
			setGunShakeLevel(1, $ShakeIndexP1);
			shakeLevelP1.setText($ShakeIndexP1);
			
			$ShakeIndexP2 = 3;
			setGunShakeLevel(2, $ShakeIndexP2);
			shakeLevelP2.setText($ShakeIndexP2);
			
			$ZhuandongIndexP1 = 3;
			setGunZhuandongLevel(1, $ZhuandongIndexP1);
			zhuandongLevelP1.setText($ZhuandongIndexP1);
			
			$ZhuandongIndexP2 = 3;
			setGunZhuandongLevel(2, $ZhuandongIndexP2);
			zhuandongLevelP2.setText($ZhuandongIndexP2);
			
			$HurtShakeIndex = 4;
			$HurtShakeValue = $HurtShakeIndex * 50;
			doudongLevel.setText($HurtShakeIndex);
			
			$ChangeItem = !$ChangeItem;
		}
		else if( $BtnIndex == 13 )
		{
			//exit
			Btn6.onAction();
			Canvas.setContent( touBiWindow );
			$EnterGameset = false;
		}
		
		//other only change the flag value
		$ChangeItem = !$ChangeItem;
   }
}

//move down, sub or move to right
function GamesetMove(%val)
{
	if (!$EnterGameset)
	{
		setIsDisplayRender();
		return;
	}
	
	if( adjustBitmap.isVisible( ) )
		return;
	
	if( %val)
	{
		if($ChangeItem)
		{
			$BtnIndex++;
			
			if( $BtnIndex > 13 )
			{
				$BtnIndex = 0;
			}
			
			switch($BtnIndex)
			{
				case 0:
					Btn6.setProfile(GuiMenuBarProfile);
					Btn0.setProfile(GuiMenuButtonProfile);
				case 1:
					Btn0.setProfile(GuiMenuBarProfile);
					Btn1.setProfile(GuiMenuButtonProfile);
				case 2:
					Btn1.setProfile(GuiMenuBarProfile);
					Btn2.setProfile(GuiMenuButtonProfile);
				case 3:
					Btn2.setProfile(GuiMenuBarProfile);
					Btn7.setProfile(GuiMenuButtonProfile);
					adjustInfor.setVisible( false );
				case 4:
					Btn7.setProfile(GuiMenuBarProfile);
					Btn3.setProfile(GuiMenuButtonProfile);
					adjustInfor.setVisible( true );
				case 5:
					Btn3.setProfile(GuiMenuBarProfile);
					Btn31.setProfile(GuiMenuButtonProfile);
					adjustInfor.setVisible( false );
					adjustInforP2.setVisible( true );
				case 6:
					Btn31.setProfile(GuiMenuBarProfile);
					Btn4.setProfile(GuiMenuButtonProfile);
					adjustInforP2.setVisible( false );
					gamesetCrossP1.setVisible(false);
					gamesetCrossP2.setVisible(false);
					setGunShakeStateGSet(1, true);
					setGunShakeStateGSet(2, false);
				case 7:
					Btn4.setProfile(GuiMenuBarProfile);
					Btn41.setProfile(GuiMenuButtonProfile);
					setGunShakeStateGSet(1, false);
					setGunShakeStateGSet(2, true);
				case 8:
					Btn41.setProfile(GuiMenuBarProfile);
					Btn42.setProfile(GuiMenuButtonProfile);
					setGunShakeStateGSet(1, true);
					setGunShakeStateGSet(2, false);
				case 9:
					Btn42.setProfile(GuiMenuBarProfile);
					Btn43.setProfile(GuiMenuButtonProfile);
					setGunShakeStateGSet(1, false);
					setGunShakeStateGSet(2, true);
				case 10:
					Btn43.setProfile(GuiMenuBarProfile);
					Btn8.setProfile(GuiMenuButtonProfile);
					setGunShakeStateGSet(1, false);
					setGunShakeStateGSet(2, false);
				case 11:
					Btn8.setProfile(GuiMenuBarProfile);
					Btn81.setProfile(GuiMenuButtonProfile);
				case 12:
					Btn81.setProfile(GuiMenuBarProfile);
					Btn5.setProfile(GuiMenuButtonProfile);
				case 13:
					Btn5.setProfile(GuiMenuBarProfile);
					Btn6.setProfile(GuiMenuButtonProfile);
			}
			return;
		}
		else
		{
			switch($BtnIndex)
			{
				case 0:
				//coin
					$TextIndex++;
					if($TextIndex > 20)
					{
						$TextIndex = 1;
					}
					TextCoin.setValue($TextIndex);
				case 1:
				//hard mode
					$HardIndex++;
					if($HardIndex > 3)
					{
						$HardIndex = 1;
					}
					%RadioName = "Hard" @ $HardIndex;
					%RadioName.setStateOn(1);
				case 2:
				//game mode
					$ModeIndex++;
					if($ModeIndex > 2)
					{
						$ModeIndex = 1;
					}
					%RadioMode = "Mode" @ $ModeIndex;
					%RadioMode.setStateOn(1);
				case 3:
				//safe belt
					$SafeBeltIndex++;
					if($SafeBeltIndex > 2)
					{
						$SafeBeltIndex = 1;
					}
					%RadioMode = "beltSelect" @ $SafeBeltIndex;
					%RadioMode.setStateOn(1);
				case 4:
				//adjust p1
				case 5:
				//adjust p2
				case 6:
				//shake level p1
					$ShakeIndexP1++;
					if($ShakeIndexP1 > 10)
					{
						$ShakeIndexP1 = 0;
					}
					
					setGunShakeLevel(1, $ShakeIndexP1);
					shakeLevelP1.setText($ShakeIndexP1);
					
					setGunShakeStateGSet(1, true);
				case 7:
				//shake level p2
					$ShakeIndexP2++;
					if($ShakeIndexP2 > 10)
					{
						$ShakeIndexP2 = 0;
					}
					
					setGunShakeLevel(2, $ShakeIndexP2);
					shakeLevelP2.setText($ShakeIndexP2);
					
					setGunShakeStateGSet(2, true);
				case 8:
				//zhuandong level p1
					$ZhuandongIndexP1++;
					if($ZhuandongIndexP1 > 10)
					{
						$ZhuandongIndexP1 = 0;
					}
					
					setGunZhuandongLevel(1, $ZhuandongIndexP1);
					zhuandongLevelP1.setText($ZhuandongIndexP1);
					
					setGunShakeStateGSet(1, true);
				case 9:
				//zhuandong level p2
					$ZhuandongIndexP2++;
					if($ZhuandongIndexP2 > 10)
					{
						$ZhuandongIndexP2 = 0;
					}
					
					setGunZhuandongLevel(2, $ZhuandongIndexP2);
					zhuandongLevelP2.setText($ZhuandongIndexP2);
					
					setGunShakeStateGSet(2, true);
				case 10:
				setGunShakeStateGSet(1, false);
				setGunShakeStateGSet(2, false);
					//shake
					$HurtShakeIndex++;
					if($HurtShakeIndex > 10)
					{
						$HurtShakeIndex = 1;
					}
					
					$HurtShakeValue = $HurtShakeIndex * 50;
					
					doudongLevel.setText($HurtShakeIndex);
				case 11:
                    $XuanZhuanDengJi++;
                    if ($XuanZhuanDengJi > 10) {
                        $XuanZhuanDengJi = 0;
                    }
                    JiTaiZhuanSu.setText($XuanZhuanDengJi);
                    WriteIniValueString("Record", "XuanZhuanDengJi", $XuanZhuanDengJi, "./Detail.hnb");
                case 12:
					//clear
				case 13:
					//exit
			}
			
			return;
		}
	}
}

function setGunShakeStateGSet(%index, %flag)
{echo("setGunShakeStateGSet====================================================", %index, " " , %flag);
	if (%index == 0)
	{//two player
		if (!%flag)
		{
			//false
			PCVRSetShakeStateP2(0, %flag);
			return;
		}
		
		if (%flag)
		{
			//true
			PCVRSetShakeStateP2(1, %flag);
		}
		
		if (%flag)
		{
			//true
			PCVRSetShakeStateP2(2, %flag);
		}
	}
	else if (%index == 1)
	{//player1
		if (%flag)
		{
			//true
			PCVRSetShakeStateP2(1, %flag);
		}
		else
		{
			PCVRSetShakeStateP2(1, false);
		}
	}
	else if (%index == 2)
	{//player2
		if (%flag)
		{
			//true
			PCVRSetShakeStateP2(2, %flag);
		}
		else
		{
			PCVRSetShakeStateP2(2, false);
		}
	}
}