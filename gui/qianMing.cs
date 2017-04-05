// ============================================================
// Project            :  game
// File               :  .\scripts\client\NSPlane.cs
// Copyright          :  
// Author             :  Administrator
// Created on         :  2010年8月25日 14:12
//
// Editor             :  Codeweaver v. 1.2.2594.2497
//
// Description        :  
//                    :  
//                    :  
// ============================================================
$PlayerEnterName = false;
$ActiveQianMingGui = false;

function QianMingGui::onWake(%this)
{
	GBitmapLetter0.setBitmap("");
	GBitmapLetter1.setBitmap("");
	GBitmapLetter2.setBitmap("");
	GBitmapLetter3.setBitmap("");
	GBitmapLetter4.setBitmap("");
	GBitmapLetter5.setBitmap("");
	$NameIndex[0] = 0;
	$NameNum = 0;
	$PlayerEnterName = false;
	activateDirectInput();
	%this.daoJiShi(60);
	QianMingGuangBiao.setVisible(false);
	QianMingGuangBiaoP2.setVisible(false);
	
	if ($player1State >= 0)
	{
		QianMingGuangBiao.setVisible(true);
	}
	
	if ($player2State >= 0)
	{
		QianMingGuangBiaoP2.setVisible(true);
	}
}

function QianMingGui::daoJiShi( %this, %timeIndex )
{
	if( $PlayerEnterName || !QianMingGui.isAwake() )
		return;
	%timeIndex--;
	%time1 =  mFloor( %timeIndex / 10 );
	%time2 = %timeIndex % 10;
	if( %timeIndex == 59 )
	{
		$ActiveQianMingGui = true;
		PCVRSetQianmingState(true);
	}
	if( %timeIndex < 0 )
	{
		saveName();
		return;
	}
	GBitmapTimer1.setDrawPartOfBitmap(1, 0, 0, %time1 * 25, 0, 25, 42);
	GBitmapTimer2.setDrawPartOfBitmap(1, 0, 0, %time2 * 25, 0, 25, 42);
	%this.schedule( 1000, daoJiShi, %timeIndex );
}

function saveName()
{
	if( $PlayerEnterName )
	{
		return;
	}
	for( %i = 4; %i > $NameNum; %i-- )
	{
		$NameIndex[ %i ] = -1;
	}
	for( %i = 9; %i >= 0; %i-- )
	{
		%index = 0;
		if( %i > 0 )
		{
			%index = %i - 1;
		}
		%compareScore = GetIniValueNum( "Score"@%index, "Score", "./nameSort.hnb" );
		if( %i == 0 || $PlayerAllScore < %compareScore )
		{
			for( %j = 9; %j >= %i; %j-- )
			{
				%jScore = GetIniValueNum( "Score"@%j, "Score", "./nameSort.hnb" );
				WriteIniValueString( "Score"@(%j+1), "Score", %jScore, "./nameSort.hnb" );
				for( %nameIndex = 0; %nameIndex < 5; %nameIndex++ )
				{
					%name = GetIniValueNum("Player"@%j, "Name"@%nameIndex, "./nameSort.hnb" );
					WriteIniValueString( "Player"@( %j + 1), "Name"@%nameIndex, %name, "./nameSort.hnb" );
				}
			}
			WriteIniValueString("Score"@%i, "Score", $PlayerAllScore, "./nameSort.hnb");
			for( %count = 0; %count < 5; %count++ )
			{
				WriteIniValueString( "Player"@%i, "Name"@%count, $NameIndex[%count], "./nameSort.hnb" );
			}
			break;
		}
	}
	$PlayerEnterName = true;
	if( $LoadingLevel != 1 )
	{
		disconnect();
	}
	else
	{
		%reset = reStartGame();
		if( %reset )
		{
			Canvas.schedule( 100, setContent, "touBiWindow" );
		}
	}
}

function QianMingGui::onSleep( %this )
{
	for( %i = 0; %i < $NameNum; %i++ )
	{
		$NameIndex[ %i ] = 0;
		%letterCtrl = "GBitmapLetter"@%i;
		%letterCtrl.setBitmap("");
	}
	$NameNum = 0;
	$ActiveQianMingGui = false;
	PCVRSetQianmingState(false);
	QianMingGuangBiao.setVisible(false);
	QianMingGuangBiaoP2.setVisible(false);
}

function setPlayerQMLetter( %index )
{
	if( $NameNum < 5 )
	{
		%letterCtrl = "GBitmapLetter"@$NameNum;
		%letterCtrl.setBitmap("art/gui/qianMingGui/showQianMingLetter.png");
		%letterCtrl.setDrawPartOfBitmap(1, 0, 0, %index * 40, 0, 40, 42);
		$NameIndex[$NameNum] = %index;
		$NameNum++;
	}
}

function deletePlayerQMLetter()
{
	if( $NameNum > 0 )
	{
		$NameNum--;
	}
	if( $NameNum >= 0 )
	{
		%letterCtrl = "GBitmapLetter"@$NameNum;
		%letterCtrl.setBitmap("");
		$NameIndex[$NameNum] = 0;
	}
}

