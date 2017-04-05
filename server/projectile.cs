// ----------------------------------------------------------------------------
// Torque 3D
// Copyright (C) GarageGames.com, Inc.
// ----------------------------------------------------------------------------

// "Universal" script methods for projectile damage handling.  You can easily
// override these support functions with an equivalent namespace method if your
// weapon needs a unique solution for applying damage.

function ProjectileData::onCollision(%data, %proj, %col, %fade, %pos, %normal)
{
   //echo("ProjectileData::onCollision("@%data.getName()@", "@%proj@", "@%col.getClassName()@", "@%fade@", "@%pos@", "@%normal@")");
   //echo( "col datablock: "@%col.getClassName() );
	if(!isObject(%col) || !isObject(%proj))
	{
		return;
	}
	
   if( %proj.sourceObj.dataBlock $= "DefaultPlayerData"  )
   {
		// Apply damage to the object all shape base objects
		if (%data.directDamage > 0)
		{
			if (%col.getType() & ($TypeMasks::ShapeBaseObjectType))
				%col.damage(%proj.sourceObj, %pos, (%data.directDamage* $attackPercent), %data.damageType, %proj.sourceObjP2);
			return;
		}	
   }
	else if( %col.dataBlock $= "DefaultPlayerData" )
	{
		if( $PlayerP1.wudi )
			return;
		if( $Playerdead )
			return;	
		
		if( %proj.dataBlock $= "Jbossdaodanwx" 
			|| %proj.dataBlock $= "Jbossdaodan01" 
			|| %proj.dataBlock $= "Lbossdaodan01"
			|| %proj.dataBlock $= "Fbossdaodan01"
			|| %proj.dataBlock $= "feijidaodan" )
		{
			////PCVRSetPlayerHurtShake();
			doudongRotate();
		}
		//$playerDamageLevel += (%proj.dataBlock.directDamage * $damagePercent);
		//clientCmddrawPlayerHealth();
		judgeHurtPlayerOnly(true, (%proj.dataBlock.directDamage * $damagePercent));
		return;		
	}
}

function ProjectileData::onExplode(%data, %proj, %position, %mod)
{
   //echo("ProjectileData::onExplode("@%data.getName()@", "@%proj@", "@%position@", "@%mod@")");

   // Damage objects within the projectiles damage radius
//    if( isObject( %proj ) )
//	{
//   		%proj.schedule(100, delete );
//	}
	if( %proj.sourceObj.dataBlock !$= "DefaultPlayerData"  )
	{
		return;
	}
    if ( %data.damageRadius > 0 )
       radiusDamage(%proj.origin, %position, %data.damageRadius, %data.radiusDamage, %data.damageType, %data.areaImpulse);
}
