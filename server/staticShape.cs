//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Hook into the mission editor.

//function StaticShapeData::create(%data)
//{
//   // The mission editor invokes this method when it wants to create
//   // an object of the given datablock type.
//   %obj = new StaticShape()
//   {
//      dataBlock = %data;
//   };
//   return %obj;
//}

function StaticShapeData::onAdd(%data, %obj)
{
   //echo("\c4StaticShapeData::onAdd("@%data.getName()@", "@%obj@")");

   // For destroyable purposes there's nothing for us to do here, but you could
   // initialize dynamic properties, activate a startup sequence, set energy,
   // recharge rate, repair rate, etc
}

function StaticShapeData::damage( %data, %obj, %sourceObject, %position, %amount, %damageType )
{
   //echo("\c4StaticShapeData::damage("@%data.getName()@", "@%obj@", "@sourceObject@", "@%position@", "@%amount@", "@%damageType@")");
   
   if (%obj.isDestroyed())
   {
      //echo("object already destroyed, returning");
      return;
   }
   if( %obj.getName() $= "bossDianJuYanYi" || %obj.getName() $= "bossDianJuYanEr" )
	{
		%damage1 = 0;
		%damage2 = 0;
		if( isObject ( bossDianJuYanYi ) )
		{
			%damage1 = bossDianJuYanYi.getDamageLevel();
		}
		else
		{
			%damage1 = $BossDianJu.getMaxDamage() * $DianJuShuangYanDamagePre / 2;
		}
		if( isObject( bossDianJuYanEr ) )
		{
			%damage2 = bossDianJuYanEr.getDamageLevel();
		}
		else
		{
			%damage2 = $BossDianJu.getMaxDamage() * $DianJuShuangYanDamagePre / 2;
		}
		%damage = %damage1 + %damage2;
		%maxDamage = $BossDianJu.getMaxDamage();
		%totalDamage = %damage + %maxDamage*( $DianJuGongJiADamagePre + $DianJuGongJiA1DamagePre + $DianJuGongJiB1DamagePre + $DianJuGongJiB2DamagePre + $DianJuSheJiDamagePre);
        if( %totalDamage > %maxDamage * 0.9 && !$ShowShine && $ShowEye)
		{
		    EyeDamage2.setVisible(false);
		    $RedShow = true;
			$ShowShine = true;
		}
		commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $DianJuShuangYanDamagePre );
		commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
	}
   %obj.applyDamage(%amount);
}

function StaticShapeData::onDamage( %data, %obj )
{
   //echo("\c4StaticShapeData::onDamage("@%data@", "@%obj@")");

   // Set damage state based on current damage level, we are comparing amount
   // of damage sustained to the damage levels described in the datablock and
   // setting the approppriate damageState

   %data.checkIsBossYiGuanBox( %obj );
   %data.checkIsBossErGuanBox( %obj );
   %data.checkIsZongBossBox( %obj );
   %data.checkIsDianJuBossBox( %obj );

   %damage = %obj.getDamageLevel();
   if (%damage >= %data.destroyedLevel)
   {
      if (%obj.getDamageState() !$= "Destroyed")
      {
         %obj.setDamageState(Destroyed);
         %obj.setDamageLevel(%data.maxDamage);
      }
   }
   else if(%damage >= %data.disabledLevel)
   {
      // you could call an animation sequence here to represent the deformation
      // of the object by damage.  You can have as many sequences as you want,
      // so long as you set up your damage amount to damage level comparisons
      if (%obj.getDamageState() !$= "Disabled")
         %obj.setDamageState(Disabled);
   }
   else
   {
      // we're just assuming that the object is still nice and healthy
      if (%obj.getDamageState() !$= "Enabled")
         %obj.setDamageState(Enabled);
   }
}

function StaticShapeData::onEnabled(%data, %obj, %state)
{
   //echo("\c4StaticShapeData::onEnabled("@%data@", "@%obj@", "@%state@")");

   // We could do things here like establishing a power connection, activation
   // sounds, play a start up sequence, add effects, etc.
}

function StaticShapeData::onDisabled(%data, %obj, %state)
{
   //echo("\c4StaticShapeData::onDisabled("@%data@", "@%obj@", "@%state@")");

   // We could do things here like disabling power, shutdown sounds, play a
   // damage sequence, swap to a specific model shape, add effects, etc.
}

function StaticShapeData::onDestroyed(%data, %obj, %prevState)
{
   //echo("\c4StaticShapeData::onDestroyed("@%data@", "@%obj@", "@%prevState@")");

   // If this is set to false then we delete the object when it is destroyed,
   // we do so while it is still obscured by the explosion fx

   if( isObject( %obj ) )
	{
		%expData = %data.explosion;
		%position = %obj.getPosition();
		if( isObject(%expData) && %expData.getClassName() $= "ExplosionData")
		{
			%exp = new Explosion()
			{
				datablock = %expData;
				position =  %position;
			};
			if (isObject(%exp))
			{
				MissionCleanup.add(%exp);
			}
		}
		%obj.schedule( 1000, delete );
		return;
	}
	// 销毁的时候如果有爆炸效果，计算爆炸的伤害. 下面是写法例子.
	//radiusDamage(%explosion, %spawnPoint, "10", "25", "HandbombDamage", 2000);
}

function StaticShapeData::onCollisionPlayer(%data, %obj, %attackObj)
{
	if( !isobject( %obj ) )
		return;
	if( %attackObj $= "DefaultPlayerData" )
	{
		%attackObjId = $PlayerP1;
		if( %data.Damage !$= "" )
		{
			%damageType = "kedabaodandao";
			%attackObjId.damage( %obj, %obj.getposition(), %data.Damage, %damageType );
		}
		//echo("player attack by kedabaodaodan");
	}
}

function StaticShapeData::checkIsDianJuBossBox(%data, %obj )
{

//	if(!isObject($BossDianJu ))
//	return;

	%name = %obj.getName();
	if(%name $= "BossBody" || %name $= "BossLeftShoulder" || %name $= "BossLeftArm" || %name $= "BossRightShoulder" || %name $= "BossRightArm" )
	{
		
		$BossDianJu.DianJuGetDamage( %obj );
	}
	
}

function StaticShapeData::checkIsBossYiGuanBox(%data, %obj )
{
	if( !isObject( $BossYiGuan ) )
		return;
	%name = %obj.getName();
	if( %name $= "boss1QuGanBox" )
	{
		%damage1 = boss1QuGanBox.getDamageLevel();
		%maxDamage = $BossYiGuan.getMaxdamage();
		%boss1Damage = %damage1;
		
		if( $BossYiGuan.curState $= "BossYiGuanZuoQuanMoveState" && $Boss1ZuoQuanMove )
		{
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $Boss1ZuoQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %boss1Damage, %maxDamage );
			if( ( %boss1Damage > %maxDamage * $Boss1ZuoQuanDamagePre ) )
			{
				$BossYiGuan.stop();
				$Boss1ZuoQuanMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanZuoQuanMoveState1" && $BossZuoQuanMove )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $BossZuoQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( ( %boss1Damage > %maxDamage * $BossZuoQuanDamagePre ) )
			{
				$BossYiGuan.stop();
				$BossZuoQuanMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanZuoTuiMoveState" && $Boss1ZuoTuiMove )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $Boss1ZuoTuiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage *  $Boss1ZuoTuiDamagePre )
			{
				$BossYiGuan.stop();
				$Boss1ZuoTuiMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt2" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanZuoTuiMoveState1" && $BossZuoTuiMove )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $BossZuoQuanDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $BossZuoTuiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage *  $BossZuoTuiDamagePre )
			{
				$BossYiGuan.stop();
				$BossZuoTuiMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt2" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanYouQuanState" && $Boss1YouQuanMove )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $BossZuoQuanDamagePre + $BossZuoTuiDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $Boss1YouQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage *  $Boss1YouQuanDamagePre )
			{
				$BossYiGuan.stop();
				$Boss1YouQuanMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt3" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanYouQuanState1" && $BossYouQuanMove )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $Boss1YouQuanDamagePre + $BossZuoQuanDamagePre + $BossZuoTuiDamagePre + $Boss1ShuangQuanDamagePre  );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $BossYouQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage *  $BossYouQuanDamagePre )
			{
				$BossYiGuan.stop();
				$BossYouQuanMove = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt3" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanShuangQuanGongJiState" && $Boss1ShuangQuan )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $Boss1YouQuanDamagePre + $BossZuoQuanDamagePre + $BossZuoTuiDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $Boss1ShuangQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage * $Boss1ShuangQuanDamagePre )
			{
				$BossYiGuan.stop();
				$Boss1ShuangQuan = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt3" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanShuangQuanGongJiState1" && $BossShuangQuan )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $Boss1YouQuanDamagePre + $BossZuoQuanDamagePre + $BossZuoTuiDamagePre + $Boss1ShuangQuanDamagePre + $BossYouQuanDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $BossShuangQuanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage * $BossShuangQuanDamagePre )
			{
				$BossYiGuan.stop();
				$BossShuangQuan = false;
				$BossYiGuan.schedule( 10, doAnimation, "hurt3" );
				return;
			}
		}
		if( $BossYiGuan.curState $= "BossYiGuanSheJiState" && $BossYiSheJi )
		{
			%totalDamage = %boss1Damage + %maxDamage*( $Boss1ZuoQuanDamagePre + $Boss1ZuoTuiDamagePre + $Boss1YouQuanDamagePre + $BossZuoQuanDamagePre + $BossZuoTuiDamagePre + $Boss1ShuangQuanDamagePre + $BossYouQuanDamagePre + $BossShuangQuanDamagePre );
			if(%totalDamage > %maxDamage*0.9 && !$ShowShine)
			{
				HeadDamage2.setVisible(false);
			    HeadShine.setVisible(true);
		     	$RedShow1 = true;
				$ShowShine = true;
			}
			commandToClient( $PlayerP1.client, 'bossGetDamage', %boss1Damage, %maxDamage * $Boss1SheJiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %boss1Damage > %maxDamage * $Boss1SheJiDamagePre )
			{
				commandToClient( $PlayerP1.client, 'hideBossHealth' );
				commandToClient( $PlayerP1.client, 'hideBossTotalHealth' );
				if( isObject( $BossYiGuan ) )
				{
					followObj1.setVisible( false );
					$BossYiGuan.stop();
					$BossYiSheJi = false;
					$PlayerKillBossScore += $BossYiGuan.aiPlayerDb.score;
					$PlayerScore += $BossYiGuan.aiPlayerDb.score;
					commandToClient( $PlayerP1.client, 'addPlayerScore', $PlayerScore );
					$PlayerKillBossNum++;
					commandToClient( $PlayerP1.client, 'addKillBossNum', $PlayerKillBossNum );
					$BossYiGuan.schedule( 10, doAnimation, "siwang" );

                    $ShowHead = false;
                    $RedShow1 = false;
			        HeadDamage1.setVisible(false);
					HeadDamage2.setVisible(false);
                    HeadShine.setVisible(false);
					HeadDamage3.setVisible(true);
						
					$HideHead = true;
                    schedule( 3000, 0, clientCmdShowGongXiGuoGuan );
                                             
					bossWeaponDamage2.setVisible( false );
					bossWeaponDamage3.setVisible( false );
					//schedule(3000, 0, setBossWeaponBitmap, 2 );
					
					$BossYiGuanDead = true;
				}
				return;
			}
		}
	}
}

function StaticShapeData::checkIsBossErGuanBox(%data, %obj )
{
	if( !isObject( $BossErGuan ) )
		return;
	%name = %obj.getName();
	if( %name $= "boss2WeiBaSanBox" || %name $= "boss2ZuoZhuaBox" ||%name $= "boss2WeiBaYiBox" ||%name $= "boss2YouZhuaBox" ||%name $= "boss2WeiBaErBox" || %name $= "boss2TouBox")
	{
		%damage1 = boss2WeiBaSanBox.getDamageLevel();
		%damage2 = boss2ZuoZhuaBox.getDamageLevel();
		%damage3 = boss2WeiBaYiBox.getDamageLevel();
		%damage4 = boss2YouZhuaBox.getDamageLevel();
		%damage5 = boss2WeiBaErBox.getDamageLevel();
		%damage6 = boss2TouBox.getDamageLevel();
		
		%maxDamage = $BossErGuan.getMaxdamage();
		%shuangZhuaDamage = %damage2 + %damage4;
		
		if( $BossErGuan.curState $= "BossErGuanZuoZhuaState" && $Boss2ZuoZhua )
		{
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage2, %maxDamage * $Boss2ZuoZhuaDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %damage2, %maxDamage );
			if( ( %damage2 > %maxDamage * $Boss2ZuoZhuaDamagePre ) )
			{
				$BossErGuan.stop();
				$Boss2ZuoZhua = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanYouZhuaState" && $Boss2YouZhua )
		{
			%totalDamage = %damage4 + %maxDamage * $Boss2ZuoZhuaDamagePre;
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage4, %maxDamage * $Boss2YouZhuaDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage4 > %maxDamage *  $Boss2YouZhuaDamagePre )
			{
				$BossErGuan.stop();
				$Boss2YouZhua = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanDuSuState" && $Boss2DuSu )
		{
			%totalDamage = %damage6 + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage6, %maxDamage * $Boss2DuSuDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage6 > %maxDamage *  $Boss2DuSuDamagePre )
			{
				$BossErGuan.stop();
				$Boss2DuSu = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanWeiBaYiState" && $Boss2WeiBa1 )
		{
			%totalDamage = %damage3 + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre + $Boss2DuSuDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage3, %maxDamage * $Boss2WeiBa1DamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage3 > %maxDamage * $Boss2WeiBa1DamagePre )
			{
				$BossErGuan.stop();
				$Boss2WeiBa1 = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanWeiBaErState" && $Boss2WeiBa2 )
		{
			%totalDamage = %damage5 + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre + $Boss2DuSuDamagePre + $Boss2WeiBa1DamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage5, %maxDamage * $Boss2WeiBa2DamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage5 > %maxDamage * $Boss2WeiBa2DamagePre )
			{
				$BossErGuan.stop();
				$Boss2WeiBa2 = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanWeiBaSanState" && $Boss2WeiBa3 )
		{
			%totalDamage = %damage1 + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre + $Boss2DuSuDamagePre + $Boss2WeiBa1DamagePre + $Boss2WeiBa2DamagePre);
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage1, %maxDamage * $Boss2WeiBa3DamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage1 > %maxDamage * $Boss2WeiBa3DamagePre )
			{
				$BossErGuan.stop();
				$Boss2WeiBa3 = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		if( $BossErGuan.curState $= "BossErGuanShuangZhuaYiState" && $Boss2ShuangZhua1 )
		{
			%totalDamage = %shuangZhuaDamage + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre + $Boss2DuSuDamagePre + $Boss2WeiBa1DamagePre + $Boss2WeiBa2DamagePre + $Boss2WeiBa3DamagePre);
			commandToClient( $PlayerP1.client, 'bossGetDamage', %shuangZhuaDamage, %maxDamage * $Boss2ShuangZhua1DamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %shuangZhuaDamage > %maxDamage * $Boss2ShuangZhua1DamagePre )
			{
				$BossErGuan.stop();
				$Boss2ShuangZhua1 = false;
				$BossErGuan.schedule( 10, doAnimation, "hurt1" );
				return;
			}
		}
		
		if( $BossErGuan.curState $= "BossErGuanShuangZhuaErState" && $Boss2ShuangZhua2 )
		{
			%totalDamage = %shuangZhuaDamage + %maxDamage *( $Boss2ZuoZhuaDamagePre+$Boss2YouZhuaDamagePre + $Boss2DuSuDamagePre + $Boss2WeiBa1DamagePre + $Boss2WeiBa2DamagePre + $Boss2WeiBa3DamagePre + $Boss2ShuangZhua1DamagePre );
			if(%totalDamage > %maxDamage *0.9 && !$ShowShine)
			{
				BothClawDamage2.setVisible(false);
			    BothClawShine.setVisible(true);
		     	$RedShow2 = true;
				$ShowShine = true;
			}
			commandToClient( $PlayerP1.client, 'bossGetDamage', %shuangZhuaDamage, %maxDamage * $Boss2ShuangZhua2DamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %shuangZhuaDamage > %maxDamage * $Boss2ShuangZhua2DamagePre )
			{
				commandToClient( $PlayerP1.client, 'hideBossHealth' );
				commandToClient( $PlayerP1.client, 'hideBossTotalHealth' );
				if( isObject( $BossErGuan ) )
				{
					followObj1.setVisible( false );
					followObj2.setVisible( false );
					
					$BossErGuan.stop();
					$Boss2ShuangZhua2 = false;
					$PlayerKillBossScore += $BossErGuan.aiPlayerDb.score;
					$PlayerScore += $BossErGuan.aiPlayerDb.score;
					commandToClient( $PlayerP1.client, 'addPlayerScore', $PlayerScore );
					$PlayerKillBossNum++;
					$BossErGuan.schedule( 10, doAnimation, "siwang" );
					commandToClient( $PlayerP1.client, 'addKillBossNum', $PlayerKillBossNum );
					//commandToClient( $PlayerP1.client, 'showGongXiGuoGuan' );
                                        
                    $ShowBothClaw = false;
					$RedShow2 = false;
					BothClawDamage1.setVisible(false);
					BothClawDamage2.setVisible(false);
                    BothClawShine.setVisible(false);
					BothClawDamage3.setVisible(true);
						
					$HideShow = true;

					schedule( 3000, 0, clientCmdShowGongXiGuoGuan );
					bossWeaponDamage3.setVisible( false );
					//schedule(3000, 0, setBossWeaponBitmap, 4 );
					
					$BossErGuanDead = true;
				}
				return;
			}
		}
	}
}

function StaticShapeData::checkIsZongBossBox(%data, %obj )
{
	//echo("zong boss current thread is: "@$BossCaiJue.currentThread );
	//echo("current box: "@%obj.getName() );
	if( !isObject( $BossCaiJue ) )
		return;
	%name = %obj.getName();
	%maxDamage = $BossCaiJue.getMaxDamage();
	if( %name $= "zongBossABox" || %name $= "zongBossBBox" || %name $= "zongBossCBox" || %name $= "zongBossDBox" || %name $= "zongBossTouBox" )
	{
		if( $BossCaiJue.curState $= "zongBossZuoShouYiState" && $ZongBossZuoYi )
		{
			%damage = zongBossBBox.getDamageLevel();
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuoYiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %damage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossZuoYiDamagePre )
			{
				$ZongBossZuoYi = false;
				$zongBossStopFireB = true;
				zongBossBBox.setDamageLevel(0);
				zongBossBBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Bhurt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossYouShouYiState" && $ZongBossYouYi )
		{
			%damage = zongBossCBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage * $ZongBossZuoYiDamagePre;
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossYouYiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossYouYiDamagePre )
			{
				$ZongBossYouYi = false;
				$zongBossStopFireC = true;
				zongBossCBox.setDamageLevel(0);
				zongBossCBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Churt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossZuoShouErState" && $ZongBossZuoEr )
		{
			%damage = zongBossDBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage * ( $ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuoErDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossZuoErDamagePre ) 
			{
				$ZongBossZuoEr = false;
				$zongBossStopFireD = true;
				zongBossDBox.setDamageLevel(0);
				zongBossDBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Dhurt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossYouShouErState" && $ZongBossYouEr )
		{
			%damage = zongBossCBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage * ($ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossYouErDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossYouErDamagePre )
			{
				$ZongBossYouEr = false;
				$zongBossStopFireC = true;
				zongBossCBox.setDamageLevel(0);
				zongBossCBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Churt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossZuoShouSanState" && $ZongBossZuoSan )
		{
			%damage = zongBossBBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage*($ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre + $ZongBossYouErDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuoSanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossZuoSanDamagePre )
			{
				$ZongBossZuoSan = false;
				$zongBossStopFireB = true;
				zongBossBBox.setDamageLevel(0);
				zongBossBBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Bhurt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossYouShouSanState" && $ZongBossYouSan )
		{
			%damage = zongBossDBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage*($ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre + $ZongBossYouErDamagePre + $ZongBossZuoSanDamagePre );
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuoSanDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossYouSanDamagePre )
			{
				$ZongBossZuoSan = false;
				$zongBossStopFireD = true;
				zongBossDBox.setDamageLevel(0);
				zongBossDBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Dhurt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossABCDState" && ( $ZongBossABCDA || $ZongBossABCDB || $ZongBossABCDC || $ZongBossABCDD ) )
		{
			%damage1 = zongBossABox.getDamageLevel();
			if( %damage1 >= %maxDamage * $ZongBossABCDADamagePre && $ZongBossABCDA )
			{
				followObj1.setVisible( false );
				
				$ZongBossABCDA = false;
				$zongBossStopFireA = true;
				zongBossABox.setDamageLevel( %maxDamage * $ZongBossABCDADamagePre );
				zongBossABox.setInvincible( true );
			}
			%damage2 = zongBossBBox.getDamageLevel();
			if( %damage2 >= %maxDamage * $ZongBossABCDBDamagePre && $ZongBossABCDB )
			{
				followObj2.setVisible( false );
				$ZongBossABCDB = false;                
				$zongBossStopFireB = true;
				zongBossBBox.setDamageLevel(%maxDamage * $ZongBossABCDBDamagePre);
				zongBossBBox.setInvincible( true );
			}
			%damage3 = zongBossCBox.getDamageLevel();
			if( %damage3 >= %maxDamage * $ZongBossABCDCDamagePre && $ZongBossABCDC )
			{
				followObj3.setVisible( false );
				$ZongBossABCDC = false;
				$zongBossStopFireC = true;
				zongBossCBox.setDamageLevel(%maxDamage * $ZongBossABCDCDamagePre);
				zongBossCBox.setInvincible( true );
			}
			%damage4 = zongBossDBox.getDamageLevel();
			if( %damage4 >= %maxDamage * $ZongBossABCDDDamagePre && $ZongBossABCDD )
			{
				followObj4.setVisible( false );
				
				$ZongBossABCDD = false;
				$zongBossStopFireD = true;
				zongBossDBox.setDamageLevel(%maxDamage * $ZongBossABCDDDamagePre );
				zongBossDBox.setInvincible( true );
			}
			%damage = %damage1 + %damage2 + %damage3 + %damage4;
			%totalDamage = %damage + %maxDamage*($ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre + $ZongBossYouErDamagePre + $ZongBossZuoSanDamagePre + $ZongBossYouSanDamagePre);
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage *( $ZongBossABCDADamagePre + $ZongBossABCDBDamagePre + $ZongBossABCDCDamagePre + $ZongBossABCDDDamagePre ) );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( $zongBossStopFireA && $zongBossStopFireB && $zongBossStopFireC && $zongBossStopFireD )
			{
				zongBossABox.setInvincible( false );
				zongBossBBox.setInvincible( false );
				zongBossCBox.setInvincible( false );
				zongBossDBox.setInvincible( false );
				zongBossABox.setDamageLevel(0);
				zongBossBBox.setDamageLevel(0);
				zongBossCBox.setDamageLevel(0);
				zongBossDBox.setDamageLevel(0);
				$BossCaiJue.schedule( 10, doAnimation, "ABCDhurt" );
				$PlayerP1.schedule(1000, continueMove );
				$PlayerP1.schedule(1100, aiMove );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossZuiGongJiYiState" && $ZongBossZuiGongJiYi )
		{
			%damage = zongBossTouBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage* ( $ZongBossABCDADamagePre + $ZongBossABCDBDamagePre + $ZongBossABCDCDamagePre + $ZongBossABCDDDamagePre +
			                                       $ZongBossZuoYiDamagePre + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre + $ZongBossYouErDamagePre + $ZongBossZuoSanDamagePre + $ZongBossYouSanDamagePre);
			commandToClient($PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuiYiDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossZuiYiDamagePre )
			{
				$ZongBossZuiGongJiYi = false;
				zongBossTouBox.setDamageLevel(0);
				zongBossTouBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Zuihurt" );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossZuiGongJiErState" && $ZongBossZuiGongJiEr )
		{
			
			%damage = zongBossTouBox.getDamageLevel();
			%totalDamage = %damage + %maxDamage* ( $ZongBossABCDADamagePre + $ZongBossABCDBDamagePre + $ZongBossABCDCDamagePre + $ZongBossABCDDDamagePre + $ZongBossZuoYiDamagePre
			                                       + $ZongBossYouYiDamagePre + $ZongBossZuoErDamagePre + $ZongBossYouErDamagePre + $ZongBossZuoSanDamagePre + $ZongBossYouSanDamagePre + $ZongBossZuiYiDamagePre);
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossZuiErDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossZuiErDamagePre )
			{
				$ZongBossZuiGongJiEr = false;
				zongBossTouBox.setDamageLevel(0);
				zongBossTouBox.setInvincible( true );
				$BossCaiJue.schedule( 10, doAnimation, "Zuihurt" );
				return;
			}
		}
		if( $BossCaiJue.curState $= "zongBossLastState" && $ZongBossLast )
		{
			%damage = zongBossABox.getDamageLevel();
			%totalDamage = %damage + %maxDamage * (1 - $ZongBossLastDamagePre);
            if(%totalDamage > %maxDamage *0.9 && !$ShowShine)
			{
				RightHand2Damage2.setVisible(false);
			    RightHand2Shine.setVisible(true);
		     	$RedShow3 = true;
				$ShowShine = true;
			}
			commandToClient( $PlayerP1.client, 'bossGetDamage', %damage, %maxDamage * $ZongBossLastDamagePre );
			commandToClient( $PlayerP1.client, 'BossGetDamageTotal', %totalDamage, %maxDamage );
			if( %damage >= %maxDamage * $ZongBossLastDamagePre )
			{
				followObj1.setVisible( false );
				$PlayerKillBossScore += $BossCaiJue.aiPlayerDb.score;
				$PlayerScore += $BossCaiJue.aiPlayerDb.score;
				commandToClient( $PlayerP1.client, 'addPlayerScore', $PlayerScore );
				$PlayerKillBossNum++;
				commandToClient( $PlayerP1.client, 'addKillBossNum', $PlayerKillBossNum );
				commandToClient( $PlayerP1.client, 'hideBossHealth' );
				commandToClient( $PlayerP1.client, 'hideBossTotalHealth' );
                                
                $ShowRightHand2 = false;
				$RedShow3 = false;
				RightHand2Damage1.setVisible(false);
				RightHand2Damage2.setVisible(false);
                RightHand2Shine.setVisible(false);
				RightHand2Damage3.setVisible(true);
					
				$HideShowFinal = true;

				commandToClient( $PlayerP1.client, 'showGongXiGuoGuan' );
				$ZongBossLast = false;
				$BossCaiJue.schedule( 10, doAnimation, "siwang" );
				$ZongBossDead = true;
			}
		}
	}
}
