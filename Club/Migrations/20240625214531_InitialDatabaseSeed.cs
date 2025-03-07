using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Club.Migrations
{
    /// <inheritdoc />
    public partial class InitialDatabaseSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Coach",
                columns: table => new
                {
                    CoachID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    HireDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Coach", x => x.CoachID);
                });

            migrationBuilder.CreateTable(
                name: "Equipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PurchaseDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Equipment", x => x.EquipmentID);
                });

            migrationBuilder.CreateTable(
                name: "Member",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LastName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RegistrationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Member", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Section",
                columns: table => new
                {
                    SectionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Desc = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Section", x => x.SectionID);
                });

            migrationBuilder.CreateTable(
                name: "CardioEquipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    MaxSpeed = table.Column<int>(type: "int", nullable: false),
                    MaxIncline = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CardioEquipment", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_CardioEquipment_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Maintenance",
                columns: table => new
                {
                    MaintenanceID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    MaintenanceDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Maintenance", x => x.MaintenanceID);
                    table.ForeignKey(
                        name: "FK_Maintenance_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StrengthEquipment",
                columns: table => new
                {
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    MaxWeight = table.Column<int>(type: "int", nullable: false),
                    MuscleGroup = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StrengthEquipment", x => x.EquipmentID);
                    table.ForeignKey(
                        name: "FK_StrengthEquipment_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProgressCard",
                columns: table => new
                {
                    ProgressCardID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MeasurementDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Weight = table.Column<double>(type: "float", nullable: false),
                    BodyFatPercentage = table.Column<double>(type: "float", nullable: false),
                    MuscleMass = table.Column<double>(type: "float", nullable: false),
                    Height = table.Column<double>(type: "float", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgressCard", x => x.ProgressCardID);
                    table.ForeignKey(
                        name: "FK_ProgressCard_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Session",
                columns: table => new
                {
                    SessionID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    StartTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MaxParticipants = table.Column<int>(type: "int", nullable: false),
                    AvailableSlots = table.Column<int>(type: "int", nullable: false),
                    SectionID = table.Column<int>(type: "int", nullable: false),
                    TermsAndConditionsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Session", x => x.SessionID);
                    table.ForeignKey(
                        name: "FK_Session_Section_SectionID",
                        column: x => x.SectionID,
                        principalTable: "Section",
                        principalColumn: "SectionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "CoachAssignment",
                columns: table => new
                {
                    CoachAssignmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CoachID = table.Column<int>(type: "int", nullable: false),
                    SessionID = table.Column<int>(type: "int", nullable: false),
                    Compensation = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsLeadCoach = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachAssignment", x => x.CoachAssignmentID);
                    table.ForeignKey(
                        name: "FK_CoachAssignment_Coach_CoachID",
                        column: x => x.CoachID,
                        principalTable: "Coach",
                        principalColumn: "CoachID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CoachAssignment_Session_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Session",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Feedback",
                columns: table => new
                {
                    FeedbackId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedback", x => x.FeedbackId);
                    table.ForeignKey(
                        name: "FK_Feedback_Session_SessionId",
                        column: x => x.SessionId,
                        principalTable: "Session",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionEquipment",
                columns: table => new
                {
                    SessionEquipmentID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<int>(type: "int", nullable: false),
                    EquipmentID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionEquipment", x => x.SessionEquipmentID);
                    table.ForeignKey(
                        name: "FK_SessionEquipment_Equipment_EquipmentID",
                        column: x => x.EquipmentID,
                        principalTable: "Equipment",
                        principalColumn: "EquipmentID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionEquipment_Session_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Session",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SessionReservation",
                columns: table => new
                {
                    SessionReservationID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<int>(type: "int", nullable: false),
                    MemberID = table.Column<int>(type: "int", nullable: false),
                    ReservationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionReservation", x => x.SessionReservationID);
                    table.ForeignKey(
                        name: "FK_SessionReservation_Member_MemberID",
                        column: x => x.MemberID,
                        principalTable: "Member",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SessionReservation_Session_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Session",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TermsAndConditions",
                columns: table => new
                {
                    TermsAndConditionsID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionID = table.Column<int>(type: "int", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TermsAndConditions", x => x.TermsAndConditionsID);
                    table.ForeignKey(
                        name: "FK_TermsAndConditions_Session_SessionID",
                        column: x => x.SessionID,
                        principalTable: "Session",
                        principalColumn: "SessionID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Coach",
                columns: new[] { "CoachID", "HireDate", "LastName", "FirstName" },
                values: new object[,]
                {
                    { 1, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Wojcik", "Tomasz" },
                    { 2, new DateTime(2022, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nowak", "Barbara" },
                    { 3, new DateTime(2021, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kowalski", "Bartosz" }
                });

            migrationBuilder.InsertData(
                table: "Equipment",
                columns: new[] { "EquipmentID", "Brand", "Name", "PurchaseDate" },
                values: new object[,]
                {
                    { 1, "FitBrand", "Treadmill", new DateTime(2022, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "StrongBrand", "Bench Press", new DateTime(2021, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Endurance", "Elliptical", new DateTime(2021, 7, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "PowerBrand", "Squat Rack", new DateTime(2021, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Member",
                columns: new[] { "ID", "Email", "LastName", "FirstName", "Password", "RegistrationDate" },
                values: new object[,]
                {
                    { 1, "jan.kowalski@com.pl", "Kowalski", "Jan", "aaa123", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "anna.nowak@com.pl", "Nowak", "Anna", "bbb123", new DateTime(2022, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "piotr.wisniewski@com.pl", "Wiśniewski", "Piotr", "eee123", new DateTime(2023, 9, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "katarzyna.wojcik@com.pl", "Wójcik", "Katarzyna", "fff123", new DateTime(2022, 12, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "marek.kowalczyk@com.pl", "Kowalczyk", "Marek", "ggg123", new DateTime(2023, 1, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Section",
                columns: new[] { "SectionID", "Desc", "Name" },
                values: new object[,]
                {
                    { 1, "Cardio workouts", "Cardio" },
                    { 2, "Strength training", "Strength" },
                    { 3, "Flexibility and stretching", "Flexibility" }
                });

            migrationBuilder.InsertData(
                table: "CardioEquipment",
                columns: new[] { "EquipmentID", "MaxIncline", "MaxSpeed" },
                values: new object[,]
                {
                    { 1, 15, 20 },
                    { 3, 10, 12 }
                });

            migrationBuilder.InsertData(
                table: "Maintenance",
                columns: new[] { "MaintenanceID", "Description", "EquipmentID", "MaintenanceDate" },
                values: new object[,]
                {
                    { 1, "Regular maintenance", 1, new DateTime(2023, 5, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Weight calibration", 2, new DateTime(2023, 6, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Belt replacement", 1, new DateTime(2023, 11, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Resistance check", 3, new DateTime(2023, 9, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Frame inspection", 4, new DateTime(2024, 2, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "ProgressCard",
                columns: new[] { "ProgressCardID", "BodyFatPercentage", "Height", "MeasurementDate", "MemberID", "MuscleMass", "Weight" },
                values: new object[,]
                {
                    { 1, 15.0, 175.0, new DateTime(2024, 6, 25, 23, 45, 31, 30, DateTimeKind.Local).AddTicks(134), 1, 30.0, 70.0 },
                    { 2, 20.0, 165.0, new DateTime(2024, 6, 25, 23, 45, 31, 30, DateTimeKind.Local).AddTicks(182), 2, 25.0, 65.0 },
                    { 3, 18.0, 180.0, new DateTime(2024, 6, 25, 23, 45, 31, 30, DateTimeKind.Local).AddTicks(185), 3, 32.0, 80.0 },
                    { 4, 22.0, 160.0, new DateTime(2024, 6, 25, 23, 45, 31, 30, DateTimeKind.Local).AddTicks(188), 4, 20.0, 55.0 },
                    { 5, 17.0, 170.0, new DateTime(2024, 6, 25, 23, 45, 31, 30, DateTimeKind.Local).AddTicks(190), 5, 28.0, 75.0 }
                });

            migrationBuilder.InsertData(
                table: "Session",
                columns: new[] { "SessionID", "AvailableSlots", "EndTime", "MaxParticipants", "Name", "SectionID", "StartTime", "TermsAndConditionsID" },
                values: new object[,]
                {
                    { 1, 20, new DateTime(2024, 5, 26, 11, 0, 0, 0, DateTimeKind.Unspecified), 20, "Morning Cardio", 1, new DateTime(2024, 5, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, 15, new DateTime(2024, 5, 27, 16, 0, 0, 0, DateTimeKind.Unspecified), 15, "Evening Strength", 2, new DateTime(2024, 5, 27, 15, 0, 0, 0, DateTimeKind.Unspecified), 2 },
                    { 3, 15, new DateTime(2024, 5, 27, 16, 0, 0, 0, DateTimeKind.Unspecified), 15, "Yoga", 3, new DateTime(2024, 5, 27, 15, 0, 0, 0, DateTimeKind.Unspecified), 3 },
                    { 4, 10, new DateTime(2024, 5, 28, 11, 0, 0, 0, DateTimeKind.Unspecified), 10, "Pilates", 1, new DateTime(2024, 5, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 4 },
                    { 5, 20, new DateTime(2024, 5, 29, 18, 0, 0, 0, DateTimeKind.Unspecified), 20, "HIIT", 2, new DateTime(2024, 5, 29, 17, 0, 0, 0, DateTimeKind.Unspecified), 5 }
                });

            migrationBuilder.InsertData(
                table: "StrengthEquipment",
                columns: new[] { "EquipmentID", "MaxWeight", "MuscleGroup" },
                values: new object[,]
                {
                    { 2, 300, "Chest" },
                    { 4, 500, "Legs" }
                });

            migrationBuilder.InsertData(
                table: "CoachAssignment",
                columns: new[] { "CoachAssignmentID", "CoachID", "Compensation", "IsLeadCoach", "SessionID" },
                values: new object[,]
                {
                    { 1, 1, 100m, true, 1 },
                    { 2, 2, 120m, false, 2 },
                    { 3, 3, 150m, true, 3 },
                    { 4, 1, 100m, true, 4 },
                    { 5, 2, 120m, false, 5 }
                });

            migrationBuilder.InsertData(
                table: "Feedback",
                columns: new[] { "FeedbackId", "Comment", "Rating", "SessionId" },
                values: new object[,]
                {
                    { 1, "Great workout! Very well conducted.", 5, 1 },
                    { 2, "Very enjoyable yoga class. Highly recommend!", 4, 3 },
                    { 3, "Intense session, I got a good workout.", 4, 2 },
                    { 4, "Pilates really helps me stay in shape.", 5, 4 },
                    { 5, "Best HIIT class I've ever attended!", 5, 5 }
                });

            migrationBuilder.InsertData(
                table: "SessionEquipment",
                columns: new[] { "SessionEquipmentID", "EquipmentID", "Quantity", "SessionID" },
                values: new object[,]
                {
                    { 1, 1, 5, 1 },
                    { 2, 2, 10, 2 },
                    { 3, 3, 15, 3 },
                    { 4, 4, 10, 4 },
                    { 5, 4, 15, 5 }
                });

            migrationBuilder.InsertData(
                table: "SessionReservation",
                columns: new[] { "SessionReservationID", "MemberID", "ReservationDate", "SessionID", "Status" },
                values: new object[,]
                {
                    { 1, 3, new DateTime(2024, 5, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 1, 1 },
                    { 2, 2, new DateTime(2024, 5, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 3, 1 },
                    { 3, 1, new DateTime(2024, 5, 26, 10, 0, 0, 0, DateTimeKind.Unspecified), 2, 1 },
                    { 4, 4, new DateTime(2024, 5, 28, 10, 0, 0, 0, DateTimeKind.Unspecified), 4, 1 },
                    { 5, 5, new DateTime(2024, 5, 29, 17, 0, 0, 0, DateTimeKind.Unspecified), 5, 1 }
                });

            migrationBuilder.InsertData(
                table: "TermsAndConditions",
                columns: new[] { "TermsAndConditionsID", "Content", "SessionID" },
                values: new object[,]
                {
                    { 1, "Standard terms and conditions for Morning Cardio. Participants must adhere to all club rules and regulations.", 1 },
                    { 2, "Standard terms and conditions for Evening Strength. Participants must adhere to all club rules and regulations.", 2 },
                    { 3, "Standard terms and conditions for Yoga. Participants must adhere to all club rules and regulations.", 3 },
                    { 4, "Standard terms and conditions for Pilates. Participants must adhere to all club rules and regulations.", 4 },
                    { 5, "Standard terms and conditions for HIIT. Participants must adhere to all club rules and regulations.", 5 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachAssignment_CoachID",
                table: "CoachAssignment",
                column: "CoachID");

            migrationBuilder.CreateIndex(
                name: "IX_CoachAssignment_SessionID",
                table: "CoachAssignment",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_Feedback_SessionId",
                table: "Feedback",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_Maintenance_EquipmentID",
                table: "Maintenance",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_ProgressCard_MemberID",
                table: "ProgressCard",
                column: "MemberID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Session_SectionID",
                table: "Session",
                column: "SectionID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEquipment_EquipmentID",
                table: "SessionEquipment",
                column: "EquipmentID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionEquipment_SessionID",
                table: "SessionEquipment",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionReservation_MemberID",
                table: "SessionReservation",
                column: "MemberID");

            migrationBuilder.CreateIndex(
                name: "IX_SessionReservation_SessionID",
                table: "SessionReservation",
                column: "SessionID");

            migrationBuilder.CreateIndex(
                name: "IX_TermsAndConditions_SessionID",
                table: "TermsAndConditions",
                column: "SessionID",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CardioEquipment");

            migrationBuilder.DropTable(
                name: "CoachAssignment");

            migrationBuilder.DropTable(
                name: "Feedback");

            migrationBuilder.DropTable(
                name: "Maintenance");

            migrationBuilder.DropTable(
                name: "ProgressCard");

            migrationBuilder.DropTable(
                name: "SessionEquipment");

            migrationBuilder.DropTable(
                name: "SessionReservation");

            migrationBuilder.DropTable(
                name: "StrengthEquipment");

            migrationBuilder.DropTable(
                name: "TermsAndConditions");

            migrationBuilder.DropTable(
                name: "Coach");

            migrationBuilder.DropTable(
                name: "Member");

            migrationBuilder.DropTable(
                name: "Equipment");

            migrationBuilder.DropTable(
                name: "Session");

            migrationBuilder.DropTable(
                name: "Section");
        }
    }
}
