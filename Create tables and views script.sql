USE [DVLD]
GO
/****** Object:  UserDefinedFunction [dbo].[GetAge]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Function [dbo].[GetAge](@BirthDate Date)
returns int
AS
BEGIN
	Declare @Age INT =DateDiff(Year , @BirthDate, DATE_BUCKET(Year,1,Cast(GetDate() AS Date) , @BirthDate));
	Return @Age;
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetApplicationStatus]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[GetApplicationStatus](@ApplicationStatus INT)
returns varchar(10)
AS
BEGIN
	DECLARE @Status varchar(10);
	set @Status = 
	case
		when @ApplicationStatus = 1 then 'New'
		when @ApplicationStatus = 3 then 'Completed'
		when @ApplicationStatus = 2 then 'Canceled'
	END
	return @Status;
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetDriverName]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[GetDriverName] (@DriverID INT)
Returns nvarchar(100)
As
BEGIN
	DECLARE @DriverName nvarchar(100);

	Select @DriverName = dbo.GetFullName(FirstName , SecondName , ThirdName , LastName)
	From People
	inner join Drivers
	on People.PersonID = Drivers.PersonID
	Where DriverID = @DriverID

	return @DriverName;
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetFullName]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[GetFullName](@FirstName varchar(20) , @SecondName varchar(20) null , @ThirdName varchar(20) null , @LastName varchar(20))
returns varchar(100)
AS
BEGIN
	DECLARE @FullName varchar(100);
	SET @FullName = @FirstName;
	if(@SecondName is not null)
		SET @FullName = @FullName + ' ' + @SecondName;
	if(@ThirdName is not null)
		SET @FullName = @FullName + ' ' + @ThirdName;
	SET @FullName = @FullName + ' ' + @LastName;

	return @FullName;
END
GO
/****** Object:  UserDefinedFunction [dbo].[GetNumberOfPassedTest]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Function [dbo].[GetNumberOfPassedTest](@LocalDrivingLicenseApplicationID INT)
returns int
AS
BEGIN
DECLARE @NumberOfPassedTests INT;
	Select @NumberOfPassedTests =  Count(Tests.TestResult) from Tests inner join TestAppointments 
    on TestAppointments.TestAppointmentID = Tests.TestAppointmentID 
    Where Tests.TestResult = 1 
    and TestAppointments.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
	return @NumberOfPassedTests;
END
GO
/****** Object:  Table [dbo].[People]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[People](
	[PersonID] [int] IDENTITY(1,1) NOT NULL,
	[NationalNo] [nvarchar](20) NOT NULL,
	[FirstName] [nvarchar](20) NOT NULL,
	[SecondName] [nvarchar](20) NOT NULL,
	[ThirdName] [nvarchar](20) NULL,
	[LastName] [nvarchar](20) NOT NULL,
	[DateOfBirth] [datetime] NOT NULL,
	[Gendor] [tinyint] NOT NULL,
	[Address] [nvarchar](500) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NULL,
	[NationalityCountryID] [int] NOT NULL,
	[ImagePath] [nvarchar](250) NULL,
 CONSTRAINT [PK_People] PRIMARY KEY CLUSTERED 
(
	[PersonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TestAppointments]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestAppointments](
	[TestAppointmentID] [int] IDENTITY(1,1) NOT NULL,
	[TestTypeID] [int] NOT NULL,
	[LocalDrivingLicenseApplicationID] [int] NOT NULL,
	[AppointmentDate] [smalldatetime] NOT NULL,
	[PaidFees] [smallmoney] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[IsLocked] [bit] NOT NULL,
	[RetakeTestApplicationID] [int] NULL,
 CONSTRAINT [PK_TestAppointments] PRIMARY KEY CLUSTERED 
(
	[TestAppointmentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Tests]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Tests](
	[TestID] [int] IDENTITY(1,1) NOT NULL,
	[TestAppointmentID] [int] NOT NULL,
	[TestResult] [bit] NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[CreatedByUserID] [int] NOT NULL,
 CONSTRAINT [PK_Tests] PRIMARY KEY CLUSTERED 
(
	[TestID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Applications]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Applications](
	[ApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicantPersonID] [int] NOT NULL,
	[ApplicationDate] [datetime] NOT NULL,
	[ApplicationTypeID] [int] NOT NULL,
	[ApplicationStatus] [tinyint] NOT NULL,
	[LastStatusDate] [datetime] NOT NULL,
	[PaidFees] [smallmoney] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
 CONSTRAINT [PK_Applications] PRIMARY KEY CLUSTERED 
(
	[ApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LicenseClasses]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LicenseClasses](
	[LicenseClassID] [int] IDENTITY(1,1) NOT NULL,
	[ClassName] [nvarchar](50) NOT NULL,
	[ClassDescription] [nvarchar](500) NOT NULL,
	[MinimumAllowedAge] [tinyint] NOT NULL,
	[DefaultValidityLength] [tinyint] NOT NULL,
	[ClassFees] [smallmoney] NOT NULL,
 CONSTRAINT [PK_LicenseClasses] PRIMARY KEY CLUSTERED 
(
	[LicenseClassID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[LocalDrivingLicenseApplications]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[LocalDrivingLicenseApplications](
	[LocalDrivingLicenseApplicationID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[LicenseClassID] [int] NOT NULL,
 CONSTRAINT [PK_DrivingLicsenseApplications] PRIMARY KEY CLUSTERED 
(
	[LocalDrivingLicenseApplicationID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[LocalDrivingLicenseApplications_View]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[LocalDrivingLicenseApplications_View]
AS
SELECT        dbo.LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID, dbo.LicenseClasses.ClassName, dbo.People.NationalNo, dbo.People.FirstName + ' ' + dbo.People.SecondName + ' ' + ISNULL(dbo.People.ThirdName, '') 
                         + ' ' + dbo.People.LastName AS FullName, dbo.Applications.ApplicationDate,
                             (SELECT        COUNT(dbo.TestAppointments.TestTypeID) AS PassedTestCount
                               FROM            dbo.Tests INNER JOIN
                                                         dbo.TestAppointments ON dbo.Tests.TestAppointmentID = dbo.TestAppointments.TestAppointmentID
                               WHERE        (dbo.TestAppointments.LocalDrivingLicenseApplicationID = dbo.LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID) AND (dbo.Tests.TestResult = 1)) AS PassedTestCount, 
                         CASE WHEN Applications.ApplicationStatus = 1 THEN 'New' WHEN Applications.ApplicationStatus = 2 THEN 'Cancelled' WHEN Applications.ApplicationStatus = 3 THEN 'Completed' END AS Status
FROM            dbo.LocalDrivingLicenseApplications INNER JOIN
                         dbo.Applications ON dbo.LocalDrivingLicenseApplications.ApplicationID = dbo.Applications.ApplicationID INNER JOIN
                         dbo.LicenseClasses ON dbo.LocalDrivingLicenseApplications.LicenseClassID = dbo.LicenseClasses.LicenseClassID INNER JOIN
                         dbo.People ON dbo.Applications.ApplicantPersonID = dbo.People.PersonID
GO
/****** Object:  Table [dbo].[TestTypes]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TestTypes](
	[TestTypeID] [int] IDENTITY(1,1) NOT NULL,
	[TestTypeTitle] [nvarchar](100) NOT NULL,
	[TestTypeDescription] [nvarchar](500) NOT NULL,
	[TestTypeFees] [smallmoney] NOT NULL,
 CONSTRAINT [PK_TestTypes] PRIMARY KEY CLUSTERED 
(
	[TestTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[TestAppointments_View]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[TestAppointments_View]
AS
SELECT        dbo.TestAppointments.TestAppointmentID, dbo.TestAppointments.LocalDrivingLicenseApplicationID, dbo.TestTypes.TestTypeTitle, dbo.LicenseClasses.ClassName, dbo.TestAppointments.AppointmentDate, 
                         dbo.TestAppointments.PaidFees, dbo.People.FirstName + ' ' + dbo.People.SecondName + ' ' + ISNULL(dbo.People.ThirdName, '') + ' ' + dbo.People.LastName AS FullName, dbo.TestAppointments.IsLocked
FROM            dbo.TestAppointments INNER JOIN
                         dbo.TestTypes ON dbo.TestAppointments.TestTypeID = dbo.TestTypes.TestTypeID INNER JOIN
                         dbo.LocalDrivingLicenseApplications ON dbo.TestAppointments.LocalDrivingLicenseApplicationID = dbo.LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID INNER JOIN
                         dbo.Applications ON dbo.LocalDrivingLicenseApplications.ApplicationID = dbo.Applications.ApplicationID INNER JOIN
                         dbo.People ON dbo.Applications.ApplicantPersonID = dbo.People.PersonID INNER JOIN
                         dbo.LicenseClasses ON dbo.LocalDrivingLicenseApplications.LicenseClassID = dbo.LicenseClasses.LicenseClassID
GO
/****** Object:  Table [dbo].[Licenses]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Licenses](
	[LicenseID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[DriverID] [int] NOT NULL,
	[LicenseClass] [int] NOT NULL,
	[IssueDate] [datetime] NOT NULL,
	[ExpirationDate] [datetime] NOT NULL,
	[Notes] [nvarchar](500) NULL,
	[PaidFees] [smallmoney] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[IssueReason] [tinyint] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
 CONSTRAINT [PK_Licenses] PRIMARY KEY CLUSTERED 
(
	[LicenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Drivers]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Drivers](
	[DriverID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[CreatedDate] [smalldatetime] NOT NULL,
 CONSTRAINT [PK_Drivers_1] PRIMARY KEY CLUSTERED 
(
	[DriverID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[Drivers_View]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[Drivers_View]
AS
SELECT        dbo.Drivers.DriverID, dbo.Drivers.PersonID, dbo.People.NationalNo, dbo.People.FirstName + ' ' + dbo.People.SecondName + ' ' + ISNULL(dbo.People.ThirdName, '') + ' ' + dbo.People.LastName AS FullName, 
                         dbo.Drivers.CreatedDate,
                             (SELECT        COUNT(LicenseID) AS NumberOfActiveLicenses
                               FROM            dbo.Licenses
                               WHERE        (IsActive = 1) AND (DriverID = dbo.Drivers.DriverID)) AS NumberOfActiveLicenses
FROM            dbo.Drivers INNER JOIN
                         dbo.People ON dbo.Drivers.PersonID = dbo.People.PersonID
GO
/****** Object:  Table [dbo].[ApplicationTypes]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ApplicationTypes](
	[ApplicationTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationTypeTitle] [nvarchar](150) NOT NULL,
	[ApplicationFees] [smallmoney] NOT NULL,
 CONSTRAINT [PK_ApplicationTypes] PRIMARY KEY CLUSTERED 
(
	[ApplicationTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Countries]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Countries](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK__Countrie__10D160BFDBD6933F] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[DetainedLicenses]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[DetainedLicenses](
	[DetainID] [int] IDENTITY(1,1) NOT NULL,
	[LicenseID] [int] NOT NULL,
	[DetainDate] [smalldatetime] NOT NULL,
	[FineFees] [smallmoney] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
	[IsReleased] [bit] NOT NULL,
	[ReleaseDate] [smalldatetime] NULL,
	[ReleasedByUserID] [int] NULL,
	[ReleaseApplicationID] [int] NULL,
 CONSTRAINT [PK_DetainedLicenses] PRIMARY KEY CLUSTERED 
(
	[DetainID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[InternationalLicenses]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[InternationalLicenses](
	[InternationalLicenseID] [int] IDENTITY(1,1) NOT NULL,
	[ApplicationID] [int] NOT NULL,
	[DriverID] [int] NOT NULL,
	[IssuedUsingLocalLicenseID] [int] NOT NULL,
	[IssueDate] [smalldatetime] NOT NULL,
	[ExpirationDate] [smalldatetime] NOT NULL,
	[IsActive] [bit] NOT NULL,
	[CreatedByUserID] [int] NOT NULL,
 CONSTRAINT [PK_InternationalLicenses] PRIMARY KEY CLUSTERED 
(
	[InternationalLicenseID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PasswordLogging]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PasswordLogging](
	[LogID] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [int] NULL,
	[OldPassword] [char](64) NULL,
	[NewPassword] [char](64) NULL,
	[ModifacationDate] [datetime2](7) NULL,
PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[PersonID] [int] NOT NULL,
	[UserName] [nvarchar](20) NOT NULL,
	[Password] [varchar](64) NOT NULL,
	[IsActive] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Applications] ADD  CONSTRAINT [DF_Applications_ApplicationStatus]  DEFAULT ((1)) FOR [ApplicationStatus]
GO
ALTER TABLE [dbo].[ApplicationTypes] ADD  CONSTRAINT [DF_ApplicationTypes_Fees]  DEFAULT ((0)) FOR [ApplicationFees]
GO
ALTER TABLE [dbo].[DetainedLicenses] ADD  CONSTRAINT [DF_DetainedLicenses_IsReleased]  DEFAULT ((0)) FOR [IsReleased]
GO
ALTER TABLE [dbo].[LicenseClasses] ADD  CONSTRAINT [DF_LicenseClasses_Age]  DEFAULT ((18)) FOR [MinimumAllowedAge]
GO
ALTER TABLE [dbo].[LicenseClasses] ADD  CONSTRAINT [DF_LicenseClasses_DefaultPeriodLength]  DEFAULT ((1)) FOR [DefaultValidityLength]
GO
ALTER TABLE [dbo].[LicenseClasses] ADD  CONSTRAINT [DF_LicenseClasses_ClassFees]  DEFAULT ((0)) FOR [ClassFees]
GO
ALTER TABLE [dbo].[Licenses] ADD  CONSTRAINT [DF_Licenses_IsActive]  DEFAULT ((1)) FOR [IsActive]
GO
ALTER TABLE [dbo].[Licenses] ADD  CONSTRAINT [DF_Licenses_IssueReason]  DEFAULT ((1)) FOR [IssueReason]
GO
ALTER TABLE [dbo].[PasswordLogging] ADD  CONSTRAINT [DF_PasswordLogging_ModifacationDate]  DEFAULT (getdate()) FOR [ModifacationDate]
GO
ALTER TABLE [dbo].[People] ADD  CONSTRAINT [DF_People_Gendor]  DEFAULT ((0)) FOR [Gendor]
GO
ALTER TABLE [dbo].[TestAppointments] ADD  CONSTRAINT [DF_TestAppointments_AppointmentLocked]  DEFAULT ((0)) FOR [IsLocked]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_ApplicationTypes] FOREIGN KEY([ApplicationTypeID])
REFERENCES [dbo].[ApplicationTypes] ([ApplicationTypeID])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_ApplicationTypes]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_People] FOREIGN KEY([ApplicantPersonID])
REFERENCES [dbo].[People] ([PersonID])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_People]
GO
ALTER TABLE [dbo].[Applications]  WITH CHECK ADD  CONSTRAINT [FK_Applications_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Applications] CHECK CONSTRAINT [FK_Applications_Users]
GO
ALTER TABLE [dbo].[DetainedLicenses]  WITH CHECK ADD  CONSTRAINT [FK_DetainedLicenses_Applications] FOREIGN KEY([ReleaseApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[DetainedLicenses] CHECK CONSTRAINT [FK_DetainedLicenses_Applications]
GO
ALTER TABLE [dbo].[DetainedLicenses]  WITH CHECK ADD  CONSTRAINT [FK_DetainedLicenses_Licenses] FOREIGN KEY([LicenseID])
REFERENCES [dbo].[Licenses] ([LicenseID])
GO
ALTER TABLE [dbo].[DetainedLicenses] CHECK CONSTRAINT [FK_DetainedLicenses_Licenses]
GO
ALTER TABLE [dbo].[DetainedLicenses]  WITH CHECK ADD  CONSTRAINT [FK_DetainedLicenses_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[DetainedLicenses] CHECK CONSTRAINT [FK_DetainedLicenses_Users]
GO
ALTER TABLE [dbo].[DetainedLicenses]  WITH CHECK ADD  CONSTRAINT [FK_DetainedLicenses_Users1] FOREIGN KEY([ReleasedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[DetainedLicenses] CHECK CONSTRAINT [FK_DetainedLicenses_Users1]
GO
ALTER TABLE [dbo].[Drivers]  WITH CHECK ADD  CONSTRAINT [FK_Drivers_People] FOREIGN KEY([PersonID])
REFERENCES [dbo].[People] ([PersonID])
GO
ALTER TABLE [dbo].[Drivers] CHECK CONSTRAINT [FK_Drivers_People]
GO
ALTER TABLE [dbo].[Drivers]  WITH CHECK ADD  CONSTRAINT [FK_Drivers_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Drivers] CHECK CONSTRAINT [FK_Drivers_Users]
GO
ALTER TABLE [dbo].[InternationalLicenses]  WITH CHECK ADD  CONSTRAINT [FK_InternationalLicenses_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[InternationalLicenses] CHECK CONSTRAINT [FK_InternationalLicenses_Applications]
GO
ALTER TABLE [dbo].[InternationalLicenses]  WITH CHECK ADD  CONSTRAINT [FK_InternationalLicenses_Drivers] FOREIGN KEY([DriverID])
REFERENCES [dbo].[Drivers] ([DriverID])
GO
ALTER TABLE [dbo].[InternationalLicenses] CHECK CONSTRAINT [FK_InternationalLicenses_Drivers]
GO
ALTER TABLE [dbo].[InternationalLicenses]  WITH CHECK ADD  CONSTRAINT [FK_InternationalLicenses_Licenses] FOREIGN KEY([IssuedUsingLocalLicenseID])
REFERENCES [dbo].[Licenses] ([LicenseID])
GO
ALTER TABLE [dbo].[InternationalLicenses] CHECK CONSTRAINT [FK_InternationalLicenses_Licenses]
GO
ALTER TABLE [dbo].[InternationalLicenses]  WITH CHECK ADD  CONSTRAINT [FK_InternationalLicenses_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[InternationalLicenses] CHECK CONSTRAINT [FK_InternationalLicenses_Users]
GO
ALTER TABLE [dbo].[Licenses]  WITH CHECK ADD  CONSTRAINT [FK_Licenses_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[Licenses] CHECK CONSTRAINT [FK_Licenses_Applications]
GO
ALTER TABLE [dbo].[Licenses]  WITH CHECK ADD  CONSTRAINT [FK_Licenses_Drivers] FOREIGN KEY([DriverID])
REFERENCES [dbo].[Drivers] ([DriverID])
GO
ALTER TABLE [dbo].[Licenses] CHECK CONSTRAINT [FK_Licenses_Drivers]
GO
ALTER TABLE [dbo].[Licenses]  WITH CHECK ADD  CONSTRAINT [FK_Licenses_LicenseClasses] FOREIGN KEY([LicenseClass])
REFERENCES [dbo].[LicenseClasses] ([LicenseClassID])
GO
ALTER TABLE [dbo].[Licenses] CHECK CONSTRAINT [FK_Licenses_LicenseClasses]
GO
ALTER TABLE [dbo].[Licenses]  WITH CHECK ADD  CONSTRAINT [FK_Licenses_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Licenses] CHECK CONSTRAINT [FK_Licenses_Users]
GO
ALTER TABLE [dbo].[LocalDrivingLicenseApplications]  WITH CHECK ADD  CONSTRAINT [FK_DrivingLicsenseApplications_Applications] FOREIGN KEY([ApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[LocalDrivingLicenseApplications] CHECK CONSTRAINT [FK_DrivingLicsenseApplications_Applications]
GO
ALTER TABLE [dbo].[LocalDrivingLicenseApplications]  WITH CHECK ADD  CONSTRAINT [FK_DrivingLicsenseApplications_LicenseClasses] FOREIGN KEY([LicenseClassID])
REFERENCES [dbo].[LicenseClasses] ([LicenseClassID])
GO
ALTER TABLE [dbo].[LocalDrivingLicenseApplications] CHECK CONSTRAINT [FK_DrivingLicsenseApplications_LicenseClasses]
GO
ALTER TABLE [dbo].[PasswordLogging]  WITH CHECK ADD FOREIGN KEY([UserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[People]  WITH CHECK ADD  CONSTRAINT [FK_People_Countries1] FOREIGN KEY([NationalityCountryID])
REFERENCES [dbo].[Countries] ([CountryID])
GO
ALTER TABLE [dbo].[People] CHECK CONSTRAINT [FK_People_Countries1]
GO
ALTER TABLE [dbo].[TestAppointments]  WITH CHECK ADD  CONSTRAINT [FK_TestAppointments_LocalDrivingLicenseApplications] FOREIGN KEY([LocalDrivingLicenseApplicationID])
REFERENCES [dbo].[LocalDrivingLicenseApplications] ([LocalDrivingLicenseApplicationID])
GO
ALTER TABLE [dbo].[TestAppointments] CHECK CONSTRAINT [FK_TestAppointments_LocalDrivingLicenseApplications]
GO
ALTER TABLE [dbo].[TestAppointments]  WITH CHECK ADD  CONSTRAINT [FK_TestAppointments_RetakeTestApplication] FOREIGN KEY([RetakeTestApplicationID])
REFERENCES [dbo].[Applications] ([ApplicationID])
GO
ALTER TABLE [dbo].[TestAppointments] CHECK CONSTRAINT [FK_TestAppointments_RetakeTestApplication]
GO
ALTER TABLE [dbo].[TestAppointments]  WITH CHECK ADD  CONSTRAINT [FK_TestAppointments_TestTypes] FOREIGN KEY([TestTypeID])
REFERENCES [dbo].[TestTypes] ([TestTypeID])
GO
ALTER TABLE [dbo].[TestAppointments] CHECK CONSTRAINT [FK_TestAppointments_TestTypes]
GO
ALTER TABLE [dbo].[TestAppointments]  WITH CHECK ADD  CONSTRAINT [FK_TestAppointments_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[TestAppointments] CHECK CONSTRAINT [FK_TestAppointments_Users]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_TestAppointments] FOREIGN KEY([TestAppointmentID])
REFERENCES [dbo].[TestAppointments] ([TestAppointmentID])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_TestAppointments]
GO
ALTER TABLE [dbo].[Tests]  WITH CHECK ADD  CONSTRAINT [FK_Tests_Users] FOREIGN KEY([CreatedByUserID])
REFERENCES [dbo].[Users] ([UserID])
GO
ALTER TABLE [dbo].[Tests] CHECK CONSTRAINT [FK_Tests_Users]
GO
ALTER TABLE [dbo].[Users]  WITH CHECK ADD  CONSTRAINT [FK_Users_People] FOREIGN KEY([PersonID])
REFERENCES [dbo].[People] ([PersonID])
GO
ALTER TABLE [dbo].[Users] CHECK CONSTRAINT [FK_Users_People]
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewApplication]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewApplication] (@ApplicantPersonID int,@ApplicationDate datetime,@ApplicationTypeID int,@ApplicationStatus tinyint,@LastStatusDate datetime,@PaidFees smallmoney,@CreatedByUserID int)

As
BEGIN
	Insert Into [Applications](ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
	Values (@ApplicantPersonID,@ApplicationDate,@ApplicationTypeID,@ApplicationStatus,@LastStatusDate,@PaidFees,@CreatedByUserID);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewApplicationType]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewApplicationType] (@ApplicationTypeTitle nvarchar(max),@ApplicationFees smallmoney)

As
BEGIN
	Insert Into [ApplicationTypes](ApplicationTypeTitle,ApplicationFees)
	Values (@ApplicationTypeTitle,@ApplicationFees);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewDetainLicense]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewDetainLicense] (@LicenseID int,@DetainDate smalldatetime,@FineFees smallmoney,@CreatedByUserID int,@IsReleased bit,@ReleaseDate smalldatetime,@ReleasedByUserID int,@ReleaseApplicationID int)

As
BEGIN
	Insert Into [DetainedLicenses](LicenseID,DetainDate,FineFees,CreatedByUserID,IsReleased,ReleaseDate,ReleasedByUserID,ReleaseApplicationID)
	Values (@LicenseID,@DetainDate,@FineFees,@CreatedByUserID,@IsReleased,@ReleaseDate,@ReleasedByUserID,@ReleaseApplicationID);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewDriver]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewDriver] (@PersonID int,@CreatedByUserID int,@CreatedDate smalldatetime)

As
BEGIN
	Insert Into [Drivers](PersonID,CreatedByUserID,CreatedDate)
	Values (@PersonID,@CreatedByUserID,@CreatedDate);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewLicense]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewLicense] (@ApplicationID int,@DriverID int,@LicenseClass int,@IssueDate datetime,@ExpirationDate datetime,@Notes nvarchar,@PaidFees smallmoney,@IsActive bit,@IssueReason tinyint,@CreatedByUserID int)

As
BEGIN
	Insert Into Licenses(ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,IssueReason,CreatedByUserID)
	Values (@ApplicationID,@DriverID,@LicenseClass,@IssueDate,@ExpirationDate,@Notes,@PaidFees,@IsActive,@IssueReason,@CreatedByUserID);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNewPerson]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[sp_AddNewPerson]
@NationalNo nvarchar(20) ,
@FirstName  nvarchar(20),
@SecondName nvarchar(20), 
@ThirdName nvarchar(20) null,
@LastName nvarchar(20),
@DateOfBirth datetime,
@Gendor tinyint,
@Address nvarchar(500),
@Phone nvarchar(20),
@Email nvarchar(50) null,
@NationalityCountryID int,
@ImagePath nvarchar(250) null,
@PersonID INT null Output

as 
BEGIN
	INSERT INTO [dbo].[People]
	           ([NationalNo]
	           ,[FirstName]
	           ,[SecondName]
	           ,[ThirdName]
	           ,[LastName]
	           ,[DateOfBirth]
	           ,[Gendor]
	           ,[Address]
	           ,[Phone]
	           ,[Email]
	           ,[NationalityCountryID]
	           ,[ImagePath])
	     VALUES
	           (@NationalNo , TRIM(@FirstName),TRIM(@SecondName),TRIM(@ThirdName),TRIM(@LastName),@DateOfBirth,@Gendor,@Address,@Phone , @Email , @NationalityCountryID
			   ,@ImagePath);

	Set @PersonID =  SCOPE_IDENTITY();
END
GO
/****** Object:  StoredProcedure [dbo].[SP_AddNewTestAppointment]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_AddNewTestAppointment] (@TestTypeID int,@LocalDrivingLicenseApplicationID int,@AppointmentDate smalldatetime,@PaidFees smallmoney,@CreatedByUserID int,@IsLocked bit,@RetakeTestApplicationID int)

As
BEGIN
	Insert Into [TestAppointments](TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked,RetakeTestApplicationID)
	Values (@TestTypeID,@LocalDrivingLicenseApplicationID,@AppointmentDate,@PaidFees,@CreatedByUserID,@IsLocked,@RetakeTestApplicationID);

	SELECT SCOPE_IDENTITY()
	
END
GO
/****** Object:  StoredProcedure [dbo].[sp_AddNewUser]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[sp_AddNewUser]
@PersonID INT,
@UserName nvarchar(20),
@Password nvarchar(64),
@IsActive bit , 
@UserID INT null Output

As
BEGIN
	INSERT INTO [dbo].[Users]
	           ([PersonID]
	           ,[UserName]
	           ,[Password]
	           ,[IsActive])
	     VALUES
	           (@PersonID,@UserName,@Password,@IsActive);

		SET @UserID =  SCOPE_IDENTITY();

END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteApplicationByApplicationID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteApplicationByApplicationID]
(@ApplicationID int) AS
BEGIN
	Delete From [Applications] Where ApplicationID = @ApplicationID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteApplicationTypeByApplicationTypeID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteApplicationTypeByApplicationTypeID]
(@ApplicationTypeID int) AS
BEGIN
	Delete From [ApplicationTypes] Where ApplicationTypeID = @ApplicationTypeID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteDetainLicenseByDetainID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteDetainLicenseByDetainID]
(@DetainID int) AS
BEGIN
	Delete From [DetainedLicenses] Where DetainID = @DetainID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteDriverByDriverID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteDriverByDriverID]
(@DriverID int) AS
BEGIN
	Delete From [Drivers] Where DriverID = @DriverID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteLocalDrivingLicenseApplication]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure  [dbo].[SP_DeleteLocalDrivingLicenseApplication]
@LocalDrivingLicenseApplicationID INT
AS
BEGIN
	DECLARE @ApplicationID INT;
    
    
    BEGIN Transaction;
    
    	BEGIN TRY
			select @ApplicationID = ApplicationID from LocalDrivingLicenseApplications 
			where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID; 
			if @@ROWCOUNT = 0 
			BEGIN
				Throw 50001 , 'No Application ID Found', 1;
			END

			DELETE From LocalDrivingLicenseApplications 
			      Where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID;
			DELETE From Applications WHERE ApplicationID = @ApplicationID
			COMMIT;
			return 1;
    	END TRY
    	
    	BEGIN CATCH
    		ROLLBACK;
			return 0;
    	END CATCH
END



GO
/****** Object:  StoredProcedure [dbo].[SP_DeletePerson]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeletePerson]
@PersonID INT
AS
BEGIN
	Delete From People Where PersonID = @PersonID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeletePersonByPersonID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeletePersonByPersonID]
(@PersonID int) AS
BEGIN
	Delete From [People] Where PersonID = @PersonID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUser]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteUser]
@UserID INT
AS
BEGIN
	Delete From Users Where UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_DeleteUserByUserID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_DeleteUserByUserID]
(@UserID int) AS
BEGIN
	Delete From [Users] Where UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindApplicationByApplicationID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindApplicationByApplicationID]
(@ApplicationID int) AS
BEGIN
	SELECT * FROM [Applications] Where ApplicationID = @ApplicationID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindApplicationTypeByApplicationTypeID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindApplicationTypeByApplicationTypeID]
(@ApplicationTypeID int) AS
BEGIN
	SELECT * FROM [ApplicationTypes] Where ApplicationTypeID = @ApplicationTypeID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindDetainLicenseByDetainID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindDetainLicenseByDetainID]
(@DetainID int) AS
BEGIN
	SELECT * FROM [DetainedLicenses] Where DetainID = @DetainID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindDriverByDriverID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindDriverByDriverID]
(@DriverID int) AS
BEGIN
	SELECT * FROM [Drivers] Where DriverID = @DriverID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindPersonByPersonID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindPersonByPersonID]
(@PersonID int) AS
BEGIN
	SELECT * FROM [People] Where PersonID = @PersonID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_FindUserByUserID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_FindUserByUserID]
(@UserID int) AS
BEGIN
	SELECT * FROM [Users] Where UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllPeople]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure  [dbo].[SP_GetAllPeople]
@Filter nvarchar(max),
@Value nvarchar(max)
AS
BEGIN
	Select PersonID , NationalNo , FirstName , SecondName , ThirdName ,LastName , Gendor = 
    case
    	when Gendor = 0 then 'Male'
    	else 'Female'
    end
    , DateOfBirth , CountryName as Nationality , Phone , Email
    From People
    inner join Countries
    on Countries.CountryID = People.NationalityCountryID
	WHERE @Filter LIKE '%'+@Value+'%';
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetAllUsers]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_GetAllUsers]

AS
BEGIN
	Select UserID , Users.PersonID ,
    FullName = (FirstName + ' ' + SecondName + ' ' + ThirdName + ' ' + LastName) ,
    UserName , IsActive   From Users
    inner join People 
    on Users.PersonID = People.PersonID;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_GetApplicationsForPage]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GetApplicationsForPage]
@ApplicationsForPage INT , @ApplicationTypeID INT , @PageNumber INT

AS
BEGIN

Select ApplicationID From Applications
Where ApplicationTypeID = @ApplicationTypeID
ORDER BY ApplicationID
OFFSET (@PageNumber - 1) * @ApplicationsForPage ROWS
Fetch Next @ApplicationsForPage ROWS ONLY
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetLocalDrivingLicenseApplications]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_GetLocalDrivingLicenseApplications]

AS
BEGIN
	
Select [L.D.L.AppID] = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID ,[Driving Class] = ClassName ,NationalNo 
, FullName = dbo.GetFullName(FirstName , SecondName , ThirdName , LastName) , ApplicationDate ,
[Passed Tests] = dbo.GetNumberOfPassedTest( LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID),
Status = dbo.GetApplicationStatus(ApplicationStatus)
From Applications
inner join 
(
	LocalDrivingLicenseApplications inner join LicenseClasses 
	on LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
)
on Applications.ApplicationID = LocalDrivingLicenseApplications.ApplicationID
inner join People on Applications.ApplicantPersonID = People.PersonID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetNumberOfPagesForApplications]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE Procedure [dbo].[SP_GetNumberOfPagesForApplications]
@ApplicationsForPage INT , @ApplicationTypeID Float , @NumberOfPages INT OUTPUT

AS
BEGIN

Select @NumberOfPages = CEILING(Count(*) / Cast (@ApplicationsForPage AS float) ) From Applications
Where ApplicationTypeID = @ApplicationTypeID

END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPersonByID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_GetPersonByID]
@PersonID INT
AS
BEGIN
	SELECT * FROM People Where PersonID = @PersonID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetPersonByNationalNumber]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure  [dbo].[SP_GetPersonByNationalNumber]
@NationalNo nvarchar(20)
AS
BEGIN
	Select * From People Where NationalNo = @NationalNo
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserByID]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_GetUserByID]
@UserID INT
AS
BEGIN
	Select UserName , IsActive , PersonID From Users Where UserID = @UserID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_GetUserIDByUserNameAndPassword]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure  [dbo].[SP_GetUserIDByUserNameAndPassword]
@UserName nvarchar(20),
@Password varchar(64),
@UserID INT OUTPUT
AS
BEGIN
	Select @UserID=UserID From Users Where UserName = @UserName and Password = @Password;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_IsUserNameUsed]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE Procedure  [dbo].[SP_IsUserNameUsed]
@UserName nvarchar(20)
AS
BEGIN
	IF ((Select COUNT(UserName) From Users Where UserName = @UserName) > 0)
		RETURN 1;
	ELSE 
		RETURN 0;
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListApplicationTypes]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_ListApplicationTypes]
AS
BEGIN
	SELECT * From [ApplicationTypes]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListDetainedLicenses]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_ListDetainedLicenses]
AS
BEGIN
	SELECT * From [DetainedLicenses]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListDrivers]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_ListDrivers]
AS
BEGIN
	SELECT * From [Drivers]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListPeople]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_ListPeople]
AS
BEGIN
	SELECT * From [People]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_ListUsers]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure [dbo].[SP_ListUsers]
AS
BEGIN
	SELECT * From [Users]
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateApplication]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SP_UpdateApplication] (@ApplicationID int,@ApplicantPersonID int,@ApplicationDate datetime,@ApplicationTypeID int,@ApplicationStatus tinyint,@LastStatusDate datetime,@PaidFees smallmoney,@CreatedByUserID int)

AS

BEGIN 
	
UPDATE [dbo].[Applications]
   SET [ApplicantPersonID] = @ApplicantPersonID
,[ApplicationDate] = @ApplicationDate
,[ApplicationTypeID] = @ApplicationTypeID
,[ApplicationStatus] = @ApplicationStatus
,[LastStatusDate] = @LastStatusDate
,[PaidFees] = @PaidFees
,[CreatedByUserID] = @CreatedByUserID

		WHERE [ApplicationID] = @ApplicationID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateApplicationType]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SP_UpdateApplicationType] (@ApplicationTypeID int,@ApplicationTypeTitle nvarchar(max),@ApplicationFees smallmoney)

AS

BEGIN 
	
UPDATE [dbo].[ApplicationTypes]
   SET [ApplicationTypeTitle] = @ApplicationTypeTitle
,[ApplicationFees] = @ApplicationFees

		WHERE [ApplicationTypeID] = @ApplicationTypeID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateDetainLicense]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SP_UpdateDetainLicense] (@DetainID int,@LicenseID int,@DetainDate smalldatetime,@FineFees smallmoney,@CreatedByUserID int,@IsReleased bit,@ReleaseDate smalldatetime,@ReleasedByUserID int,@ReleaseApplicationID int)

AS

BEGIN 
	
UPDATE [dbo].[DetainedLicenses]
   SET [LicenseID] = @LicenseID
,[DetainDate] = @DetainDate
,[FineFees] = @FineFees
,[CreatedByUserID] = @CreatedByUserID
,[IsReleased] = @IsReleased
,[ReleaseDate] = @ReleaseDate
,[ReleasedByUserID] = @ReleasedByUserID
,[ReleaseApplicationID] = @ReleaseApplicationID

		WHERE [DetainID] = @DetainID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateDriver]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SP_UpdateDriver] (@DriverID int,@PersonID int,@CreatedByUserID int,@CreatedDate smalldatetime)

AS

BEGIN 
	
UPDATE [dbo].[Drivers]
   SET [PersonID] = @PersonID
,[CreatedByUserID] = @CreatedByUserID
,[CreatedDate] = @CreatedDate

		WHERE [DriverID] = @DriverID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateLicense]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create procedure [dbo].[SP_UpdateLicense] (@LicenseID int,@ApplicationID int,@DriverID int,@LicenseClass int,@IssueDate datetime,@ExpirationDate datetime,@Notes nvarchar,@PaidFees smallmoney,@IsActive bit,@IssueReason tinyint,@CreatedByUserID int)

AS

BEGIN 
	
UPDATE [dbo].[Licenses]
   SET [ApplicationID] = @ApplicationID
,[DriverID] = @DriverID
,[LicenseClass] = @LicenseClass
,[IssueDate] = @IssueDate
,[ExpirationDate] = @ExpirationDate
,[Notes] = @Notes
,[PaidFees] = @PaidFees
,[IsActive] = @IsActive
,[IssueReason] = @IssueReason
,[CreatedByUserID] = @CreatedByUserID

		WHERE [LicenseID] = @LicenseID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdatePerson]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
Create Procedure  [dbo].[SP_UpdatePerson]

@NationalNumber nvarchar(20),
@FirstName nvarchar(20),
@SecondName nvarchar(20),
@ThirdName nvarchar(20) null,
@LastName nvarchar(20),
@DateOfBirth datetime,
@Gendor tinyint,
@Address nvarchar(500),
@Phone nvarchar(20),
@Email nvarchar(50) null,
@NationalityCountryID INT,
@ImagePath nvarchar(250) null,
@PersonID INT
AS
BEGIN
	Update People
                            set NationalNo = @NationalNumber,
                                FirstName = @FirstName,
                                SecondName = @SecondName,
                                ThirdName = @ThirdName,
                                LastName = @LastName,
                                DateOfBirth = @DateOfBirth,
                                Gendor = @Gendor,
                                Address = @Address,
                                Phone = @Phone,
                                Email = @Email,
                                NationalityCountryID = @NationalityCountryID,
                                ImagePath = @ImagePath
                                Where PersonID = @PersonID;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateTestAppointment]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE procedure [dbo].[SP_UpdateTestAppointment] (@TestAppointmentID int,@AppointmentDate smalldatetime)

AS

BEGIN 
	
UPDATE [dbo].[TestAppointments]
   SET 
[AppointmentDate] = @AppointmentDate


		WHERE [TestAppointmentID] = @TestAppointmentID;
END 
GO
/****** Object:  StoredProcedure [dbo].[SP_UpdateUser]    Script Date: 5/30/2026 7:06:18 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create Procedure  [dbo].[SP_UpdateUser]

@UserName nvarchar(20),
@IsActive bit,
@UserID INT
AS
BEGIN
	Update Users
    set UserName = @UserName,
	IsActive = @IsActive 
	where UserID = @UserID
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-New 2-Cancelled 3-Completed' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Applications', @level2type=N'COLUMN',@level2name=N'ApplicationStatus'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Minmum age allowed to apply for this license' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LicenseClasses', @level2type=N'COLUMN',@level2name=N'MinimumAllowedAge'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'How many years the licesnse will be valid.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'LicenseClasses', @level2type=N'COLUMN',@level2name=N'DefaultValidityLength'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'1-FirstTime, 2-Renew, 3-Replacement for Damaged, 4- Replacement for Lost.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Licenses', @level2type=N'COLUMN',@level2name=N'IssueReason'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 Male , 1 Femail' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'People', @level2type=N'COLUMN',@level2name=N'Gendor'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0 - Fail 1-Pass' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'Tests', @level2type=N'COLUMN',@level2name=N'TestResult'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "Drivers"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 215
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 253
               Bottom = 136
               Right = 454
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Drivers_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'Drivers_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "LocalDrivingLicenseApplications"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 119
               Right = 309
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Applications"
            Begin Extent = 
               Top = 6
               Left = 347
               Bottom = 136
               Right = 534
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LicenseClasses"
            Begin Extent = 
               Top = 196
               Left = 343
               Bottom = 326
               Right = 549
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 816
               Bottom = 136
               Right = 1017
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LocalDrivingLicenseApplications_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'LocalDrivingLicenseApplications_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "TestAppointments"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 136
               Right = 304
            End
            DisplayFlags = 280
            TopColumn = 3
         End
         Begin Table = "TestTypes"
            Begin Extent = 
               Top = 6
               Left = 342
               Bottom = 136
               Right = 537
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LocalDrivingLicenseApplications"
            Begin Extent = 
               Top = 6
               Left = 575
               Bottom = 119
               Right = 841
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "Applications"
            Begin Extent = 
               Top = 6
               Left = 879
               Bottom = 136
               Right = 1066
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "People"
            Begin Extent = 
               Top = 6
               Left = 1104
               Bottom = 136
               Right = 1305
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "LicenseClasses"
            Begin Extent = 
               Top = 6
               Left = 1343
               Bottom = 136
               Right = 1549
            End
            DisplayFlags = 280
            TopColumn = 0
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TestAppointments_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane2', @value=N'= 11
         Column = 1440
         Alias = 900
         Table = 1170
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TestAppointments_View'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=2 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'TestAppointments_View'
GO
