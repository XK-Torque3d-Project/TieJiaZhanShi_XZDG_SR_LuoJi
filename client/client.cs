//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Server Admin Commands
//-----------------------------------------------------------------------------

function SAD(%password)
{
   if (%password !$= "")
      commandToServer('SAD', %password);
}

function SADSetPassword(%password)
{
   commandToServer('SADSetPassword', %password);
}

//----------------------------------------------------------------------------
// Misc server commands
//----------------------------------------------------------------------------

function clientCmdSyncClock(%time)
{
   // Time update from the server, this is only sent at the start of a mission
   // or when a client joins a game in progress.
}

//-----------------------------------------------------------------------------
// Numerical Health Counter
//-----------------------------------------------------------------------------

function clientCmdSetNumericalHealthHUD(%curHealth)
{
   // Skip if the hud is missing.
   if (!isObject(numericalHealthHUD))
      return;

   // The server has sent us our current health, display it on the HUD
   numericalHealthHUD.setValue(%curHealth);

   // Ensure the HUD is set to visible while we have health / are alive
   if (%curHealth)
      HealthHUD.setVisible(true);
   else
      HealthHUD.setVisible(false);
}

// ----------------------------------------------------------------------------
// WeaponHUD
// ----------------------------------------------------------------------------

// Update the Ammo Counter with current ammo, if not any then hide the counter.

function clientCmdSetAmmoAmountHud(%amount)
{
   if (!%amount)
      AmmoAmount.setVisible(false);
   else
   {
      AmmoAmount.setVisible(true);
      AmmoAmount.setText("Ammo: "@%amount);
   }
}

// Here we update the Weapon Preview image & reticle for each weapon.  We also
// update the Ammo Counter (just so we don't have to call it separately).
// Passing an empty parameter ("") hides the HUD component.

function clientCmdRefreshWeaponHUD(%amount, %preview, %ret)
{
}

function clientCmdsetZoom(%reticle)
{
   $ZoomOn = true;
   setFov($Pref::Player::CurrentFOV);
   zoomReticle.setBitmap("art/gui/weaponHud/"@ detag(%reticle));
   zoomReticle.setVisible(true);

   DOFPostEffect.setAutoFocus( true );
   DOFPostEffect.setFocusParams( 0.5, 0.5, 50, 500, -5, 5 );
   DOFPostEffect.enable();
}

function clientCmdUnSetZoom(%reticle)
{
   $ZoomOn = false;
   setFov($Pref::Player::defaultFOV);
   zoomReticle.setVisible(false);

   DOFPostEffect.disable();
}

//近身攻击调用受伤画面
function clientCmdShowDamagePicture( %id )
{
	ShowDamagePicture.setBitmap("art/gui/playGui/DamagePicture_"@%id@".png" );
	if( !ShowDamagePicture.isVisible() )
	{
		ShowDamagePicture.setVisible(true);
		////PCVRSetPlayerHurtShake();
		doudongRotate();
	}
	if( %id == 0 )
	{
		schedule( 50,0,flashDamage );
		schedule( 100,0,hideShowDamagePicture );
	}
	else
	{
		schedule( 500,0,hideShowDamagePicture );
	}
	return;
}

//隐藏近身攻击受伤画面
function hideShowDamagePicture()
{
	if( ShowDamagePicture.isVisible() )
	{
		ShowDamagePicture.setVisible(false);
	}
	ShowDamagePicture.setBitmap("art/gui/playGui/DamagePicture_0.png" );
	return;
}

//子弹攻击时受伤画面闪�?
function flashDamage()
{
	ShowDamagePicture.setBitmap("art/gui/playGui/demageFlash.png" );
	return;
}
function P1deadLe()
{
	if ($player1State > 0)
	{
		$player1State = 0;
	}
	
	if ($player2State <= 0)
	{
		$PlayerP1.stopMove();
		setTurnRotation(0, 0);
		$WaitContinue = true;
		$Playerdead = true;
		emptyMissile.setVisible( false );
		
		PlayerDeadLe();
	}
	else
	{
		//judge show "please insert coin" or "please press start button"
		if ($CurrentCoinNum >= $RequestCoinNum )
		{
			PleaseStartGameP1();
		}
		else
		{
			PleaseInsertCoinP1( );
		}
	}
	
	setGunShakeState(1, false);
	
	playerCross.setVisible(false);
}

function P2deadLe()
{
	if ($player2State > 0)
	{
		$player2State = 0;
	}
	
	if ($player1State <= 0)
	{
		$PlayerP1.stopMove();
		setTurnRotation(0, 0);
		$WaitContinue = true;
		$Playerdead = true;
		emptyMissile.setVisible( false );
		
		PlayerDeadLe();
	}
	else
	{
		//judge show "please insert coin" or "please press start button"
		if ($CurrentCoinNumP2 >= $RequestCoinNum )
		{
			PleaseStartGameP2();
		}
		else
		{
			PleaseInsertCoinP2( );
		}
	}
	
	setGunShakeState(2, false);
	
	playerCrossP2.setVisible(false);
}

//玩家死亡
//function clientCmdPlayerDead()
function PlayerDeadLe()
{
	$damagePercent = 1;
	$attackPercent = 1;
	
	willContinue.setVisible( true );
	siWangDaoJiShi(0);
	return;
}

//死亡倒计�?
function siWangDaoJiShi( %time )
{
	if( !$WaitContinue )
	{
		willContinue.setVisible( false );
		continueNumber.setVisible( false );
		zhunbei.setVisible( false );
		return;
	}
	%showIndex = 9 - %time;
	if( %time > 9 )
	{
		reStartShowGui();
		setGunShakeState(0, false);
		$GameOver = true;
		$CANTStart = true;
		willContinue.setVisible( false );
		continueNumber.setVisible( false );
		clientCmdopenJiFenKuang();
		return;
	}
	if( %time == 0 )
	{
		continueNumber.setVisible( true );
	}
	continueNumber.setDrawPartOfBitmap( 1, 0, 0, %showIndex * 83, 0, 83, 77 );
	%time++;
	schedule( 1500, 0, siWangDaoJiShi, %time );
	return;
}

//主角血条绘�?
function clientCmddrawPlayerHealth( )
{
	if(	!isObject( $PlayerP1 ) )
		return;
		
	if ($HardIndex == 1)
	{
		$playerDamageLevel = 0;
	}
	%maxDamage = $PlayerP1.getMaxDamage() - 100000;
	%damage = $playerDamageLevel;
	
	%totalDamage = $PlayerP1.getMaxDamage()-100000;
	%nowDamage = $playerDamageLevel;
	//error("当前血值是 ==========="@%maxDamage-%damage);
	//通知客户端机甲损�?
	if( %nowDamage > %totalDamage * 0.5 && %nowDamage < %totalDamage * 0.7 && !$JiJiaPercent50IsShow )
	{
		sfxPlayOnce(panel_Sound_tiShi01);
		commandToClient( %client, 'showJiJiaSunHuaiPercent', 50 );
		//showJiJiaSunHuaiPercent( 50 );
		$JiJiaPercentShowOver = false;
		$JiJiaPercent50IsShow = true;
	}
	if( %nowDamage > %totalDamage * 0.7 && %nowDamage < %totalDamage * 0.9 && !$JiJiaPercent70IsShow && $JiJiaPercentShowOver )
	{
		$FirstPlay = true;
		PlaySecondSound(0);
		commandToClient( %client, 'showJiJiaSunHuaiPercent', 70 );
		//showJiJiaSunHuaiPercent( 70 );
		$JiJiaPercentShowOver = false;
		$JiJiaPercent70IsShow = true;
	}
	if( %nowDamage > %totalDamage * 0.9 && !$JiJiaPercent90IsShow && $JiJiaPercentShowOver )
	{
		$FirstPlay = false;
		$ThirdPlay = true;
		PlayThirdSound(0);
		commandToClient( %client, 'showJiJiaSunHuaiPercent', 90 );
		//showJiJiaSunHuaiPercent( 90 );
		$JiJiaPercentShowOver = false;
		$JiJiaPercent90IsShow = true;
	}
				
	if( %maxDamage - %damage <= 0 )
	{
		if( !$WaitContinue )
		{
			P1deadLe();
		}
	}
	//%width = ( 1 - %damage / %maxDamage ) * PlayerHealth.extent.x;
	//%height = PlayerHealth.extent.y;
	//PlayerHealth.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	%width = ( %damage / %maxDamage ) * PlayerHealth.extent.x;
	%height = PlayerHealth.extent.y;
	PlayerHealth.setDrawPartOfBitmap( 1, %width, 0, %width, 0, PlayerHealth.extent.x, %height );
	return;
}

//player two blood
function changeBloodP2()
{
	if(	!isObject( $PlayerP1 ) )
		return;
		
	/*if ($player2State <= 0)
	{
		return;
	}*/
		
	if ($HardIndex == 1)
	{
		$playerDamageLevelp2 = 0;
	}
	
	%width = ( 1 - $playerDamageLevelp2 / 5400 ) * PlayerHealthP2.extent.x;
	%height = PlayerHealthP2.extent.y;
	PlayerHealthP2.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	
	if( $playerDamageLevelp2 >= 5400 )
	{
		if( !$WaitContinue )
		{
			P2deadLe();
		}
	}
}

function judgeHurtPlayer(%judge, %damageValue, %objT)
{
	if (%judge)
	{
		if ($player1State > 0 && $player2State > 0)
		{
			if (getrandom(1,2) == 1)
			{
				$PlayerP1.getDataBlock().damage($PlayerP1, %objT, %objT.getPosition(), %damageValue,"a" );
				$PlayerP1.updateHealth();
				commandToClient($PlayerP1.client,'drawPlayerHealth');
			}
			else
			{
				$playerDamageLevelp2 += %damageValue;
				changeBloodP2();
			}
		}
		else if ($player1State > 0)
		{
			$PlayerP1.getDataBlock().damage($PlayerP1, %objT, %objT.getPosition(), %damageValue,"a" );
			$PlayerP1.updateHealth();
			commandToClient($PlayerP1.client,'drawPlayerHealth');
		}
		else if ($player2State > 0)
		{
			$playerDamageLevelp2 += %damageValue;
			changeBloodP2();
		}
	}
	else
	{
		if ($player1State > 0)
		{
			$PlayerP1.getDataBlock().damage($PlayerP1, %objT, %objT.getPosition(), %damageValue,"a" );
			$PlayerP1.updateHealth();
			commandToClient($PlayerP1.client,'drawPlayerHealth');
		}
		
		if ($player2State > 0)
		{
			$playerDamageLevelp2 += %damageValue;
			changeBloodP2();
		}
	}
}

function judgeHurtPlayerOnly(%judge, %damageValue, %objT)
{
	if (%judge)
	{
		if ($player1State > 0 && $player2State > 0)
		{
			if (getrandom(1,2) == 1)
			{
				$playerDamageLevel += %damageValue;
				clientCmddrawPlayerHealth();
			}
			else
			{
				$playerDamageLevelp2 += %damageValue;
				changeBloodP2();
			}
		}
		else if ($player1State > 0)
		{
			$playerDamageLevel += %damageValue;
			clientCmddrawPlayerHealth();
		}
		else if ($player2State > 0)
		{
			$playerDamageLevelp2 += %damageValue;
			changeBloodP2();
		}
	}
	else
	{
		if ($player1State > 0)
		{
			$playerDamageLevel += %damageValue;
			clientCmddrawPlayerHealth();
			
			if (isObject(%objT))
			{
				%objT.applyDamage(%damageValue);
			}
		}
		
		if ($player2State > 0)
		{
			$playerDamageLevelp2 += %damageValue;
			changeBloodP2();
		}
	}
}

//显示boss血�?
function clientCmdshowBossHealth()
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	BossHealthBottom.setVisible(true);
	BossHealthTop.setVisible(true);
	return;
}

//boss获得伤害,分血条显�?
function clientCmdBossGetDamage( %damage, %maxDamage )
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	%width = ( 1 - %damage / %maxDamage ) * BossHealthTop.extent.x;
	%height = BossHealthTop.extent.y;
	BossHealthTop.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	BossHealthFlash.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	BossHealthTop.setVisible( false );
	schedule( 70, 0, clientCmdshowBossHealth );
	return;
}

//隐藏boss分血�?
function clientCmdhideBossHealth()
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	BossHealthBottom.setVisible( false );
	BossHealthTop.setVisible( false );
	return;
}

//boss获得伤害,总血条显�?
function clientCmdBossGetDamageTotal( %damage, %maxDamage )
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	%width = ( 1 - %damage / %maxDamage ) * bossTotalHealthTop.extent.x;
	%height = bossTotalHealthTop.extent.y;
	bossTotalHealthTop.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	bossTotalHealthFlash.setDrawPartOfBitmap( 1, 0, 0, 0, 0, %width, %height );
	bossTotalHealthTop.setVisible( false );
	schedule( 70, 0, clientCmdshowBossHealthTotal );
	return;
}

//显示boss总血�?
function clientCmdshowBossHealthTotal()
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	BossName.setVisible( true );
	bossTotalHealthFlash.setVisible(true);
	bossTotalHealthTop.setVisible(true);
	return;
}

//隐藏boss总血�?
function clientCmdhideBossTotalHealth()
{
	if($LoadingLevel != 2)
	{
		return;
	}
	
	bossTotalHealthFlash.setVisible( false );
	bossTotalHealthTop.setVisible( false );
	BossName.setVisible( true );
	return;
}

//游戏界面玩家加分
function clientCmdaddPlayerScore( %score )
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
	playerScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0,  12, 23 );
	playerScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0,  12, 23 );
	playerScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0,  12, 23 );
	playerScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0,  12, 23 );
	playerScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0,  12, 23 );
	playerScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 12, 0,  12, 23 );
	playerScore7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 12, 0,  12, 23 );
	playerScore8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 12, 0,  12, 23 );
	playerScore9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 12, 0,  12, 23 );
	return;
}

//游戏界面杀死npc计数增加
function clientCmdaddKillNpcNum( %number )
{
	%score1 = mFloor( %number / 10000 );
	%number = %number % 10000;
	%score2 = mFloor( %number / 1000 );
	%number = %number % 1000;
	%score3 = mFloor( %number / 100 );
	%number = %number % 100;
	%score4 = mFloor( %number / 10 );
	%number = %number % 10;
	%score5 = %number;
	NpcNum1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0, 12, 23 );
	NpcNum2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0, 12, 23 );
	NpcNum3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 12, 0, 12, 23 );
	NpcNum4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 12, 0, 12, 23 );
	NpcNum5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 12, 0, 12, 23 );
	return;
}

//游戏界面杀死boss计数增加
function clientCmdaddKillBossNum( %number )
{
	%score1 = mFloor( %number / 10 );
	%number = %number % 10;
	%score2 = %number;
	BossNum1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 12, 0, 12, 23 );
	BossNum2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 12, 0, 12, 23 );
	return;
}

//时间控件显示
function clientCmdsetPlayerTimeNumber( %number )
{
	%Minute1Number = mFloor( $PlayerTimeNumber / 600 );
	%Minute2Number = mFloor( ( $PlayerTimeNumber / 60 ) % 10 );
	%Second1Number = mFloor( ( $PlayerTimeNumber / 10 ) % 6 );
	%Second2Number = mFloor( $PlayerTimeNumber % 10 );
		
	timeNum1.setDrawPartOfBitmap(1, 0, 0, %Minute1NUmber*12, 0, 12, 23 );
	timeNum2.setDrawPartOfBitmap(1, 0, 0, %Minute2Number*12, 0, 12, 23 );
	timeNum3.setDrawPartOfBitmap(1, 0, 0, %Second1Number*12, 0, 12, 23 );
	timeNum4.setDrawPartOfBitmap(1, 0, 0, %Second2Number*12, 0, 12, 23 );
	return;
}

//导弹数量显示
function clientCmdshowDaoDanNum()
{
	if($PlayerDaoDanNum > 0)
	{
		MyShine.setVisible(false);
		daoDanFlash.setVisible(false);
	}
	
	%number1 = mFloor( $PlayerDaoDanNum / 100 );
	%number2 = mFloor( ( $PlayerDaoDanNum / 10 ) % 10 );
	%number3 = mFloor( $PlayerDaoDanNum % 10 );
	daoDanNum1.setDrawPartOfBitmap(1, 0, 0, %number1*12, 0, 12, 23 );
	daoDanNum2.setDrawPartOfBitmap(1, 0, 0, %number2*12, 0, 12, 23 );
	daoDanNum3.setDrawPartOfBitmap(1, 0, 0, %number3*12, 0, 12, 23 );
	return;
}

function clientCmdshowDaoDanNumP2()
{
	if($PlayerDaoDanNumP2 > 0)
	{
		MyShineP2.setVisible(false);
		daoDanFlashP2.setVisible(false);
	}
	
	%number1 = mFloor( $PlayerDaoDanNumP2 / 100 );
	%number2 = mFloor( ( $PlayerDaoDanNumP2 / 10 ) % 10 );
	%number3 = mFloor( $PlayerDaoDanNumP2 % 10 );
	daoDanNum1P2.setDrawPartOfBitmap(1, 0, 0, %number1*12, 0, 12, 23 );
	daoDanNum2P2.setDrawPartOfBitmap(1, 0, 0, %number2*12, 0, 12, 23 );
	daoDanNum3P2.setDrawPartOfBitmap(1, 0, 0, %number3*12, 0, 12, 23 );
	return;
}

//boss线框图改变消�?
function clientCmdwrapBossWeapon()
{
	changeBossWeaponToBig( 30, 20, 40, 20 );
	return;
}

//boss线框图位移变�?
function changeBossWeaponToBig( %detX, %detY, %addX, %addY )
{
	%pos = bossWeapon.getPosition();
	%extent = bossWeapon.getExtent();
	if( %pos.x < 387 || %extent.x < 585 )
	{
		if( %pos.x < 387 )
		{
			%pos.x += %detX; 
			%pos.y += %detY;
			bossWeapon.setPosition( %pos.x, %pos.y );
			bossWeaponDamage1.setPosition( %pos.x, %pos.y );
			bossWeaponDamage2.setPosition( %pos.x, %pos.y );
			bossWeaponDamage3.setPosition( %pos.x, %pos.y );
			bossWeaponDamage4.setPosition( %pos.x, %pos.y );
		}
		if( %extent.x < 585 )
		{
			%extent.x += %addX;
			%extent.y += %addY;
			bossWeapon.setExtent( %extent );
			bossWeaponDamage1.setExtent( %extent );
			bossWeaponDamage2.setExtent( %extent );
			bossWeaponDamage3.setExtent( %extent );
			bossWeaponDamage4.setExtent( %extent );
		}
		schedule(30, 0, changeBossWeaponToBig, %detX, %detY, %addX, %addY );
		return;
	}
	else
	{
		schedule(3000, 0, changeBossWeaponToSmall, %detX, %detY, %addX, %addY );
		flashBossWeaponDamage(0);
		return;
	}
	return;
}

//boss线框图位移变�?
function changeBossWeaponToSmall( %detX, %detY, %addX, %addY )
{
	%pos = bossWeapon.getPosition();
	%extent = bossWeapon.getExtent();
	if( %pos.x > 87 || %extent.x > 185 )
	{
		if( %pos.x > 87 )
		{
			%pos.x -= %detX; 
			%pos.y -= %detY;
			bossWeapon.setPosition( %pos.x, %pos.y );
			bossWeaponDamage1.setPosition( %pos.x, %pos.y );
			bossWeaponDamage2.setPosition( %pos.x, %pos.y );
			bossWeaponDamage3.setPosition( %pos.x, %pos.y );
			bossWeaponDamage4.setPosition( %pos.x, %pos.y );
		}
		if( %extent.x > 185 )
		{
			%extent.x -= %addX;
			%extent.y -= %addY;
			bossWeapon.setExtent( %extent );
			bossWeaponDamage1.setExtent( %extent );
			bossWeaponDamage2.setExtent( %extent );
			bossWeaponDamage3.setExtent( %extent );
			bossWeaponDamage4.setExtent( %extent );
		}
		schedule(30, 0, changeBossWeaponToSmall, %detX, %detY, %addX, %addY );
		return;
	}
	return;
}
//boss武器闪烁
function flashBossWeaponDamage( %time )
{
	if( %time > 1500 )
	{
		bossWeaponDamage1.setVisible( true );
		bossWeaponDamage2.setVisible( true );
		bossWeaponDamage3.setVisible( true );
		if( bossWeaponDamage4.bitmap !$= "" )
		{
			bossWeaponDamage4.setVisible( true );
		}
		return;
	}
	if( bossWeaponDamage1.isVisible() )
	{
		bossWeaponDamage1.setVisible( false );
		bossWeaponDamage2.setVisible( false );
		bossWeaponDamage3.setVisible( false );
		if( bossWeaponDamage4.bitmap !$= "" )
		{
			bossWeaponDamage4.setVisible( false );
		}
	}
	else
	{
		bossWeaponDamage1.setVisible( true );
		bossWeaponDamage2.setVisible( true );
		bossWeaponDamage3.setVisible( true );
		if( bossWeaponDamage4.bitmap !$= "" )
		{
			bossWeaponDamage4.setVisible( true );
		}
	} 
	%time += 150;
	schedule( 150, 0, flashBossWeaponDamage, %time );
	return;
}

function clientCmdPlaySound( )
{
	return;
	if($GameOver)
	{
		return;
	}
	if( !isObject($PlayerP1) )
		return;
	%totalDamage = $PlayerP1.getMaxDamage()-100000;
	%nowDamage = $PlayerP1.getDamageLevel();
	if(%nowDamage < %totalDamage*0.5 || %nowDamage >= %totalDamage)
	{
		$FirstPlay = false;
        $ThirdPlay = false;
    }
		
	schedule(100, 0, clientCmdPlaySound);
	return;
}

//闪烁仪表�?
function clientCmdstartFlashYiBiaoPan( %index )
{
	if( $GameOver || !isObject( $PlayerP1 ) )
	{
		return;
	}
	%totalDamage = $PlayerP1.getMaxDamage()-100000;
	%nowDamage = $PlayerP1.getDamageLevel();
	if( %index > 6 )
	{
		if( %index == 13 )
		{
			%index = 1;
			%bitmapIndex = 1;
		}
		else
		{
			%bitmapIndex = 13 - %index;
		}
	}
	else
	{
		%bitmapIndex = %index;
	}
	if( %nowDamage < %totalDamage * 0.5 )
	{
		yiBiaoPan.setBitmap( "art/gui/playGui/yiBiaoGreen_"@%bitmapIndex );
		schedule(125, 0, clientCmdstartFlashYiBiaoPan, %index++ );
	}
	if( %nowDamage >= %totalDamage * 0.5 && %nowDamage < %totalDamage * 0.8 )
	{
		yiBiaoPan.setBitmap( "art/gui/playGui/yiBiaoYellow_"@%bitmapIndex );
		schedule(65, 0, clientCmdstartFlashYiBiaoPan, %index++ );
	}
	if( %nowDamage >= %totalDamage * 0.8 )
	{
		yiBiaoPan.setBitmap( "art/gui/playGui/yiBiaoRed_"@%bitmapIndex );
		schedule(40, 0, clientCmdstartFlashYiBiaoPan, %index++ );
	}
	return;
}

//游戏币数�?
function changeCoinNum()
{
	%num1 = mFloor( $CurrentCoinNum / 100 );
	%num2 = mFloor( $CurrentCoinNum /10 ) % 10;
	%num3 = $CurrentCoinNum % 10;
	%resNum1 = mFloor( $RequestCoinNum / 10 );
	%resNum2 = $RequestCoinNum % 10;
	currentCoin1.setDrawPartOfBitmap(1, 0, 0, %num1*12, 0, 12, 23 );
	currentCoin2.setDrawPartOfBitmap(1, 0, 0, %num2*12, 0, 12, 23 );
	currentCoin3.setDrawPartOfBitmap(1, 0, 0, %num3*12, 0, 12, 23 );
	requestCoin1.setDrawPartOfBitmap(1, 0, 0, %resNum1*12, 0, 12, 23 );
	requestCoin2.setDrawPartOfBitmap(1, 0, 0, %resNum2*12, 0, 12, 23 );
	
	insertNum1.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
	insertNum2.setDrawPartOfBitmap(1, 0, 0, %num3*16, 0, 16, 21 );
	requestNum1.setDrawPartOfBitmap(1, 0, 0, %resNum1*16, 0, 16, 21 );
	requestNum2.setDrawPartOfBitmap(1, 0, 0, %resNum2*16, 0, 16, 21 );
	if( touBiWindow.isAwake() )
	{
		touBiGuiInsertCoinNum(1);
	}
	
	if ($player1State == 0 && $CurrentCoinNum == $RequestCoinNum)
	{
		PleaseStartGameP1();
	}
	return;
}

//游戏币数�?
function changeCoinNumP2()
{
	%num1 = mFloor( $CurrentCoinNumP2 / 100 );
	%num2 = mFloor( $CurrentCoinNumP2 /10 ) % 10;
	%num3 = $CurrentCoinNumP2 % 10;
	%resNum1 = mFloor( $RequestCoinNum / 10 );
	%resNum2 = $RequestCoinNum % 10;
	currentCoin1P2.setDrawPartOfBitmap(1, 0, 0, %num1*12, 0, 12, 23 );
	currentCoin2P2.setDrawPartOfBitmap(1, 0, 0, %num2*12, 0, 12, 23 );
	currentCoin3P2.setDrawPartOfBitmap(1, 0, 0, %num3*12, 0, 12, 23 );
	requestCoin1P2.setDrawPartOfBitmap(1, 0, 0, %resNum1*12, 0, 12, 23 );
	requestCoin2P2.setDrawPartOfBitmap(1, 0, 0, %resNum2*12, 0, 12, 23 );
	
	insertNum1P2.setDrawPartOfBitmap(1, 0, 0, %num2*16, 0, 16, 21 );
	insertNum2P2.setDrawPartOfBitmap(1, 0, 0, %num3*16, 0, 16, 21 );
	requestNum1P2.setDrawPartOfBitmap(1, 0, 0, %resNum1*16, 0, 16, 21 );
	requestNum2P2.setDrawPartOfBitmap(1, 0, 0, %resNum2*16, 0, 16, 21 );
	if( touBiWindow.isAwake() )
	{
		touBiGuiInsertCoinNumP2(1);//FFFFFFFFFFFFFFFF LXY CHANGE
	}
	
	if ($player2State == 0 && $CurrentCoinNumP2 == $RequestCoinNum)
	{
		PleaseStartGameP2();
	}
	return;
}

//锁定飞机图标
function clientCmdaimBitmapCheck()
{
	if( !isObject( $PlayerP1 ) )
		return;
	%minDes = 0;
	%minDesObj = aimFlyShapeBitmap.getBitmapFollowObj();
	if(!isObject( %minDesObj ) )
	{
		for( %i = 0; %i < $FlyShapeNum; %i++ )
		{
			%obj = $FlyShapeArray[ %i ];
			if( isObject( %obj ) )
			{
				%rtt=vectorDist( $PlayerP1.getposition(), %obj.getposition());
				if( %i == 0 )
				{
					%minDes = %rtt;
					%minDesObj = %obj;
				}
				else
				{
					if( %rtt < %minDes )
					{
						%minDes = %rtt;
						%minDesObj = %obj;
					}
				}
			}
		}
	}
	if( isObject( %minDesObj ) )
	{
		if( aimFlyShapeBitmap.getBitmapFollowObj() != %minDesObj )
		{
			%extent = aimFlyShapeBitmap.getExtent();
			%extent.x = 200; 
			%extent.y = 150;
			aimFlyShapeBitmap.setExtent( %extent );
			aimFlyShapeBitmap.setBitmapFollowObj( %minDesObj );
			aimFlyShapeBitmap.setVisible( true );
			schedule(50, 0, changeSize );
			
			if ($player1State == 1)
			{
				schedule(100, 0, flashDaoDan );
			}
			
			if ($player2State == 1)
			{
				schedule(100, 0, flashDaoDanP2 );
			}
			
			if( !$IsDaoDanTiShiShow )
			{
				schedule( 10, 0, flashDaoDanTiShi, 0 );
				$IsDaoDanTiShiShow = true;
			}
		}
	}
	else
	{
		aimFlyShapeBitmap.setVisible( false );
	}
	schedule(1000, 0, clientCmdaimBitmapCheck );
	return;
}

//改变锁定图标大小和位�?
function changeSize()
{
	%extent = aimFlyShapeBitmap.getExtent();
	if( %extent.x > 60 )
	{
		%extent.x -= 20;
		%extent.y -= 15;
		aimFlyShapeBitmap.setExtent( %extent );
		schedule(40, 0, changeSize );
		return;
	}
	daoDanFlash.setVisible(false);
	daoDanFlashP2.setVisible(false);
	return;
}

//闪烁导弹提示
function flashDaoDan()
{
	if($PlayerDaoDanNum <= 0)
	{
		MyShine.setVisible(true);
		return;
	}
	
	if( isObject( aimFlyShapeBitmap.getBitmapFollowObj() ) )
	{
		if( daoDanFlash.isVisible() )
		{
			daoDanFlash.setVisible(false);
		}
		else
		{
			daoDanFlash.setVisible(true);
		}
		schedule( 200, 0, flashDaoDan );
	}
	else
	{
		daoDanFlash.setVisible( false );
	}
	return;
}

function flashDaoDanP2()
{
	if($PlayerDaoDanNumP2 <= 0)
	{
		MyShineP2.setVisible(true);
		return;
	}
	
	if( isObject( aimFlyShapeBitmap.getBitmapFollowObj() ) )
	{
		if( daoDanFlashP2.isVisible() )
		{
			daoDanFlashP2.setVisible(false);
		}
		else
		{
			daoDanFlashP2.setVisible(true);
		}
		schedule( 200, 0, flashDaoDanP2 );
	}
	else
	{
		daoDanFlashP2.setVisible( false );
	}
	return;
}

function flashDaoDanTiShi( %index )
{
	if($PlayerDaoDanNum <= 0 && $PlayerDaoDanNumP2 <= 0)
	{
		daoDanTiShi.setVisible(false);
		$IsDaoDanTiShiShow = false;
		return;
	}
	
	if( !isObject( aimFlyShapeBitmap.getBitmapFollowObj() ) )
	{
		daoDanTiShi.setVisible(false);
		$IsDaoDanTiShiShow = false;
		return;
	}
	
	daoDanTiShi.setVisible( true );
	
	if( %index == 10 )
	{
		daoDanTiShi.setVisible( false );
		$IsDaoDanTiShiShow = false;
		return;
	}
	else
	{
		if( ( %index % 2 ) == 0 )
		{
			daoDanTiShi.setBitmap("art/gui/PlaneGui/Push1.png");
		}
		else
		{
			daoDanTiShi.setBitmap("art/gui/PlaneGui/Push2.png");
		}
		%index++;
		schedule(500, 0, flashDaoDanTiShi, %index );
	}
	return;
}

function flashPush( %index )
{
	if( %index == 10 )
	{
		PushButt.setVisible( false );
		return;
	}
	else
	{
		if( ( %index % 2 ) == 0 )
		{
			PushButt.setBitmap("art/gui/PlaneGui/Push1.png");
		}
		else
		{
			PushButt.setBitmap("art/gui/PlaneGui/Push2.png");
		}
		%index++;
		schedule(500, 0, flashPush, %index );
	}
	return;
}

function setBossWeaponBitmap( %id )
{
	if( %id != 2 )
	{
		BossName.setBitmap("");
		bossWeapon.setBitmap("");
		bossWeaponDamage1.setBitmap("");
		bossWeaponDamage2.setBitmap(""); 
		bossWeaponDamage3.setBitmap("");
		bossWeaponDamage4.setBitmap("");
		
		BossName.setVisible(false);
		bossWeapon.setVisible(false);
		bossWeaponDamage1.setVisible(false);
		bossWeaponDamage2.setVisible(false);
		bossWeaponDamage3.setVisible(false);
		bossWeaponDamage4.setVisible(false);
		
		bossTotalHealthFlash.setVisible(false);
		bossTotalHealthTop.setVisible(false);
		BossHealthBottom.setVisible(false);
		BossHealthFlash.setVisible(false);
		BossHealthTop.setVisible(false);
		return;
	}
	
	if( %id == 1 )
	{
		BossName.setBitmap("art/gui/playGui/bossYiGuanName.png");
		bossWeapon.setBitmap("art/gui/playGui/boss1Weapon1.png");
		bossWeaponDamage1.setBitmap("art/gui/playGui/boss1Weapon2.png");
		bossWeaponDamage2.setBitmap("art/gui/playGui/boss1Weapon3.png");
		bossWeaponDamage3.setBitmap("art/gui/playGui/boss1Weapon4.png");
		bossWeaponDamage4.setBitmap("art/gui/playGui/boss1Weapon5.png");
	}
	else if( %id == 2 )
	{
		BossName.setBitmap("");
		bossWeapon.setBitmap("");
		bossWeaponDamage1.setBitmap("");
		bossWeaponDamage2.setBitmap(""); 
		bossWeaponDamage3.setBitmap("");
		bossWeaponDamage4.setBitmap("");
		
		BossName.setVisible(true);
		bossWeapon.setVisible(true);
		bossWeaponDamage1.setVisible(true);
		bossWeaponDamage2.setVisible(true);
		bossWeaponDamage3.setVisible(true);
		bossWeaponDamage4.setVisible(true);
		
		bossTotalHealthFlash.setVisible(true);
		bossTotalHealthTop.setVisible(true);
		BossHealthBottom.setVisible(true);
		BossHealthFlash.setVisible(true);
		BossHealthTop.setVisible(true);
		
		BossName.setBitmap("art/gui/playGui/bossXieZiName.png");
		bossWeapon.setBitmap("art/gui/playGui/bossXieZiWeapon.png");
		bossWeaponDamage1.setBitmap("art/gui/playGui/bossXieZiWeapon_1.png");
		bossWeaponDamage2.setBitmap("art/gui/playGui/bossXieZiWeapon_2.png");
		bossWeaponDamage3.setBitmap("art/gui/playGui/bossXieZiWeapon_3.png");
		bossWeaponDamage4.setBitmap("");
	}
	else if( %id == 3 )
	{
		bossWeapon.setBitmap("art/gui/playGui/bossDianJuWeapon.png");
		bossWeaponDamage1.setBitmap("art/gui/playGui/bossDianJuWeapon_1.png");
		bossWeaponDamage2.setBitmap("art/gui/playGui/bossDianJuWeapon_2.png");
		bossWeaponDamage3.setBitmap("art/gui/playGui/bossDianJuWeapon_3.png");
		bossWeaponDamage4.setBitmap("");
	}
	else if( %id == 4 )
	{
		BossName.setBitmap("art/gui/playGui/bossCaiJueName.png");
		bossWeapon.setBitmap("art/gui/playGui/bossCaiJueWeapon.png");
		bossWeaponDamage1.setBitmap("art/gui/playGui/bossCaiJueWeapon_1.png");
		bossWeaponDamage2.setBitmap("art/gui/playGui/bossCaiJueWeapon_2.png"); 
		bossWeaponDamage3.setBitmap("art/gui/playGui/bossCaiJueWeapon_3.png");
		bossWeaponDamage4.setBitmap("art/gui/playGui/bossCaiJueWeapon_4.png");
	}
}
//淡黑填充游戏主界�?
function ClientCmdBackGroundFadeInBlack()
{
	if( !isObject( GroundFadeInBlack ) )
		return;
	$PlayerP1.setActionThread("xiuxi");
	playGuiContainer.setVisible( false );
	GroundFadeInBlack.setVisible(true);
	GroundFadeInBlack.setDrawPartOfBitmap(1, 0, 0, 0, 0, 16, 16);
	schedule(50, 0, "changeFadeInBackGround", 1 );
}

function changeFadeInBackGround( %index )
{
	if( %index > 46 )
	{
		if( !$GameOver )
		{
			%nodeObj = $PlayerP1.showJiFenNode;
			if (isObject(%nodeObj))
			{
				%nodeNameT = %nodeObj.getname();
			}
			else
			{
				%nodeNameT = "";
			}
			
			if( %nodeNameT $= "jiedian01_49_a4"
		 		|| %nodeNameT $= "jiedian01_268"
		 		|| %nodeNameT $= "jiedian01_389" )
			{
				$PlayerP1.stopMove();
				if( %nodeNameT $= "jiedian01_389" || %nodeNameT $= "jiedian01_268" )
				{
					if( $BossErGuanDead && $ZongBossDead )
					{
						$CANTStart = true;
						initPCBegin(1);
					}
					else
					{
						disconnect();
					}
				}
				else
				{
					disconnect();
				}
			}
			else
			{
				schedule(100, 0, setGunShakeState, 0, true);
				SetRespawnState(0, true);
				GroundFadeInBlack.setDrawPartOfBitmap(1, 0, 0, 47*16, 0, 16,  16 );
			}
	    	return;
		}
		else
		{
			initPCBegin(2);
			return;
		}
	}
	else
	{
		%index += 1;
		GroundFadeInBlack.setDrawPartOfBitmap(1, 0, 0, %index*16, 0, 16,  16 );
		schedule(50, 0, "changeFadeInBackGround", %index );
	}
	return;
}

function endIniPCPassLevel()
{
	sfxStopAll();
	%lastIndex = 9;
	%lastScore = GetIniValueNum( "Score"@%lastIndex, "Score", "./nameSort.hnb" );
	if( $PlayerAllScore > %lastScore )
	{
		if( isObject(QianMingGui) )
		{
			Canvas.schedule(2000, setContent, "QianMingGui" );
			return;
		}
	}
	
	disconnect();
}

function endIniPCGameover()
{
	sfxStopAll();
	showGameOver( true );
	%lastIndex = 9;
	%lastScore = GetIniValueNum( "Score"@%lastIndex, "Score", "./nameSort.hnb" );
	if( $PlayerAllScore > %lastScore )
	{
		if( isObject(QianMingGui) )
		{
			Canvas.schedule(2000, setContent, "QianMingGui" );
		}
		else
		{
			//echo("\c4there is something wrong with QianMingGui");
		}
	}
	else
	{
		if( $LoadingLevel == 1 )
		{
			%reset = reStartGame();
			if( %reset )
			{
				Canvas.schedule( 2000, setContent, "touBiWindow" );
			}
		}
		else
		{
			$PlayerP1.stopMove();
			disconnect();
		}
	}
}

function showGameOver( %isVisible )
{
	if( %isVisible )
		PlayGuiGameOver.setVisible( true );
	else
		PlayGuiGameOver.setVisible( false );
}
//淡白填充游戏主界�?
function ClientCmdBackGroundFadeInWhite( %nownode )
{
	if( !isObject( GroundFadeInBlack ) )
		return;
	
	setGunShakeState(0, false);
	GroundFadeInBlack.setVisible(true);
	playGuiContainer.setVisible( false );
	GroundFadeInBlack.setDrawPartOfBitmap(1, 0, 0, 47*16, 0, 16, 16);
	schedule(50, 0, "changeFadeInBackWhite", 47, %nownode );
}

function changeFadeInBackWhite( %index, %nownode )
{
	if( %index == 47 )
	{
		%control = $PlayerP1.client.gameCamera;
		%control.setVelocity("0 0 0");
		$PlayerP1.client.setControlObject(%control);
		if( %nownode.cameraPath !$= "" )
		{
			%control.setPathNode( %nownode );
		}
		$PlayerP1.setScale( "0 0 0" );//隐藏主角
		$PlayerP1.getCameraState();
		//需要显示npc信息的特殊点
		if( %nownode.getName() $= "jiedian01_01" )
		{
			setNpcShuoMing(1);
		}
		if( %nownode.getName() $= "jiedian01_05" )
		{
			setNpcShuoMing(2);
		}
		if( %nownode.getName() $= "jiedian01_21" )
		{
			setNpcShuoMing(3);
		}
		if( %nownode.getName() $= "jiedian01_52" )
		{
			setNpcShuoMing(4);//工人说话单独处理不显示图片等攻击信息
		}
		if( %nownode.getName() $= "jiedian01_44_a0" )
		{
			//setNpcShuoMing(5);
		}
		if( %nownode.getName() $= "jiedian01_252" )
		{
			setNpcShuoMing(6);
		}
		if( %nownode.getName() $= "zboss01_359" )
		{
			//setNpcShuoMing(7);
		}
	}
	if( %index < 1 )
	{
		GroundFadeInBlack.setVisible( false );
		return;
	}
	else
	{
		%test = %index - 1;
		GroundFadeInBlack.setDrawPartOfBitmap(1, 0, 0, %test*16, 0, 16,  16 );
		schedule(50, 0, "changeFadeInBackWhite", %test, %nownode );
	}
	return;
}

function clientCmdopenJiFenKuang()
{
	sfxPlayOnce(panel_Sound_shuZi01);
	jiFenKuang.setVisible( true );
	%extent = jiFenKuang.getExtent();
	%addY = mFloor( %extent.y / 10 );
	%extent.y = 0;
	jiFenKuang.setExtent( %extent );
	schedule(40, 0, setJiFenKuangExtentToBig, %addY );
}

function closeJiFenKuang()
{
	%extent = jiFenKuang.getExtent();
	%subY = mFloor( %extent.y / 10 );
	schedule(40, 0, setJiFenKuangExtentToSmall, %subY );
}

function setJiFenKuangExtentToBig( %addY )
{
	%extent = jiFenKuang.getExtent();
	if( ( %extent.y + %addY ) > 600 )
	{
		%extent.y = 600;
		jiFenKuang.setExtent( %extent );
		schedule(30, 0, showPlayerScore );
	}
	else
	{
		%extent.y += %addY;
		jiFenKuang.setExtent( %extent );
		schedule( 40, 0, setJiFenKuangExtentToBig, %addY );
	}
	return;
}

function setJiFenKuangExtentToSmall( %subY )
{
	%extent = jiFenKuang.getExtent();
	if( ( %extent.y - %subY ) < 0 )
	{
		%extent.y = 0;
		jiFenKuang.setExtent( %extent );
		jiFenKuang.setVisible( false );
		%extent.y = 600;
		jiFenKuang.setExtent( %extent );
		if( $GameOver )
		{
			$PlayerP1.schedule( 50, doAnimation, "siwang" );
			lookDeathAnimation();
			return;
		}
		
		%nodeObj = $PlayerP1.showJiFenNode;
		if (isObject(%nodeObj))
		{
			%nodeNameT = %nodeObj.getname();
		}
		else
		{
			%nodeNameT = "";
		}
		
		if( %nodeNameT $= "jiedian01_49_a4"
			 || %nodeNameT $= "jiedian01_268"
			 || %nodeNameT $= "jiedian01_389" )
		{
			ClientCmdBackGroundFadeInBlack();
		}
	}
	else
	{
		%extent.y -= %subY;
		jiFenKuang.setExtent( %extent );
		schedule( 40, 0, setJiFenKuangExtentToSmall, %subY );
	}
	return;
}

function showPlayerScore( )
{
	%allScore = 0;
	%hitNum = mFloor( ( ( $PlayerKillNpcNum * 3 ) / $PlayerUseAmmoCount ) *100 ) ;
	%hitScore = %hitNum * $MingZhongJiangLi;
	if( %hitNum != 0 )
	{
		%count1 = %hitScore / %hitNum;
		%allScore+= %hitScore;
	}
	%maxNum = %hitNum;
	
	%npcNum = $PlayerKillNpcNum;
	
	%npcScore = $PlayerKillNpcScore;
	if( %npcNum != 0 )
	{	
		%count2 = %npcScore / %npcNum;
		%allScore+= %npcScore;
	}
	if( %maxNum < %npcNum  )
	{
		%maxNum = %npcNum;
	}
	%bossNum = $PlayerKillBossNum;
	if( %bossNum != 0 )
	{
		%bossScore = $PlayerKillBossScore;
		%count3 = %bossScore / %bossNum;
		
		%allScore+= %bossScore;
	}
	if( %maxNum < %bossNum )
	{
		%maxNum = %bossNum;
	}
	if( $PlayerP1.getMaxDamage() - $PlayerP1.getDamageLevel() > 100000 )
	{
		%percent = $PlayerP1.getDamageLevel() / ( $PlayerP1.getMaxDamage() - 100000 );
	}
	else
	{
		%percent = 1;
	}
	%healthNum = mFloor( ( 1 - %percent ) * 100 );
	if( %healthNum != 0 )
	{
		%healthScore = %healthNum * $ShengMingZhiJiangLi;
		%count4 = %healthScore / %healthNum;
		%allScore+= %healthScore;
	}
	if( %maxNum < %healthNum )
	{
		%maxNum = %healthNum;
	}
	%daoDanNum = $PlayerDaoDanNum + $PlayerDaoDanNumP2;
	if( %daoDanNum != 0 )
	{
		%daoDanScore = %daoDanNum * $DaoDanJiangLi;
		%count5 = %daoDanScore / %daoDanNum;
		
		%allScore+= %daoDanScore;
	}
	if( %maxNum < %daoDanNum )
	{
		%maxNum = %daoDanNum;
	}

    if( %maxNum != 0 )
	{
		%curZongFen = $PlayerScore;
		%count6 = mFloor( %curZongFen / %maxNum );
		
	
		%allScore += %curZongFen;
		%count7 = mFloor( %allScore / %maxNum );
	}
	$JiFenKuangMaxNum = %maxNum;
	setHitNum( %hitNum, 1, 0 );
	setHitScore( %hitScore, %count1, 0 );
	setNpcNum( %npcNum, 1, 0 );
	setNpcScore( %npcScore, %count2, 0 );
	setBossNum( %bossNum, 1, 0 );
	setBossScore( %bossScore, %count3, 0  );
	setHealthNum( %healthNum, 1, 0 );
	setHealthScore( %healthScore, %count4, 0 );
	setDaoDanNum( %daoDanNum, 1, 0 );
	setDaoDanScore( %daoDanScore, %count5, 0 );
	setJiFenZong( %curZongFen, %count6, 0 );
	setAllScore( %allScore, %count7, 0 );
	$PlayerAllScore = %allScore;
}

function setJiFenZong( %num, %count, %addScore )
{
    if (%num > 999999999) {
        %num = 999999999;
    }

	if(  %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setJiFenZong, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setJiFenZong, %num, %count, %addScore );
		}
		
	}
	else
	{
		%score = %num;
	}
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
	jiFenZong1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenZong2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenZong3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenZong4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenZong5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenZong6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	jiFenZong7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 16, 0,  16, 21 );
	jiFenZong8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 16, 0,  16, 21 );
	jiFenZong9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 16, 0,  16, 21 );
	return;
}

function setHitNum( %num, %count, %addScore )
{
    if (%num > 99) {
        %num = 99;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore  += %count;
		%score = %addScore;
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setHitNum, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setHitNum, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10 );
	%score = %score % 10;
	%score2 = %score;
	jiFenHit1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenHit2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	return;
}

function setHitScore( %num, %count, %addScore )
{
    if (%num > 999999) {
        %num = 999999;
    }

	if(  %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setHitScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setHitScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 100000 );
	%score = %score % 100000;
	%score2 = mFloor( %score / 10000 );
	%score = %score % 10000;
	%score3 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score4 = mFloor( %score / 100 );
	%score = %score % 100;
	%score5 = mFloor( %score / 10 );
	%score = %score % 10;
	%score6 = %score;
	jiFenHitScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenHitScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenHitScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenHitScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenHitScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenHitScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	return;
}

function setNpcNum( %num, %count, %addScore )
{
    if (%num > 9999) {
        %num = 9999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setNpcNum, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setNpcNum, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score2 = mFloor( %score / 100 );
	%score = %score % 100;
	%score3 = mFloor( %score / 10 );
	%score = %score % 10;
	%score4 = %score;
	jiFenNpc1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenNpc2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenNpc3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenNpc4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	return;
}

function setNpcScore( %num, %count, %addScore )
{
    if (%num > 99999999) {
        %num = 99999999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setNpcScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setNpcScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10000000 );
	%score = %score % 10000000;
	%score2 = mFloor( %score / 1000000 );
	%score = %score % 1000000;
	%score3 = mFloor( %score / 100000 );
	%score = %score % 100000;
	%score4 = mFloor( %score / 10000 );
	%score = %score % 10000;
	%score5 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score6 = mFloor( %score / 100 );
	%score = %score % 100;
	%score7 = mFloor( %score / 10 );
	%score = %score % 10;
	%score8 = %score;
	jiFenNpcScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenNpcScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenNpcScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenNpcScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenNpcScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenNpcScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	jiFenNpcScore7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 16, 0,  16, 21 );
	jiFenNpcScore8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 16, 0,  16, 21 );
	return;
}

function setBossNum( %num, %count, %addScore )
{
    if (%num > 99) {
        %num = 99;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setBossNum, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setBossNum, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10 );
	%score = %score % 10;
	%score2 = %score;
	jiFenBoss1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenBoss2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	return;
}

function setBossScore( %num, %count, %addScore )
{
    if (%num > 99999999) {
        %num = 99999999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setBossScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setBossScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10000000 );
	%score = %score % 10000000;
	%score2 = mFloor( %score / 1000000 );
	%score = %score % 1000000;
	%score3 = mFloor( %score / 100000 );
	%score = %score % 100000;
	%score4 = mFloor( %score / 10000 );
	%score = %score % 10000;
	%score5 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score6 = mFloor( %score / 100 );
	%score = %score % 100;
	%score7 = mFloor( %score / 10 );
	%score = %score % 10;
	%score8 = %score;
	jiFenBossScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenBossScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenBossScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenBossScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenBossScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenBossScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	jiFenBossScore7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 16, 0,  16, 21 );
	jiFenBossScore8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 16, 0,  16, 21 );
	return;
}

function setHealthNum( %num, %count, %addScore )
{
    if (%num > 999) {
        %num = 999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setHealthNum, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setHealthNum, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 100 );
	%score = %score % 100;
	%score2 = mFloor( %score / 10 );
	%score = %score % 10;
	%score3 = %score;
	jiFenHealth1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenHealth2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenHealth3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	return;
}

function setHealthScore( %num, %count, %addScore )
{
    if (%num > 99999999) {
        %num = 99999999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setHealthScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setHealthScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10000000 );
	%score = %score % 10000000;
	%score2 = mFloor( %score / 1000000 );
	%score = %score % 1000000;
	%score3 = mFloor( %score / 100000 );
	%score = %score % 100000;
	%score4 = mFloor( %score / 10000 );
	%score = %score % 10000;
	%score5 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score6 = mFloor( %score / 100 );
	%score = %score % 100;
	%score7 = mFloor( %score / 10 );
	%score = %score % 10;
	%score8 = %score;
	jiFenHealthScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenHealthScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenHealthScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenHealthScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenHealthScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenHealthScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	jiFenHealthScore7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 16, 0,  16, 21 );
	jiFenHealthScore8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 16, 0,  16, 21 );
	return;
}

function setDaoDanNum( %num, %count, %addScore )
{
    if (%num > 99) {
        %num = 99;
    }

	if(  %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setDaoDanNum, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setDaoDanNum, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 10 );
	%score = %score % 10;
	%score2 = %score;
	jiFenDaoDan1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenDaoDan2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	return;
}

function setDaoDanScore( %num, %count, %addScore )
{
    if (%num > 999999) {
        %num = 999999;
    }

	if(  %addScore< %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setDaoDanScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setDaoDanScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
	}
	%score1 = mFloor( %score / 100000 );
	%score = %score % 100000;
	%score2 = mFloor( %score / 10000 );
	%score = %score % 10000;
	%score3 = mFloor( %score / 1000 );
	%score = %score % 1000;
	%score4 = mFloor( %score / 100 );
	%score = %score % 100;
	%score5 = mFloor( %score / 10 );
	%score = %score % 10;
	%score6 = %score;
	jiFenDaoDanScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenDaoDanScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenDaoDanScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenDaoDanScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenDaoDanScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenDaoDanScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	return;
}

function setAllScore( %num, %count, %addScore )
{
    if (%num > 999999999) {
        %num = 999999999;
    }

	if( %addScore < %num )
	{
        if (%count < 1) {
            %count = 1;
        }
		%addScore += %count;
		%score = %addScore;
		
		if( $JiFenKuangMaxNum != 0 )
		{
			%timer = mFloor( 10000 / $JiFenKuangMaxNum );
			schedule(%timer, 0, setAllScore, %num, %count, %addScore );
		}
		else
		{
			schedule(50, 0, setAllScore, %num, %count, %addScore );
		}
	}
	else
	{
		%score = %num;
		schedule( 1000, 0, closeJiFenKuang );
	}
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
	jiFenAllScore1.setDrawPartOfBitmap( 1, 0, 0, %score1 * 16, 0,  16, 21 );
	jiFenAllScore2.setDrawPartOfBitmap( 1, 0, 0, %score2 * 16, 0,  16, 21 );
	jiFenAllScore3.setDrawPartOfBitmap( 1, 0, 0, %score3 * 16, 0,  16, 21 );
	jiFenAllScore4.setDrawPartOfBitmap( 1, 0, 0, %score4 * 16, 0,  16, 21 );
	jiFenAllScore5.setDrawPartOfBitmap( 1, 0, 0, %score5 * 16, 0,  16, 21 );
	jiFenAllScore6.setDrawPartOfBitmap( 1, 0, 0, %score6 * 16, 0,  16, 21 );
	jiFenAllScore7.setDrawPartOfBitmap( 1, 0, 0, %score7 * 16, 0,  16, 21 );
	jiFenAllScore8.setDrawPartOfBitmap( 1, 0, 0, %score8 * 16, 0,  16, 21 );
	jiFenAllScore9.setDrawPartOfBitmap( 1, 0, 0, %score9 * 16, 0,  16, 21 );
	return;
}

function clientCmdshowJiJiaSunHuaiPercent( %percent )
{
	if( jiJiaSunHuaiPercent.isVisible() )
	{
		return;
	}
	
	jiJiaSunHuaiPercent.setBitmap( "art/gui/playGui/damagePercent"@%percent );
	jiJiaSunHuaiPercent.setVisible( true );
	changeJiJiaSunHuaiToBig( -65, -20, 51, 15, %percent );
}

function changeJiJiaSunHuaiToBig( %detX, %detY, %addX, %addY )
{
	%pos = jiJiaSunHuaiPercent.getPosition();
	%extent = jiJiaSunHuaiPercent.getExtent();
	if( %pos.x > 425 || %extent.x < 512 )
	{
		if( %pos.x > 425 )
		{
			%pos.x += %detX; 
			%pos.y += %detY;
			jiJiaSunHuaiPercent.setPosition( %pos.x, %pos.y );
		}
		if( %extent.x < 512 )
		{
			%extent.x += %addX;
			%extent.y += %addY;
			jiJiaSunHuaiPercent.setExtent( %extent );
		}
		schedule(30, 0, changeJiJiaSunHuaiToBig, %detX, %detY, %addX, %addY );
		return;
	}
	else
	{
		schedule(1500, 0, changeJiJiaSunHuaiToSmall, %detX, %detY, %addX, %addY );
		return;
	}
	return;
}

function changeJiJiaSunHuaiToSmall( %detX, %detY, %addX, %addY )
{
	%pos = jiJiaSunHuaiPercent.getPosition();
	%extent = jiJiaSunHuaiPercent.getExtent();
	if( %pos.x < 1070 || %extent.x > 2 )
	{
		if( %pos.x < 1070 )
		{
			%pos.x -= %detX; 
			%pos.y -= %detY;
			jiJiaSunHuaiPercent.setPosition( %pos.x, %pos.y );
		}
		if( %extent.x > 2 )
		{
			%extent.x -= %addX;
			%extent.y -= %addY;
			jiJiaSunHuaiPercent.setExtent( %extent );
		}
		schedule(30, 0, changeJiJiaSunHuaiToSmall, %detX, %detY, %addX, %addY );
		return;
	}
	$JiJiaPercentShowOver = true;
	jiJiaSunHuaiPercent.setVisible( false );
	return;
}

function clientCmdshowZhunBeiZhanDou()
{
	zhunBeiZhanDou.setVisible(true);
	zhunBeiZhanDou.schedule( 1000, setVisible, false );
	schedule(2000, 0, setPlayerCanshoot, true);
}

function clientCmdShowGongXiGuoGuan()
{
        RightArmDamage3.setVisible(false);
        BodyDamage3.setVisible(false);
        LeftArmDamage3.setVisible(false);
	EyeDamage3.setVisible(false);
			
	if($HideHead == true)
	{
		HeadDamage3.setVisible(false);
		$HideHead = false;
	}
			
	if($HideShow == true)
	{
		RightClawDamage3.setVisible(false);
		LeftClawDamage3.setVisible(false);
		PoisonFogDamage3.setVisible(false);
		TailDamage3.setVisible(false);
		BothClawDamage3.setVisible(false);
		$HideShow = false;
	}
	if($HideShowFinal == true)
	{
		LeftHand1Damage3.setVisible(false);
		RightHand1Damage3.setVisible(false);
		LeftHand2Damage3.setVisible(false);
		RightHand2Damage3.setVisible(false);
		MouthDamage3.setVisible(false);
		$HideShowFinal = false;
	}
    $ShowShine = false;
	gongXiGuoGuan.setVisible( true );
	gongXiGuoGuan.schedule( 2000, setVisible, false ); 
}

function lookDeathAnimation()
{
	playGuiContainer.setVisible( false );
	if(!isobject(MissionGroup))
		return;
	
	//%this.setMoveSpeed(0.5);
	%simCounting = MissionGroup.getCount();
	for(%i = 0; %i < %simCounting; %i++)
	{
		%obj = MissionGroup.getObject(%i);
		if( isObject( %obj ) )
		{
			%objClassName = %obj.getClassName();
			%objName = %obj.getName();
			if("Path" $= %objClassName)
			{	
				if( %objName $= "zhujiaohangpailujing" )
				{
					%playerPos = $PlayerP1.getPosition();
					%number = %obj.getCount();
					for( %i = 0; %i < %number; %i++ )
					{
						%node = %obj.getObject(%i);
						%pos = %node.getPosition();
						%pos.x -= 356.949;
						%pos.y -= 517.814;
						%pos.z -= 512.02;
						%pos.x += %playerPos.x;
						%pos.y += %playerPos.y;
						%pos.z += %playerPos.z;
						%node.setPosition( %pos );
					}
					%control = $PlayerP1.client.gameCamera;
					%control.setVelocity("0 0 0");
					$PlayerP1.client.setControlObject(%control);
					%control.followNode = zhujiaohangpailujing.getObject(0);
   					%pathString = "zhujiaohangpailujing";
  					%control.pathString = %pathString;
  					%control.pathId = 0;
  					%curPathName = getWord( %pathString, %control.pathId );

   					%control.reset();
   					%control.path = %curPathName;   
   					%control.pushPath(%curPathName);      
   					%control.popFront();
				
					for( %i = 0; %i < %number; %i++ )
					{
						%node = %obj.getObject(%i);
						%pos = %node.getPosition();
						%pos.x += ( 356.949 - %playerPos.x );
						%pos.y += ( 517.814 - %playerPos.y );
						%pos.z += ( 512.02 - %playerPos.z );
						%node.setPosition( %pos );
					}
					break;
				}
			}
		}
	}
}

//窗口抖动,此方法备�?现在可以直接调用shakeCamera
function clientCmdDoRecoil(%type)
{
   switch$ (%type)
   {
      case "min":
         $mvPitch += getrandom(-2, -3)/1000;
         $mvYaw += getrandom(-2, 2)/1000;
      case "Light":
         $mvPitch += getrandom(-4, -6)/1000;
         $mvYaw += getrandom(-4, 4)/1000;
      case "Moderate":
         $mvPitch += getrandom(-1, -2)/100;
         $mvYaw += getrandom(-8,8)/1000;
      case "High":
         $mvPitch += getrandom(-3, -5)/100;
         $mvYaw += getrandom(-8, 8)/1000;
      case "Heavy":
         $mvPitch += getrandom(-4, -6)/100;
         $mvYaw += getrandom(-2, 2)/100;
      case "Extreme":
         $mvPitch += getrandom(-1, -2)/10;
         $mvYaw += getrandom(-4, 4)/100;
      case "Hit":
         $mvPitch += getrandom(4, -4)/100;
         $mvYaw += getrandom(-8, 8)/1000;
      case "BumpRight":
         $mvYaw += getrandom(1, 2)/33;
   }
}

$ShowRightArm = false;
$ShowBody = false;
$ShowLeftArm = false;
$ShowEye = false;
$RedShow = false;

$ShowHead = false;
$RedShow1 = false;

$ShowRightClaw = true;
$ShowLeftClaw = false;
$ShowPoisonFog = false;
$ShowTail1 = false;
$ShowTail2 = false;
$ShowTail3 = false;
$ShowBothClaw = false;
$RedShow2 = false;

$ShowLeftHand1 = true;
$ShowRightHand1 = false;
$ShowLeftHand2 = false;
$ShowMouth = false;
$ShowRightHand2 = false;
$RedShow3 = false;

$ShowShine = false;

function ShowRightDamage()
{
	if($ShowRightArm == true)
	{
	    if(RightArmDamage2.isVisible())
    	{
    	RightArmDamage2.setVisible(false);
         }
     	else
     	{
    	RightArmDamage2.setVisible(true);
        }
    }
	else
	return;
	schedule(100, 0, ShowRightDamage);
	return;
}
function ShowBodyDamage()
{
	if($ShowBody == true)
	{
		if(BodyDamage2.isVisible())
    	{
    	BodyDamage2.setVisible(false);
         }
     	else
     	{
    	BodyDamage2.setVisible(true);
        }
	}
	else
	return;
	schedule(100, 0, ShowBodyDamage);
	return;
}
function ShowLeftArmDamage()
{
	if($ShowLeftArm == true)
	{
		if(LeftArmDamage2.isVisible())
		{
			LeftArmDamage2.setVisible(false);
		}
		else
		{
			LeftArmDamage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowLeftArmDamage);
	return;
}

function ShowEyeDamage(%show)
{
	if($ShowEye == true)
	{
		if(%show == true)
		{
			if($RedShow == true)
			EyeShine.setVisible(false);
			else
			EyeDamage2.setVisible(false);
			%show = false;
		}
		else
		{
			if($RedShow == true)
			EyeShine.setVisible(true);
			else
			EyeDamage2.setVisible(true);
			%show = true;
		}
	}
	else
		return;
	schedule(100, 0, ShowEyeDamage, %show);
		return;
}

function ShowHeadDamage(%show)
{
	if($ShowHead == true)
	{
		if(%show == true)
		{
			if($RedShow1 == true)
			HeadShine.setVisible(false);
			else
			HeadDamage2.setVisible(false);
			%show = false;
		}
		else
		{
			if($RedShow1 == true)
			HeadShine.setVisible(true);
			else
			HeadDamage2.setVisible(true);
			%show = true;
		}
	}
	else
	return;
	schedule(100, 0, ShowHeadDamage, %show);
	return;
}

function ShowRightClawDamage()
{
	if($ShowRightClaw == true)
	{
		if(RightClawDamage2.isVisible())
		{
			RightClawDamage2.setVisible(false);
		}
		else
		{
			RightClawDamage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowRightClawDamage);
	return;
}

function ShowLeftClawDamage()
{
	if($ShowLeftClaw == true)
	{
	    if(LeftClawDamage2.isVisible())
    	{
    		LeftClawDamage2.setVisible(false);
        }
     	else
     	{
    		LeftClawDamage2.setVisible(true);
        }
    }
	else
	return;
	schedule(100, 0, ShowLeftClawDamage);
	return;
}

function ShowPoisonFogDamage()
{
	if($ShowPoisonFog == true)
	{
		if(PoisonFogDamage2.isVisible())
		{
			PoisonFogDamage2.setVisible(false);
		}
		else
		{
			PoisonFogDamage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowPoisonFogDamage);
	return;
}

function ShowTail1Damage()
{
	if($ShowTail1 == true)
	{
		if(TailDamage21.isVisible())
		{
			TailDamage21.setVisible(false);
		}
		else
		{
			TailDamage21.setVisible(true);
		}
	}
	else
		return;
	schedule(100, 0, ShowTail1Damage);
	return;
}

function ShowTail2Damage()
{
	if($ShowTail2 == true)
	{
		if(TailDamage22.isVisible())
		{
			TailDamage22.setVisible(false);
		}
		else
		{
			TailDamage22.setVisible(true);
		}
	}
	else
		return;
	schedule(100, 0, ShowTail2Damage);
	return;
}

function ShowTail3Damage()
{
	if($ShowTail3 == true)
	{
		if(TailDamage23.isVisible())
		{
			TailDamage23.setVisible(false);
		}
		else
		{
			TailDamage23.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowTail3Damage);
	return;
}

function ShowBothClawDamage(%show)
{
	if($ShowBothClaw == true)
	{
		if(%show == true)
		{
			if($RedShow2 == true)
			BothClawShine.setVisible(false);
			else
			BothClawDamage2.setVisible(false);
			%show = false;
		}
		else
		{
			if($RedShow2 == true)
			BothClawShine.setVisible(true);
			else
			BothClawDamage2.setVisible(true);
			%show = true;
		}
	}
	else
	return;
	schedule(100, 0, ShowBothClawDamage, %show);
	return;
}

function ShowLeftHand1Damage()
{
	if($ShowLeftHand1 == true)
	{
		if(LeftHand1Damage2.isVisible())
		{
			LeftHand1Damage2.setVisible(false);
		}
		else
		{
			LeftHand1Damage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowLeftHand1Damage);
	return;
}

function ShowRightHand1Damage()
{
	if($ShowRightHand1 == true)
	{
		if(RightHand1Damage2.isVisible())
		{
			RightHand1Damage2.setVisible(false);
		}
		else
		{
			RightHand1Damage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowRightHand1Damage);
	return;
}

function ShowLeftHand2Damage()
{
	if($ShowLeftHand2 == true)
	{
		if(LeftHand2Damage2.isVisible())
		{
			LeftHand2Damage2.setVisible(false);
		}
		else
		{
			LeftHand2Damage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowLeftHand2Damage);
	return;
}

function ShowMouthDamage()
{
	if($ShowMouth == true)
	{
		if(MouthDamage2.isVisible())
		{
			MouthDamage2.setVisible(false);
		}
		else
		{
			MouthDamage2.setVisible(true);
		}
	}
	else
	return;
	schedule(100, 0, ShowMouthDamage);
	return;
}

function ShowRightHand2Damage(%show)
{
	if($ShowRightHand2 == true)
	{
		if(%show == true)
		{
			if($RedShow3 == true)
			RightHand2Shine.setVisible(false);
			else
			RightHand2Damage2.setVisible(false);
			%show = false;
		}
		else
		{
			if($RedShow3 == true)
			RightHand2Shine.setVisible(true);
			else
			RightHand2Damage2.setVisible(true);
			%show = true;
		}
	}
	else
		return;
	schedule(100, 0, ShowRightHand2Damage, %show);
	return;
}

function setCollision( %name, %state )
{
	if( isObject(%name) )
	{
		%name.setCollisionState( %state );
	}
}

function initBloodBagFly(%index)
{
	bloodBag.setPosition(445, 279 );
	bloodBag.setExtent( 400, 250 );
	if( %index == 1 )
	{
		bloodBag.setBitmap("art/gui/NpcShuoMing/bloodPlus3000.png");
	}
	else
	{
		bloodBag.setBitmap("art/gui/NpcShuoMing/bloodPlus1000.png");
	}
	bloodBag.setVisible( true );
	//%p_x = (1081-445)/20;
	%p_x = (645-445)/20;
	%p_y = (700-279)/20;
	%e_x = (400-40)/20;
	%e_y = (250-25)/20;
	bloodBagFly(%p_x, %p_y, %e_x, %e_y, 0 );
}

function bloodBagFly( %p_x, %p_y, %e_x, %e_y, %count )
{
	if( %count >= 20 )
	{
		bloodBag.setVisible( false );
		return;
	}
	bloodBag.position.x += %p_x;
	bloodBag.position.y += %p_y;
	bloodBag.extent.x -= %e_x;
	bloodBag.extent.y -= %e_y;
	schedule( 50, 0, bloodBagFly, %p_x, %p_y, %e_x, %e_y, %count++ );
}
function initAmmoBagFly()
{
	AmmoBag.setPosition(445, 279 );
	AmmoBag.setExtent( 400, 250 );
	AmmoBag.setVisible( true );
	%p_x = (70-445)/20;
	%p_y = (700-279)/20;
	%e_x = (400-40)/20;
	%e_y = (250-25)/20;
	AmmoBagBagFly(%p_x, %p_y, %e_x, %e_y, 0 );
}
function AmmoBagBagFly( %p_x, %p_y, %e_x, %e_y, %count )
{
	if( %count >= 20 )
	{
		AmmoBag.setVisible( false );
		return;
	}
	AmmoBag.position.x += %p_x;
	AmmoBag.position.y += %p_y;
	AmmoBag.extent.x -= %e_x;
	AmmoBag.extent.y -= %e_y;
	schedule( 50, 0, AmmoBagBagFly, %p_x, %p_y, %e_x, %e_y, %count++ );
}

function initAmmoBagFlyP2()
{
	AmmoBagP2.setPosition(445, 279 );
	AmmoBagP2.setExtent( 400, 250 );
	AmmoBagP2.setVisible( true );
	%p_x = (1280-445)/20;
	%p_y = (700-279)/20;
	%e_x = (400-40)/20;
	%e_y = (250-25)/20;
	AmmoBagBagFlyP2(%p_x, %p_y, %e_x, %e_y, 0 );
}

function AmmoBagBagFlyP2( %p_x, %p_y, %e_x, %e_y, %count )
{
	if( %count >= 20 )
	{
		AmmoBagP2.setVisible( false );
		return;
	}
	AmmoBagP2.position.x += %p_x;
	AmmoBagP2.position.y += %p_y;
	AmmoBagP2.extent.x -= %e_x;
	AmmoBagP2.extent.y -= %e_y;
	schedule( 50, 0, AmmoBagBagFlyP2, %p_x, %p_y, %e_x, %e_y, %count++ );
}

//npc说明相关
function setNpcShuoMing( %id )
{
	hideNpcShuoMing();
	if( %id == 1 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction01.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
	}
	else if( %id == 2 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction02.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
		warningInformation.setVisible(true);
		warningInformationFlicker(2, 0);
	}
	else if( %id == 3 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction03.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
		warningInformation.setVisible(true);
		warningInformationFlicker(3, 0);
	}
	else if( %id == 4 )
	{
		warningInformationShow(4);
	}
	else if( %id == 5 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction01.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
	}
	else if( %id == 6 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction04.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
	}
	else if( %id == 7 )
	{
		bossIntroduction.setBitmap("art/gui/NpcShuoMing/NPCIntroduction01.png");
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
		bossIntroduction.setVisible(true);
		showNpcShuoMing(0);
	}
}

function showNpcShuoMing(%val)
{
	if(!bossIntroduction.isVisible())
	{
		return;
	}
	
	if(%val < 13)
	{
		%posY = -384 + %val * 30;
	}
	else if(%val == 13)
	{
		%posY = 20;
	}
	else
	{
		bossIntroduction.setPosition(670, 20);
		return;
	}
		
	bossIntroduction.setPosition(670, %posY);
	
	%val++;
	schedule(60, 0, showNpcShuoMing, %val);
}

function hideNpcShuoMing()
{
	bossIntroduction.setPosition(670, -384);
	bossIntroduction.setExtent(680, 384);
	bossIntroduction.setVisible(false);
	
	warningInformation.setPosition(265, 560);
	warningInformation.setExtent(827, 64);
	warningInformation.setVisible(false);
}

function warningInformationFlicker(%type, %val)
{
	if(!warningInformation.isVisible())
	{
		return;
	}
	
	if(%type == 2)
	{
		if(%val % 2 == 0)
		{
			warningInformation.setBitmap("art/gui/NpcShuoMing/warnInformation01.png");
		}
		else
		{
			warningInformation.setBitmap("art/gui/NpcShuoMing/warnInformation02.png");
		}
	}
	else if(%type == 3)
	{
		if(%val % 2 == 0)
		{
			warningInformation.setBitmap("art/gui/NpcShuoMing/warnInformation03.png");
		}
		else
		{
			warningInformation.setBitmap("art/gui/NpcShuoMing/warnInformation04.png");
		}
	}
	
	%val++;
	schedule(200, 0, warningInformationFlicker, %type, %val);
}

function warningInformationShow(%type)
{
	if(%type == 4)
	{
		warningInformation.setVisible(false);
		warningInformation.schedule(600, setVisible, true);
		warningInformation.schedule(600, setBitmap, "art/gui/NpcShuoMing/warnInformation05.png");
		warningInformation.schedule(5500, setBitmap, "art/gui/NpcShuoMing/warnInformation06.png");
		warningInformation.schedule(11000, setBitmap, "art/gui/NpcShuoMing/warnInformation07.png");
		warningInformation.schedule(18500, setBitmap, "art/gui/NpcShuoMing/warnInformation08.png");
		warningInformation.schedule(23000, setBitmap, "art/gui/NpcShuoMing/warnInformation09.png");
		return;
	}
}

//lxy
//add some functions
//check the safe belt state
function checkAnquandai()
{
	if( !PlayGui.isAwake() )
	{
		return;
	}
	
	%safe = PCVRGetBeltsafe();
	if( %safe || $player1State < 0 )
	{
		//if safe do nothing
		anquandai2.setVisible( false );
		return;
	}
	else
	{
		//waring
		anquandai2.setBitmap("art/gui/Rotation/anquandai2.png");
		anquandai2.setVisible( true );
		safeBeltWaring(0);
	}
	
	%safeP2 = PCVRGetBeltsafeP2();
	if( %safeP2 || $player2State < 0 )
	{
		//if safe do nothing
		anquandai2P2.setVisible( false );
		return;
	}
	else
	{
		//waring
		anquandai2P2.setBitmap("art/gui/Rotation/anquandai2.png");
		anquandai2P2.setVisible( true );
		safeBeltWaringP2(0);
	}
}

function safeBeltWaring(%value)
{
	if( PCVRGetBeltsafe() || !PlayGui.isAwake() || !anquandai2.isVisible() || $player1State < 0)
	{
		if ($SafeBeltIndex == 1 
		&& (($player1State >= 0 && PCVRGetBeltsafe()) || $player1State < 0)
		&& (($player2State >= 0 && PCVRGetBeltsafeP2()) || $player2State < 0))
		{
			$urgentStopRotate = false;
			urgentShow.setBitmap("art/gui/Rotation/open.png");
			urgentShow.setVisible(true);
		}
		anquandai2.setBitmap("art/gui/Rotation/anquandai2.png");
		anquandai2.setVisible( false );
		return;
	}
	
	if( %value )
	{
		anquandai2.setBitmap("art/gui/Rotation/anquandai2.png");
	}
	else
	{
		anquandai2.setBitmap("art/gui/Rotation/anquandai3.png");
		schedule(100, 0, sfxPlayOnce, soundanquandai);
	}
	
	schedule( 1000, 0, safeBeltWaring, !%value );
}

function safeBeltWaringP2(%value)
{
	if( PCVRGetBeltsafeP2() || !PlayGui.isAwake() || !anquandai2P2.isVisible() || $player2State < 0)
	{
		if ($SafeBeltIndex == 1 
		&& (($player1State >= 0 && PCVRGetBeltsafe()) || $player1State < 0)
		&& (($player2State >= 0 && PCVRGetBeltsafeP2()) || $player2State < 0))
		{
			$urgentStopRotate = false;
			urgentShow.setBitmap("art/gui/Rotation/open.png");
			urgentShow.setVisible(true);
		}
		anquandai2P2.setBitmap("art/gui/Rotation/anquandai2.png");
		anquandai2P2.setVisible( false );
		return;
	}
	
	if( %value )
	{
		anquandai2P2.setBitmap("art/gui/Rotation/anquandai2.png");
	}
	else
	{
		anquandai2P2.setBitmap("art/gui/Rotation/anquandai3.png");
		schedule(100, 0, sfxPlayOnce, soundanquandai);
	}
	
	schedule( 1000, 0, safeBeltWaringP2, !%value );
}

function safeBeltLoosen()
{//warn("safeBeltLoosen=============================");
	if ($SafeBeltIndex == 2)
	{
		return;
	}
	//stop rotate
	//safe-belt warning
	//toubigui
	//playgui
	//qianminggui
	if ( !theoraPlayCtrl.isPlaying() 
		&& touBiWindow.isAwake() 
		&& !anquandai1.isVisible() )
	{
		anquandai1.setVisible(true);
	}
	else if( PlayGui.isAwake() )
	{
		if (!anquandai2.isVisible() && $player1State >= 0)
		{
			setTurnRotation(0, 0);
			anquandai2.setBitmap("art/gui/Rotation/anquandai2.png");
			anquandai2.setVisible( true );
			safeBeltWaring(0);
		}
		
		if ($SafeBeltIndex == 1 && $player1State >= 0)
		{
			$urgentStopRotate = true;
			urgentShow.setBitmap("art/gui/Rotation/close.png");
			urgentShow.setVisible(true);
		}
	}
}

function safeBeltLoosenP2()
{//warn("safeBeltLoosen=============================");
	if ($SafeBeltIndex == 2)
	{
		return;
	}
	//stop rotate
	//safe-belt warning
	//toubigui
	//playgui
	//qianminggui
	if ( !theoraPlayCtrl.isPlaying() 
		&& touBiWindow.isAwake() 
		&& !anquandai1P2.isVisible() )
	{
		anquandai1P2.setVisible(true);
	}
	else if( PlayGui.isAwake() )
	{
		if (!anquandai2P2.isVisible() && $player2State >= 0)
		{
			setTurnRotation(0, 0);
			
			anquandai2P2.setBitmap("art/gui/Rotation/anquandai2.png");
			anquandai2P2.setVisible( true );
			safeBeltWaringP2(0);
		}
		
		if ($SafeBeltIndex == 1 && $player2State >= 0)
		{
			$urgentStopRotate = true;
			urgentShow.setBitmap("art/gui/Rotation/close.png");
			urgentShow.setVisible(true);
		}
	}
}

function SetRespawnState(%index, %flag)
{
	echo("SetRespawnState======================================", %index, " " , %flag);
	if (%index == 0)
	{
		if (%flag && $player1State == 1)
		{
			PCVRSetRespawnStateP2(1, %flag);
		}
		if (%flag && $player2State == 1)
		{
			//change
			PCVRSetRespawnStateP2(2, %flag);
		}
	}
	else if (%index == 1)
	{
		PCVRSetRespawnStateP2(1, %flag);
	}
	else if (%index == 2)
	{
		//change
		PCVRSetRespawnStateP2(2, %flag);
	}
}

function setGunShakeState(%index, %flag)
{echo("setGunShakeState====================================================", %index, " " , %flag);
	if (%index == 0)
	{//two player
		if (!%flag)
		{
			//false
			PCVRSetShakeStateP2(0, %flag);
			return;
		}
		
		if (%flag && $player1State == 1)
		{
			//true
			PCVRSetShakeStateP2(1, %flag);
		}
		
		if (%flag && $player2State == 1)
		{
			//true
			PCVRSetShakeStateP2(2, %flag);
		}
	}
	else if (%index == 1)
	{//player1
		if (%flag && $player1State == 1)
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
		if (%flag && $player2State == 1)
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

function setGunShakeLevel(%index, %level)
{
	if (%level < 0)
	{
		return;
	}
	echo("levellevellevellevel========after============================================",%index, " " ,%level);
	PCVRSetShakeLevelP2(%index, %level);
}

function setGunZhuandongLevel(%index, %level)
{
	if (%level < 0)
	{
		return;
	}
	
	PCVRSetZhuandongLevelP2(%index, %level);
}

//rotate
function judgeRotateState(%node, %time)
{//echo("node    ", %node.turn, " " , %node.name);
	if( %time > 0 )
	{
		setTurnRotation(0, 0);
		setTimeToTurn(%node, %time);
	}
	else if( %node.stop $= "" )
	{
		setTurnRotation(0, 0);
		setTimeToTurn(%node, 0);
	}
	else if( %node.stop !$= "" )
	{
		setTurnRotation(0, 0);
	}
}

function setTimeToTurn( %node, %time )
{
	if( %time <= 0 )
	{
		beginTurning(%node);
		return;
	}
	
	%time -= 150;
	schedule( 150, 0, setTimeToTurn, %node, %time );
}

function beginTurning( %node )
{
	%buffer = %node.turn;
	%turn = getWord( %buffer, 0 );
	%speed = getWord( %buffer, 1 );
	
	if (%turn $= "stop")
	{
		%turn = 3;
	}
	else if (%turn $= "left")
	{
		%turn = 1;
	}
	else if (%turn $= "right")
	{
		%turn = 2;
	}
	else
	{
		%turn = 0;
	}
	
	if( %speed $= "" )
	{
		%speed = 2;
	}
	
	if( $Playerdead )
	{
		setTurnRotation( 0, 0 );
	}
	else
	{
		if (%turn != 3)
		{
			setTurnRotation( %turn, %speed );
		}
		else if (!$pathCameraFly)
		{
			//straight running
			$zhixingTimes ++;
			zhixingRotate($zhixingTimes);
		}
	}
}

//$XuanZhuanDengJi为0-10,为0时机台不动,为1-10时机台有动感.
//$XuanZhuanDengJi = 5; $XuanZhuanDengJi默认值为5.
$XuanZhuangVal[0,0]=1;
$XuanZhuangVal[0,1]=4;
$XuanZhuangVal[0,2]=11;
$XuanZhuangVal[1,0]=1;
$XuanZhuangVal[1,1]=5;
$XuanZhuangVal[1,2]=12;
$XuanZhuangVal[2,0]=1;
$XuanZhuangVal[2,1]=6;
$XuanZhuangVal[2,2]=13;
$XuanZhuangVal[3,0]=1;
$XuanZhuangVal[3,1]=7;
$XuanZhuangVal[3,2]=14;
$XuanZhuangVal[4,0]=1;
$XuanZhuangVal[4,1]=8;
$XuanZhuangVal[4,2]=15;
$XuanZhuangVal[5,0]=2;
$XuanZhuangVal[5,1]=9;
$XuanZhuangVal[5,2]=15;
$XuanZhuangVal[6,0]=3;
$XuanZhuangVal[6,1]=9;
$XuanZhuangVal[6,2]=15;
$XuanZhuangVal[7,0]=4;
$XuanZhuangVal[7,1]=9;
$XuanZhuangVal[7,2]=15;
$XuanZhuangVal[8,0]=4;
$XuanZhuangVal[8,1]=10;
$XuanZhuangVal[8,2]=15;
$XuanZhuangVal[9,0]=4;
$XuanZhuangVal[9,1]=11;
$XuanZhuangVal[9,2]=15;

function setTurnRotation(%rotation, %speed, %flag)
{//warn("setTurnRotation======",%rotation,"=", %speed, " " ,anquandai2.isVisible() , " ", anquandai2P2.isVisible(), " " , !$initializeResult, " ", $urgentStopRotate, " ", $zhixingTimes, " ", $rotateSpeed);
	if (anquandai2.isVisible() || anquandai2P2.isVisible() || !$initializeResult || $urgentStopRotate || $gameEndNow)
	{
		%rotation = 0;
		%speed = 0;
	}
	
	if (%rotation == 0)
	{
		%speed = 0;
	}
	
	if (%rotation == 0 && %speed == 0)
	{
		$zhixingTimes ++;
	}
	
	if (!%flag || %speed == 0)
	$rotateSpeed = %speed;
	
    if ($XuanZhuanDengJi == 0) {
        %speed = 0;
        %rotation = 0;
    }
    else {
        if (%speed != 0) {
            %indexV1 = $XuanZhuanDengJi - 1;
            %indexV2 = %speed == 0 ? 0 : %speed - 1;
            %speed = $XuanZhuangVal[%indexV1,%indexV2];
        }

        if (%rotation == 1) {
            %speed += 16; 
        }
    }
    //echo("xuanZhuanSpeed -> speed ",%speed,", rotation ",%rotation);
	PCVRSetTurnRotation(%rotation, %speed);
}

function doudongRotate()
{//return;
	if ($player1State <= 0 && $player2State <= 0)
	{
		return;
	}
	
	if (anquandai2.isVisible() || anquandai2P2.isVisible())
	{
		return;
	}
	
	if ($rotateSpeed > 0)
	{
		return;
	}
	
	$rotateSpeed = 500;
	
	if ($HurtShakeValue < 50)
	{
		$HurtShakeValue = 50;
	}
	
	doudongNow(0);
}

function doudongNow(%index)
{
	if ($rotateSpeed != 500)
	{
		return;
	}
	
	if (%index % 2 == 1)
	{
		setTurnRotation(1, 3, true);
	}
	else if (%index % 2 == 0)
	{
		setTurnRotation(2, 3, true);
	}
	
	if (%index >= 2)
	{
		$rotateSpeed = 0;
		setTurnRotation(0, 0);
		return;
	}

	//schedule($HurtShakeValue, 0, doudongNow, (%index + 1));	
	schedule(300, 0, doudongNow, (%index + 1));
}

function zhixingRotate(%times)
{//echo("gaileeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeeee   ", %times);
	if ($player1State <= 0 && $player2State <= 0)
	{
		return;
	}
	
	if (anquandai2.isVisible() || anquandai2P2.isVisible())
	{
		return;
	}
	
	if ($rotateSpeed > 0)
	{
		return;
	}
	
	/*if ($HurtShakeValue < 50)
	{
		$HurtShakeValue = 50;
	}*/
	
	zhixingNow(0, %times);
}

function zhixingNow(%index, %times)
{
	if ($player1State <= 0 && $player2State <= 0)
	{
		setTurnRotation(0, 0);
		return;
	}
	
	if (%times != $zhixingTimes)
	{
		return;
	}
	
	if (anquandai2.isVisible() || anquandai2P2.isVisible()
		|| !PlayGui.isAwake() || $pathCameraFly)
	{
		setTurnRotation(0, 0);
		return;
	}
	
	if ($rotateSpeed > 0)
	{
		return;
	}
	
	if (%index % 2 == 1)
	{
		setTurnRotation(1, 3, true);
	}
	else if (%index % 2 == 0)
	{
		setTurnRotation(2, 3, true);
	}
	
	schedule(800, 0, zhixingNow, (%index + 1), %times);
}

//when press the urgent key, will show the information
function pressUrgentKey(%val)
{
	if ($gameEndNow)
	{
		return;
	}
	
	if (%val && PlayGui.isAwake())
	{
		if (!$initializeResult)
		{
			//return;
		}
		
		if (!$urgentStopRotate || ($SafeBeltIndex == 1 && (anquandai2.isVisible() || anquandai2P2.isVisible())))
		{
			$urgentStopRotate = true;
			urgentShow.setBitmap("art/gui/Rotation/close.png");
			urgentShow.setVisible(true);
		}
		else
		{
			$urgentStopRotate = false;
			urgentShow.setBitmap("art/gui/Rotation/open.png");
			urgentShow.setVisible(true);
		}
	}
}

function unShowUrgentInfor(%num)
{return;
	if (%num == $urgentPressNum)
	{
		$urgentPressNum = 0;
		urgentShow.setVisible(false);
	}
}

//if the coin number is enough but not start game
function flashLightTest()
{
	if (PlayGui.isAwake() || !touBiWindow.isAwake())
	{
		flashLight(false);
		flashLightP2(false);
		return;
	}
	else 
	{
		if( $CurrentCoinNum >= $RequestCoinNum )
		flashLight(true);
		if( $CurrentCoinNumP2 >= $RequestCoinNum )
		flashLightP2(true);
	}
	
	schedule(800, 0, flashLightTest);
}

function flashLight(%flag)
{
	PCVROpenCloseFlashLight(%flag);
}

function flashLightP2(%flag)
{
	PCVROpenCloseFlashLightP2(%flag);
}

//read some values from the recording
function readInforFromGameset()
{
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
	
	if( %TextIndex > 0 )
	{
		$RequestCoinNum = %TextIndex;
	}
	
	if( %HardIndex != -1 )
	{
		$HardIndex = %HardIndex;
	}
	
	if( %ModeIndex != -1 )
	{
		$ModeIndex = %ModeIndex;
	}
	
	if( %SafeBeltIndex != -1 )
	{
		$SafeBeltIndex = %SafeBeltIndex;
	}
		
	if( %ShakeIndexP1 != -1 )
	{
		$ShakeIndexP1 = %ShakeIndexP1;
	}
		
	if( %ShakeIndexP2 != -1 )
	{
		$ShakeIndexP2 = %ShakeIndexP2;
	}
		
	if( %ZhuandongIndexP1 != -1 )
	{
		$ZhuandongIndexP1 = %ZhuandongIndexP1;
	}
		
	if( %ZhuandongIndexP2 != -1 )
	{
		$ZhuandongIndexP2 = %ZhuandongIndexP2;
	}
		
	if( %HurtShakeIndex != -1 )
	{
		$HurtShakeIndex = %HurtShakeIndex;
	}
	
	$HurtShakeValue = $HurtShakeIndex * 50;
	
	if ($HurtShakeValue < 50)
	{
		$HurtShakeValue = 50;
	}
}

//end game, begin initalize
//%val-------1-passlevel; 2-gameover
function initPCBegin(%val)
{
	initSomeVariables();
	$initializeResult = false;
	zhunbei.setVisible( true );
	$gameEndNow = true;
	
	//judge here............... lxy
	if (anquandai2.isVisible() || anquandai2P2.isVisible())
	{
		schedule(6000, 0, PCVRBeginInitializePC, true);
		schedule(6500, 0, judgeInitResult, %val);
	}
	else
	{
		schedule(1000, 0, PCVRBeginInitializePC, true);
		schedule(1500, 0, judgeInitResult, %val);
	}
}

function judgeInitResult(%val)
{
	if( !PlayGui.isAwake() || !zhunbei.isVisible() )
	{
		zhunbei.setVisible(false);
		return;
	}
	else
	{
		%result = PCVRGetInitializePCResult();
		//%result = 1;	//ffffff
		if( %result == 1 || %result == 2)
		{
			if (%result == 1)
			{
				//sucess
				$initializeResult = true;
			}
			else if (%result == 2)
			{
				//failure
				$initializeResult = false;
			}
			
			zhunbei.setVisible(false);
			chushihua.setBitmap("art/gui/Rotation/chushihuazaijian.png");
			chushihua.setVisible(true);
			chushihua.schedule(2100, setVisible, false);
			
			if (%val == 1)
			{
				schedule(2000, 0, endIniPCPassLevel);
			}
			else if (%val == 2)
			{
				schedule(2000, 0, endIniPCGameover);
			}
			
			PCVRBeginInitializePC(false);
			$urgentStopRotate = true;
			if ($initializeResult)
			{
				urgentShow.setBitmap("art/gui/Rotation/close.png");
			}
			else
			{
				urgentShow.setBitmap("art/gui/Rotation/broke.png");
			}
			
			return;
		}
		
		sfxPlayOnce( soundChuShiHua );
		schedule(1000, 0, judgeInitResult, %val);
	}
}

//end the game
function initSomeVariables()
{
	flashLight(false);
	flashLightP2(false);
	setGunShakeState(0, false);
	setGunShakeLevel(1, $ShakeIndexP1);
	setGunShakeLevel(2, $ShakeIndexP2);
	setGunZhuandongLevel(1, $ZhuandongIndexP1);
	setGunZhuandongLevel(2, $ZhuandongIndexP2);
	setTurnRotation(0, 0);
	$urgentStopRotate = true;
	urgentShow.setBitmap("art/gui/Rotation/close.png");
	emptyMissile.setVisible( false );
}

//after into playgui
function playGuiInitSomeInfor()
{
	flashLight(false);
	flashLightP2(false);
	setGunShakeState(0, false);
	setGunShakeLevel(1, $ShakeIndexP1);
	setGunShakeLevel(2, $ShakeIndexP2);
	setGunZhuandongLevel(1, $ZhuandongIndexP1);
	setGunZhuandongLevel(2, $ZhuandongIndexP2);
	
	AmmoBag.setVisible( false );
	AmmoBagP2.setVisible( false );
	
	if (isObject(soundChuShiHua))
	{
		soundChuShiHua.delete();
	}
	
	datablock SFXProfile(soundChuShiHua)
	{
		fileName = "art/sound/chushihua.ogg";
		description = "jiTaiChuShiHua";
		preload = "true";
	};
}

$FrameCount = 0;
renderTimeCtrl.timeCur = 0;
//$FrameTime = 0;
function showRenderTime(%val)
{
	if(!renderTimeCtrl.isVisible())
	{
		$FrameCount = 0;
		renderTimeCtrl.timeCur = 0;
		renderTimeCtrl.setVisible(true);
		//renderTimeCtrl.setCenter(400, 24);
	}
	
	$FrameCount++;
	//$FrameTime += %val;
	if(%val == -1)
	{
		$FrameCount = 0;
		renderTimeCtrl.timeCur = 0;
		renderTimeCtrl.setVisible(false);
		return;
	}
	
	renderTimeCtrl.timeCur += %val;
	%timeCur = renderTimeCtrl.timeCur;
	if (%timeCur >= 1000)
	{
		//$FrameCount *= 2;
		//%val = mFloor($FrameTime / $FrameCount);
		if( $FrameCount > 22 && $FrameCount < 30 )
		{
			$FrameCount = getRandom(30, 33 );
		}
		
		renderTime.setText($FrameCount);
		renderTimeCtrl.timeCur = 0;
		$FrameCount = 0;
		//$FrameTime = 0;
	}
}

function setPlayerCanshoot(%flag)
{
	$PlayerCanshoot = %flag;
	
	if (!$PlayerCanshoot)
	{
		$testIsFireStart = false;
		return;
	}
	
	if ($ActiveQianMingGui || $ActiveChooseGui)
	{
		return;
	}
	
	if( $pathCameraFly || $WaitContinue || $gameEndNow )
	{
		return;
	}
	
	if( $PlayerP1.isHidden() )
	{
		return;
	}
	
	if (!$testIsFireStart && ($PlayerIsOnFire || $PlayerIsOnFireP2) && $PlayerCanshoot)
	{
		$testIsFireStart = true;
		$PlayerP1.setImageTrigger( 0, true);
		$PlayerP1.setImageTrigger(0, false);
	}
}

function jiaoyanFailed(%val)
{echo("ffffffffff jy");
	//jiaoyan failed
	//failGui.setVisible(true);
	failGui.schedule( 2000, setVisible, true);
}

function PleaseInsertCoinP1( )
{
	if (!PlayGui.isAwake() || $player1State > 0 || $CurrentCoinNum >= $RequestCoinNum || $CANTStart)
	{
		insertCoinP1.setVisible( false );
		return;
	}
	
	if( insertCoinP1.isVisible() )
	{
		insertCoinP1.setVisible( false );
	}
	else
	{
		insertCoinP1.setVisible( true );
	}
	
	schedule( 500, 0, PleaseInsertCoinP1 );
}

function PleaseStartGameP1( )
{
	if (!PlayGui.isAwake() || $player1State > 0 || $CANTStart)
	{
		startGameP1.setVisible( false );
		return;
	}
	
	insertCoinP1.setVisible( false );
	
	if( startGameP1.isVisible() )
	{
		startGameP1.setVisible( false );
	}
	else
	{
		startGameP1.setVisible( true );
	}
	
	schedule( 500, 0, PleaseStartGameP1 );
}

function PleaseInsertCoinP2( )
{
	if (!PlayGui.isAwake() || $player2State > 0 ||  $CurrentCoinNumP2 >= $RequestCoinNum || $CANTStart)
	{
		insertCoinP2.setVisible( false );
		return;
	}
	
	if( insertCoinP2.isVisible() )
	{
		insertCoinP2.setVisible( false );
	}
	else
	{
		insertCoinP2.setVisible( true );
	}
	
	schedule( 500, 0, PleaseInsertCoinP2 );
}

function PleaseStartGameP2( )
{
	if (!PlayGui.isAwake() || $player2State > 0 || $CANTStart)
	{
		startGameP2.setVisible( false );
		return;
	}
	
	insertCoinP2.setVisible( false );
	
	if( startGameP2.isVisible() )
	{
		startGameP2.setVisible( false );
	}
	else
	{
		startGameP2.setVisible( true );
	}
	
	schedule( 500, 0, PleaseStartGameP2 );
}
