namespace webkyo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class xx : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Alumno",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Apellido = c.String(),
                        Telefono = c.String(),
                        Edad = c.Short(nullable: false),
                        Sexo = c.Short(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                        Cinturon_Id = c.Int(),
                        Dojo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Cinturon", t => t.Cinturon_Id)
                .ForeignKey("dbo.Dojo", t => t.Dojo_Id)
                .Index(t => t.Cinturon_Id)
                .Index(t => t.Dojo_Id);
            
            CreateTable(
                "dbo.Cinturon",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        ColorCodigo = c.String(),
                        Nivel = c.Short(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Dojo",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Nombre = c.String(),
                        Direccion = c.String(),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                        Usuario_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Usuario", t => t.Usuario_Id)
                .Index(t => t.Usuario_Id);
            
            CreateTable(
                "dbo.Asistencia",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                        Alumno_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Alumno", t => t.Alumno_Id)
                .Index(t => t.Alumno_Id);
            
            CreateTable(
                "dbo.Examen",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Fecha = c.DateTime(nullable: false),
                        Comentario = c.String(),
                        Aprobado = c.Boolean(nullable: false),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                        Alumno_Id = c.Int(),
                        CinturonActual_Id = c.Int(),
                        CinturonProximo_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Alumno", t => t.Alumno_Id)
                .ForeignKey("dbo.Cinturon", t => t.CinturonActual_Id)
                .ForeignKey("dbo.Cinturon", t => t.CinturonProximo_Id)
                .Index(t => t.Alumno_Id)
                .Index(t => t.CinturonActual_Id)
                .Index(t => t.CinturonProximo_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.Int(nullable: false),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
						DojoId = c.Int(nullable: true),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.Int(nullable: false),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Usuario",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        NombreUsuario = c.String(),
                        Clave = c.String(),
                        FechaAlta = c.DateTime(nullable: false),
                        FechaModificacion = c.DateTime(nullable: false),
                        UsuarioAltaId = c.String(),
                        UsuarioModificacionId = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Dojo", "Usuario_Id", "dbo.Usuario");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Examen", "CinturonProximo_Id", "dbo.Cinturon");
            DropForeignKey("dbo.Examen", "CinturonActual_Id", "dbo.Cinturon");
            DropForeignKey("dbo.Examen", "Alumno_Id", "dbo.Alumno");
            DropForeignKey("dbo.Asistencia", "Alumno_Id", "dbo.Alumno");
            DropForeignKey("dbo.Alumno", "Dojo_Id", "dbo.Dojo");
            DropForeignKey("dbo.Alumno", "Cinturon_Id", "dbo.Cinturon");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.Examen", new[] { "CinturonProximo_Id" });
            DropIndex("dbo.Examen", new[] { "CinturonActual_Id" });
            DropIndex("dbo.Examen", new[] { "Alumno_Id" });
            DropIndex("dbo.Asistencia", new[] { "Alumno_Id" });
            DropIndex("dbo.Dojo", new[] { "Usuario_Id" });
            DropIndex("dbo.Alumno", new[] { "Dojo_Id" });
            DropIndex("dbo.Alumno", new[] { "Cinturon_Id" });
            DropTable("dbo.Usuario");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Examen");
            DropTable("dbo.Asistencia");
            DropTable("dbo.Dojo");
            DropTable("dbo.Cinturon");
            DropTable("dbo.Alumno");
        }
    }
}
