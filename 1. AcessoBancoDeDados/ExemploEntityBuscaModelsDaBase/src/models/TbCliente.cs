using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ExemploEntityBuscaModelsDaBase.src.models;

[Table("TB_CLIENTES")]
[Index("Codigoclasseconsumo", Name = "FK_TB_CLIENTES_TBCLASCONSLIG")]
[Index("Codigomeiodivulgacao", Name = "FK_TB_CLIEN_R_1004156_TB_MEIOD")]
[Index("Codigoprofissao", Name = "FK_TB_CLIEN_R_1004164_TB_PROFI")]
[Index("Codigoreligiao", Name = "FK_TB_CLIEN_R_1200_TB_RELIG")]
[Index("Codigocidade", Name = "FK_TB_CLIEN_R_1400_TB_CIDAD")]
[Index("Codigosexo", Name = "FK_TB_CLIEN_R_1500_TB_SEXO")]
[Index("Codigoestadocivil", Name = "FK_TB_CLIEN_R_2000_TB_ESTAD")]
[Index("Codigotipopessoa", Name = "FK_TB_CLIEN_R_2700_TB_TIPOP")]
[Index("Codigoescolaridade", Name = "FK_TB_CLIEN_R_3300_TB_ESCOL")]
[Index("Codigoempresa", Name = "FK_TB_CLIEN_R_6000169_TB_EM")]
[Index("Ordemclassificacaoentidade", Name = "FK_TB_CLIEN_R_900_TB_CLASS")]
[Index("Codigocliente", Name = "PK_TB_CLIENTES", IsUnique = true)]
public partial class TbCliente
{
    [Key]
    [Column("CODIGOCLIENTE")]
    public int Codigocliente { get; set; }

    [Column("ORDEMCLASSIFICACAOENTIDADE")]
    public int? Ordemclassificacaoentidade { get; set; }

    [Column("CODIGOMEIODIVULGACAO")]
    public int? Codigomeiodivulgacao { get; set; }

    [Column("CODIGOSEXO", TypeName = "CHAR(1)")]
    public string? Codigosexo { get; set; }

    [Column("CODIGOESCOLARIDADE")]
    public int? Codigoescolaridade { get; set; }

    [Column("CODIGORELIGIAO")]
    public int? Codigoreligiao { get; set; }

    [Column("CODIGOTIPOPESSOA", TypeName = "CHAR(1)")]
    public string? Codigotipopessoa { get; set; }

    [Column("CODIGOESTADOCIVIL", TypeName = "CHAR(1)")]
    public string? Codigoestadocivil { get; set; }

    [Column("CODIGOCIDADE")]
    public int Codigocidade { get; set; }

    [Column("RAZAOSOCIALCLIENTE")]
    [StringLength(50)]
    public string? Razaosocialcliente { get; set; }

    [Column("FANTASIACLIENTE")]
    [StringLength(50)]
    public string? Fantasiacliente { get; set; }

    [Column("CNPJCLIENTE")]
    [StringLength(18)]
    public string? Cnpjcliente { get; set; }

    [Column("CPFCLIENTE")]
    [StringLength(14)]
    public string? Cpfcliente { get; set; }

    [Column("RGCLIENTE")]
    [StringLength(25)]
    public string? Rgcliente { get; set; }

    [Column("ENDERECOCLIENTE")]
    [StringLength(50)]
    public string? Enderecocliente { get; set; }

    [Column("BAIRROCLIENTE")]
    [StringLength(30)]
    public string? Bairrocliente { get; set; }

    [Column("CEPCLIENTE")]
    [StringLength(9)]
    public string? Cepcliente { get; set; }

    [Column("FONECLIENTE")]
    [StringLength(20)]
    public string? Fonecliente { get; set; }

    [Column("FAXCLIENTE")]
    [StringLength(20)]
    public string? Faxcliente { get; set; }

    [Column("CELULARCLIENTE")]
    [StringLength(20)]
    public string? Celularcliente { get; set; }

    [Column("EMAILCLIENTE")]
    [StringLength(500)]
    public string? Emailcliente { get; set; }

    [Column("DTNASCCLIENTE", TypeName = "DATE")]
    public DateTime? Dtnasccliente { get; set; }

    [Column("NATURALIDADECLIENTE")]
    [StringLength(50)]
    public string? Naturalidadecliente { get; set; }

    [Column("NACIONALIDADECLIENTE")]
    [StringLength(30)]
    public string? Nacionalidadecliente { get; set; }

    [Column("ATIVIDADECLIENTE")]
    [StringLength(100)]
    public string? Atividadecliente { get; set; }

    [Column("DTCADASTROCLIENTE", TypeName = "DATE")]
    public DateTime? Dtcadastrocliente { get; set; }

    [Column("CCECLIENTE")]
    [StringLength(20)]
    public string? Ccecliente { get; set; }

    [Column("CONTATOCLIENTE")]
    [StringLength(50)]
    public string? Contatocliente { get; set; }

    [Column("HOMEPAGECLIENTE")]
    [StringLength(100)]
    public string? Homepagecliente { get; set; }

    [Column("DADOSADICIONAISCLIENTE", TypeName = "VARCHAR(20000)")]
    public string? Dadosadicionaiscliente { get; set; }

    [Column("CAIXAPOSTALCLIENTE")]
    [StringLength(10)]
    public string? Caixapostalcliente { get; set; }

    [Column("CODIGOPROFISSAO")]
    public int? Codigoprofissao { get; set; }

    [Column("DTCLIENTEDESDE", TypeName = "DATE")]
    public DateTime? Dtclientedesde { get; set; }

    [Column("CODIGOEMPRESA")]
    public int? Codigoempresa { get; set; }

    [Column("EMAILNFECLIENTE")]
    [StringLength(500)]
    public string? Emailnfecliente { get; set; }

    [Column("INSCRICAOSUFRAMACLIENTE")]
    [StringLength(10)]
    public string? Inscricaosuframacliente { get; set; }

    [Column("COMPLEMENTOENDERECOCLIENTE")]
    [StringLength(100)]
    public string? Complementoenderecocliente { get; set; }

    [Column("NUMEROENDERECOCLIENTE")]
    [StringLength(10)]
    public string? Numeroenderecocliente { get; set; }

    [Column("DTEMISSAORGCLIENTE", TypeName = "DATE")]
    public DateTime? Dtemissaorgcliente { get; set; }

    [Column("ORGAOEXPEDIDORRGCLIENTE")]
    [StringLength(10)]
    public string? Orgaoexpedidorrgcliente { get; set; }

    [Column("CORPELECLIENTE")]
    [StringLength(30)]
    public string? Corpelecliente { get; set; }

    [Column("RNTRCCLIENTE")]
    [StringLength(14)]
    public string? Rntrccliente { get; set; }

    [Column("TIPOCCECLIENTE", TypeName = "CHAR(1)")]
    public string? Tipoccecliente { get; set; }

    [Column("IDESTRANGEIROCLIENTE")]
    [StringLength(20)]
    public string? Idestrangeirocliente { get; set; }

    [Column("INCLUIRNUMEROPEDNFE", TypeName = "CHAR(1)")]
    public string? Incluirnumeropednfe { get; set; }

    [Column("COPIARENDERECOCOBRANCA", TypeName = "CHAR(1)")]
    public string? Copiarenderecocobranca { get; set; }

    [Column("SUFRAMACLIENTE")]
    [StringLength(9)]
    public string? Suframacliente { get; set; }

    [Column("CODIGOCLASSECONSUMO")]
    public int? Codigoclasseconsumo { get; set; }

    [Column("CODIGORENASEM")]
    [StringLength(30)]
    public string? Codigorenasem { get; set; }

    [Column("ANEXO")]
    public bool? Anexo { get; set; }
}
