// ============================================================
// Project            :  game
// File               :  .\scripts\server\flyShape.cs
// Copyright          :  
// Author             :  Administrator
// Created on         :  2010年7月29日 星期四 11:42
//
// Editor             :  Codeweaver v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================

$FlyShapeNum = 0;					  //飞机数组
$FlyShapeArray[ $FlyShpaeNum ] = 0; 

function FlyShapeData::onAdd( %this, %obj )
{
	return;
}

function FlyShapeData::damage( %data, %obj, %sourceObject, %position, %amount, %damageType )
{
   //echo("\c4FlyShapeData::damage("@%data.getName()@", "@%obj@", "@sourceObject@", "@%position@", "@%amount@", "@%damageType@")");

   if (%obj.isDestroyed())
   {
      //echo("object already destroyed, returning");
      return;
   }
   %obj.applyDamage(%amount);
}

function FlyShapeData::onDamage( %data, %obj )
{
   //echo("\c4FlyShapeData::onDamage("@%data@", "@%obj@")");

   // Set damage state based on current damage level, we are comparing amount
   // of damage sustained to the damage levels described in the datablock and
   // setting the approppriate damageState
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

function FlyShapeData::onEnabled(%data, %obj, %state)
{
   //echo("\c4StaticShapeData::onEnabled("@%data@", "@%obj@", "@%state@")");

   // We could do things here like establishing a power connection, activation
   // sounds, play a start up sequence, add effects, etc.
}

function FlyShapeData::onDisabled(%data, %obj, %state)
{
   //echo("\c4FlyShapeData::onDisabled("@%data@", "@%obj@", "@%state@")");

   // We could do things here like disabling power, shutdown sounds, play a
   // damage sequence, swap to a specific model shape, add effects, etc.
}

function FlyShapeData::onDestroyed(%data, %obj, %prevState)
{
	for( %i = 0; %i < $FlyShapeNum; %i++ )
	{
		if( %obj == $FlyShapeArray[%i] )
		{
			for( %j = %i; %j < ( $FlyShapeNum-1 ); %j++ )
			{
				if( isObject( $FlyShapeArray[%j+1] ) )
				{
					$FlyShapeArray[ %j ] = $FlyShapeArray[%j+1];
				}
			}
			$FlyShapeNum-=1;
			//echo("when flyShapeData destroyed, the $FlyShapeArray move ahead");
			break;
		}
	}
	//echo("is time to delete flyshape:  "@%obj.aiPlayerDb@"   "@%obj.path.getName() );
	
	%expData = %obj.aiPlayerDb.explosion;
	if( isObject(%expData) 
		&& %expData.getClassName() $= "ExplosionData" 
		&& isObject(%obj))
	{
  		%exp = new Explosion()
		{
			datablock = %expData;
			position =  %obj.getPosition();
		};
	}
	//echo("checkIsTimeToDelete new Explosion is successful");
	
	if (isObject(%exp))
	{
		MissionCleanup.add(%exp);
		radiusDamage(%obj.aiPlayerDb.explosion, %obj.getPosition(),%obj.aiPlayerDb.explosion.damageRadius,%obj.aiPlayerDb.explosion.radiusDamage,"RocketDamage",%obj.aiPlayerDb.explosion.areaImpulse);
	}
	
	%obj.schedule( 50, delete );
}

function FlyShapeData::onNode(%this,%flyShape,%nodeId )
{
	%nodeObj = %flyShape.path.getObject( %nodeId );
	if( %nodeObj.fireInfo !$= "" )
	{
		%string = %nodeObj.fireInfo;
		if( %string !$= "" )
		{
			%fireDelayTime = getWord( %string, 0 );
			%fireTimes = getWord( %string, 1 );
			%fireTimeSpace = getWord( %string, 2 );
		}
	}
	if( %string !$= "" )
	{
		for( %i = 0; %i < %fireTimes; %i++ )
		{
			%timeDealy = %fireDelayTime + %i * %fireTimeSpace;
			//schedule( %timeDealy, shotFireball, %flyShape, 30, "Fbossdaodan01" );
			%flyShape.schedule( %timeDealy, singleShot );
		}
	}
}

function FlyShape::spawnOnPath( %path, %aiPlayerDb )
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
		//echo("in flyshape.cs the spawn node is not object!");
	}
	//echo("in flyshape.cs spawn a npc on  "@%path.getName() );
	
   	%player = new FlyShape()
   	{
      	dataBlock = %aiPlayerDb;
      	path = "";
   	};
   	%player.currentNode = %spawnPoint;
   	%player.AiPlayerDb = %aiPlayerDb;
   	%player.radius = %spawnPoint.radius;
   	%player.animationDone = true;
	
	if( isObject( %player ) )
	{
		//把当前点产生的NPc存起来，待用
		$CurNodeNpcArrary[$CurNodeNpcNum] = %player;
		$CurNodeNpcNum++;
	}
 
   	MissionCleanup.add(%player);
   	%player.setTransform( %spawnPoint.getTransform() );
	
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
		//echo("in flyshape.cs spawn a aiPlayer on path is not successful!");
		return 0;
	}
	
	$FlyShapeArray[ $FlyShapeNum ] = %player;
	$FlyShapeNum+=1;
   	return %player;
}

//装备NPC
function FlyShape::equipObject( %this )
{
	//%this.setInventory(RocketLauncher, 1);
   	//%this.setInventory(RocketLauncherAmmo, %this.maxInventory(RocketLauncherAmmo));
  	%this.mountImage(%this.AiPlayerDb.weapon, 0);
	return;
}

function FlyShape::getAttackObj( %this )
{
	if(!isObject($PlayerP1))
		return;
		
	if( !isObject( %this ) )
		return;

	// 检测是否到了警戒范围
	%rtt=vectorDist($PlayerP1.getposition(), %this.getposition());
	%radius = %this.radius;
	if(%rtt < %radius)
	{
			%this.Activation();
	}
	else
		%this.schedule( 1000, getAttackObj );
	return;
}

function FlyShape::Activation( %this )
{
	for (%i = 0; %i < %this.path.getCount(); %i++)
	{
      	%this.pushNode(%this.path.getObject(%i) );
	}
		
	%this.continueMove();
	
	if( %this.path.lifeTime !$= "" )
	{
		%this.checkIsTimeToDelete();
	}
}

function FlyShape::pushNode(%this,%node)   
{   
   %speed = %node.msToNext * 2;   
   if ((%type = %node.type) $= "")   
      %type = "Normal";   
   if ((%smoothing = %node.smoothing ) $= "")   
      %smoothing = "Linear";
   %this.pushBack(%node.getTransform(),%speed,%type,%smoothing);
   return;
}

function FlyShape::singleShot(%this)
{
   // The shooting delay is used to pulse the trigger
    if( !isObject( %this ) )
		return;
	shotFireball( %this, %this.getPosition(), 40, "Fbossdaodan01" );
	//zidanfei( %this, "Fbossdaodan01" );	
}

function FlyShape::checkIsTimeToDelete( %this )
{
	if( !isObject( %this ) )
		return;
		 
	if( %this.path.lifeTime !$= "" )
	{
		%lifeTime = %this.path.lifeTime * 1000;
		if(  %lifeTime <= %this.spawnTime )
		{
			%this.setDamageState(Destroyed);
         	%this.setDamageLevel( %this.getMaxDamage() );
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

function FlyShape::BossActivation( %this )
{
	for (%i = 0; %i < %this.path.getCount(); %i++)
	{
      	%this.pushNode(%this.path.getObject(%i) );
	}
		
	%this.continueMove();
}
function FlyShape::mountEffect( %this, %effectName, %index )
{
	%newEmt = new ParticleEmitterNode()
	{
    	active = "1";
    	emitter = %effectName;
    	velocity = "1";
    	dataBlock = "SmokeEmitterNode";
       	position = %srcPos1;
    	rotation = "0.462181 0.62061 0.633429 32.0885";
    	scale = "1 1 1";
    	pitchAngle = "90";
		FollowObject = bossDianJuYanYi.getId();
		dstPosition = %pos1;
		xSpeed = 100;
		ySpeed = 100;
		zSpeed = 100;
     	canSaveDynamicFields = "1";
	};
	MissionCleanup.add( %newEmt );
	%this.mountObject( %newEmt, %index );
	return %newEmt;
}

function BOSSfeiJi02::onNode(%this,%flyShape,%nodeId )
{
	%node = %flyShape.path.getObject( %nodeId );
	//error("BOSSfeiJi02::onNode %this="@%this@" %flyShape="@%flyShape@" %nodeId="@%nodeId@" %node="@%node@" %nodeName="@%node.getName() );
	//echo(%node@".delete = "@%node.delete);
	if( %node.delete !$= "" )
	{
//		%flyShape.unMountObject( 0 );
//		%flyShape.unMountObject( 1 );
//		%flyShape.unMountObject( 2 );
//		%flyShape.unMountObject( 3 );
//		for( %i = 0; %i < 4; %i++ )
//		{
//			deleteObj(%flyShape.effect[%i]);
//		}
		%flyShape.schedule( 400, delete );
	}
	if( %node.cameraMove !$= "" )
	{
		$PlayerP1.ContinueMove();
	}
	if( %node.fireInfo !$= "" )
	{
		%string = %node.fireInfo;
		if( %string !$= "" )
		{
			%fireDelayTime = getWord( %string, 0 );
			%fireTimes = getWord( %string, 1 );
			%fireTimeSpace = getWord( %string, 2 );
		}
	}
	if( %string !$= "" )
	{
		for( %i = 0; %i < %fireTimes; %i++ )
		{
			%timeDealy = %fireDelayTime + %i * %fireTimeSpace;
			schedule( %timeDealy, 0, getFlyAmmo, %flyShape );
		}
	}
}

function getFlyAmmo( %obj )
{
	shotFireball(%obj, %obj.getPosition(), 40, "Fbossdaodan01" );
	//zidanfei( %obj, "Fbossdaodan01" );
}

$DaoDanNum = 0;
function shotFireball( %obj, %startPos, %speed, %name )
{
	if (!isObject(%name) || %name.getClassName() !$= "ProjectileData")
	{
		return;
	}
	
	%endPos = $PlayerP1.getPosition();
	%muzzleVector = VectorSub( %endPos, %startPos );
	%objectVelocity = %obj.getVelocity();
	%muzzleVelocity = VectorAdd( VectorScale(%muzzleVector, %speed),VectorScale(%objectVelocity, 0.3) );
	%p = new ( projectile )()
	{
		dataBlock = %name;
		initialVelocity = %muzzleVelocity;
		initialPosition = %startPos;
		sourceObject = %obj;
		aimObject = $PlayerP1;
		sourceSlot = 0;
		client = %obj.client;
	};
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	else
	{
		return;
	}
	
	%p.sourceObj = %obj;
	%p.setIsDaodan( true );
	%p.sourceObj = %obj;
	$DaoDanList[$DaoDanNum] = %p;
	$DaoDanNum++;
	//spawnDangerCtrlFollowBoss( %p, 1, 18200, 140, true );
	//schedule( 10, 0, spawnDangerCtrlFlickFragment, %p, 1, 6 );
	spawnFlickFragmentSpecialOne(%p,80,1);
}

function newExplode( %obj, %pos, %name )
{
	if( !isObject(%name) 
		|| %name.getClassName() !$= "ExplosionData" 
		|| !isObject(%obj))
	{
		return;
	}
	
	%exp = new Explosion()
	{
		datablock = %name;
		position =  %pos;
	};
	if (%exp)
	{
		MissionCleanup.add(%exp);
		//error("当前爆炸位置 = "@%pos);
	}
	else if( !%exp )
	{
		//echo("粒子产生失败");
		return;
	}
}
function findDaoDan()
{
	for( %i = 0; %i < $DaoDanNum; %i++ )
	{
		//error("$DaoDanList["@%i@"] = "@$DaoDanList[%i]);
		if( isObject($DaoDanList[%i]) )
		{
			if( !($DaoDanList[%i]).lock )
			{
				return $DaoDanList[%i];
				//error("找到导弹了");
			
				for( %j = 0; %j <= $DaoDanNum - %i; %j++ )
				{
					if( %j == $DaoDanNum - %i )
					{
						$DaoDanNum = $DaoDanNum - %i;
						return 0;
					}
					$DaoDanList[%j] = $DaoDanList[%j + %i+1];
				}
			}
		}
	}
	$DaoDanNum = 0;
}

function explodeLoop( %obj )
{
	if( isObject(%obj) )
	{
		newExplode( %obj, %obj.getAttackPoint1(), "feiJiPenHuoFaSheQi" ); 
		newExplode( %obj, %obj.getAttackPoint2(), "feiJiPenHuoFaSheQi" ); 
		schedule(100, 0, explodeLoop, %obj );
	}
	else 
	{
		return;
	}
}

function DaoDanIsEmpty()
{
	for( %i = 0; %i < $DaoDanNum; %i++ )
	{
		if( isObject($DaoDanList[%i]) )
		{
			return true;
		}
	}
	$DaoDanNum = 0;
	return false;
}

function zidanfei( %obj, %datablock )
{
	if (!isObject(%obj))
	{
		return;
	}
	
	%clientObj = %obj.getClientObject();
	
	if (!isObject(%clientObj) 
		|| !isDefined(%dataBlock) 
		|| !isObject(%datablock) 
		|| %datablock.getClassName() !$= "ProjectileData")
	{
		return;
	}
	
	%muzzleVector = $PlayerP1.getPosition();
	%muzzleVector.z += 0.6;
	%muzzleVector = VectorSub( %muzzleVector, %obj.getPosition() );
	%objectVelocity = %obj.getVelocity();
	%muzzleVelocity = VectorAdd(
			VectorScale(%muzzleVector, 35 ),
			VectorScale(%objectVelocity, 0.3));      
	%test1 = $PlayerP1.getposition().x - %obj.getposition().x;
	%test2 = $PlayerP1.getposition().y - %obj.getposition().y;
	%test3 = msqrt(%test1*%test1 + %test2*%test2);
	%aimskewing = %obj.getPosition();
	%aimskewing.x = %test3;          //和目标点的距离距
	%aimskewing.y = 0;
	%muzzleVelocity.z += 0.35;
	%aimskewing.z = getRandom(5,7);
	%sourcePosition = %obj.getposition();
	%endPosition = $PlayerP1.getPosition();
	%p = new ( projectile )()
	{
		dataBlock = %datablock;
		initialVelocity = %muzzleVelocity;
		initialskewing = %aimskewing;
		initialPosition = %clientObj.getAttackPoint1();
		initstartposition = %sourcePosition;
		initendposition = %endPosition;
		sourceObject = %obj;
		client = %obj.client;
		chaos = true;
	};
	
	if (isObject(%p))
	{
		MissionCleanup.add(%p);
	}
	else
	{
		return;
	}
	
	%p.sourceObj = %obj;
	$DaoDanList[$DaoDanNum] = %p;
	$DaoDanNum++;
}