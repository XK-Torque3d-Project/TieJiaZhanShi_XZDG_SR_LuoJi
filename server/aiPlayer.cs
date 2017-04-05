//-----------------------------------------------------------------------------

// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// AIPlayer callbacks
// The AIPlayer class implements the following callbacks:
//
//    PlayerData::onStuck(%this,%obj)
//    PlayerData::onUnStuck(%this,%obj)
//    PlayerData::onStop(%this,%obj)
//    PlayerData::onMove(%this,%obj)
//    PlayerData::onReachDestination(%this,%obj)
//    PlayerData::onTargetEnterLOS(%this,%obj)
//    PlayerData::onTargetExitLOS(%this,%obj)
//    PlayerData::onAdd(%this,%obj)
//
// Since the AIPlayer doesn't implement it's own datablock, these callbacks
// all take place in the PlayerData namespace.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// Demo Pathed AIPlayer.
//-----------------------------------------------------------------------------

//-----------------------------------------------------------------------------
// AIPlayer static functions
//-----------------------------------------------------------------------------
$AiPlayerNumber = 0;
$AiPlayerArray[$AiPlayerNumber] = 0;
$IsOnTest = false;

$CurNodeNpcNum = 0;                   //当前点产生的NPC数量
$CurNodeNpcArrary[$CurNodeNpcNum] = 0;   //当前点产生的NPC数组
 
function AIPlayer::animationDone( %this )
{		
	if( !isObject( %this ) )
		return;
	if( %this == $PlayerP1 && %this.currentThread $= "siwang" )
	{
		//commandToClient( %this.client, 'BackGroundFadeInBlack');
	}
	if( !isobject( %this ) )
		return;
	
	if( %this.AiPlayerDb.attackType $= "jinShen" && %this.currentThread !$= "" && !%this.animationDone)
	{
		%this.animationDone = true;
		%this.schedule( 10, execJinShenAniScripts );
	}
	else if( %this.AiPlayerDb.attackType $= "jinShen" && %this.currentThread $= "" )
	{
		%this.animationDone = true;
	}
	else
	{
		if( %this.currentThread !$= "" )//npc出生时也会调用到这里,加入判断防止出生时误调用
		{
			if( %this.IsChooseRun() )//如果没有选择移动则在IsChooseRun()函数中作相应处理
			{
   				%this.schedule( 10, AiMove );
			}
		}
	}
	return;
}

function AIPlayer::onReachDestination(%this)
{
   // Moves to the next node on the path.
   // Override for all player.  Normally we'd override this for only
   // a specific player datablock or class of players.
    if( !isobject( %this ) )
		return;
		
	if( %this.aiPlayerDb.attackType $= "jinshen" )
	{
		%this.schedule( 10, moveToNode );
		return;
	}
		
    if( %this == $PlayerP1 )
	{
		//echo("$PlayerP1 is onReachDestination");
		//%time = %this.currentNode.stayTime*1000;
		//%this.schedule( %time, AiMove );
	}
	else
	{
		if( %this.follow == true || %this.IsChooseRun() )
		{
			//echo("obj's follow is true and IsChooseRun is return true");
			%time = %this.currentNode.stayTime*1000;
			if( %time != 0 )
			{
				%this.schedule( %time, AiMove );
			}
			else
			{
   				%this.schedule( 10,AiMove );
			}
		}
	}
	return;
}

function AIPlayer::onNode( %this )
{
	if( !isObject( %this ) )
		return;
	
	if( %this.currentNodeIndex + 1 >= 31 )
	{
		%path = %this.path.getName();
		%pathNum = getSubStr(%path, 13, 1 ); 
		%pathNum++;
		%newPath = "zhujiaolujing"@%pathNum;
		%this.path = %newPath;
		%this.currentNodeIndex = -1;
	}
	%this.isOnNode = true;
	%time = %this.currentNode.stayTime*1000;
	%node = %this.currentNode;
	if( %node.end !$= "" )
	{
		return;
	}
	if( %node.stop !$= "" )
	{
		%this.stopMove();
	}
	
	judgeRotateState(%node, %time);
	
	%this.getStayTime(%time);
	if( %this.currentNode.mustKillAll )
	{
		%this.setKillAllState( true );
		%this.schedule(10, checkIsAllNpcDead );
	}
	if( !$WaitContinue )
	{
		if( %this.currentNode.mustDestroy $= "true" ) 
		{
			%this.stopMove();
		}
		else
		{
			%this.AiMove();
		}
	}
}

function AIPlayer::checkIsAllNpcDead( %this )
{
	//echo("checkIsAllNpcDead was called");
	for( %i = 0; %i < $AiPlayerNumber; %i++ )
	{
		%obj = $AiPlayerArray[%i];
		if ( isObject( %obj ) && %obj.path.donghua != 1 )
		{
			//echo(""@%i@"\n");
			//echo( %obj.aiPlayerDb );
			//echo( %obj.path.getName() );
			%this.schedule( 500, checkIsAllNpcDead );
			return;
		}
	}
	if( !$GameOver )
	{
		%this.setKillAllState( false );
		//echo("checkIsAllNpcDead has return;");
	}
	return;
}

function AIPlayer::OnAnimationTrigger( %this )
{
	if( !isObject( %this ) )
		return;
	//动作的触发点 作相应的处理
//	echo("OnAnimationTrigger");
//	if( %this.attackType !$= "jinshen" )
//	{
//		return;
//	}
	if ( $GameOver )
	{
    	return;
	}
	%attackId = 0;
	switch$( %this.currentThread )
	{
		//各种不同的case对应不同的伤害,不同的屏幕抖动
		case "fire":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
		case "fire1":
			%attackId = 1;
			$PlayerP1.shakeCamera(RocketSubExplosion);
		case "fire2":
			%attackId = 1;
			$PlayerP1.shakeCamera(RocketSubExplosion);
		case "zuoquangongji":
		    sfxPlayOnce(Geweinisi_Sound_jinShen03);
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
		case "zuotuigongji":
             sfxPlayOnce(Geweinisi_Sound_jinShen03);
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
		case "youquangongji":
            sfxPlayOnce(Geweinisi_Sound_jinShen03);
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
		case "shuangquangongji":
            sfxPlayOnce(Geweinisi_Sound_jinShen03);
			%attackId = 1;
			Bomb::spawnExplosion( %this, Boss1shuangshouchuidixiaoguo_1 );
		case "dianjuAgongji":
		    sfxPlayOnce(boss1_Sound_boss01_01);
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
		case "dianjuBgongji":
		    sfxPlayOnce(boss1_Sound_boss01_01);
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "weibagongji":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "youzhuagongji":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "zuozhuagongji":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "shuangzhuagongji":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "koutudusu":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );
        case "zuigongji":
			%attackId = 1;
			$PlayerP1.shakeCamera( RocketSubExplosion );                              
	}
	if( %attackId != 0 )
	{
		commandToClient($PlayerP1.client, 'ShowDamagePicture', %attackId );
		//commandToClient($PlayerP1.client,'drawPlayerHealth' );
		judgeHurtPlayer(false, %this.aiPlayerDb.attackDamage, %this);
	}
	return;
}

function AIPlayer::onEndOfPath(%this,%path)
{
   %this.nextTask();
}

function AIPlayer::onEndSequence(%this,%slot)
{
   //echo("Sequence Done!");
   %this.stopThread(%slot);
   %this.nextTask();
}

function AIPlayer::spawn( %spawnPoint, %aiPlayerDb )
{
   // Create the demo player object
   	if( !isObject(%aiPlayerDb ) )
   	{
		return;
	}
   %player = new AiPlayer()
   {
      dataBlock = %aiPlayerDb;
      path = "";
   };
   %player.currentNode = %spawnPoint;
   %player.AiPlayerDb = %aiPlayerDb;
   %player.radius = %spawnPoint.radius;
   %player.animationDone = true;
   %player.tool = %spawnPoint.tool;
   MissionCleanup.add(%player);
	if (isObject(%spawnPoint))
	{
		%player.setTransform( %spawnPoint.getTransform() );
	}
   if(  %player.AiPlayerDb.attackType !$= "jinshen"&&%player.AiPlayerDb.attackType !$= "fly" )
	{
   		%player.aimAt($PlayerP1);
	}
	
    //把产生的NPc存起来，待用
	$AiPlayerArray[$AiPlayerNumber] = %player;
	$AiPlayerNumber++;
	
   //echo("spawn a aiplayer is successful!");
   return %player;
}

function AIPlayer::spawnOnPath( %path, %aiPlayerDb )
{
   	// Spawn a player and place him on a random node of the path
   	if (!isObject(%path))
      	return 0;
	
   	%nodeCount = %path.getCount();
	%canSpawnPoint = 0;
	for( %i = 0; %i < %nodeCount; %i++ )//遍历可出生点
	{
		if( %path.getObject( %i ).canSpawnHere )
		{
			%canSpawnPoint++;
		}
	}
	%randomMax = %canSpawnPoint - 1;
   	%randomNum = getRandom( 0, %randomMax );
	%spawnCount = -1;
	for( %i = 0; %i < %nodeCount; %i++ )
	{
		if( %path.getObject( %i ).canSpawnHere )
		{
			%spawnCount++;
		}
		if( %randomNum == %spawnCount )
		{
			%spawnPoint = %path.getObject( %i );
			break;
		}
	}

	if( !isObject( %spawnPoint ) )
	{
		//echo("the spawn node is not object!");
	}
	//echo("spawn a npc on  "@%path.getName() );
   	%player = AIPlayer::spawn(%spawnPoint, %aiPlayerDb );
	%path.aiPlayer = %player;
	if( isObject( %player ) )
	{
		//把当前点产生的NPc存起来，待用
		$CurNodeNpcArrary[$CurNodeNpcNum] = %player;
		$CurNodeNpcNum++;
	}
	if( isObject( %player ) )
	{
		%player.spawnPoint = %spawnPoint;
		%palyer.spawnTime = 0;
		%player.path = %path;
		%player.currentNodeIndex = -1;
		%player.AnimateScripts = %path.AnimateScripts;
		
		if( %path.follow )
		{
			%player.follow = true;
		}
		
		//NPC装备武器
   		%player.equipObject();
		//获取攻击对象
		%player.schedule(1000, getAttackObj );
	}
	else
	{
		//echo("spawn a aiPlayer on path is not successful!");
		return 0;
	}
   	return %player;
}

//-----------------------------------------------------------------------------
// AIPlayer methods
//-----------------------------------------------------------------------------
function AIPlayer::IsChooseRun( %this )
{
	if( !isObject( %this ) )
		return;
	%string1 = %this.currentAniEndDo;
	%string2 = %this.currentAniEndProb;
	%num1 = getWordCount(%string1);//防止溢出
	%num2 = getWordCount(%string2);
	%random = getRandom( 1, 100 );
	for(%i = 0; %i < %num2; %i++ )
	{
		%max = 0;
		if( %i > 0 )
		{
			for(%j = 0; %j <= %i; %j++ )
			{
				%max += getWord( %string2, %j );
			}
		}
		else
		{
			%max = getWord( %string2, 0 );
		}
		if( %random <= %max )
		{
			%ani = getWord( %string1, %i );
			break;
		}
	}
	%this.currentThread = %ani;
	%AniCount = %this.GetScriptsAniCount();
	for( %i = 1; %i <= %AniCount; %i++ )
	{
		if( %ani $= getVariable(%this.AnimateScripts@".Ani"@%i ) )
		{
			%this.currentAniEndDo = getVariable(%this.AnimateScripts@".Ani"@%i@"EndDo" );
			%this.currentAniEndProb = getVariable(%this.AnimateScripts@".Ani"@%i@"EndProb" );
		}
	}
	if( %ani $= "run" )
	{
		%this.GetAniFireInformation(%ani);
		return true;
	}
	else
	{
		%this.schedule(10, doAnimation, %ani );
		return false;
	}
}

//获取动作脚本中列出的动作数量
function AIPlayer::GetScriptsAniCount( %this )
{
	if( !isObject( %this ) )
		return;
	if( !isObject( %this.AnimateScripts ) )
		return;
	%AniCount = 0;
	
	for( %i = 1; getVariable(%this.AnimateScripts@".Ani"@%i) !$= ""; %i++ )
	{
		%AniCount++;
	}
	return %AniCount;
}

//NPC移动
function AIPlayer::AiMove( %this )
{
	if( !isObject( %this ) )
		return;
	if (%this.path !$= "" && %this.follow )
   	{
       %this.moveToNextNode();	
   	}//顺序路径
   	else
   	{
		%this.moveToNode();
   	}//ai激活
	return;
}

function AIPlayer::followPath(%this,%path,%node)
{
	if( !isObject( %this ) )
		return;
   // Start the player following a path
   %this.stopThread(0);
   %this.follow = true;
   if( %this == $PlayerP1 )
	{
   		%this.moveToNodeByIndex(1);
		return;
	}
   if (!isObject(%this.path))
   {
      %this.path = "";
      return;
   }

   if (%node > %this.path.getCount() - 1)
      %this.targetNodeIndex = %this.path.getCount() - 1;
   else
      %this.targetNodeIndex = %node;

   if ( %this.currentNodeIndex != -1 )
   {
      %this.moveToNodeByIndex(%this.currentNodeIndex);
   }
   else
   {
      %this.moveToNodeByIndex(0);
   }
}

function AIPlayer::moveToNextNode(%this)
{
	if( !isObject( %this ) )
		return;
    if (%this.currentNodeIndex < %this.path.getCount() - 1)
        %this.moveToNodeByIndex(%this.currentNodeIndex + 1);
    else
        %this.moveToNodeByIndex(0);
	return;
}

function getObjectIndex( %obj, %group )
{
	//if( !isObject(%obj) || !isObject(%group) )
	//	echo("obj or group is not object");
	for( %i = 0; %i < %group.getCount(); %i++ )
	{
		if( %obj == %group.getObject( %i ) )
			return %i;
	}
}

function AIPlayer::moveToNodeByIndex(%this,%index)
{
	if( !isObject( %this ) )
		return;
    // Move to the given path node index
    
    if( %this == $PlayerP1 )
	{
		%node = %this.path.getObject(%index);
		if( %index > 0 )
		{
			%nownode = %this.path.getObject(%index - 1);
			//error("moveToNodeByIndex      %this.path========"@%this.path.getName());  
			if( %nownode.danhei $= "true" )
			{
				commandToClient( %this.client, 'BackGroundFadeInBlack', %nownode );
			}
		}
		else
		{
			%path = %this.path.getName();
			%pathNum = getSubStr(%path, 13, 1 ); 
			if( %pathNum != 1 )
			{
				%pathNum -= 1;
				%prePath = "zhujiaolujing"@%pathNum;
				%nownode = %prePath.getObject( 30 );
			}
		}
		
		if( isObject( %nownode ) && %nownode.cameraFollowPath $= "true" )
		{
			%this.StopMove();
			
			%this.cameraNode = %node;
			
			if( %nownode.spawnNPC !$= "" )
			{
				%this.spawnNodeNPC( %nownode );
			}
			
			if(%nownode.getName() $= "jiedian01_05")
			{
				sfxStopAll();
				sfxPlayOnce(Back_Sound_nvRenBeiJing);
			}
			if(%nownode.getName() $= "jiedian01_21")
			{
				sfxStopAll();
				sfxPlayOnce(back_Sound_sanGuanBeiJing);
			}
			if(%nownode.getName() $= "jiedian01_252")
			{
				sfxStopAll();
				sfxPlayOnce(back_Sound_boss2BeiJing);
			}
                        
			$pathCameraFly = true;
			//commandToClient( %this.client, 'BackGroundFadeInBlack', %nownode );
			commandToClient( %this.client, 'BackGroundFadeInWhite', %nownode );
		}
		else
		{
			%this.currentNodeIndex = %index;
			%this.currentNode = %node;
			//echo("set Move Destination node:  "@%node.getName() );
			%speed = %nownode.msToNext;
			%this.setMoveSpeed( %speed );
			//电锯最后一个点,若电锯信息有残留需清除
			if( %node.getName() $= "jiedian01_20_a20" )
			{
				if( isObject( $DianJuYanYi ) || isObject( $DianJuYanEr ) || isObject(bossDianJuYanYi) || isObject(bossDianJuYanEr) )
				{
					if( isObject(bossDianJuYanYi) )
					{
						bossDianJuYanYi.setDamageState(Destroyed);
					}
					if( isObject(bossDianJuYanEr) )
					{
						bossDianJuYanEr.setDamageState(Destroyed);
					}
					if( isObject($DianJuYanYi) )
					{
						$DianJuYanYi.delete();
					}
					if( isObject($DianJuYanEr) )
					{
						$DianJuYanEr.delete();
					}
					
					$BossDianJu.schedule( 3000, setDamageState, "Destroyed" );
					$PlayerP1.schedule( 10000, getStayTime, 0 );
					
					commandToClient( $PlayerP1.client, 'hideBossHealth' );
					commandToClient( $PlayerP1.client, 'hideBossTotalHealth' );
					
					bossWeaponDamage2.setVisible( false );
					schedule(13000, 0, setBossWeaponBitmap, 1 );
				}
			}
			//error( "AIPlayer::moveToNodeByIndex  setMoveDestination   "@%this.currentNode@"     "@%this.currentNode.getName() );
			%this.setMoveDestination(%node.getTransform(), false );
			%this.isOnNode = false;
			if( %nownode.ActiveExplosion !$= "" )
			{
				%this.ActiveNodeExplosion( %nownode );
			}
			if( %nownode.spawnNPC !$= "" )
			{
				%this.spawnNodeNPC( %nownode );
			}
			if( %nownode.showJiFen $= "true" )
			{
				setGunShakeState(0, false);
				%this.showJiFenNode = %nownode;
				commandToClient( %this.client, 'openJiFenKuang');
			}
			
		}
	}
	else
	{
		%node = %this.path.getObject(%index);
		%this.currentNodeIndex = %index;
		%this.currentNode = %node;
		%speed = %node.msToNext;
		%this.setMoveSpeed( %speed );
		%this.setMoveDestination(%node.getTransform(), false );
	}
}

function AIPlayer::getCameraState( %this )
{
	if( $GameOver )
		return;
	if( !isObject( %this ) )
		return;
	if( !$pathCameraFly )
	{
		if( %this.cameraNode.getName() $= "jiedian01_02")
	    {
		    PushButt.setVisible( true );
		    flashPush( 0 );
	    }
		if(%this.cameraNode.getName() $= "jiedian01_06")
		{
			sfxStopAll();
			sfxPlayOnce(back_Sound_erGuanBeiJing);
		}
		if(%this.cameraNode.getName() $= "jiedian01_44_a")
	    {
	        sfxStopAll();
			sfxPlayOnce(back_Sound_boss2BeiJing);
		}

		if( isObject( %this.cameraNode ) )
		{
			//echo("get camera state current node Index is:  "@%this.currentNodeIndex );
			%this.currentNodeIndex += 1;
			%this.currentNode = %this.cameraNode;
			%speed = %this.cameraNode.msToNext;
			%this.setMoveSpeed( %speed );
			%this.setMoveDestination(%this.cameraNode.getTransform(), false );
		}
		commandToClient( %this.client, 'showZhunBeiZhanDou' );
		%this.ContinueMove();
	}
	else
	    %this.schedule( 1000, getCameraState );
	return;
}

function AIPlayer::moveToNode( %this )
{
	if( !isObject( %this ) )
		return;
	if( !isObject( %this.path ) )
		return;
	%number = %this.path.getCount();
	%randomNode = 0;
	for( %i = 0; %i < %number; %i++ )
	{
		%node = %this.path.getObject(%i);
		if( %node == %this.currentNode )
		{
			//call the function of nodeMethods.cs
			%num = getLianTongNodeCount( %node );
			if( %num != 0 )
			{
				%randomNum = getRandom( 1, %num );
				%randomNode = getLianTongNode( %node, %randomNum );
		    	if( isobject( %randomNode ) )
				{
					%this.currentNode = %randomNode;
				}
			}
			break;
		}
	}
	if( !isobject( %randomNode )/*&&( %this == $BossYiGuan || %this == $BossErGuan || %this == $BossDianJu )*/ )
	{
		%randomNode = %this.path.getObject(0);
		%this.currentNode = %randomNode;
	}
	
	if( isobject( %randomNode ) )
	{
		%speed = %randomNode.msToNext;
		%this.setMoveSpeed( %speed );
		%this.setMoveDestination(%randomNode.getTransform(), false );
	}
	return;
}

function AIPlayer::Activation( %this )
{
	if( !isObject( %this ) )
		return;
	if( %this.AiPlayerDb.attackType !$= "jinshen" )
	{
		%this.currentAniEndDo = %this.AnimateScripts.Ani1EndDo;
		%this.currentAniEndProb = %this.AnimateScripts.Ani1EndProb;
		%this.currentThread = %this.AnimateScripts.Ani1;
	
		if( %this.AnimateScripts.Ani1 !$= "run" )
		{
			%this.setActionThread( %this.AnimateScripts.Ani1 );
			%this.GetAniFireInformation( %this.AnimateScripts.Ani1 );
		}
		else
		{
			%this.GetAniFireInformation("run");
			if( %this.path.lifeTime !$= "" )
			{
				%this.checkIsTimeToDelete();
			}
			//boss1做特殊逻辑

			if( %this.aiPlayerDb !$= "BossYiGuan" && %this.aiPlayerDb !$= "BossErGuan" && %this.aiPlayerDb !$= "BossDianJu")
			{
				%this.moveToNode();
			}
			else if( %this.aiPlayerDb $= "BossYiGuan" || %this.aiPlayerDb $= "BossDianJu" )
			{
				commandToClient( $PlayerP1.client, 'showBossHealth');
				commandToClient( $PlayerP1.client, 'showBossHealthTotal');
				%ani = "tiaoxin";
				%this.schedule( 10, doAnimation, %ani );
				schedule( 1000, 0, clientCmdwrapBossWeapon );
                if(%this.aiPlayerDb $= "BossDianJu")
				{
                    RightArmDamage1.setVisible(true);
                    BodyDamage1.setVisible(true);
                    LeftArmDamage1.setVisible(true);
                    EyeDamage1.setVisible(true);
					$ShowRightArm = true;
                    ShowRightDamage();
			    }
				if(%this.aiPlayerDb $= "BossYiGuan")
				{
					HeadDamage1.setVisible(true);
					$ShowHead = true;
					ShowHeadDamage(false);
				}
			}
			else if( %this.aiPlayerDb $= "BossErGuan" )
			{
				commandToClient( $PlayerP1.client, 'showBossHealth');
				commandToClient( $PlayerP1.client, 'showBossHealthTotal');
				schedule( 1000, 0, clientCmdwrapBossWeapon );
				BossErGuanZuoZhuaState::OnEnter( %this );
                RightClawDamage1.setVisible(true);
				LeftClawDamage1.setVisible(true);  
				PoisonFogDamage1.setVisible(true);
				TailDamage1.setVisible(true);
				BothClawDamage1.setVisible(true);
				$ShowRightClaw = true;
				ShowRightClawDamage();
			}
			else if( %this.aiPlayerDb $= "zongBoss" )
			{
				commandToClient( $PlayerP1.client, 'showBossHealth' );
				commandToClient( $PlayerP1.client, 'showBossHealthTotal');
				schedule( 1000, 0, clientCmdwrapBossWeapon );
			}
		}
	}
	else
	{
		%this.currentAniEndDo = "";
		%this.currentAniEndProb = "";
		%this.currentThread = "";
		if( %this.path.lifeTime !$= "" )
	    {
			%this.checkIsTimeToDelete();
		}
		%this.moveToNode();
        if(%this.aiPlayerDb $= "zongBoss")
		{
			LeftHand1Damage1.setVisible(true);
			RightHand1Damage1.setVisible(true);
			LeftHand2Damage1.setVisible(true);
			RightHand2Damage1.setVisible(true);
			MouthDamage1.setVisible(true);
			$ShowLeftHand1 = true;
			ShowLeftHand1Damage();
		}
	} 
	return;
}

//获得攻击对象
function AIPlayer::getAttackObj( %this )
{
	if(!isObject($PlayerP1))
		return;
		
	if( !isObject( %this ) )
		return;

	// 检测是否到了警戒范围
	if( %this.AiPlayerDb.attackType !$= "jinshen" )
	{
		%rtt=vectorDist($PlayerP1.getposition(), %this.getposition());
		%radius = %this.radius;
		if(%rtt < %radius)
		{
			%this.Activation();
		}
		else
			%this.schedule( 1000, getAttackObj );
	}
	else
	{
		%this.Activation();
		if( %this.aiPlayerDb !$= "zongBoss" )
		{
			%this.getPlayerInfo();
		}
	}		
	return;
}

//获取玩家信息
function AIPlayer::getPlayerInfo( %this )
{
	if( !isObject( %this ) )
		return;
	%rtt=vectorDist($PlayerP1.getposition(), %this.getposition());
	%radius = %this.radius;
	if(%rtt < %radius)
	{
		%this.checkEnoughToAttack();
		return;
	}
	if( isObject( %this ) )
	{
		%this.schedule( 1000, getPlayerInfo );
	}
	return;
}

//检测是否到达攻击距离
function AIPlayer::checkEnoughToAttack(%this)
{
	if( !isObject( %this ) )
		return;
	%des = vectorDist($PlayerP1.getposition(), %this.getposition());
	if( %des < %this.AiPlayerDb.attackRadius  )
	{
		%this.stop();
		if( %this.path.dongHua != 1 )
		{
			%this.aimAt($PlayerP1);
		}
		%this.animationDone = false;
		%this.execJinShenAniScripts();
	}
	else 
	{
		if( %this.animationDone && %this.AiPlayerDb !$= "bossDaMen" && %this.AiPlayerDb !$= "bingyingdonghua" )
		{
			%speed = %this.currentNode.msToNext;
			%this.setMoveSpeed( %speed );
			%this.setMoveDestination($PlayerP1.getTransform(), false );
		}
	}
	
	%this.schedule(1000, checkEnoughToAttack );
	return;
}
//近身攻击执行脚本
function AIPlayer::execJinShenAniScripts( %this )
{
	if( !isObject( %this ) )
		return;
	if( %this.currentThread $= "" )
	{
		%this.currentAniEndDo = %this.AnimateScripts.Ani1EndDo;
		%this.currentAniEndProb = %this.AnimateScripts.Ani1EndProb;
		%this.currentThread = %this.AnimateScripts.Ani1;
		%this.schedule( 10,doAnimation,%this.currentThread );
	}
	else
	{
		%string1 = %this.currentAniEndDo;
		%string2 = %this.currentAniEndProb;
		%num1 = getWordCount(%string1);//防止溢出
		%num2 = getWordCount(%string2);
		%random = getRandom( 1, 100 );
		for(%i = 0; %i < %num2; %i++ )
		{
			%max = 0;
			if( %i > 0 )
			{
				for(%j = 0; %j <= %i; %j++ )
				{
					%max += getWord( %string2, %j );
				}
			}
			else
			{
				%max = getWord( %string2, 0 );
			}
			if( %random <= %max )
			{
				%ani = getWord( %string1, %i );
				break;
			}
		}
		%this.currentThread = %ani;
		%this.schedule( 10,doAnimation,%this.currentThread );
		
		%AniCount = %this.GetScriptsAniCount();
		for( %i = 1; %i <= %AniCount; %i++ )
		{
			if( %ani $= getVariable(%this.AnimateScripts@".Ani"@%i ) )
			{
				%endDo = getVariable(%this.AnimateScripts@".Ani"@%i@"EndDo");
				
				if( %endDo !$= "" )
				{
					%this.currentAniEndDo = %endDo;
					%this.currentAniEndProb = getVariable(%this.AnimateScripts@".Ani"@%i@"EndProb" );
				}
				else
				{
					%this.animationDone = true;
				}
				break;
			}
		}
	}
	return;
}

//装备NPC
function AIPlayer::equipObject( %this )
{
	//%this.setInventory(RocketLauncher, 1);
   	//%this.setInventory(RocketLauncherAmmo, %this.maxInventory(RocketLauncherAmmo));
  	%this.mountImage(%this.AiPlayerDb.weapon, 0);
	return;
}

//检测有生存时间的NPC是否到了删除时间
function AIPlayer::checkIsTimeToDelete( %this )
{
	if( !isObject( %this ) )
		return;
		 
	if( %this.path.lifeTime !$= "" )
	{
		%lifeTime = %this.path.lifeTime * 1000;
		if( %lifeTime == 0 )
		{
			%lifeTime = 1000;
			%this.path.lifeTime = 1;
		}
		if(  %lifeTime <= %this.spawnTime )
		{
			//echo("is time to delete npc:  "@%this.aiPlayerDb@"   "@%this.path.getName() );
			if( %this.aiPlayerDb.explosion !$= "" )
			{
				if( %this.dataBlock $= "BOSSzhaoHuanLang" )
				{
					%random = getRandom(1,2);
					%ani = "siwang"@%random;
					%this.setActionThread("%ani", true);
					%this.schedule(800, delete );
				}
				
				%expData = %this.aiPlayerDb.explosion;
				if( isObject(%expData) 
					&& %expData.getClassName() $= "ExplosionData" 
					&& isObject(%this) )
				{
					%exp = new Explosion()
					{
						datablock = %expData;
						position = %this.getPosition();
					};
				}
				
				if (isObject(%exp))
				{
					MissionCleanup.add(%exp);
					//echo("checkIsTimeToDelete new Explosion is successful");
					radiusDamage(%this.aiPlayerDb.explosion, %this.getPosition(),%this.aiPlayerDb.explosion.damageRadius,%this.aiPlayerDb.explosion.radiusDamage,"RocketDamage",%this.aiPlayerDb.explosion.areaImpulse);
				}
			}
			//echo( "delete npc index: "@%this );
			//%this.dump();
			%this.schedule(100, delete );
			//%obj.setHidden(true);
			return;
		}
		else
		{
			%this.spawnTime+=1000;
			%this.schedule( 1000, checkIsTimeToDelete);
		}
	}
	return;
}

function AIPlayer::doAnimation( %this, %ani )
{
	if( !isObject( %this ) )
		return;
	%this.currentThread = %ani;
	%this.setActionThread( %ani );
	%this.GetAniFireInformation( %ani );
	return;
}

function AIPlayer::ActiveNodeExplosion( %this, %node )
{
	%string = %node.ActiveExplosion;
	%count =getWordCount( %string );
	for( %i = 0; %i < %count; %i++ )
	{
		%ExpName = getWord( %string, %i);
		%ExpPoint = getVariable(%ExpName);
		Bomb::spawn( %ExpName );
	}
	return;
}

function AIPlayer::cameraActiveNodeExplosion(%this, %node )
{
	%string = %node.ActiveExplosion;
	%count =getWordCount( %string );
	for( %i = 0; %i < %count; %i++ )
	{
		%ExpName = getWord( %string, %i);
		%ExpPoint = getVariable(%ExpName);
		Bomb::spawn( %ExpName );
	}
	return;
}

function AIPlayer::spawnNodeNPC( %this, %node )
{
	%string = %node.spawnNPC;
	if(%string $= "" )
		return;
	%count1 = getWordCount( %string );
	%objNumber = MissionGroup.getCount();
	
	for( %i = 0; %i < %count1; %i++ )
	{
		%pathName = getWord( %string, %i );
		for( %count2 = 0; %count2 < %objNumber; %count2++ )
		{
			%misObj = MissionGroup;
			if (isObject(%misObj))
			{
				%obj = %misObj.getObject(%count2);
			}
			else
			{
				%obj = 0;
			}
			
			if (isObject(%obj))
			{
				%objClassName = %obj.getClassName();
				%objName = %obj.getName();
			}
			else
			{
				%objClassName = "";
				%objName = "";
			}
		
			if( %pathName $= %objName )
			{
				%path = %obj;
				if( %path.aiPlayerDb !$= "" )
				{
					if( isObject( %path.aiPlayer ) )
					{
						//error("AIPlayer::spawnNodeNPC if( isObject( %path.aiPlayer ) )");
						%path.aiPlayer.schedule( 100, delete );
					}
					%time = 5 * ( %i + 1);
				    if( isObject(%path.aiPlayerDb) && %path.aiPlayerDb.getClassName() $= "FlyShapeData" )
					{
						%this.schedule( %time, pathFlyShapeSpawn, %path, %path.aiPlayerDb );
					}
					else
					{
						%this.schedule( %time, pathNpcSpawn, %path, %path.aiPlayerDb );
					}
				}
				break;
			}
		}
	}
	return;
}

function AIPlayer::pathNpcSpawn( %this, %path, %dataBlock )
{
	AIPlayer::spawnOnPath( %path, %path.aiPlayerDb );
}

function AIPlayer::pathFlyShapeSpawn( %this, %path, %dataBlock )
{
	FlyShape::spawnOnPath( %path, %path.aiPlayerDb );
}


function AIPlayer::cameraSpawnNodeNPC( %this, %node )
{
	%string = %node.spawnNPC;
	%count1 = getWordCount( %string );
	%objNumber = MissionGroup.getCount();
	
	for( %i = 0; %i < %count1; %i++ )
	{
		%pathName = getWord( %string, %i );
		for( %count2 = 0; %count2 < %objNumber; %count2++ )
		{
			%misObj = MissionGroup;
			if (isObject(%misObj))
			{
				%obj = %misObj.getObject(%count2);
			}
			else
			{
				%obj = 0;
			}
			
			if (isObject(%obj))
			{
				%objClassName = %obj.getClassName();
				%objName = %obj.getName();
			}
			else
			{
				%objClassName = "";
				%objName = "";
			}
		
			if( %pathName $= %objName )
			{
				%path = %obj;
				if( %path.aiPlayerDb !$= "" )
				{
					if( isObject( %path.aiPlayer ) )
					{
						%path.aiPlayer.delete();
					}
					%aiPlayer = AIPlayer::spawnOnPath( %path, %path.AiPlayerDb);
					%path.aiPlayer = %aiPlayer;
				}
				break;
			}
		}
	}
	return;
}
//-----------------------------------------------------------------------------
//
//-----------------------------------------------------------------------------

function AIPlayer::pushTask(%this,%method)
{
   if (%this.taskIndex $= "")
   {
      %this.taskIndex = 0;
      %this.taskCurrent = -1;
   }
   %this.task[%this.taskIndex] = %method;
   %this.taskIndex++;
   if (%this.taskCurrent == -1)
      %this.executeTask(%this.taskIndex - 1);
}

function AIPlayer::clearTasks(%this)
{
   %this.taskIndex = 0;
   %this.taskCurrent = -1;
}

function AIPlayer::nextTask(%this)
{
   if (%this.taskCurrent != -1)
      if (%this.taskCurrent < %this.taskIndex - 1)
         %this.executeTask(%this.taskCurrent++);
      else
         %this.taskCurrent = -1;
}

function AIPlayer::executeTask(%this,%index)
{
   %this.taskCurrent = %index;
   eval(%this.getId() @"."@ %this.task[%index] @";");
}

//-----------------------------------------------------------------------------

function AIPlayer::singleShot(%this)
{
   // The shooting delay is used to pulse the trigger
   if( !isObject(%this) )
		return;
   %this.setImageTrigger(0, true);
   %this.setImageTrigger(0, false);
   return;
   //%this.trigger = %this.schedule(%this.shootingDelay, singleShot);
}

//-----------------------------------------------------------------------------

function AIPlayer::wait(%this, %time)
{
   %this.schedule(%time * 1000, "nextTask");
}

function AIPlayer::done(%this,%time)
{
   %this.schedule(5, "delete");
}

function AIPlayer::fire(%this,%bool)
{
   if (%bool)
   {
      cancel(%this.trigger);
      %this.singleShot();
   }
   else
      cancel(%this.trigger);
   %this.nextTask();
}

function AIPlayer::aimAt(%this,%object)
{
   //echo("Aim: "@ %object);
   %this.setAimObject(%object);
   %this.nextTask();
}

function AIPlayer::GetAniFireInformation( %this, %ani )
{
	if( !isObject( %this ) )
		return;
	%AniCount = %this.GetScriptsAniCount();
	for( %i = 1; %i <= %AniCount; %i++ )
	{
		if( %ani $= getVariable(%this.AnimateScripts@".Ani"@%i ) )
		{
			%string = getVariable(%this.AnimateScripts@".Ani"@%i@"FireInfo" );
			if( %string !$= "" )
			{
				%fireDelayTime = getWord( %string, 0 );
				%fireTimes = getWord( %string, 1 );
				%fireTimeSpace = getWord( %string, 2 );
			}
			break;
		}
	}
	if( %string !$= "" )
	{
		for( %i = 0; %i < %fireTimes; %i++ )
		{
			%timeDealy = %fireDelayTime + %i * %fireTimeSpace;
			%this.schedule( %timeDealy, singleShot );
		}
	}
	return;
}

function AIPlayer::GetAniHoldInformation( %this, %ani )
{
	if( !isObject( %this ) )
		return;
	%AniCount = %this.GetScriptsAniCount();
	for( %i = 1; %i <= %AniCount; %i++ )
	{
		%test = getVariable(%this.AnimateScripts@".Ani"@%i );
		if( %ani $= getVariable(%this.AnimateScripts@".Ani"@%i ) )
		{
			%string = getVariable(%this.AnimateScripts@".Ani"@%i@"HoldInfo" );
			if( %string !$= "" )
			{
				%HoldTime = getWord( %string, 0 );
			}
			break;
		}
	}
	return %HoldTime;
}

function AIPlayer::OnPlayerStartGame( %this )
{
	error("AIPlayer::OnPlayerStartGame");
	if( $StartGame )
		return;
	if(!isobject(MissionGroup))
		return;
       
    $PlayerP1.reset();
	$PlayerP1.ContinueMove();
	
	%simCounting = MissionGroup.getCount();
	for( %j = 1; %j < 7; %j++ )
	{
		for(%i = 0; %i < %simCounting; %i++)
		{
			%obj = MissionGroup.getObject(%i);
			%objClassName = %obj.getClassName();
			%objName = %obj.getName();
			if("Path" $= %objClassName)
			{	
				//error("j = "@%j@"   i  =  "@%i@"     %objName = "@%objName);
				%str = "ZhuJiaoLuJing"@%j;
				if( %objName $= %str )
				{
					$PlayerP1.pushPath(%obj);
					if( %objName $= "ZhuJiaoLuJing1" )
					{
						$PlayerP1.path = %obj;
						$PlayerP1.followPath(%obj, -1);
						$PlayerP1.currentNodeIndex = 1;	
						$PlayerP1.currentNode = %obj.getObject(1);
					}
					break;
				}
			}
		}
	}
	setStartGameState(true);
	$StartGame = true;
	
	//schedule(5000, 0, setPlayerCanshoot, true);
	return;
}

function AIPlayer::pushPath( %this, %path )
{
	for (%i = 0; %i < %path.getCount(); %i++)
	{
    	%this.pushNode(%path.getObject(%i));
	}
}

function AIPlayer::pushNode(%this,%node)   
{
   %speed = %node.msToNext * 2;   
   if ((%type = %node.type) $= "")   
      %type = "Normal";   
   if ((%smoothing = %node.smoothing ) $= "")   
      %smoothing = "Linear";
   %this.pushBack(%node.getTransform(),%speed,%type,%smoothing);
   return;
}
// ----------------------------------------------------------------------------
// Some handy getDistance/nearestTarget functions for the AI to use
// ----------------------------------------------------------------------------

function AIPlayer::getTargetDistance(%this, %target)
{
   //echo("\c4AIPlayer::getTargetDistance("@ %this @", "@ %target @")");
   $tgt = %target;
   %tgtPos = %target.getPosition();
   %eyePoint = %this.getWorldBoxCenter();
   %distance = VectorDist(%tgtPos, %eyePoint);
   //echo("Distance to target = "@ %distance);
   return %distance;
}

function AIPlayer::getNearestPlayerTarget(%this)
{
   //echo("\c4AIPlayer::getNearestPlayerTarget("@ %this @")");

   %index = -1;
   %botPos = %this.getPosition();
   %count = ClientGroup.getCount();
   for(%i = 0; %i < %count; %i++)
   {
      %client = ClientGroup.getObject(%i);
      if (%client.player $= "" || %client.player == 0)
         return -1;
      %playerPos = %client.player.getPosition();

      %tempDist = VectorDist(%playerPos, %botPos);
      if (%i == 0)
      {
         %dist = %tempDist;
         %index = %i;
      }
      else
      {
         if (%dist > %tempDist)
         {
            %dist = %tempDist;
            %index = %i;
         }
      }
   }
   return %index;
}
//-----------------------------------------------------------------------------

function AIManager::think(%this)
{
   // We could hook into the player's onDestroyed state instead of having to
   // "think", but thinking allows us to consider other things...
   if (!isObject(%this.player))
      %this.player = %this.spawn();
   %this.schedule(500, think);
}

function AIManager::spawn(%this)
{
   %player = AIPlayer::spawnOnPath("Shootme", "MissionGroup/Path/testPath");

   if (isObject(%player))
   {
      %player.followPath("MissionGroup/Path/testPath", -1);

      // slow this sucker down, I'm tired of chasing him!
      %player.setMoveSpeed(0.5);

      //%player.mountImage(xxxImage, 0);
      //%player.setInventory(xxxAmmo, 1000);

      return %player;
   }
   else
      return 0;
}

function AIManager::StartGame( %this )
{
	%objNumber = MissionGroup.getCount();
	for( %count = 0; %count < %objNumber; %count++ )
	{
		%obj = MissionGroup.getObject(%count);
		%objClassName = %obj.getClassName();
		%objName = %obj.getName();
		
		if("Path" $= %objClassName && %objName !$= "ZhuJiaoLuJing" )
		{	
			if( %obj.aiPlayerDb !$= "" )
			{
				%aiPlayer = AIPlayer::spawnOnPath( %obj, %obj.AiPlayerDb);
				if( isObject( %aiPlayer ) )
				{
					//把产生的NPc存起来，待用
					$AiPlayerArray[$AiPlayerNumber] = %aiPlayer;
					$AiPlayerNumber++;
				}
				else
				{
					%pathName = %obj.getName();
					//echo( "something was wrong on this path!!!!!!!! "@%pathName );
				}
			}
		}
	}
	return;
}

function AIPlayer::setPositionByFollow( %this )
{
	if( !isObject( %this ) )
		return;
	if( isObject( %this.followObj ) )
	{
		%test = %this.followObj.getPosition();
		%this.setPosition( %this.followObj.getPosition() );
		%this.schedule( 40, setPositionByFollow );
		return;
	}
	else
		return;
}

function AIPlayer::changeBig( %obj, %multiple )
{
	%large = %obj.getLargeX();
	if( %multiple > %large )
	{
		%large += 0.1;
	}
	else
	{
		%large -= 0.1;
	}
	if( %large == %multiple )
	{
		return;
	}
	%obj.setLarge( %large, %large, %large );
	%obj.schedule( 100, changeBig, %multiple ); 
}
