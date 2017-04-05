// ============================================================
// Project            :  game
// File               :  .\scripts\client\reStartGame.cs
// Copyright          :  
// Author             :  Administrator
// Created on         :  2010�?1�?�?星期�?13:27
//
// Editor             :  Codeweaver v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================
//restartGame
function reStartGame()
{
	reSetGlobalVariable();
	reSpawnStaticShape();
	reSetPathInformation();
	reSetPlayerInformation();
	return true;
}

function reStartShowGui()
{
	$ShowRightArm = false;
    $ShowBody = false;
    $ShowLeftArm = false;
    $ShowEye = false;
    $RedShow = false;
	$ShowHead = false;
    $RedShow1 = false;
    $ShowRightClaw = false;
    $ShowLeftClaw = false;
    $ShowPoisonFog = false;
    $ShowTail1 = false;
    $ShowTail2 = false;
    $ShowTail3 = false;
    $ShowBothClaw = false;
    $RedShow2 = false;
    $ShowLeftHand1 = false;
    $ShowRightHand1 = false;
    $ShowLeftHand2 = false;
    $ShowMouth = false;
    $ShowRightHand2 = false;
    $RedShow3 = false;
	$ShowShine = false;
	RightArmDamage1.setVisible(false);
	RightArmDamage2.setVisible(false);
	RightArmDamage3.setVisible(false);
	BodyDamage1.setVisible(false);
	BodyDamage2.setVisible(false);
	BodyDamage3.setVisible(false);
	LeftArmDamage1.setVisible(false);
	LeftArmDamage2.setVisible(false);
	LeftArmDamage3.setVisible(false);
	EyeDamage1.setVisible(false);
	EyeDamage2.setVisible(false);
	EyeDamage3.setVisible(false);
	LeftClawDamage1.setVisible(false);
	LeftClawDamage2.setVisible(false);
	LeftClawDamage3.setVisible(false);
	RightClawDamage1.setVisible(false);
	RightClawDamage2.setVisible(false);
	RightClawDamage3.setVisible(false);
	PoisonFogDamage1.setVisible(false);
	PoisonFogDamage2.setVisible(false);
	PoisonFogDamage3.setVisible(false);
	TailDamage1.setVisible(false);
	TailDamage21.setVisible(false);
	TailDamage22.setVisible(false);
	TailDamage23.setVisible(false);
	TailDamage3.setVisible(false);
	BothClawDamage1.setVisible(false);
	BothClawDamage2.setVisible(false);
	BothClawDamage3.setVisible(false);
	LeftHand1Damage1.setVisible(false);
	LeftHand1Damage2.setVisible(false);
	LeftHand1Damage3.setVisible(false);
	RightHand1Damage1.setVisible(false);
	RightHand1Damage2.setVisible(false);
	RightHand1Damage3.setVisible(false);
	LeftHand2Damage1.setVisible(false);
	LeftHand2Damage2.setVisible(false);
	LeftHand2Damage3.setVisible(false);
	RightHand2Damage1.setVisible(false);
	RightHand2Damage2.setVisible(false);
	RightHand2Damage3.setVisible(false);
	MouthDamage1.setVisible(false);
	MouthDamage2.setVisible(false);
	MouthDamage3.setVisible(false);
	HeadDamage1.setVisible(false);
	HeadDamage2.setVisible(false);
	HeadDamage3.setVisible(false);
	MyShine.setVisible(false);
	MyShineP2.setVisible(false);
    EyeShine.setVisible(false);
	BothClawDamage3.setVisible( false );
	BothClawShine.setVisible( false );
	$playerDamageLevel = 0;
	$playerDamageLevelP2 = 0;
	$Playerdead = false;
	$BOSSjuwuba1 = 0;
	$BOSSjuwuba2 = 0;
	$BOSSjuwuba3 = 0;
	$BOSSjuwuba4 = 0;
	$langren01 = 0;
	$BOSSlangren = 0;
	$BOSSlangren02 = 0;
	$BOSSfeiJi01 = 0;
	$BOSSfeiJi03 = 0;
}

function reSetBossBoxDamage()
{
	FBOSSbox01.setDamageLevel(0);
	FBOSSbox02.setDamageLevel(0);
	FBOSSbox03.setDamageLevel(0);
	FBOSSbox04.setDamageLevel(0);
	FBOSSbox05.setDamageLevel(0);
	FBOSSbox06.setDamageLevel(0);
	FBOSSbox07.setDamageLevel(0);
	
	LBOSSbox01.setDamageLevel(0);
	LBOSSbox02.setDamageLevel(0);
	LBOSSbox03.setDamageLevel(0);
	LBOSSbox04.setDamageLevel(0);
	LBOSSbox05.setDamageLevel(0);
	LBOSSbox06.setDamageLevel(0);
	LBOSSbox07.setDamageLevel(0);
	LBOSSbox08.setDamageLevel(0);
	LBOSSbox09.setDamageLevel(0);
	
	JBOSSbox01.setDamageLevel(0);
	JBOSSbox02.setDamageLevel(0);
	JBOSSbox03.setDamageLevel(0);
	JBOSSbox04.setDamageLevel(0);
	JBOSSbox05.setDamageLevel(0);
	JBOSSbox06.setDamageLevel(0);
	JBOSSbox07.setDamageLevel(0);
	JBOSSbox08.setDamageLevel(0);
	JBOSSbox09.setDamageLevel(0);
}

function reSetPlayerInformation()
{
	if(	!isObject( $PlayerP1 ) )
		return;
	$PlayerP1.reset();
	$PlayerP1.client.gameCamera.reset();
	$PlayerP1.setDamageLevel(0);
	%maxDamage = $PlayerP1.getDatablock().maxDamage;
	
	
	$PlayerP1.client.setControlObject($PlayerP1);
	$PlayerP1.setActionThread("pao");
	
	%startPos = playerDropPoints.getRandom();
	if( isObject( %startPos ) )
	{
		$PlayerP1.setTransform( %startPos.getTransform() );
		$PlayerP1.setHeadRotation( $PlayerP1.getRotation() );
	}
	else
	{
		//echo("\c4the player reStart point is not an object! check!\n\n\n");
	}
}

function reSetPathInformation()
{
	%count = MissionCleanup.getCount();
	for( %i = 0; %i < %count; %i++ )
	{
		%object = MissionCleanup.getObject(%i);
		if( isObject( %object ) )
		{
			%dataName = MissionCleanup.getObject(%i).dataBlock;
			if( %dataName $= "daXueBao"
		  	||%dataName $= "xiaoXueBao"
		  	||%dataName $= "daoDanBao" )
			{
				%object.schedule( 10, delete );
			}
			%classname = %object.getClassName();
			if( %classname $= "Projectile" )
			{
				%object.schedule( 10, delete );
			}
			if( %classname $= "Explosion" )
			{
				%object.schedule( 10, delete );
			}
		}
	}
	if( !isobject(MissionGroup))
		return;

}

function reSetGlobalVariable()
{
	//player.cs
	setStartGameState(false);
	$StartGame = false;
	$WaitContinue = false;
	$PlayerTimeNumber = 0;
	$PlayerCanshoot = false;
	$testIsFireStart = false;
	
	$PlayerScore = 0;
	$PlayerKillBossNum = 0;
	$PlayerKillBossScore = 0;
	$PlayerKillNpcNum = 0;
	$PlayerKillNpcScore = 0;
	$PlayerUseAmmoCount = 0;
	$PlayerDaoDanNum = 20;
	$PlayerDaoDanNumP2 = 20;
	emptyMissile.setVisible( false );
	
	$JiJiaPercent50IsShow = false;
	$JiJiaPercent70IsShow = false;
	$JiJiaPercent90IsShow = false;
	$JiJiaPercentShowOver = false;
	
	$IsDaoDanTiShiShow = false;
	$JiFenKuangMaxNum = 0;
	//toubi.cs
	$PlayerStartGame = false;
	$PlayerStartGameP2 = false;
	//aiplayer.cs
	for( %i = 0; %i <= $AiPlayerNumber; %i++ )
	{
		if( isObject( $AiPlayerArray[ %i ] ) )
		{
			$AiPlayerArray[%i].delete();
		}
		//echo("\c4reStartGame is delete all npc"@%i@"\n\n\n");
	}

	$AiPlayerNumber = 0;
	$AiPlayerArray[$AiPlayerNumber] = 0;
	$IsOnTest = false;
	//...scripts/server/commands.cs
	$PlayerIsOnFire = false;
	$PlayerIsOnFireP2 = false;
	//...scripts/server/flyShape.cs
	for( %i = 0; %i <= $FlyShapeNum; %i++ )
	{
		if( isObject( $AiPlayerArray[ %i ] ) )
		{
			$FlyShapeArray[%i].delete();
		}
	}
	$FlyShapeNum = 0;					  //飞机数组
	$FlyShapeArray[ $FlyShpaeNum ] = 0; 
	//.../art/dataBlocks/bossYiGuan.cs
	$Boss1ZuoQuanMove = false;
	$Boss1ZuoTuiMove = false; 
	$Boss1YouQuanMove = false;
	$Boss1ShuangQuan = false;
	$BossYiSheJi = false;
	//.../art/dataBlocks/bossErGuan.cs
	$Boss2ZuoZhua = false;
	$Boss2YouZhua = false; 
	$Boss2DuSu = false;
	$Boss2WeiBa1 = false;
	$Boss2WeiBa2 = false;
	$Boss2WeiBa3 = false;
	$BossShuangZhua1 = false;
	$BossShuangZhua2 = false;
	//.../art/dataBlocks/bossDianJu.cs
	$DianJuGongJiA = false;
	$DianJuGongJiB1 = false; 
	$DianJuGongJiB2 = false;
	$DianJuSheJi = false;
	$DianJuShuangYan = false; 
	//.../art/dataBlocks/zongBoss.cs
	$ZongBossZuoYi = false;
	$ZongBossZuoEr = false;
	$ZongBossZuoSan = false;
	$ZongBossYouYi = false;
	$ZongBossYouEr = false;
	$ZongBossYouSan = false;
	$ZongBossABCDA = false;
	$ZongBossABCDB = false;
	$ZongBossABCDC = false;
	$ZongBossABCDD = false;
	$ZongBossZuiGongJiYi = false;
	$ZongBossZuiGongJiEr = false;
	$ZongBossLast = false;
	
	$zongBossStopFireA = true;
	$zongBossStopFireB = true;
	$zongBossStopFireC = true;
	$zongBossStopFireD = true;
	
	if( isObject( $DianJuYanYi ) )
	{
		$DianJuYanYi.delete();
	}
	if( isObject( $DianJuYanEr ) )
	{
		$DianJuYanEr.delete();
	}
	ClientCmddrawPlayerHealth();
	changeBloodP2();
	ClientCmdaddKillNpcNum();
	ClientCmdaddKillBossNum(0);
	setBossWeaponBitmap(3);
	bossWeaponDamage1.setVisible( false );
	bossWeaponDamage2.setVisible( false );
	bossWeaponDamage3.setVisible( false );
	bossWeaponDamage4.setVisible( false );
	
	$CANTStart = false;
}

function reSpawnStaticShape()
{
	return;
	if( !isObject( cheBao01 ) )
	{
	 	new StaticShape(cheBao01)
		{
        	receiveSunLight = "1";
        	receiveLMLighting = "1";
        	useCustomAmbientLighting = "0";
        	customAmbientLighting = "0 0 0 1";
        	initialPosition = "0 0 0";
        	initialVelocity = "0 0 1";
        	gravityMod = "0 0.999 0.3";
       	 	bounceElasticity = "0.999 0.3 0";
        	bounceFriction = "0.3 0 -1.#QNAN";
        	sourceObject = "-1";
        	sourceSlot = "-1";
        	dataBlock = "jingchebaozha";
        	position = "1244.2 385.938 511.839";
        	rotation = "0 0 -1 73.7859";
        	scale = "1 1 1";
        	pitchAngle = "0";
        	canSaveDynamicFields = "1";
      	};
	}
	else
	{
		%pos = cheBao01.getPosition();
		%pos.x = 1244.2;
		%pos.y = 385.938;
		%pos.z = 511.839;
		cheBao01.setPosition( %pos );
	}
	if( !isObject( BossBody ) )
	{
		new StaticShape(BossBody) {
      receiveSunLight = "1";
      receiveLMLighting = "1";
      useCustomAmbientLighting = "0";
      customAmbientLighting = "0 0 0 1";
      initialPosition = "0 0 0";
      initialVelocity = "0 0 1";
      gravityMod = "0 0.999 0.3";
      bounceElasticity = "0.999 0.3 0";
      bounceFriction = "0.3 0 -1.#QNAN";
      sourceObject = "-1";
      sourceSlot = "-1";
      dataBlock = "BossBodyBox";
      position = "654.149 472.783 9.28722";
      rotation = "0.00355889 -0.265663 -0.964059 38.4589";
      scale = "0.853298 0.853298 0.853298";
      pitchAngle = "0";
      canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = BossBody.getPosition();
		%pos.x = 654.149;
		%pos.y = 472.783;
		%pos.z = 9.28722;
		BossBody.setPosition( %pos );
	}
	if( !isObject( BossLeftShoulder ) )
	{
		new StaticShape(BossLeftShoulder) {
      receiveSunLight = "1";
      receiveLMLighting = "1";
      useCustomAmbientLighting = "0";
      customAmbientLighting = "0 0 0 1";
      initialPosition = "0 0 0";
      initialVelocity = "0 0 1";
      gravityMod = "0 0.999 0.3";
      bounceElasticity = "0.999 0.3 0";
      bounceFriction = "0.3 0 -1.#QNAN";
      sourceObject = "-1";
      sourceSlot = "-1";
      dataBlock = "BossLeftShoulderBox";
      position = "651.081 470.938 10.9493";
      rotation = "-0.203617 -0.907443 -0.367541 99.5183";
      scale = "1.3927 1.3927 1.3927";
      pitchAngle = "0";
      canSaveDynamicFields = "1";
       };
	}
	else
	{
		%pos = BossLeftShoulder.getPosition();
		%pos.x = 651.081;
		%pos.y = 470.938;
		%pos.z = 10.9493;
		BossLeftShoulder.setPosition( %pos );
	}
	if( !isObject( BossLeftArm ) )
	{
		new StaticShape(BossLeftArm) {
      receiveSunLight = "1";
      receiveLMLighting = "1";
      useCustomAmbientLighting = "0";
      customAmbientLighting = "0 0 0 1";
      initialPosition = "0 0 0";
      initialVelocity = "0 0 1";
      gravityMod = "0 0.999 0.3";
      bounceElasticity = "0.999 0.3 0";
      bounceFriction = "0.3 0 -1.#QNAN";
      sourceObject = "-1";
      sourceSlot = "-1";
      dataBlock = "BossLeftArmBox";
      position = "646.989 470.039 10.9264";
      rotation = "-0.405301 -0.910449 -0.0825523 109.694";
      scale = "0.779206 0.779206 0.779206";
      pitchAngle = "0";
      canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = BossLeftArm.getPosition();
		%pos.x = 646.989;
		%pos.y = 470.039;
		%pos.z = 10.9264;
		BossLeftArm.setPosition( %pos );
	}
	if( !isObject( BossRightShoulder ) )
	{
		new StaticShape(BossRightShoulder) {
      receiveSunLight = "1";
      receiveLMLighting = "1";
      useCustomAmbientLighting = "0";
      customAmbientLighting = "0 0 0 1";
      initialPosition = "0 0 0";
      initialVelocity = "0 0 1";
      gravityMod = "0 0.999 0.3";
      bounceElasticity = "0.999 0.3 0";
      bounceFriction = "0.3 0 -1.#QNAN";
      sourceObject = "-1";
      sourceSlot = "-1";
      dataBlock = "BossRightShoulderBox";
      position = "657.861 474.296 10.2027";
      rotation = "0.0315582 0.981235 0.190217 46.3349";
      scale = "1.18524 1.18524 1.18524";
      pitchAngle = "0";
      canSaveDynamicFields = "1";
       };
	}
	else
	{
		%pos = BossRightShoulder.getPosition();
		%pos.x = 657.861;
		%pos.y = 474.296;
		%pos.z = 10.2027;
		BossRightShoulder.setPosition( %pos );
	}
	if( !isObject( BossRightArm ) )
	{
		new StaticShape(BossRightArm) {
      receiveSunLight = "1";
      receiveLMLighting = "1";
      useCustomAmbientLighting = "0";
      customAmbientLighting = "0 0 0 1";
      initialPosition = "0 0 0";
      initialVelocity = "0 0 1";
      gravityMod = "0 0.999 0.3";
      bounceElasticity = "0.999 0.3 0";
      bounceFriction = "0.3 0 -1.#QNAN";
      sourceObject = "-1";
      sourceSlot = "-1";
      dataBlock = "BossRightArmBox";
      position = "660.963 473.765 7.1339";
      rotation = "-0.819772 0.49945 -0.28022 87.5859";
      scale = "0.871218 0.674991 1.32841";
      pitchAngle = "0";
      canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = BossRightArm.getPosition();
		%pos.x = 660.963;
		%pos.y = 473.765;
		%pos.z = 7.1339;
		BossRightArm.setPosition( %pos );
	}
	if( !isObject( boss1QuGanBox ) )
	{
      new StaticShape(boss1QuGanBox)
		{
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss1ShenTiBox";
         position = "1161.99 394.859 517.108";
         rotation = "0.149642 0.154904 -0.976531 83.2532";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss1QuGanBox.getPosition();
		%pos.x = 1161.99;
		%pos.y = 394.859;
		%pos.z = 517.108;
		boss1QuGanBox.setPosition( %pos );
	}
	if( !isObject( boss1ZuoBiBox ) )
	{
      new StaticShape(boss1ZuoBiBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss1SiZhiBox";
         position = "1161.93 390.818 516.719";
         rotation = "-0.192934 -0.640736 -0.743125 80.9874";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss1ZuoBiBox.getPosition();
		%pos.x = 1161.93;
		%pos.y = 390.818;
		%pos.z = 516.719;
		boss1ZuoBiBox.setPosition( %pos );
	}
	if( !isObject( boss1YouBiBox ) )
	{
      new StaticShape(boss1YouBiBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss1SiZhiBox";
         position = "1163.67 398.48 517.224";
         rotation = "-0.251522 0.183906 -0.950219 101.743";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss1YouBiBox.getPosition();
		%pos.x = 1163.67;
		%pos.y = 398.48;
		%pos.z = 517.224;
		boss1YouBiBox.setPosition( %pos );
	}
	if( !isObject( boss1ZuoTuiBox ) )
	{
      new StaticShape(boss1ZuoTuiBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss1TuiBox";
         position = "1163.66 392.837 513.709";
         rotation = "0.60619 0.538666 -0.585127 115.188";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss1ZuoTuiBox.getPosition();
		%pos.x = 1163.66;
		%pos.y = 392.837;
		%pos.z = 513.709;
		boss1ZuoTuiBox.setPosition( %pos );
	}
	if( !isObject( boss1YouTuiBox ) )
	{
      new StaticShape(boss1YouTuiBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss1TuiBox";
         position = "1160.89 396.87 515.826";
         rotation = "-0.0512361 -0.0711094 -0.996152 85.1381";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = boss1YouTuiBox.getPosition();
		%pos.x = 1160.89;
		%pos.y = 396.87;
		%pos.z = 515.826;
		boss1YouTuiBox.setPosition( %pos );
	}
	if( !isObject( cheBao02 ) )
	{
      new StaticShape(cheBao02) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zhuangjiachebaozha";
         position = "1339.82 407.145 511.476";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao02.getPosition();
		%pos.x = 1339.82;
		%pos.y = 407.145;
		%pos.z = 511.476;
		cheBao02.setPosition( %pos );
	}
	if( !isObject( cheBao03 ) )
	{
      new StaticShape(cheBao03) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zhuangjiachebaozha";
         position = "1266.71 386.484 511.476";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao03.getPosition();
		%pos.x = 1266.71;
		%pos.y = 386.484;
		%pos.z = 511.476;
		cheBao03.setPosition( %pos );
	}
	if( !isObject( cheBao04 ) )
	{
      new StaticShape(cheBao04) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zhuangjiachebaozha";
         position = "1378 403.363 511.476";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao04.getPosition();
		%pos.x = 1378;
		%pos.y = 403.363;
		%pos.z = 511.476;
		cheBao04.setPosition( %pos );
	}
	if( !isObject( cheBao05 ) )
	{
      new StaticShape(cheBao05) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jipuchebaozha";
         position = "1365.37 402.12 512.057";
         rotation = "0 0 1 2.86599";
         scale = "2.05747 2.05747 2.05747";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao05.getPosition();
		%pos.x = 1365.37;
		%pos.y = 402.12;
		%pos.z = 512.057;
		cheBao05.setPosition( %pos );
	}
	if( !isObject( cheBao06 ) )
	{
      new StaticShape(cheBao06) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jipuchebaozha";
         position = "1323.68 407.918 512.057";
         rotation = "0 0 1 2.86599";
         scale = "2.05747 2.05747 2.05747";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao06.getPosition();
		%pos.x = 1323.68;
		%pos.y = 407.918;
		%pos.z = 512.057;
		cheBao06.setPosition( %pos );
	}
	if( !isObject( cheBao07 ) )
	{
      new StaticShape(cheBao07) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jingchebaozha";
         position = "1298.48 380.879 511.839";
         rotation = "0 0 -1 73.7859";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao07.getPosition();
		%pos.x = 1298.48;
		%pos.y = 380.879;
		%pos.z = 511.839;
		cheBao07.setPosition( %pos );
	}
	if( !isObject( cheBao08 ) )
	{
      new StaticShape(cheBao08) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jingchebaozha";
         position = "1284.64 384.915 511.839";
         rotation = "0 0 -1 73.7859";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao08.getPosition();
		%pos.x = 1284.64;
		%pos.y = 384.915;
		%pos.z = 511.839;
		cheBao08.setPosition( %pos );
	}
	if( !isObject( cheBao09 ) )
	{
      new StaticShape(cheBao09) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jipuchebaozha";
         position = "1310.04 377.853 512.057";
         rotation = "0 0 1 10.5295";
         scale = "2.05747 2.05747 2.05747";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao09.getPosition();
		%pos.x = 1310.04;
		%pos.y = 377.853;
		%pos.z = 512.057;
		cheBao09.setPosition( %pos );
	}
	if( !isObject( cheBao10 ) )
	{
      new StaticShape(cheBao10) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jipuchebaozha";
         position = "1253.38 382.1 512.057";
         rotation = "0 0 1 10.5295";
         scale = "2.05747 2.05747 2.05747";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
    }
	else
	{
		%pos = cheBao10.getPosition();
		%pos.x = 1253.38;
		%pos.y = 382.1;
		%pos.z = 512.057;
		cheBao10.setPosition( %pos );
	}
	if( !isObject( cheBao11 ) )
	{
      new StaticShape(cheBao11) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "jingchebaozha";
         position = "1311.49 1300.69 511.839";
         rotation = "0 0 1 59.1598";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao11.getPosition();
		%pos.x = 1311.49;
		%pos.y = 1300.69;
		%pos.z = 511.839;
		cheBao11.setPosition( %pos );
	}
	if( !isObject( cheBao12 ) )
	{
      new StaticShape(cheBao12) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zhuangjiachebaozha";
         position = "1308.3 409.781 511.476";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao12.getPosition();
		%pos.x = 1308.3;
		%pos.y = 409.781;
		%pos.z = 511.476;
		cheBao12.setPosition( %pos );
	}
	if( !isObject( boss2ZuoZhuaBox ) )
	{
      new StaticShape(boss2ZuoZhuaBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2ZuoZhuaBoxA";
         position = "1354.41 1429.02 516.565";
         rotation = "-0.214438 -0.13172 -0.967815 91.5212";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2ZuoZhuaBox.getPosition();
		%pos.x = 1354.41;
		%pos.y = 1429.02;
		%pos.z = 516.565;
		boss2ZuoZhuaBox.setPosition( %pos );
	}
	if( !isObject( boss2TouBox ) )
	{
      new StaticShape(boss2TouBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2TouBoxA";
         position = "1357.85 1421.97 516.098";
         rotation = "6.28368e-005 -1.05907e-005 -1 70.8691";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2TouBox.getPosition();
		%pos.x = 1357.85;
		%pos.y = 1421.97;
		%pos.z = 516.098;
		boss2TouBox.setPosition( %pos );
	}
	if( !isObject( boss2WeiBaErBox ) )
	{
      new StaticShape(boss2WeiBaErBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2WeiBaErBoxA";
         position = "1359.99 1421.32 522.949";
         rotation = "-0.143147 -0.102555 -0.984374 71.3464";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2WeiBaErBox.getPosition();
		%pos.x = 1359.99;
		%pos.y = 1421.32;
		%pos.z = 522.949;
		boss2WeiBaErBox.setPosition( %pos );
	}
	if( !isObject( boss2WeiBaSanBox ) )
	{
      new StaticShape(boss2WeiBaSanBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2WeiBaSanBoxA";
         position = "1363.3 1420.07 520.599";
         rotation = "-0.523244 -0.372311 -0.766551 85.7471";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2WeiBaSanBox.getPosition();
		%pos.x = 1363.3;
		%pos.y = 1420.07;
		%pos.z = 520.599;
		boss2WeiBaSanBox.setPosition( %pos );
	}
	if( !isObject( boss2WeiBaYiBox ) )
	{
      new StaticShape(boss2WeiBaYiBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2WeiBaYiBoxA";
         position = "1352.3 1423.89 522.869";
         rotation = "0.0657862 0.0468675 -0.996733 71.0526";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2WeiBaYiBox.getPosition();
		%pos.x = 1352.3;
		%pos.y = 1423.89;
		%pos.z = 522.869;
		boss2WeiBaYiBox.setPosition( %pos );
	}
	if( !isObject( boss2YouZhuaBox ) )
	{
      new StaticShape(boss2YouZhuaBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "boss2YouZhuaBoxA";
         position = "1350.95 1418.36 516.533";
         rotation = "-0.378453 -0.294707 -0.877452 50.6398";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = boss2YouZhuaBox.getPosition();
		%pos.x = 1350.95;
		%pos.y = 1418.36;
		%pos.z = 516.533;
		boss2YouZhuaBox.setPosition( %pos );
	}
	if( !isObject( cheBao13 ) )
	{
      new StaticShape(cheBao13) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "qiaoloubaozha";
         position = "431.369 505.591 521.907";
         rotation = "0 0 1 6.67906";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = cheBao13.getPosition();
		%pos.x = 431.369;
		%pos.y = 505.591;
		%pos.z = 521.907;
		cheBao13.setPosition( %pos );
	}
	if( !isObject( zongBossDBox ) )
	{
      new StaticShape(zongBossDBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zongBossD";
         position = "551.538 1587.61 519.883";
         rotation = "0.755733 0.63028 0.177804 16.3463";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = zongBossDBox.getPosition();
		%pos.x = 551.538;
		%pos.y = 1587.61;
		%pos.z = 519.883;
		zongBossDBox.setPosition( %pos );
	}
	if( !isObject( zongBossCBox ) )
	{
      new StaticShape(zongBossCBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zongBossC";
         position = "560.959 1581.68 525.483";
         rotation = "-0.257816 0.281433 0.924298 56.3328";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = zongBossCBox.getPosition();
		%pos.x = 560.959;
		%pos.y = 1581.68;
		%pos.z = 525.483;
		zongBossCBox.setPosition( %pos );
	}
	if( !isObject( zongBossBBox ) )
	{
      new StaticShape(zongBossBBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zongBossB";
         position = "551.167 1586.46 526.255";
         rotation = "-0.984103 -0.0584847 -0.16769 44.2775";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = zongBossBBox.getPosition();
		%pos.x = 551.167;
		%pos.y = 1586.46;
		%pos.z = 526.255;
		zongBossBBox.setPosition( %pos );
	}
	if( !isObject( zongBossABox ) )
	{
      new StaticShape(zongBossABox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zongBossA";
         position = "561.439 1582.53 520.043";
         rotation = "0.314237 -0.121732 0.941507 34.2291";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = zongBossABox.getPosition();
		%pos.x = 561.439;
		%pos.y = 1582.53;
		%pos.z = 520.043;
		zongBossABox.setPosition( %pos );
	}
	if( !isObject( bossDianJuYanEr ) )
	{
      new StaticShape(bossDianJuYanEr) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "bossdianjuBox2";
         position = "689.529 441.113 517.274";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = bossDianJuYanEr.getPosition();
		%pos.x = 689.529;
		%pos.y = 441.113;
		%pos.z = 517.274;
		bossDianJuYanEr.setPosition( %pos );
	}
	if( !isObject( bossDianJuYanYi ) )
	{
      new StaticShape(bossDianJuYanYi) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "bossdianjuBox1";
         position = "693.197 437.711 515.485";
         rotation = "1 0 0 0";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = bossDianJuYanYi.getPosition();
		%pos.x = 693.197;
		%pos.y = 437.711;
		%pos.z = 515.485;
		bossDianJuYanYi.setPosition( %pos );
	}
	if( !isObject( zongBossTouBox ) )
	{
      new StaticShape(zongBossTouBox) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "zongBossTou";
         position = "557.968 1588.04 518.536";
         rotation = "-0.842 0.190456 0.50474 48.3198";
         scale = "1 1 1";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = zongBossTouBox.getPosition();
		%pos.x = 557.968;
		%pos.y = 1588.04;
		%pos.z = 518.536;
		zongBossTouBox.setPosition( %pos );
	}
	if( !isObject( buChongBao15 ) )
	{
      new StaticShape(buChongBao15) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "1709.78 1034.23 514.993";
         rotation = "-0.176475 0.00137042 0.984304 238.091";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao15.getPosition();
		%pos.x = 1709.78;
		%pos.y = 1034.23;
		%pos.z = 514.993;
		buChongBao15.setPosition( %pos );
	}
	if( !isObject( buChongBao14 ) )
	{
      new StaticShape(buChongBao14) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "688.861 1678.51 512.128";
         rotation = "-0.0155856 -0.0187136 -0.999703 83.1612";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao14.getPosition();
		%pos.x = 688.861;
		%pos.y = 1678.51;
		%pos.z = 512.128;
		buChongBao14.setPosition( %pos );
	}
	if( !isObject( buChongBao13 ) )
	{
      new StaticShape(buChongBao13) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "805.229 1810.5 512.706";
         rotation = "-0.0429999 0.286659 -0.957067 95.6766";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao13.getPosition();
		%pos.x = 805.229;
		%pos.y = 1810.5;
		%pos.z = 512.706;
		buChongBao13.setPosition( %pos );
	}
	if( !isObject( buChongBao12 ) )
	{
      new StaticShape(buChongBao12) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "974.154 1416.7 515.31";
         rotation = "0.0164592 -0.0189927 0.999684 230.721";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao12.getPosition();
		%pos.x = 974.154;
		%pos.y = 1416.7;
		%pos.z = 515.31;
		buChongBao12.setPosition( %pos );
	}
	if( !isObject( buChongBao11 ) )
	{
      new StaticShape(buChongBao11) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "1381.78 1427.59 513.981";
         rotation = "0.444324 0.190909 0.875288 171.19";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao11.getPosition();
		%pos.x = 1381.78;
		%pos.y = 1427.59 ;
		%pos.z =  513.981;
		buChongBao11.setPosition( %pos );
	}
	if( !isObject( buChongBao10 ) )
	{
      new StaticShape(buChongBao10) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "DaoDanBox";
         position = "1344.71 1441.68 532.341";
         rotation = "-0.542858 -0.589411 0.598247 76.7485";
         scale = "2.78128 2.78128 2.78128";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao10.getPosition();
		%pos.x = 1344.71;
		%pos.y = 1441.68;
		%pos.z = 532.341;
		buChongBao10.setPosition( %pos );
	}
	if( !isObject( buChongBao09 ) )
	{
      new StaticShape(buChongBao09) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "1503.62 1155.3 517.747";
         rotation = "-0.00823992 0.00410658 -0.999958 98.1416";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao09.getPosition();
		%pos.x = 1503.62;
		%pos.y = 1155.3;
		%pos.z = 517.747;
		buChongBao09.setPosition( %pos );
	}
	if( !isObject( buChongBao08 ) )
	{
      new StaticShape(buChongBao08) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "DaoDanBox";
         position = "1699.23 1139.28 523.995";
         rotation = "-0.205738 -0.258858 0.94375 89.4929";
         scale = "2.78128 2.78128 2.78128";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao08.getPosition();
		%pos.x = 1699.23;
		%pos.y = 1139.28;
		%pos.z = 523.995;
		buChongBao08.setPosition( %pos );
	}
	if( !isObject( buChongBao07 ) )
	{
      new StaticShape(buChongBao07) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "1564.72 803.051 522.649";
         rotation = "-0.373459 0.286075 -0.882434 82.1073";
         scale = "3.61175 3.61175 3.32265";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao07.getPosition();
		%pos.x = 1564.72;
		%pos.y = 803.051;
		%pos.z = 522.649;
		buChongBao07.setPosition( %pos );
	}
	if( !isObject( buChongBao06 ) )
	{
      new StaticShape(buChongBao06) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "DaoDanBox";
         position = "1456.17 775.738 528.862";
         rotation = "0.0778961 0.433258 0.897897 136.803";
         scale = "2.78128 2.78128 2.78128";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao06.getPosition();
		%pos.x = 1456.17;
		%pos.y = 775.738;
		%pos.z = 528.862;
		buChongBao06.setPosition( %pos );
	}
	if( !isObject( buChongBao05 ) )
	{
      new StaticShape(buChongBao05) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "494.666 465.981 512.587";
         rotation = "0.757782 -0.268072 0.594898 239.512";
         scale = "2.2171 2.2171 2.2171";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao05.getPosition();
		%pos.x = 494.666;
		%pos.y = 465.981;
		%pos.z = 512.587;
		buChongBao05.setPosition( %pos );
	}
	if( !isObject( buChongBao04 ) )
	{
      new StaticShape(buChongBao04) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "1127.64 348.459 523.424";
         rotation = "-0.00582026 0.00603563 -0.999965 40.5033";
         scale = "6.16865 6.16865 6.16865";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao04.getPosition();
		%pos.x = 1127.64;
		%pos.y = 348.459;
		%pos.z = 523.424;
		buChongBao04.setPosition( %pos );
	}
	if( !isObject( buChongBao03 ) )
	{
      new StaticShape(buChongBao03) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "816.989 530.144 519.374";
         rotation = "-0.00761636 0.0246942 0.999666 13.3972";
         scale = "2.76896 2.76896 2.76896";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao03.getPosition();
		%pos.x = 816.989;
		%pos.y = 530.144;
		%pos.z = 519.374;
		buChongBao03.setPosition( %pos );
	}
	if( !isObject( buChongBao02 ) )
	{
      new StaticShape(buChongBao02) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "DaoDanBox";
         position = "1461.78 478.913 516.895";
         rotation = "-0.766304 0.243919 -0.594375 57.326";
         scale = "2.01484 2.01484 2.01484";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };	
	}
	else
	{
		%pos = buChongBao02.getPosition();
		%pos.x = 1461.78;
		%pos.y = 478.913;
		%pos.z = 516.895;
		buChongBao02.setPosition( %pos );
	}
	if( !isObject( buChongBao01 ) )
	{
      new StaticShape(buChongBao01) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "xueLiangBox";
         position = "683.845 535.818 520.263";
         rotation = "0.0514283 0.961511 0.269909 94.1107";
         scale = "2.2171 2.2171 2.2171";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao01.getPosition();
		%pos.x = 683.845;
		%pos.y = 535.818;
		%pos.z = 520.263;
		buChongBao01.setPosition( %pos );
	}
	if( !isObject( buChongBao16 ) )
	{
      new StaticShape(buChongBao16) {
         receiveSunLight = "1";
         receiveLMLighting = "1";
         useCustomAmbientLighting = "0";
         customAmbientLighting = "0 0 0 1";
         initialPosition = "0 0 0";
         initialVelocity = "0 0 1";
         gravityMod = "0 0.999 0.3";
         bounceElasticity = "0.999 0.3 0";
         bounceFriction = "0.3 0 -1.#QNAN";
         sourceObject = "-1";
         sourceSlot = "-1";
         dataBlock = "DaoDanBox";
         position = "1502.84 1148.89 517.161";
         rotation = "0.22927 -0.862106 -0.451895 54.3369";
         scale = "2.78128 2.78128 2.78128";
         pitchAngle = "0";
         canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao16.getPosition();
		%pos.x = 1502.84;
		%pos.y = 1148.89;
		%pos.z = 517.161;
		buChongBao16.setPosition( %pos );
	}
	if( !isObject( buChongBao17 ) )
	{
      new StaticShape(buChongBao17) {
        receiveSunLight = "1";
        receiveLMLighting = "1";
        useCustomAmbientLighting = "0";
        customAmbientLighting = "0 0 0 1";
        initialPosition = "0 0 0";
        initialVelocity = "0 0 1";
        gravityMod = "0 0.999 0.3";
        bounceElasticity = "0.999 0.3 0";
        bounceFriction = "0.3 0 -1.#QNAN";
        sourceObject = "-1";
        sourceSlot = "-1";
        dataBlock = "xueLiangBox";
        position = "405.821 554.25 518.157";
        rotation = "0.00482497 0.999814 0.0186601 89.3289";
        scale = "2.2171 2.2171 2.2171";
        pitchAngle = "0";
        canSaveDynamicFields = "1";
      };
	}
	else
	{
		%pos = buChongBao17.getPosition();
		%pos.x = 405.821;
		%pos.y = 554.25;
		%pos.z = 518.157;
		buChongBao17.setPosition( %pos );
	}
}
