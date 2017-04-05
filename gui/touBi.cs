// ============================================================
// Project            :  game
// File               :  .\scripts\server\touBi.cs
// Copyright          :  
// Author             :  Administrator
// Created on         :  2010年8月16日 星期一 10:28
//
// Editor             :  Codeweaver v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================
if( isObject( touBiMoveMap ) )
	touBiMoveMap.delete();
new ActionMap(touBiMoveMap);

touBiMoveMap.bind( keyboard, l, touBiGuiInsertCoinNum );
touBiMoveMap.bind( keyboard, p, touBiGuiStartGame );
touBiMoveMap.bind( keyboard, s, enterMenu );
$PlayerStartGame = false;
$PlayerStartGameP2 = false;
$PlayerCanStartGame = false;
$PlayerCanStartGameP2 = false;
$ShowPleaseInsertCoin = true;
$PleaseInsertCoinCount = 0;
$CANTStart = false;

function PleaseInsertCoin(%this)
{
	$PleaseInsertCoinCount++;
	if($ShowPleaseInsertCoin == true)
	{
		if($PleaseInsertCoinCount == 4)
		{
			CInsert.setVisible(false);
			Please.setVisible(true);
		}
		if($PleaseInsertCoinCount == 8)
		{
			Please.setVisible(false);
			CInsert.setVisible(true);
		}
		if($PleaseInsertCoinCount == 20)
		{
			Please.setVisible(false);
			CInsert.setVisible(false);
		}
		if($PleaseInsertCoinCount ==25)
		{
			$PleaseInsertCoinCount = 0;
		}
	}
	else
		return;
	schedule(150, 0, PleaseInsertCoin, 0);
		return;
}

function touBiWindow::onWake( %this )
{echo("touBiWindow::touBiWindow::touBiWindow::touBiWindow::onWakeMainMenuGui::onWake");
	$gameEndNow = false;
	PCVRSetTwoplayer(true);
	if ($initializeResult)
	{
		$urgentStopRotate = false;
		urgentShow.setBitmap("art/gui/Rotation/open.png");
	}
	
	$PlayerP1.setDamageLevel(0);
	PCVREnablePcvrProcess(true);
    $ShowPleaseInsertCoin = true;
    Please.setVisible(false);
	CInsert.setVisible(false);
	PleaseInsertCoin(0);

	paiHangBackGround.setVisible( true );
	touBiXieGang.setVisible( true );
	%resNum1 = mFloor( $RequestCoinNum / 10 );
	%resNum2 = $RequestCoinNum % 10;
	%num2 = mFloor( $CurrentCoinNum /10 ) % 10;
	%num3 = $CurrentCoinNum % 10;
	insertNum1.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
	insertNum2.setDrawPartOfBitmap(1, 0, 0, %num3*16, 0, 16, 21 );
	requestNum1.setDrawPartOfBitmap(1, 0, 0, %resNum1*16, 0, %resNum1*16+16, 21 );
	requestNum2.setDrawPartOfBitmap(1, 0, 0, %resNum2*16, 0, %resNum2*16+16, 21 );
	
	touBiXieGangP2.setVisible( true );
	%num2P2 = mFloor( $CurrentCoinNumP2 /10 ) % 10;
	%num3P2 = $CurrentCoinNumP2 % 10;
	insertNum1P2.setDrawPartOfBitmap(1, 0, 0, %num2P2*16, 0, 16, 21 );
	insertNum2P2.setDrawPartOfBitmap(1, 0, 0, %num3P2*16, 0, 16, 21 );
	requestNum1P2.setDrawPartOfBitmap(1, 0, 0, %resNum1*16, 0, %resNum1*16+16, 21 );
	requestNum2P2.setDrawPartOfBitmap(1, 0, 0, %resNum2*16, 0, %resNum2*16+16, 21 );
	
	touBiFadeInWhite.setDrawPartOfBitmap(1, 0, 0, 47*16, 0, 16, 16);
	theoraPlayCtrl.setVisible( true );
	theoraPlayCtrl.play();
	%this.schedule( 3000, isPlayBackDone );
	playGuiContainer.setVisible( true );
	PlayGuiBlackBackGround.setVisible( true );
	showGameOver( false );
	GroundFadeInBlack.setVisible( false );//主界面淡黑不可见
	
	if($showOtherInofr)
	{
		touBiMoveMap.schedule(1500, push );
	}
	
	schedule(3000, 0, touBiWindowEnableSet );
	
	$BossYiGuanDead = false;
	$BossErGuanDead = false;
	$ZongBossDead = false;
	
	$GameOver = true;
	
	$player1State = -1;
	$player2State = -1;
	$damagePercent = 1;
	$attackPercent = 1;
	
	playerCross.setVisible(false);
	playerCrossP2.setVisible(false);
	
	schedule(1000, 0, flashLightTest);
	return;
}

function touBiWindowEnableSet()
{
	$PlayerCanStartGame = true;
	$PlayerCanStartGameP2 = true;
}

function touBiWindow::onSleep( %this )
{
    $ShowPleaseInsertCoin = false;
	$PlayerCanStartGame = false;
	$PlayerCanStartGameP2 = false;
	
	touBiMoveMap.pop();
	insertCoin.setVisible( false );
	startGame.setVisible( false );
	anquandai1.setVisible( false );
	anquandai1P2.setVisible( false );
	return;
}

function touBiWindow::flashInsertCoin( %this )
{
	if( !theoraPlayCtrl.isPlaying() )
	{
		if( insertCoin.isVisible() )
		{
			insertCoin.setVisible( false );
		}
		else
		{
			insertCoin.setVisible( true );
		}
		if( $CurrentCoinNum >= $RequestCoinNum || $CurrentCoinNumP2 >= $RequestCoinNum )
		{
			insertCoin.setVisible( false );
			%this.flashStartGame();
			if ($SafeBeltIndex == 2)
			{
				anquandai1.setVisible( false );
				anquandai1P2.setVisible( false );
			}
			else
				xiAnquandai();
			return;
		}
	}
	else
		return;
	%this.schedule( 500, flashInsertCoin );
	return;
}

function touBiWindow::flashStartGame( %this )
{
	if( startGame.isVisible() )
	{
		startGame.setVisible( false );
	}
	else
	{
		startGame.setVisible( true );
	}
	if( $PlayerStartGame || theoraPlayCtrl.isPlaying() )
	{
		startGame.setVisible( false );
		return;
	}
	else
	{
		%this.schedule( 500, flashStartGame );
	}
	return;
}

function touBiWindow::isPlayBackDone( %this )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
	{
		return;
	}
	if( theoraPlayCtrl.isPlayBackDone() && theoraPlayCtrl.isVisible() && !theoraPlayCtrl.isPlaying() )
	{
		theoraPlayCtrl.setVisible( false );
		%this.hidePaiHang( true );
		%this.fadeInBackWhite(47);
		return;
	}
	if( !theoraPlayCtrl.isVisible() )
	{
		return;
	}
	%this.schedule( 1000, isPlayBackDone );
	return;
}

function touBiWindow::setPaiHangToSide( %this, %index )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
	{
		return;
	}
	%this.setPHScoreAndName( %index );
	
	%pos1 = paiHang1.getPosition();
	paiHang1.setPosition( 1360, %pos1.y );
	%pos2 = paiHang2.getPosition();
	paiHang2.setPosition( 1360, %pos2.y );
	%pos3 = paiHang3.getPosition();
	paiHang3.setPosition( 1360, %pos3.y );
	%pos4 = paiHang4.getPosition();
	paiHang4.setPosition( 1360, %pos4.y );
	%pos5 = paiHang5.getPosition();
	paiHang5.setPosition( 1360, %pos5.y );
	
	%this.hidePaiHang( false );
	
	%this.schedule( 10, paiHangFly, %pos1, %pos2, %pos3, %pos4, %pos5, %index );
	return;
}

function touBiWindow::paiHangFly( %this, %pos1, %pos2, %pos3, %pos4, %pos5, %index )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
	{
		return;
	}
	%addX1 = ( 1360 - %pos1.x ) / 10;
	%addX2 = ( 1360 - %pos2.x ) / 10;
	%addX3 = ( 1360 - %pos3.x ) / 10;
	%addX4 = ( 1360 - %pos4.x ) / 10;
	%addX5 = ( 1360 - %pos5.x ) / 10;
	if( %pos1.x != paiHang1.getPosition().x )
	{
		if( paiHang1.getPosition().x - %pos1.x >= %addX1 )
		{
			%this.movePaiHang( %addX1, paiHang1 );
		}
		else
		{
			paiHang1.setPosition( %pos1.x, %pos1.y );
		}
	}
	else if( %pos2.x != paiHang2.getPosition().x )
	{
		if( paiHang2.getPosition().x - %pos2.x >= %addX2 )
		{
			%this.movePaiHang( %addX2, paiHang2 );
		}
		else
		{
			paiHang2.setPosition( %pos2.x, %pos2.y );
		}
	}
	else if( %pos3.x != paiHang3.getPosition().x )
	{
		if( paiHang3.getPosition().x - %pos3.x >= %addX3 )
		{
			%this.movePaiHang( %addX3, paiHang3 );
		}
		else
		{
			paiHang3.setPosition( %pos3.x, %pos3.y );
		}
	}
	else if( %pos4.x != paiHang4.getPosition().x )
	{
		if( paiHang4.getPosition().x - %pos4.x >= %addX4 )
		{
			%this.movePaiHang( %addX4, paiHang4 );
		}
		else
		{
			paiHang4.setPosition( %pos4.x, %pos4.y );
		}
	}
	else if( %pos5.x != paiHang5.getPosition().x )
	{
		if( paiHang5.getPosition().x - %pos5.x >= %addX5 )
		{
			%this.movePaiHang( %addX5, paiHang5 );
		}
		else
		{
			paiHang5.setPosition( %pos5.x, %pos5.y );
		}
	}
	else
	{
		if( %index == 1 )
		{
			%this.schedule(5000, reFlyPaiHang );
		}
		else if( %index == 2 )
		{
			%this.schedule( 5000, rePlayVedio );
		}
		return;
	}
	%this.schedule(40, paiHangFly, %pos1, %pos2, %pos3, %pos4, %pos5, %index );
}

function touBiWindow::reFlyPaiHang( %this )
{
	%this.setPaiHangToSide(2);
	return;
}

function touBiWindow::rePlayVedio( %this )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
	{
		return;
	}
	theoraPlayCtrl.setVisible( true );
	theoraPlayCtrl.play();
	%this.schedule(1000, isPlayBackDone );
	return;
}

function touBiWindow::movePaiHang( %this, %add, %ctrl )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
	{
		return;
	}
	%pos = %ctrl.getPosition();
	%pos.x-=%add;
	%ctrl.setPosition( %pos.x, %pos.y );
	return;
}

//默认存入时是有序的
function touBiWindow::setPHScoreAndName( %this, %index )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
		return;
	if( %index == 1 )
	{
		%x1 = GetIniValueNum("Score0", "Score", "./nameSort.hnb");
		%x2 = GetIniValueNum("Score1", "Score", "./nameSort.hnb");
		%x3 = GetIniValueNum("Score2", "Score", "./nameSort.hnb");
		%x4 = GetIniValueNum("Score3", "Score", "./nameSort.hnb");
		%x5 = GetIniValueNum("Score4", "Score", "./nameSort.hnb");
		
		%name1Letter1 = GetIniValueNum("Player0", "Name0", "./nameSort.hnb");
		%name1Letter2 = GetIniValueNum("Player0", "Name1", "./nameSort.hnb");
		%name1Letter3 = GetIniValueNum("Player0", "Name2", "./nameSort.hnb");
		%name1Letter4 = GetIniValueNum("Player0", "Name3", "./nameSort.hnb");
		%name1Letter5 = GetIniValueNum("Player0", "Name4", "./nameSort.hnb");
		
		%name2Letter1 = GetIniValueNum("Player1", "Name0", "./nameSort.hnb");
		%name2Letter2 = GetIniValueNum("Player1", "Name1", "./nameSort.hnb");
		%name2Letter3 = GetIniValueNum("Player1", "Name2", "./nameSort.hnb");
		%name2Letter4 = GetIniValueNum("Player1", "Name3", "./nameSort.hnb");
		%name2Letter5 = GetIniValueNum("Player1", "Name4", "./nameSort.hnb");
		
		%name3Letter1 = GetIniValueNum("Player2", "Name0", "./nameSort.hnb");
		%name3Letter2 = GetIniValueNum("Player2", "Name1", "./nameSort.hnb");
		%name3Letter3 = GetIniValueNum("Player2", "Name2", "./nameSort.hnb");
		%name3Letter4 = GetIniValueNum("Player2", "Name3", "./nameSort.hnb");
		%name3Letter5 = GetIniValueNum("Player2", "Name4", "./nameSort.hnb");
		
		%name4Letter1 = GetIniValueNum("Player3", "Name0", "./nameSort.hnb");
		%name4Letter2 = GetIniValueNum("Player3", "Name1", "./nameSort.hnb");
		%name4Letter3 = GetIniValueNum("Player3", "Name2", "./nameSort.hnb");
		%name4Letter4 = GetIniValueNum("Player3", "Name3", "./nameSort.hnb");
		%name4Letter5 = GetIniValueNum("Player3", "Name4", "./nameSort.hnb");
		
		%name5Letter1 = GetIniValueNum("Player4", "Name0", "./nameSort.hnb");
		%name5Letter2 = GetIniValueNum("Player4", "Name1", "./nameSort.hnb");
		%name5Letter3 = GetIniValueNum("Player4", "Name2", "./nameSort.hnb");
		%name5Letter4 = GetIniValueNum("Player4", "Name3", "./nameSort.hnb");
		%name5Letter5 = GetIniValueNum("Player4", "Name4", "./nameSort.hnb");
		
		%this.setPaiHangNum( 1, paiHangNum1 );
		%this.setPaiHangNum( 2, paiHangNum2 );
		%this.setPaiHangNum( 3, paiHangNum3 );
		%this.setPaiHangNum( 4, paiHangNum4 );
		%this.setPaiHangNum( 5, paiHangNum5 );
	}
	else if( %index == 2 )
	{
		%x1 = GetIniValueNum("Score5", "Score", "./nameSort.hnb");	
		%x2 = GetIniValueNum("Score6", "Score", "./nameSort.hnb");	
		%x3 = GetIniValueNum("Score7", "Score", "./nameSort.hnb");	
    	%x4 = GetIniValueNum("Score8", "Score", "./nameSort.hnb");
		%x5 = GetIniValueNum("Score9", "Score", "./nameSort.hnb");
		
		%name1Letter1 = GetIniValueNum("Player5", "Name0", "./nameSort.hnb");
		%name1Letter2 = GetIniValueNum("Player5", "Name1", "./nameSort.hnb");
		%name1Letter3 = GetIniValueNum("Player5", "Name2", "./nameSort.hnb");
		%name1Letter4 = GetIniValueNum("Player5", "Name3", "./nameSort.hnb");
		%name1Letter5 = GetIniValueNum("Player5", "Name4", "./nameSort.hnb");
		
		%name2Letter1 = GetIniValueNum("Player6", "Name0", "./nameSort.hnb");
		%name2Letter2 = GetIniValueNum("Player6", "Name1", "./nameSort.hnb");
		%name2Letter3 = GetIniValueNum("Player6", "Name2", "./nameSort.hnb");
		%name2Letter4 = GetIniValueNum("Player6", "Name3", "./nameSort.hnb");
		%name2Letter5 = GetIniValueNum("Player6", "Name4", "./nameSort.hnb");
		
		%name3Letter1 = GetIniValueNum("Player7", "Name0", "./nameSort.hnb");
		%name3Letter2 = GetIniValueNum("Player7", "Name1", "./nameSort.hnb");
		%name3Letter3 = GetIniValueNum("Player7", "Name2", "./nameSort.hnb");
		%name3Letter4 = GetIniValueNum("Player7", "Name3", "./nameSort.hnb");
		%name3Letter5 = GetIniValueNum("Player7", "Name4", "./nameSort.hnb");
		
		%name4Letter1 = GetIniValueNum("Player8", "Name0", "./nameSort.hnb");
		%name4Letter2 = GetIniValueNum("Player8", "Name1", "./nameSort.hnb");
		%name4Letter3 = GetIniValueNum("Player8", "Name2", "./nameSort.hnb");
		%name4Letter4 = GetIniValueNum("Player8", "Name3", "./nameSort.hnb");
		%name4Letter5 = GetIniValueNum("Player8", "Name4", "./nameSort.hnb");
		
		%name5Letter1 = GetIniValueNum("Player9", "Name0", "./nameSort.hnb");
		%name5Letter2 = GetIniValueNum("Player9", "Name1", "./nameSort.hnb");
		%name5Letter3 = GetIniValueNum("Player9", "Name2", "./nameSort.hnb");
		%name5Letter4 = GetIniValueNum("Player9", "Name3", "./nameSort.hnb");
		%name5Letter5 = GetIniValueNum("Player9", "Name4", "./nameSort.hnb");
		
		%this.setPaiHangNum( 6, paiHangNum1 );
		%this.setPaiHangNum( 7, paiHangNum2 );
		%this.setPaiHangNum( 8, paiHangNum3 );
		%this.setPaiHangNum( 9, paiHangNum4 );
		%this.setPaiHangNum( 10, paiHangNum5 );
	}
		
	name1Letter1.setDrawPartOfBitmap(1, 0, 0, %name1Letter1 * 40, 0, 40, 42);
	name1Letter2.setDrawPartOfBitmap(1, 0, 0, %name1Letter2 * 40, 0, 40, 42);
	name1Letter3.setDrawPartOfBitmap(1, 0, 0, %name1Letter3 * 40, 0, 40, 42);
	name1Letter4.setDrawPartOfBitmap(1, 0, 0, %name1Letter4 * 40, 0, 40, 42);
	name1Letter5.setDrawPartOfBitmap(1, 0, 0, %name1Letter5 * 40, 0, 40, 42);
		
	name2Letter1.setDrawPartOfBitmap(1, 0, 0, %name2Letter1 * 40, 0, 40, 42);
	name2Letter2.setDrawPartOfBitmap(1, 0, 0, %name2Letter2 * 40, 0, 40, 42);
	name2Letter3.setDrawPartOfBitmap(1, 0, 0, %name2Letter3 * 40, 0, 40, 42);
	name2Letter4.setDrawPartOfBitmap(1, 0, 0, %name2Letter4 * 40, 0, 40, 42);
	name2Letter5.setDrawPartOfBitmap(1, 0, 0, %name2Letter5 * 40, 0, 40, 42);
		
	name3Letter1.setDrawPartOfBitmap(1, 0, 0, %name3Letter1 * 40, 0, 40, 42);
	name3Letter2.setDrawPartOfBitmap(1, 0, 0, %name3Letter2 * 40, 0, 40, 42);
	name3Letter3.setDrawPartOfBitmap(1, 0, 0, %name3Letter3 * 40, 0, 40, 42);
	name3Letter4.setDrawPartOfBitmap(1, 0, 0, %name3Letter4 * 40, 0, 40, 42);
	name3Letter5.setDrawPartOfBitmap(1, 0, 0, %name3Letter5 * 40, 0, 40, 42);
		
	name4Letter1.setDrawPartOfBitmap(1, 0, 0, %name4Letter1 * 40, 0, 40, 42);
	name4Letter2.setDrawPartOfBitmap(1, 0, 0, %name4Letter2 * 40, 0, 40, 42);
	name4Letter3.setDrawPartOfBitmap(1, 0, 0, %name4Letter3 * 40, 0, 40, 42);
	name4Letter4.setDrawPartOfBitmap(1, 0, 0, %name4Letter4 * 40, 0, 40, 42);
	name4Letter5.setDrawPartOfBitmap(1, 0, 0, %name4Letter5 * 40, 0, 40, 42);
		
	name5Letter1.setDrawPartOfBitmap(1, 0, 0, %name5Letter1 * 40, 0, 40, 42);
	name5Letter2.setDrawPartOfBitmap(1, 0, 0, %name5Letter2 * 40, 0, 40, 42);
	name5Letter3.setDrawPartOfBitmap(1, 0, 0, %name5Letter3 * 40, 0, 40, 42);
	name5Letter4.setDrawPartOfBitmap(1, 0, 0, %name5Letter4 * 40, 0, 40, 42);
	name5Letter5.setDrawPartOfBitmap(1, 0, 0, %name5Letter5 * 40, 0, 40, 42);
		
	%this.setScoreCon( %x1, 1 );
	%this.setScoreCon( %x2, 2 );
	%this.setScoreCon( %x3, 3 );
	%this.setScoreCon( %x4, 4 );
	%this.setScoreCon( %x5, 5 );
	return;
}

function touBiWindow::setPaiHangNum( %this, %index, %ctrl )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
		return;
	%index -= 1;
	%ctrl.setDrawPartOfBitmap(1, 0, 0, %index * 30, 0, 30, 25 );
	return;
}

function touBiWindow::setScoreCon( %this, %score, %ctrlIndex )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
		return;
	if( %score < 0 )
	{
		%score1 = -1;
		%score2 = -1;
		%score3 = -1;
		%score4 = -1;
		%score5 = -1;
		%score6 = -1;
		%score7 = -1;
		%score8 = -1;
		%score9 = -1;
	}
	else
	{
		%score1 = mFloor( %score / 100000000 );
		%score = %score % 100000000;
		%score2 = mFloor( %score / 10000000 );
		%score = %score % 10000000;
		%score3 = mFloor( %score / 1000000 );
		%score = %score % 1000000;
		%score4 = mFloor( %score / 100000 );
		%score = %score % 100000;
		%score5 = mFloor( %score / 10000 );
		%score = %score % 10000;
		%score6 = mFloor( %score / 1000 );
		%score = %score % 1000;
		%score7 = mFloor( %score / 100 );
		%score = %score % 100;
		%score8 = mFloor( %score / 10 );
		%score = %score % 10;
		%score9 = %score;
	}
	switch( %ctrlIndex )
	{
		case 1:
			score1Num1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
			score1Num2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
			score1Num3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
			score1Num4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
			score1Num5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
			score1Num6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
			score1Num7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
			score1Num8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
			score1Num9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
		case 2:
			score2Num1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
			score2Num2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
			score2Num3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
			score2Num4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
			score2Num5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
			score2Num6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
			score2Num7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
			score2Num8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
			score2Num9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
		case 3:
			score3Num1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
			score3Num2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
			score3Num3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
			score3Num4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
			score3Num5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
			score3Num6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
			score3Num7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
			score3Num8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
			score3Num9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
		case 4:
			score4Num1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
			score4Num2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
			score4Num3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
			score4Num4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
			score4Num5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
			score4Num6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
			score4Num7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
			score4Num8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
			score4Num9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
		case 5:
			score5Num1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
			score5Num2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
			score5Num3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
			score5Num4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
			score5Num5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
			score5Num6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
			score5Num7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
			score5Num8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
			score5Num9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
		}
	return;
}

function touBiWindow::fadeInBackWhite( %this, %index )
{
	if( $PlayerStartGame || $PlayerStartGameP2 )
		return;
	if( %index == 20 )
	{
		%this.setPaiHangToSide(1);
	}
	if( %index < 1 )
	{
		touBiFadeInWhite.setVisible( false );
		return;
	}
	else
	{
		%test = %index - 1;
		touBiFadeInWhite.setDrawPartOfBitmap(1, 0, 0, %test*16, 0, 16,  16 );
		%this.schedule(50, fadeInBackWhite, %test );
	}
	return;
}

function touBiWindow::hidePaiHang( %this, %bool )
{
	if( $PlayerStartGame || $PlayerStartGameP2)
		return;
	if( %bool )
	{
		paiHang1.setVisible( false );
		paiHang2.setVisible( false );
		paiHang3.setVisible( false );
		paiHang4.setVisible( false );
		paiHang5.setVisible( false );
	}
	else
	{
		paiHang1.setVisible( true );
		paiHang2.setVisible( true );
		paiHang3.setVisible( true );
		paiHang4.setVisible( true );
		paiHang5.setVisible( true );
	}
	return;
}

function touBiWindow::showTouBiMessage( %this, %bool )
{
	if( %bool )
	{
		insertNum1.setVisible( true );
		insertNum2.setVisible( true );
		requestNum1.setVisible( true );
		requestNum2.setVisible( true );
		touBiXieGang.setVisible( true );
		
		insertNum1P2.setVisible( true );
		insertNum2P2.setVisible( true );
		requestNum1P2.setVisible( true );
		requestNum2P2.setVisible( true );
		touBiXieGangP2.setVisible( true );
		
		startGame.setVisible( true );
		insertCoin.setVisible( true );
	}
	else
	{
		insertNum1.setVisible( false );
		insertNum2.setVisible( false );
		requestNum1.setVisible( false );
		requestNum2.setVisible( false );
		touBiXieGang.setVisible( false );
		
		insertNum1P2.setVisible( false );
		insertNum2P2.setVisible( false );
		requestNum1P2.setVisible( false );
		requestNum2P2.setVisible( false );
		touBiXieGangP2.setVisible( false );
		
		startGame.setVisible( false );
		insertCoin.setVisible( false );
	}
}

function touBiGuiStartGame( %val )
{
	if ($CANTStart)
	{
		return;
	}
	
	if( $PlayerStartGame )
	{
		if($ModeIndex == 1 || ($ModeIndex != 1 && ( $CurrentCoinNum >= $RequestCoinNum )) )
		{
			if( %val && $player1State == 0 && $player2State == 1 )
			{
				if( $ModeIndex == 1  )
				{
					reSpawnPlayer();
					
					$player1State = 1;
					
					if ($player2State > 0)
					{
						//$damagePercent = 0.5;
						$attackPercent = 0.7;
					}
					$PlayerP1.setDamageLevel( 0 );
					$playerDamageLevel = 0;
					playerCross.setVisible(true);
				}
				else if( $ModeIndex != 1 && ( $CurrentCoinNum >= $RequestCoinNum ) )
				{
					reSpawnPlayer();
					
					$player1State = 1;
					if ($player2State > 0)
					{
						//$damagePercent = 0.5;
						$attackPercent = 0.7;
					}
					$PlayerP1.setDamageLevel( 0 );
					$playerDamageLevel = 0;
					playerCross.setVisible(true);
					
					%num1 = mFloor( $CurrentCoinNum / 10 );
					%num2 = $CurrentCoinNum % 10;
				
					%num1 = mFloor( $CurrentCoinNum / 10 );
					%num2 = $CurrentCoinNum % 10;
					insertNum1.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
					insertNum2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
				}
			}
			else if( %val && $player1State == 0 && $player2State <= 0 )
			{
				if( $ModeIndex == 1  )
				{
					reSpawnPlayer();
					
					$player1State = 1;
					$PlayerP1.setDamageLevel( 0 );
					$playerDamageLevel = 0;
					playerCross.setVisible(true);
				}
				else if( $ModeIndex != 1 && ( $CurrentCoinNum >= $RequestCoinNum ) )
				{
					reSpawnPlayer();
					
					$player1State = 1;
					$PlayerP1.setDamageLevel( 0 );
					$playerDamageLevel = 0;
					playerCross.setVisible(true);
					
					%num1 = mFloor( $CurrentCoinNum / 10 );
					%num2 = $CurrentCoinNum % 10;
				
					%num1 = mFloor( $CurrentCoinNum / 10 );
					%num2 = $CurrentCoinNum % 10;
					insertNum1.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
					insertNum2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
				}
			}
		}
		return;
	}
	
	if( !$PlayerCanStartGame && $player2State < 0)
		return;
	if( (%val && theoraPlayCtrl.isPlaying()) || (%val && paiHangBackGround.isVisible()) )
	{
		if( $ModeIndex != 1 && ( $CurrentCoinNum < $RequestCoinNum ) )
		{
			return;
		}
		
	    schedule(1000, 0, touBiGuiShowCoin );
		theoraPlayCtrl.stop();
		theoraPlayCtrl.setVisible( false );
		paiHangBackGround.setVisible( false );
	}
	if( %val )
	{
		if( $ModeIndex == 1  )
		{
			reSpawnPlayer();
			
			$PlayerStartGame = true;
			$player1State = 1;
			$PlayerP1.setDamageLevel( 0 );
			$playerDamageLevel = 0;
			playerCross.setVisible(true);
			
			if ($player2State < 0)
			Canvas.schedule( 2000, setContent, "PlayGui" );
		}
		else if( $ModeIndex != 1 && ( $CurrentCoinNum >= $RequestCoinNum ) )
		{
			reSpawnPlayer();
			
			$player1State = 1;
			playerCross.setVisible(true);
			$PlayerP1.setDamageLevel( 0 );
			$playerDamageLevel = 0;
			
			%num1 = mFloor( $CurrentCoinNum / 10 );
			%num2 = $CurrentCoinNum % 10;
			$PlayerStartGame = true;
		
			%num1 = mFloor( $CurrentCoinNum / 10 );
			%num2 = $CurrentCoinNum % 10;
			insertNum1.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
			insertNum2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
			if ($player2State < 0)
			Canvas.schedule( 2000, setContent, "PlayGui" );
		}
	}
	return;
}

function touBiGuiStartGameP2( %val )
{
	if ($CANTStart)
	{
		return;
	}
	
	if( $PlayerStartGameP2 )
	{
		if( %val && $player2State <= 0 && $player1State == 1 )
		{
			if( $ModeIndex == 1  )
			{
				reSpawnPlayerP2();
				
				$player2State = 1;
				if ($player1State > 0)
				{
					//$damagePercent = 0.5;
					$attackPercent = 0.7;
				}
				$playerDamageLevelP2 = 0;
				changeBloodP2();
				playerCrossP2.setVisible(true);
			}
			else if( $ModeIndex != 1 && ( $CurrentCoinNumP2 >= $RequestCoinNum ) )
			{
				reSpawnPlayerP2();
				
				$player2State = 1;
				if ($player1State > 0)
				{
					//$damagePercent = 0.5;
					$attackPercent = 0.7;
				}
				$playerDamageLevelP2 = 0;
				changeBloodP2();
				playerCrossP2.setVisible(true);
				
				%num1 = mFloor( $CurrentCoinNumP2 / 10 );
				%num2 = $CurrentCoinNumP2 % 10;
			
				%num1 = mFloor( $CurrentCoinNumP2 / 10 );
				%num2 = $CurrentCoinNumP2 % 10;
				insertNum1P2.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
				insertNum2P2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
			}
		}
		else if( %val && $player2State <= 0 && $player1State <= 0 )
		{
			if( $ModeIndex == 1  )
			{
				reSpawnPlayerP2();
				
				$player2State = 1;
				$playerDamageLevelP2 = 0;
				changeBloodP2();
				playerCrossP2.setVisible(true);
			}
			else if( $ModeIndex != 1 && ( $CurrentCoinNumP2 >= $RequestCoinNum ) )
			{
				reSpawnPlayerP2();
				
				$player2State = 1;
				$playerDamageLevelP2 = 0;
				changeBloodP2();
				playerCrossP2.setVisible(true);
				
				%num1 = mFloor( $CurrentCoinNumP2 / 10 );
				%num2 = $CurrentCoinNumP2 % 10;
			
				%num1 = mFloor( $CurrentCoinNumP2 / 10 );
				%num2 = $CurrentCoinNumP2 % 10;
				insertNum1P2.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
				insertNum2P2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
			}
		}
		return;
	}
	
	if( !$PlayerCanStartGameP2 && $player1State < 0 )
		return;
	if( (%val && theoraPlayCtrl.isPlaying()) || (%val && paiHangBackGround.isVisible()) )
	{
		if( $ModeIndex != 1 && ( $CurrentCoinNumP2 < $RequestCoinNum ) )
		{
			return;
		}
		
	    schedule(1000, 0, touBiGuiShowCoin );
		theoraPlayCtrl.stop();
		theoraPlayCtrl.setVisible( false );
		paiHangBackGround.setVisible( false );
	}
	if( %val )
	{
		if( $ModeIndex == 1  )
		{
			reSpawnPlayerP2();
			
			$player2State = 1;echo("true      1");
			playerCrossP2.setVisible(true);
			$PlayerStartGameP2 = true;
			$playerDamageLevelP2 = 0;
			changeBloodP2();
			
			if ($player1State < 0)
			Canvas.schedule( 2000, setContent, "PlayGui" );
		}
		else if( $ModeIndex != 1 && ( $CurrentCoinNumP2 >= $RequestCoinNum ) )
		{
			reSpawnPlayerP2();
			
			$player2State = 1;
			playerCrossP2.setVisible(true);
			$playerDamageLevelP2 = 0;
			changeBloodP2();
			
			%num1 = mFloor( $CurrentCoinNumP2 / 10 );
			%num2 = $CurrentCoinNumP2 % 10;
			$PlayerStartGameP2 = true;
		
			%num1 = mFloor( $CurrentCoinNumP2 / 10 );
			%num2 = $CurrentCoinNumP2 % 10;
			insertNum1P2.setDrawPartOfBitmap(1, 0, 0, %num1*16, 0, 16, 21 );
			insertNum2P2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
			
			if ($player1State < 0)
			Canvas.schedule( 2000, setContent, "PlayGui" );
		}
	}
	return;
}

function touBiGuiInsertCoinNum( %val )
{
	if( !$PlayerCanStartGame )
		return;
    $ShowPleaseInsertCoin = false;
	Please.setVisible(false);
	CInsert.setVisible(false);
	if( (%val && theoraPlayCtrl.isPlaying()) || (%val && paiHangBackGround.isVisible()) )
	{
		schedule(1000, 0, touBiGuiShowCoin );
		theoraPlayCtrl.stop();
		theoraPlayCtrl.setVisible( false );
		paiHangBackGround.setVisible( false );
		checkIsTimeToRestart(0);
	}
	
	return;
}

function touBiGuiInsertCoinNumP2( %val )
{
	if( !$PlayerCanStartGameP2 )
		return;
    $ShowPleaseInsertCoin = false;
	Please.setVisible(false);
	CInsert.setVisible(false);
	if( (%val && theoraPlayCtrl.isPlaying()) || (%val && paiHangBackGround.isVisible()) )
	{
		schedule(1000, 0, touBiGuiShowCoin );
		theoraPlayCtrl.stop();
		theoraPlayCtrl.setVisible( false );
		paiHangBackGround.setVisible( false );
		checkIsTimeToRestartP2(0);
	}
	
	return;
}

function touBiGuiShowCoin()
{
	insertNum1.setVisible( true );
	insertNum2.setVisible( true );
	requestNum1.setVisible( true );
	requestNum2.setVisible( true );
	touBiXieGang.setVisible( true );
	
	insertNum1P2.setVisible( true );
	insertNum2P2.setVisible( true );
	requestNum1P2.setVisible( true );
	requestNum2P2.setVisible( true );
	touBiXieGangP2.setVisible( true );
	
	touBiFadeInWhite.setVisible( false );
	touBiWindow::flashInsertCoin( touBiWindow.getId() );
}

function touBiGuiStartSet( %val )
{
	if( !touBiWindow.isAwake() )
		return;
	if( %val && theoraPlayCtrl.isPlaying() || %val && paiHangBackGround.isVisible() )
	{
		theoraPlayCtrl.stop();
		paiHangBackGround.setVisible( false );
		theoraPlayCtrl.setVisible( false );
		touBiFadeInWhite.setVisible( false );
	}
	if( %val && isObject( gameSetGui ) )
	{
		Canvas.schedule( 1000, setContent, "gameSetGui" );
	}
}

function checkIsTimeToRestart( %timeNumber )
{
	if( $PlayerStartGame || $PlayerStartGameP2 || $CurrentCoinNum >= $RequestCoinNum || $CurrentCoinNumP2 >= $RequestCoinNum)
		return;
	if( %timeNumber > 15000 )
	{
		if( $CurrentCoinNum < $RequestCoinNum )
		{
			Canvas.setContent( touBiWindow );
		}
		return;
	}
	else
	{
		%timeNumber+=500;
		schedule(500, 0, checkIsTimeToRestart, %timeNumber );
	}
}

function checkIsTimeToRestartP2( %timeNumber )
{
	if( $PlayerStartGame || $PlayerStartGameP2 || $CurrentCoinNum >= $RequestCoinNum || $CurrentCoinNumP2 >= $RequestCoinNum)
		return;
	if( %timeNumber > 15000 )
	{
		if( $CurrentCoinNumP2 < $RequestCoinNum )
		{
			Canvas.setContent( touBiWindow );
		}
		return;
	}
	else
	{
		%timeNumber+=500;
		schedule(500, 0, checkIsTimeToRestartP2, %timeNumber );
	}
}

function oggTheoraEnd()
{
	theoraPlayCtrl.stop();
}

function xiAnquandai()
{
	if( !touBiWindow.isAwake() )
	{
		return;
	}
	
	%safe = PCVRGetBeltsafe();
	
	if( %safe )
	{
		anquandai1.setVisible( false );
	}
	else
	{
		anquandai1.setVisible( true );
	}
	
	%safeP2 = PCVRGetBeltsafeP2();
	
	if( %safeP2 )
	{
		anquandai1P2.setVisible( false );
	}
	else
	{
		anquandai1P2.setVisible( true );
	}
	
	schedule( 1000, 0, xiAnquandai );
}
