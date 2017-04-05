//-----------------------------------------------------------------------------
// Torque Game Engine
// Copyright (C) GarageGames.com, Inc.
//-----------------------------------------------------------------------------

// This file contains ShapeBase methods used by all the derived classes

//-----------------------------------------------------------------------------
// ShapeBase object
//-----------------------------------------------------------------------------

// A raycast helper function to keep from having to duplicate code everytime
// that a raycast is needed.
//  %this = the object doing the cast, usually a player
//  %range = range to search
//  %mask = what to look for

function ShapeBase::doRaycast(%this, %range, %mask)
{
   // get the eye vector and eye transform of the player
   %eyeVec = %this.getEyeVector();
   %eyeTrans = %this.getEyeTransform();

   // extract the position of the player's camera from the eye transform (first 3 words)
   %eyePos = getWord(%eyeTrans, 0) SPC getWord(%eyeTrans, 1) SPC getWord(%eyeTrans, 2);

   // normalize the eye vector
   %nEyeVec = VectorNormalize(%eyeVec);

   // scale (lengthen) the normalized eye vector according to the search range
   %scEyeVec = VectorScale(%nEyeVec, %range);

   // add the scaled & normalized eye vector to the position of the camera
   %eyeEnd = VectorAdd(%eyePos, %scEyeVec);

   // see if anything gets hit
   %searchResult = containerRayCast(%eyePos, %eyeEnd, %mask, %this);

   return %searchResult;
}

//-----------------------------------------------------------------------------

function ShapeBase::damage(%this, %sourceObject, %position, %damage, %damageType, %other)
{
   // All damage applied by one object to another should go through this method.
   // This function is provided to allow objects some chance of overriding or
   // processing damage values and types.  As opposed to having weapons call
   // ShapeBase::applyDamage directly. Damage is redirected to the datablock,
   // this is standard procedure for many built in callbacks.

    if( %this.dataBlock $= "daoDanBao" )
	{
		if( %this.wudi )
		{
			if (%other $= "p2Obj")
			{
				initAmmoBagFlyP2();
				$PlayerDaoDanNumP2 += %this.dataBlock.buChongNum;
				emptyMissile.setVisible( false );
				MyShineP2.setVisible(false);
				commandToClient($PlayerP1.client, 'showDaoDanNumP2' );
			}
			else
			{
				initAmmoBagFly();
				$PlayerDaoDanNum += %this.dataBlock.buChongNum;
				emptyMissile.setVisible( false );
				MyShine.setVisible(false);
				commandToClient($PlayerP1.client, 'showDaoDanNum' );
			}
			%this.schedule( 10, delete );
			return;
		}
	}
	if( %this.dataBlock $= "daXueBao" || %this.dataBlock $= "xiaoXueBao" )
	{
		if( %this.wudi )
		{
			if (%other $= "p2Obj")
			{
				if ($player2State > 0)
				{
					if( %this.dataBlock $= "daXueBao" )
					{
						initBloodBagFly(1);
						$playerDamageLevelp2 -= 3000;
						if( $playerDamageLevelp2 < 0 )
							$playerDamageLevelp2 = 0;
					}
					else
					{
						initBloodBagFly(0);
						$playerDamageLevelp2 -= 1000;
						if( $playerDamageLevelp2 < 0 )
							$playerDamageLevelp2 = 0;
					}
					
					changeBloodP2();
				}
			}
			else
			{
				if ($player1State > 0)
				{
					if( %this.dataBlock $= "daXueBao" )
					{
						initBloodBagFly(1);
						$playerDamageLevel -= 3000;
						if( $playerDamageLevel < 0 )
							$playerDamageLevel = 0;
					}
					else
					{
						initBloodBagFly(0);
						$playerDamageLevel -= 1000;
						if( $playerDamageLevel < 0 )
							$playerDamageLevel = 0;
					}
					commandToClient( $PlayerP1.client,'drawPlayerHealth' );
				}
			}
			%this.schedule( 10, delete );
			return;		
		}
	} 
	if (isObject(%this))
      %this.getDataBlock().damage(%this, %sourceObject, %position, %damage, %damageType);
}

//-----------------------------------------------------------------------------

function ShapeBase::setDamageDt(%this, %damageAmount, %damageType)
{
   // This function is used to apply damage over time.  The damage is applied
   // at a fixed rate (50 ms).  Damage could be applied over time using the
   // built in ShapBase C++ repair functions (using a neg. repair), but this
   // has the advantage of going through the normal script channels.

   if (%this.getState() !$= "Dead")
   {
      %this.damage(0, "0 0 0", %damageAmount, %damageType);
      %this.damageSchedule = %this.schedule(50, "setDamageDt", %damageAmount, %damageType);
   }
   else
      %this.damageSchedule = "";
}

function ShapeBase::clearDamageDt(%this)
{
   if (%this.damageSchedule !$= "")
   {
      cancel(%this.damageSchedule);
      %this.damageSchedule = "";
   }
}

//-----------------------------------------------------------------------------
// ShapeBase datablock
//-----------------------------------------------------------------------------

function ShapeBaseData::damage(%this, %obj, %position, %source, %amount, %damageType)
{
   // Ignore damage by default. This empty method is here to
   // avoid console warnings.
}