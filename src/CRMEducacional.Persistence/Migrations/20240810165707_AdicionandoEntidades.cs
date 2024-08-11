﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CRMEducacional.Persistence.Migrations;

/// <inheritdoc />
public partial class AdicionandoEntidades : Migration
{
    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Inscricoes");

        migrationBuilder.DropTable(
            name: "Leads");

        migrationBuilder.DropTable(
            name: "Ofertas");

        migrationBuilder.DropTable(
            name: "ProcessosSeletivos");
    }

    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Leads",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CPF = table.Column<string>(type: "nvarchar(11)", maxLength: 11, nullable: false),
                Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                Telefone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Leads", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Ofertas",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Descricao = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                VagasDisponiveis = table.Column<int>(type: "int", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Ofertas", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "ProcessosSeletivos",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                DataInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                DataTermino = table.Column<DateTime>(type: "datetime2", nullable: false),
                Nome = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_ProcessosSeletivos", x => x.Id);
            });

        migrationBuilder.CreateTable(
            name: "Inscricoes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                LeadId = table.Column<int>(type: "int", nullable: false),
                NumeroInscricao = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                OfertaId = table.Column<int>(type: "int", nullable: false),
                ProcessoSeletivoId = table.Column<int>(type: "int", nullable: false),
                Status = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Inscricoes", x => x.Id);
                table.ForeignKey(
                    name: "FK_Inscricoes_Leads_LeadId",
                    column: x => x.LeadId,
                    principalTable: "Leads",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Inscricoes_Ofertas_OfertaId",
                    column: x => x.OfertaId,
                    principalTable: "Ofertas",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
                table.ForeignKey(
                    name: "FK_Inscricoes_ProcessosSeletivos_ProcessoSeletivoId",
                    column: x => x.ProcessoSeletivoId,
                    principalTable: "ProcessosSeletivos",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Restrict);
            });

        migrationBuilder.CreateIndex(
            name: "IX_Inscricoes_LeadId",
            table: "Inscricoes",
            column: "LeadId");

        migrationBuilder.CreateIndex(
            name: "IX_Inscricoes_OfertaId",
            table: "Inscricoes",
            column: "OfertaId");

        migrationBuilder.CreateIndex(
            name: "IX_Inscricoes_ProcessoSeletivoId",
            table: "Inscricoes",
            column: "ProcessoSeletivoId");

        migrationBuilder.CreateIndex(
            name: "IX_Leads_CPF",
            table: "Leads",
            column: "CPF",
            unique: true);
    }
}