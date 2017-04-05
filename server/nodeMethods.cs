//游戏中路径点的有关方法
function getLianTongNode( %node, %randomNum )
{
	%nextNode = getVariable(%node@".LianTongNode"@%randomNum );
	if( isObject(%nextNode) )
		return %nextNode.getId();
	else
		echo("Node name: "@%node.getName()@"  the name of liangTongNode "@%randomNum@" is not an object!\n");
	return 0;
}

function getLianTongNodeCount( %node )
{
	if( !isObject( %node ) )
		return;
		
	%lianTongNodeCount = 0;
	for( %i = 1; isObject( getVariable(%node@".LianTongNode"@%i ) ); %i++ )
		%lianTongNodeCount++;
		
	return %lianTongNodeCount;
}





