// ============================================================
// Project            :  game
// File               :  .\scripts\server\explosionPoint.cs
// Copyright          :  
// Author             :  Administrator
// Created on         :  2010年4月14日 星期三 16:06
//
// Editor             :  Codeweaver v. 1.2.3430.42233
//
// Description        :  
//                    :  
//                    :  
// ============================================================
function Bomb::spawn( %point )
{
	return;
	%projectileDb = %point.BombDb;
	if( %point.ProSpeed !$= "" )
	{
		%speed = %point.ProSpeed;
	}
	else
	{
		%speed = %projectileDb.muzzleVelocity;
	}
	%projectile = new Projectile()
   {
      dataBlock = %projectileDb;
      initialVelocity = VectorScale(%point.getZReverseVector(), %speed );
	  initialPosition = %point.getPosition();
   };
   MissionCleanup.add(%projectile);
   return %projectile;
}

function Bomb::spawnExplosion( %obj, %dataBlock )
{
	if( isObject(%dataBlock) 
		&& %dataBlock.getClassName() $= "ExplosionData" 
		&& isObject(%obj) )
	{
		%pos = %obj.getPosition();
		%pos.y += 11;
		%pos.z += 2;
		%pos.x -= 12;
		
		%exp = new Explosion()
		{
			datablock = %dataBlock;
			position = %pos;
		};
		
		if (isObject(%exp))
		{
			MissionCleanup.add(%exp);
		}
	}
	else
	{
		%exp = "";
	}
	
	return %exp;
}