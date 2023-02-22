

Console.Clear();

//!preparando variavel para realizar as chamadas
ChamadasPersonagens chamaP = new ChamadasPersonagens("https://localhost:7101");

//!criando personagem 1
chamaP.criaPersonagem("Jack", "Mage");

//!criando personagem 2
chamaP.criaPersonagem("Bernard", "Warrior");

//!listando personagem 1
//chamaP.listaPersonagemPorId(1);

//!listando personagem 1
//chamaP.listaPersonagemPorId2(2);

//!listando todos
chamaP.listaTodos();


new ChamadasArena("https://localhost:7101").combate(chamaP.retornaPersonagemPorId(1), chamaP.retornaPersonagemPorId(2));

chamaP.listaTodos();

