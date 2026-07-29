/*
  Rollback da estrutura do motor de integração SAP Business One.

  Atenção: este script remove as tabelas e todos os dados armazenados nelas.
*/

DROP TABLE TB_SAP_INTEGRACAO_PENDENTES;
DROP TABLE TB_SAP_INTEGRACAO_OPERACAO;

