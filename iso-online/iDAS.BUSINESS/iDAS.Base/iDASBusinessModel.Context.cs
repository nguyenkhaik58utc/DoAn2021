//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace iDAS.Base
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class iDASBusinessEntities : DbContext
    {
        public iDASBusinessEntities()
        {
        }
        public virtual DbSet<AccountingDebtType> AccountingDebtTypes { get; set; }
        public virtual DbSet<AccountingPaymentPlan> AccountingPaymentPlans { get; set; }
        public virtual DbSet<AccountingPayment> AccountingPayments { get; set; }
        public virtual DbSet<AccountingReasonSpending> AccountingReasonSpendings { get; set; }
        public virtual DbSet<AccountingSpendingBillDetail> AccountingSpendingBillDetails { get; set; }
        public virtual DbSet<AccountingSpendingBill> AccountingSpendingBills { get; set; }
        public virtual DbSet<AccountingSuggestAdvance> AccountingSuggestAdvances { get; set; }
        public virtual DbSet<AttachmentFile> AttachmentFiles { get; set; }
        public virtual DbSet<AuditResult> AuditResults { get; set; }
        public virtual DbSet<Audit> Audits { get; set; }
        public virtual DbSet<BusinessAction> BusinessActions { get; set; }
        public virtual DbSet<BusinessFunction> BusinessFunctions { get; set; }
        public virtual DbSet<BusinessModule> BusinessModules { get; set; }
        public virtual DbSet<CalendarCategory> CalendarCategories { get; set; }
        public virtual DbSet<CalendarOverride> CalendarOverrides { get; set; }
        public virtual DbSet<CalendarTimeOverride> CalendarTimeOverrides { get; set; }
        public virtual DbSet<Chat> Chats { get; set; }
        public virtual DbSet<CommonCity> CommonCities { get; set; }
        public virtual DbSet<CommonCountry> CommonCountries { get; set; }
        public virtual DbSet<CustomerAssess> CustomerAssesses { get; set; }
        public virtual DbSet<CustomerAssessObject> CustomerAssessObjects { get; set; }
        public virtual DbSet<CustomerAssessResult> CustomerAssessResults { get; set; }
        public virtual DbSet<CustomerCampaignAudit> CustomerCampaignAudits { get; set; }
        public virtual DbSet<CustomerCampaignPlan> CustomerCampaignPlans { get; set; }
        public virtual DbSet<CustomerCampaign> CustomerCampaigns { get; set; }
        public virtual DbSet<CustomerCampaignTarget> CustomerCampaignTargets { get; set; }
        public virtual DbSet<CustomerCareObject> CustomerCareObjects { get; set; }
        public virtual DbSet<CustomerCareResult> CustomerCareResults { get; set; }
        public virtual DbSet<CustomerCare> CustomerCares { get; set; }
        public virtual DbSet<CustomerCategory> CustomerCategories { get; set; }
        public virtual DbSet<CustomerCategoryCustomer> CustomerCategoryCustomers { get; set; }
        public virtual DbSet<CustomerCategoryDepartment> CustomerCategoryDepartments { get; set; }
        public virtual DbSet<CustomerContactCalendar> CustomerContactCalendars { get; set; }
        public virtual DbSet<CustomerContactForm> CustomerContactForms { get; set; }
        public virtual DbSet<CustomerContactHistory> CustomerContactHistories { get; set; }
        public virtual DbSet<CustomerContactHistoryAttachmentFile> CustomerContactHistoryAttachmentFiles { get; set; }
        public virtual DbSet<CustomerContact> CustomerContacts { get; set; }
        public virtual DbSet<CustomerContractAttachmentFile> CustomerContractAttachmentFiles { get; set; }
        public virtual DbSet<CustomerContract> CustomerContracts { get; set; }
        public virtual DbSet<CustomerCriteriaCategory> CustomerCriteriaCategories { get; set; }
        public virtual DbSet<CustomerCriteria> CustomerCriterias { get; set; }
        public virtual DbSet<CustomerFeedback> CustomerFeedbacks { get; set; }
        public virtual DbSet<CustomerOrderAttachmentFile> CustomerOrderAttachmentFiles { get; set; }
        public virtual DbSet<CustomerOrder> CustomerOrders { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<CustomerSurveyObject> CustomerSurveyObjects { get; set; }
        public virtual DbSet<CustomerSurveyQuestion> CustomerSurveyQuestions { get; set; }
        public virtual DbSet<CustomerSurveyResult> CustomerSurveyResults { get; set; }
        public virtual DbSet<CustomerSurvey> CustomerSurveys { get; set; }
        public virtual DbSet<CustomerType> CustomerTypes { get; set; }
        public virtual DbSet<DepartmentBroadCategory> DepartmentBroadCategories { get; set; }
        public virtual DbSet<DepartmentBroad> DepartmentBroads { get; set; }
        public virtual DbSet<DepartmentLegalAttachment> DepartmentLegalAttachments { get; set; }
        public virtual DbSet<DepartmentLegalAuditResult> DepartmentLegalAuditResults { get; set; }
        public virtual DbSet<DepartmentLegal> DepartmentLegals { get; set; }
        public virtual DbSet<DepartmentPolicy> DepartmentPolicies { get; set; }
        public virtual DbSet<DepartmentPolicyAttachment> DepartmentPolicyAttachments { get; set; }
        public virtual DbSet<DepartmentRegulatory> DepartmentRegulatories { get; set; }
        public virtual DbSet<DepartmentRegulatoryAttachment> DepartmentRegulatoryAttachments { get; set; }
        public virtual DbSet<DepartmentRegulatoryAuditResult> DepartmentRegulatoryAuditResults { get; set; }
        public virtual DbSet<DepartmentRequirmentAttachment> DepartmentRequirmentAttachments { get; set; }
        public virtual DbSet<DepartmentRequirmentAuditResult> DepartmentRequirmentAuditResults { get; set; }
        public virtual DbSet<DepartmentRequirment> DepartmentRequirments { get; set; }
        public virtual DbSet<DispatchCategory> DispatchCategories { get; set; }
        public virtual DbSet<DispatchGoAttachmentFile> DispatchGoAttachmentFiles { get; set; }
        public virtual DbSet<DispatchGoCC> DispatchGoCCs { get; set; }
        public virtual DbSet<DispatchGoDepartment> DispatchGoDepartments { get; set; }
        public virtual DbSet<DispatchGoEmployee> DispatchGoEmployees { get; set; }
        public virtual DbSet<DispatchGo> DispatchGoes { get; set; }
        public virtual DbSet<DispatchGoOutSide> DispatchGoOutSides { get; set; }
        public virtual DbSet<DispatchGoTask> DispatchGoTasks { get; set; }
        public virtual DbSet<DispatchMethod> DispatchMethods { get; set; }
        public virtual DbSet<DispatchSecurity> DispatchSecurities { get; set; }
        public virtual DbSet<DispatchToAttachmentFile> DispatchToAttachmentFiles { get; set; }
        public virtual DbSet<DispatchToCC> DispatchToCCs { get; set; }
        public virtual DbSet<DispatchToDepartment> DispatchToDepartments { get; set; }
        public virtual DbSet<DispatchToEmployee> DispatchToEmployees { get; set; }
        public virtual DbSet<DispatchTo> DispatchToes { get; set; }
        public virtual DbSet<DispatchToTask> DispatchToTasks { get; set; }
        public virtual DbSet<DispatchUrgency> DispatchUrgencies { get; set; }
        public virtual DbSet<DocumentAttachmentFile> DocumentAttachmentFiles { get; set; }
        public virtual DbSet<DocumentAttachment> DocumentAttachments { get; set; }
        public virtual DbSet<DocumentCategory> DocumentCategories { get; set; }
        public virtual DbSet<DocumentDistribution> DocumentDistributions { get; set; }
        public virtual DbSet<DocumentRequirement> DocumentRequirements { get; set; }
        public virtual DbSet<Document> Documents { get; set; }
        public virtual DbSet<DocumentSecurity> DocumentSecurities { get; set; }
        public virtual DbSet<DocumentTask> DocumentTasks { get; set; }
        public virtual DbSet<EquipmentCalibration> EquipmentCalibrations { get; set; }
        public virtual DbSet<EquipmentMeasureAttachmentFile> EquipmentMeasureAttachmentFiles { get; set; }
        public virtual DbSet<EquipmentMeasureCalibrationResult> EquipmentMeasureCalibrationResults { get; set; }
        public virtual DbSet<EquipmentMeasureCalibration> EquipmentMeasureCalibrations { get; set; }
        public virtual DbSet<EquipmentMeasureCategory> EquipmentMeasureCategories { get; set; }
        public virtual DbSet<EquipmentMeasureDepartment> EquipmentMeasureDepartments { get; set; }
        public virtual DbSet<EquipmentMeasure> EquipmentMeasures { get; set; }
        public virtual DbSet<EquipmentProductionAttach> EquipmentProductionAttaches { get; set; }
        public virtual DbSet<EquipmentProductionAttachmentFile> EquipmentProductionAttachmentFiles { get; set; }
        public virtual DbSet<EquipmentProductionCategory> EquipmentProductionCategories { get; set; }
        public virtual DbSet<EquipmentProductionDepartment> EquipmentProductionDepartments { get; set; }
        public virtual DbSet<EquipmentProductionError> EquipmentProductionErrors { get; set; }
        public virtual DbSet<EquipmentProductionHistory> EquipmentProductionHistories { get; set; }
        public virtual DbSet<EquipmentProductionMaintain> EquipmentProductionMaintains { get; set; }
        public virtual DbSet<EquipmentProduction> EquipmentProductions { get; set; }
        public virtual DbSet<HumanAbsent> HumanAbsents { get; set; }
        public virtual DbSet<HumanAbsentType> HumanAbsentTypes { get; set; }
        public virtual DbSet<HumanAnswer> HumanAnswers { get; set; }
        public virtual DbSet<HumanAuditCriteriaCategory> HumanAuditCriteriaCategories { get; set; }
        public virtual DbSet<HumanAuditCriteriaPoint> HumanAuditCriteriaPoints { get; set; }
        public virtual DbSet<HumanAuditCriteria> HumanAuditCriterias { get; set; }
        public virtual DbSet<HumanAuditDepartment> HumanAuditDepartments { get; set; }
        public virtual DbSet<HumanAuditGradationCriteriaPoint> HumanAuditGradationCriteriaPoints { get; set; }
        public virtual DbSet<HumanAuditGradationCriteria> HumanAuditGradationCriterias { get; set; }
        public virtual DbSet<HumanAuditGradationRole> HumanAuditGradationRoles { get; set; }
        public virtual DbSet<HumanAuditGradation> HumanAuditGradations { get; set; }
        public virtual DbSet<HumanAuditLevel> HumanAuditLevels { get; set; }
        public virtual DbSet<HumanAuditTickResultBonusScore> HumanAuditTickResultBonusScores { get; set; }
        public virtual DbSet<HumanAuditTickResult> HumanAuditTickResults { get; set; }
        public virtual DbSet<HumanAuditTick> HumanAuditTicks { get; set; }
        public virtual DbSet<HumanCategoryQuestion> HumanCategoryQuestions { get; set; }
        public virtual DbSet<HumanDepartment> HumanDepartments { get; set; }
        public virtual DbSet<HumanEmployeeAudit> HumanEmployeeAudits { get; set; }
        public virtual DbSet<HumanEmployee> HumanEmployees { get; set; }
        public virtual DbSet<HumanEmployeeTimeKeeping> HumanEmployeeTimeKeepings { get; set; }
        public virtual DbSet<HumanOrganization> HumanOrganizations { get; set; }
        public virtual DbSet<HumanPermission> HumanPermissions { get; set; }
        public virtual DbSet<HumanPhaseAudit> HumanPhaseAudits { get; set; }
        public virtual DbSet<HumanProfileAssess> HumanProfileAssesses { get; set; }
        public virtual DbSet<HumanProfileAttachment> HumanProfileAttachments { get; set; }
        public virtual DbSet<HumanProfileCertificate> HumanProfileCertificates { get; set; }
        public virtual DbSet<HumanProfileContract> HumanProfileContracts { get; set; }
        public virtual DbSet<HumanProfileCuriculmViate> HumanProfileCuriculmViates { get; set; }
        public virtual DbSet<HumanProfileDiploma> HumanProfileDiplomas { get; set; }
        public virtual DbSet<HumanProfileDiscipline> HumanProfileDisciplines { get; set; }
        public virtual DbSet<HumanProfileInsurance> HumanProfileInsurances { get; set; }
        public virtual DbSet<HumanProfileRelationship> HumanProfileRelationships { get; set; }
        public virtual DbSet<HumanProfileReward> HumanProfileRewards { get; set; }
        public virtual DbSet<HumanProfileSalary> HumanProfileSalaries { get; set; }
        public virtual DbSet<HumanProfileTraining> HumanProfileTrainings { get; set; }
        public virtual DbSet<HumanProfileWorkExperience> HumanProfileWorkExperiences { get; set; }
        public virtual DbSet<HumanProfileWorkTrialResult> HumanProfileWorkTrialResults { get; set; }
        public virtual DbSet<HumanProfileWorkTrial> HumanProfileWorkTrials { get; set; }
        public virtual DbSet<HumanQuestion> HumanQuestions { get; set; }
        public virtual DbSet<HumanRecruitmentCriteria> HumanRecruitmentCriterias { get; set; }
        public virtual DbSet<HumanRecruitmentInterview> HumanRecruitmentInterviews { get; set; }
        public virtual DbSet<HumanRecruitmentPlanInterview> HumanRecruitmentPlanInterviews { get; set; }
        public virtual DbSet<HumanRecruitmentPlanRequirement> HumanRecruitmentPlanRequirements { get; set; }
        public virtual DbSet<HumanRecruitmentPlan> HumanRecruitmentPlans { get; set; }
        public virtual DbSet<HumanRecruitmentProfileInterview> HumanRecruitmentProfileInterviews { get; set; }
        public virtual DbSet<HumanRecruitmentProfileResult> HumanRecruitmentProfileResults { get; set; }
        public virtual DbSet<HumanRecruitmentProfile> HumanRecruitmentProfiles { get; set; }
        public virtual DbSet<HumanRecruitmentRequirement> HumanRecruitmentRequirements { get; set; }
        public virtual DbSet<HumanRecruitmentReview> HumanRecruitmentReviews { get; set; }
        public virtual DbSet<HumanRecruitmentTask> HumanRecruitmentTasks { get; set; }
        public virtual DbSet<HumanResultAnswer> HumanResultAnswers { get; set; }
        public virtual DbSet<HumanResultQuestion> HumanResultQuestions { get; set; }
        public virtual DbSet<HumanRole> HumanRoles { get; set; }
        public virtual DbSet<HumanTrainingPlanAttachment> HumanTrainingPlanAttachments { get; set; }
        public virtual DbSet<HumanTrainingPlanDetail> HumanTrainingPlanDetails { get; set; }
        public virtual DbSet<HumanTrainingPlanRequirement> HumanTrainingPlanRequirements { get; set; }
        public virtual DbSet<HumanTrainingPlan> HumanTrainingPlans { get; set; }
        public virtual DbSet<HumanTrainingPractionerRank> HumanTrainingPractionerRanks { get; set; }
        public virtual DbSet<HumanTrainingPractioner> HumanTrainingPractioners { get; set; }
        public virtual DbSet<HumanTrainingRequirementEmployee> HumanTrainingRequirementEmployees { get; set; }
        public virtual DbSet<HumanTrainingRequirement> HumanTrainingRequirements { get; set; }
        public virtual DbSet<HumanUser> HumanUsers { get; set; }
        public virtual DbSet<Notify> Notifies { get; set; }
        public virtual DbSet<ProductDevelopmentRequirementAttachmentFile> ProductDevelopmentRequirementAttachmentFiles { get; set; }
        public virtual DbSet<ProductDevelopmentRequirementPlanDocument> ProductDevelopmentRequirementPlanDocuments { get; set; }
        public virtual DbSet<ProductDevelopmentRequirementPlan> ProductDevelopmentRequirementPlans { get; set; }
        public virtual DbSet<ProductDevelopmentRequirement> ProductDevelopmentRequirements { get; set; }
        public virtual DbSet<ProductionCommand> ProductionCommands { get; set; }
        public virtual DbSet<ProductionLevel> ProductionLevels { get; set; }
        public virtual DbSet<ProductionPerformHistory> ProductionPerformHistories { get; set; }
        public virtual DbSet<ProductionPerformMateria> ProductionPerformMaterias { get; set; }
        public virtual DbSet<ProductionPerformProductError> ProductionPerformProductErrors { get; set; }
        public virtual DbSet<ProductionPerformResult> ProductionPerformResults { get; set; }
        public virtual DbSet<ProductionPerform> ProductionPerforms { get; set; }
        public virtual DbSet<ProductionPlanEquipment> ProductionPlanEquipments { get; set; }
        public virtual DbSet<ProductionPlanMaterial> ProductionPlanMaterials { get; set; }
        public virtual DbSet<ProductionPlanProductDetail> ProductionPlanProductDetails { get; set; }
        public virtual DbSet<ProductionPlanProduct> ProductionPlanProducts { get; set; }
        public virtual DbSet<ProductionPlan> ProductionPlans { get; set; }
        public virtual DbSet<ProductionRequirement> ProductionRequirements { get; set; }
        public virtual DbSet<ProductionShift> ProductionShifts { get; set; }
        public virtual DbSet<ProductionStageCriteriaCategory> ProductionStageCriteriaCategories { get; set; }
        public virtual DbSet<ProductionStageEquipment> ProductionStageEquipments { get; set; }
        public virtual DbSet<ProductionStageMaterial> ProductionStageMaterials { get; set; }
        public virtual DbSet<ProductionStageProduct> ProductionStageProducts { get; set; }
        public virtual DbSet<ProductionStage> ProductionStages { get; set; }
        public virtual DbSet<ProfileAttachmentFile> ProfileAttachmentFiles { get; set; }
        public virtual DbSet<ProfileBorrowCategory> ProfileBorrowCategories { get; set; }
        public virtual DbSet<ProfileBorrow> ProfileBorrows { get; set; }
        public virtual DbSet<ProfileCancelEmployee> ProfileCancelEmployees { get; set; }
        public virtual DbSet<ProfileCancelMethod> ProfileCancelMethods { get; set; }
        public virtual DbSet<ProfileCancelProfile> ProfileCancelProfiles { get; set; }
        public virtual DbSet<ProfileCancel> ProfileCancels { get; set; }
        public virtual DbSet<ProfileCategory> ProfileCategories { get; set; }
        public virtual DbSet<Profile> Profiles { get; set; }
        public virtual DbSet<ProfileSecurity> ProfileSecurities { get; set; }
        public virtual DbSet<QualityAuditPlan> QualityAuditPlans { get; set; }
        public virtual DbSet<QualityAuditProgramDepartment> QualityAuditProgramDepartments { get; set; }
        public virtual DbSet<QualityAuditProgramEmployee> QualityAuditProgramEmployees { get; set; }
        public virtual DbSet<QualityAuditProgramISO> QualityAuditProgramISOes { get; set; }
        public virtual DbSet<QualityAuditProgram> QualityAuditPrograms { get; set; }
        public virtual DbSet<QualityAuditRecordedVote> QualityAuditRecordedVotes { get; set; }
        public virtual DbSet<QualityAuditResult> QualityAuditResults { get; set; }
        public virtual DbSet<QualityCAPARequirement> QualityCAPARequirements { get; set; }
        public virtual DbSet<QualityCAPA> QualityCAPAs { get; set; }
        public virtual DbSet<QualityCAPATask> QualityCAPATasks { get; set; }
        public virtual DbSet<QualityCriteriaCategory> QualityCriteriaCategories { get; set; }
        public virtual DbSet<QualityCriteria> QualityCriterias { get; set; }
        public virtual DbSet<QualityMeetingAttachmentFile> QualityMeetingAttachmentFiles { get; set; }
        public virtual DbSet<QualityMeetingObject> QualityMeetingObjects { get; set; }
        public virtual DbSet<QualityMeetingProblem> QualityMeetingProblems { get; set; }
        public virtual DbSet<QualityMeetingResultAttachmentFile> QualityMeetingResultAttachmentFiles { get; set; }
        public virtual DbSet<QualityMeetingResult> QualityMeetingResults { get; set; }
        public virtual DbSet<QualityMeeting> QualityMeetings { get; set; }
        public virtual DbSet<QualityMeetingTask> QualityMeetingTasks { get; set; }
        public virtual DbSet<QualityNCEmployee> QualityNCEmployees { get; set; }
        public virtual DbSet<QualityNCRole> QualityNCRoles { get; set; }
        public virtual DbSet<QualityNC> QualityNCs { get; set; }
        public virtual DbSet<QualityNCStockAdjustment> QualityNCStockAdjustments { get; set; }
        public virtual DbSet<QualityNCTask> QualityNCTasks { get; set; }
        public virtual DbSet<QualityPlan> QualityPlans { get; set; }
        public virtual DbSet<QualityPlanTask> QualityPlanTasks { get; set; }
        public virtual DbSet<QualityTargetCategory> QualityTargetCategories { get; set; }
        public virtual DbSet<QualityTargetLevel> QualityTargetLevels { get; set; }
        public virtual DbSet<QualityTarget> QualityTargets { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportTemplate> ReportTemplates { get; set; }
        public virtual DbSet<RiskAudit> RiskAudits { get; set; }
        public virtual DbSet<RiskContract> RiskContracts { get; set; }
        public virtual DbSet<RiskControl> RiskControls { get; set; }
        public virtual DbSet<RiskControlSolution> RiskControlSolutions { get; set; }
        public virtual DbSet<RiskControlTask> RiskControlTasks { get; set; }
        public virtual DbSet<RiskIncedent> RiskIncedents { get; set; }
        public virtual DbSet<RiskLegal> RiskLegals { get; set; }
        public virtual DbSet<RiskLevel> RiskLevels { get; set; }
        public virtual DbSet<RiskLibrarySolution> RiskLibrarySolutions { get; set; }
        public virtual DbSet<RiskRegulatory> RiskRegulatories { get; set; }
        public virtual DbSet<Risk> Risks { get; set; }
        public virtual DbSet<ServiceCategory> ServiceCategories { get; set; }
        public virtual DbSet<ServiceCommandContract> ServiceCommandContracts { get; set; }
        public virtual DbSet<ServiceCommand> ServiceCommands { get; set; }
        public virtual DbSet<ServicePlan> ServicePlans { get; set; }
        public virtual DbSet<ServicePlanStage> ServicePlanStages { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<ServiceStage> ServiceStages { get; set; }
        public virtual DbSet<StockAdjustmentDetail> StockAdjustmentDetails { get; set; }
        public virtual DbSet<StockAdjustment> StockAdjustments { get; set; }
        public virtual DbSet<StockBuildDetail> StockBuildDetails { get; set; }
        public virtual DbSet<StockBuild> StockBuilds { get; set; }
        public virtual DbSet<StockInventory> StockInventories { get; set; }
        public virtual DbSet<StockInventoryDetail> StockInventoryDetails { get; set; }
        public virtual DbSet<StockInwardDetail> StockInwardDetails { get; set; }
        public virtual DbSet<StockInward> StockInwards { get; set; }
        public virtual DbSet<StockListCurrency> StockListCurrencies { get; set; }
        public virtual DbSet<StockOutwardDetail> StockOutwardDetails { get; set; }
        public virtual DbSet<StockOutward> StockOutwards { get; set; }
        public virtual DbSet<StockProductBuild> StockProductBuilds { get; set; }
        public virtual DbSet<StockProductGroup> StockProductGroups { get; set; }
        public virtual DbSet<StockProductModel> StockProductModels { get; set; }
        public virtual DbSet<StockProductPrice> StockProductPrices { get; set; }
        public virtual DbSet<StockProduct> StockProducts { get; set; }
        public virtual DbSet<StockProductType> StockProductTypes { get; set; }
        public virtual DbSet<Stock> Stocks { get; set; }
        public virtual DbSet<StockTransferDetail> StockTransferDetails { get; set; }
        public virtual DbSet<StockTransferRef> StockTransferRefs { get; set; }
        public virtual DbSet<StockTransfer> StockTransfers { get; set; }
        public virtual DbSet<StockUnit> StockUnits { get; set; }
        public virtual DbSet<SupplierAuditPlan> SupplierAuditPlans { get; set; }
        public virtual DbSet<SupplierAuditResult> SupplierAuditResults { get; set; }
        public virtual DbSet<SupplierAudit> SupplierAudits { get; set; }
        public virtual DbSet<SupplierBidToAttachmentFile> SupplierBidToAttachmentFiles { get; set; }
        public virtual DbSet<Supplier> Suppliers { get; set; }
        public virtual DbSet<SuppliersBidOrder> SuppliersBidOrders { get; set; }
        public virtual DbSet<SuppliersBill> SuppliersBills { get; set; }
        public virtual DbSet<SuppliersGroup> SuppliersGroups { get; set; }
        public virtual DbSet<SuppliersOrderDetail> SuppliersOrderDetails { get; set; }
        public virtual DbSet<SuppliersOrderRequirementDetail> SuppliersOrderRequirementDetails { get; set; }
        public virtual DbSet<SuppliersOrderRequirement> SuppliersOrderRequirements { get; set; }
        public virtual DbSet<SuppliersOrder> SuppliersOrders { get; set; }
        public virtual DbSet<TaskAttachmentFile> TaskAttachmentFiles { get; set; }
        public virtual DbSet<TaskCalendarEvent> TaskCalendarEvents { get; set; }
        public virtual DbSet<TaskCalendar> TaskCalendars { get; set; }
        public virtual DbSet<TaskCategory> TaskCategories { get; set; }
        public virtual DbSet<TaskCC> TaskCCs { get; set; }
        public virtual DbSet<TaskCheck> TaskChecks { get; set; }
        public virtual DbSet<TaskComment> TaskComments { get; set; }
        public virtual DbSet<TaskHistory> TaskHistories { get; set; }
        public virtual DbSet<TaskLevel> TaskLevels { get; set; }
        public virtual DbSet<TaskPerformAttachmentFile> TaskPerformAttachmentFiles { get; set; }
        public virtual DbSet<TaskPerform> TaskPerforms { get; set; }
        public virtual DbSet<TaskPersonal> TaskPersonals { get; set; }
        public virtual DbSet<TaskRequest> TaskRequests { get; set; }
        public virtual DbSet<Task> Tasks { get; set; }
        public virtual DbSet<TemplateExportField> TemplateExportFields { get; set; }
        public virtual DbSet<TemplateExportFieldStyle> TemplateExportFieldStyles { get; set; }
        public virtual DbSet<TemplateExport> TemplateExports { get; set; }
        public virtual DbSet<V3ProblemAttachmentFile> V3ProblemAttachmentFiles { get; set; }
        public virtual DbSet<V3ProblemEvent> V3ProblemEvent { get; set; }
    }
}
