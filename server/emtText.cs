datablock ParticleEmitterNodeData(TextEmitterNodeData)
{
   timeMultiple   = 1;
   isLooping      = false;
};

function numToText(%damage)
{
   // Loop through each character
   for(%i=0;%i <= strlen(%damage);%i++)
   {
      switch$(getSubStr(%damage,%i,1))
      {
         case "0":
            %damages = setWord(%damages,%i,"zero");
         case "1":
            %damages = setWord(%damages,%i,"one");
         case "2":
            %damages = setWord(%damages,%i,"two");
         case "3":
            %damages = setWord(%damages,%i,"three");
         case "4":
            %damages = setWord(%damages,%i,"four");
         case "5":
            %damages = setWord(%damages,%i,"five");
         case "6":
            %damages = setWord(%damages,%i,"six");
         case "7":
            %damages = setWord(%damages,%i,"seven");
         case "8":
            %damages = setWord(%damages,%i,"eight");
         case "9":
            %damages = setWord(%damages,%i,"nine");
      }
   }
   return %damages;
}

function showDamage(%obj,%damage)
{
   // Give us the text for the datablocks
   %damages = numToText(%damage);
   // Loop through each word and display them
   for(%i = 0;%i<getWordCount(%damages);%i++)
   {
      %pDam = new ParticleEmitterNode()
               {
                 // Spaces the particles out (there has to be a better way)
                 position = setWord(setWord(%obj.getTransform(),2,getWord(%obj.getTransform(),2)+2),0,getWord(%obj.getTransform(),0)+(%i*0.5));
                 rotation = "1 0 0 180";
                 scale = "1 1 1";
                 dataBlock = TextEmitterNodeData;
                 //choose the emitter
                 emitter = getWord(%damages,%i)@"Emitter";
                 velocity = 0.5;
               };
      // Throw off numbers
	  %obj.emitter[%i] = %pDam;
	  // schedule the emitter to die in 2.5 seconds (just after the lifetime of it) This is definitely not the best way to do it... but it works.
	  schedule(2500,"deleteDamage",%obj.emitter[%i]);
   }
}

function deleteDamage(%obj)
{
   %obj.delete();
}

function showScore(%obj,%score)
{
   // Give us the text for the datablocks
   //%damages = numToText(%score);
   // Loop through each word and display them
   	switch( %score )
   	{
		case 100:
			%showEmitter = yiBaiFenShuEmitter;
		case 300:
			%showEmitter = sanBaiFenShuEmitter;
		case 500:
			%showEmitter = wuBaiFenShuEmitter;
		case 700:
			%showEmitter = qiBaiFenShuEmitter;
		case 1000:
			%showEmitter = yiQianFenShuEmitter;
		case 3000:
			%showEmitter = sanQianFenShuEmitter;
		case 6000:
			%showEmitter = liuQianFenShuEmitter;
		case 9000:
			%showEmitter = jiuQianFenShuEmitter;
		case 12000:
			%showEmitter = yiWanLiangQianFenShuEmitter;
	}
    %pDam = new ParticleEmitterNode()
          {
            // Spaces the particles out (there has to be a better way)
             position = setWord( setWord(%obj.getTransform(),2,getWord(%obj.getTransform(),2)+2), 0, getWord(%obj.getTransform(), 0 ) );
             rotation = "1 0 0 180";
             scale = "1 1 1";
             dataBlock = TextEmitterNodeData;
             //choose the emitter
             emitter = %showEmitter;
             velocity = 0.5;
           };
    // Throw off numbers
    //%obj.emitter[%i]= %pDam;
	// schedule the emitter to die in 2.5 seconds (just after the lifetime of it) This is definitely not the best way to do it... but it works.
	schedule(2500,0,"deleteScore", %pDam );
}

function deleteScore(%obj)
{
   %obj.delete();
}