
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 06/24/2016 14:01:26
-- Generated from EDMX file: D:\CodeDAS\iDAS.BUSINESS\Source Code\iDAS.Base\iDASBusinessModel.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [iDAS_BUSINESS];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_CalendarCategories_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarCategories] DROP CONSTRAINT [FK_CalendarCategories_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarOverrides_CalendarCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarOverrides] DROP CONSTRAINT [FK_CalendarOverrides_CalendarCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarOverrides_TaskCalendarEvents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarOverrides] DROP CONSTRAINT [FK_CalendarOverrides_TaskCalendarEvents];
GO
IF OBJECT_ID(N'[dbo].[FK_CalendarTimeOverrides_CalendarOverrides]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CalendarTimeOverrides] DROP CONSTRAINT [FK_CalendarTimeOverrides_CalendarOverrides];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerAssesses_CustomerCriteriaCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssesses] DROP CONSTRAINT [FK_CustomerAssesses_CustomerCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCategoryDepartments_CustomerCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCategoryDepartments] DROP CONSTRAINT [FK_CustomerCategoryDepartments_CustomerCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCategoryDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCategoryDepartments] DROP CONSTRAINT [FK_CustomerCategoryDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContactHistoryAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactHistoryAttachmentFiles] DROP CONSTRAINT [FK_CustomerContactHistoryAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContactHistoryAttachmentFiles_CustomerContactHistories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactHistoryAttachmentFiles] DROP CONSTRAINT [FK_CustomerContactHistoryAttachmentFiles_CustomerContactHistories];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContacts_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContacts] DROP CONSTRAINT [FK_CustomerContacts_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContractAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContractAttachmentFiles] DROP CONSTRAINT [FK_CustomerContractAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContractAttachmentFiles_CustomerContracts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContractAttachmentFiles] DROP CONSTRAINT [FK_CustomerContractAttachmentFiles_CustomerContracts];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerOrderAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerOrderAttachmentFiles] DROP CONSTRAINT [FK_CustomerOrderAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerOrderAttachmentFiles_CustomerOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerOrderAttachmentFiles] DROP CONSTRAINT [FK_CustomerOrderAttachmentFiles_CustomerOrders];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountingPaymentPlans_dbo_AccountingPayments_AccountingPaymentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountingPaymentPlans] DROP CONSTRAINT [FK_dbo_AccountingPaymentPlans_dbo_AccountingPayments_AccountingPaymentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountingPaymentPlans_dbo_QualityPlans_QualityPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountingPaymentPlans] DROP CONSTRAINT [FK_dbo_AccountingPaymentPlans_dbo_QualityPlans_QualityPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AccountingPayments_dbo_CustomerContracts_CustomerContractID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AccountingPayments] DROP CONSTRAINT [FK_dbo_AccountingPayments_dbo_CustomerContracts_CustomerContractID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AuditResults_dbo_Audits_AuditID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuditResults] DROP CONSTRAINT [FK_dbo_AuditResults_dbo_Audits_AuditID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AuditResults_dbo_QualityCriterias_QualityCriteriaID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuditResults] DROP CONSTRAINT [FK_dbo_AuditResults_dbo_QualityCriterias_QualityCriteriaID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_AuditResults_dbo_QualityNCs_QualityNCID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AuditResults] DROP CONSTRAINT [FK_dbo_AuditResults_dbo_QualityNCs_QualityNCID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Audits_dbo_QualityCriteriaCategories_QualityCriteriaCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Audits] DROP CONSTRAINT [FK_dbo_Audits_dbo_QualityCriteriaCategories_QualityCriteriaCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Chats_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Chats] DROP CONSTRAINT [FK_dbo_Chats_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerAssesses_dbo_Audits_AuditID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssesses] DROP CONSTRAINT [FK_dbo_CustomerAssesses_dbo_Audits_AuditID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerAssessObjects_dbo_CustomerAssesses_CustomerAssessID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssessObjects] DROP CONSTRAINT [FK_dbo_CustomerAssessObjects_dbo_CustomerAssesses_CustomerAssessID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerAssessObjects_dbo_CustomerCategories_CustomerCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssessObjects] DROP CONSTRAINT [FK_dbo_CustomerAssessObjects_dbo_CustomerCategories_CustomerCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerAssessResults_dbo_CustomerAssesses_CustomerAssessID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssessResults] DROP CONSTRAINT [FK_dbo_CustomerAssessResults_dbo_CustomerAssesses_CustomerAssessID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerAssessResults_dbo_CustomerCriterias_CustomerCriteriaID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerAssessResults] DROP CONSTRAINT [FK_dbo_CustomerAssessResults_dbo_CustomerCriterias_CustomerCriteriaID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignAudits_dbo_Audits_AuditID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignAudits] DROP CONSTRAINT [FK_dbo_CustomerCampaignAudits_dbo_Audits_AuditID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignAudits_dbo_CustomerCampaigns_CustomerCampaignID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignAudits] DROP CONSTRAINT [FK_dbo_CustomerCampaignAudits_dbo_CustomerCampaigns_CustomerCampaignID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignPlans_dbo_CustomerCampaigns_CustomerCampaignID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignPlans] DROP CONSTRAINT [FK_dbo_CustomerCampaignPlans_dbo_CustomerCampaigns_CustomerCampaignID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignPlans_dbo_QualityPlans_QualityPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignPlans] DROP CONSTRAINT [FK_dbo_CustomerCampaignPlans_dbo_QualityPlans_QualityPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignTargets_dbo_CustomerCampaigns_CustomerCampaignID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignTargets] DROP CONSTRAINT [FK_dbo_CustomerCampaignTargets_dbo_CustomerCampaigns_CustomerCampaignID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCampaignTargets_dbo_QualityTargets_QualityTargetID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCampaignTargets] DROP CONSTRAINT [FK_dbo_CustomerCampaignTargets_dbo_QualityTargets_QualityTargetID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCareObjects_dbo_CustomerCares_CustomerCareID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCareObjects] DROP CONSTRAINT [FK_dbo_CustomerCareObjects_dbo_CustomerCares_CustomerCareID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCareObjects_dbo_CustomerCategories_CustomerCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCareObjects] DROP CONSTRAINT [FK_dbo_CustomerCareObjects_dbo_CustomerCategories_CustomerCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCareResults_dbo_CustomerCares_CustomerCareID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCareResults] DROP CONSTRAINT [FK_dbo_CustomerCareResults_dbo_CustomerCares_CustomerCareID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCategoryCustomers_dbo_CustomerCategories_CustomerCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCategoryCustomers] DROP CONSTRAINT [FK_dbo_CustomerCategoryCustomers_dbo_CustomerCategories_CustomerCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCategoryCustomers_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCategoryCustomers] DROP CONSTRAINT [FK_dbo_CustomerCategoryCustomers_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContactCalendars_dbo_CustomerContactForms_CustomerContactFormID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactCalendars] DROP CONSTRAINT [FK_dbo_CustomerContactCalendars_dbo_CustomerContactForms_CustomerContactFormID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContactCalendars_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactCalendars] DROP CONSTRAINT [FK_dbo_CustomerContactCalendars_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContactHistories_dbo_CustomerContactForms_CustomerContactFormID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactHistories] DROP CONSTRAINT [FK_dbo_CustomerContactHistories_dbo_CustomerContactForms_CustomerContactFormID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContactHistories_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContactHistories] DROP CONSTRAINT [FK_dbo_CustomerContactHistories_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContacts_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContacts] DROP CONSTRAINT [FK_dbo_CustomerContacts_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerContracts_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerContracts] DROP CONSTRAINT [FK_dbo_CustomerContracts_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerCriterias_dbo_CustomerCriterionCategories_CustomerCriterionCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerCriterias] DROP CONSTRAINT [FK_dbo_CustomerCriterias_dbo_CustomerCriterionCategories_CustomerCriterionCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerFeedbacks_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerFeedbacks] DROP CONSTRAINT [FK_dbo_CustomerFeedbacks_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerOrders_dbo_CustomerContracts_CustomerContractID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerOrders] DROP CONSTRAINT [FK_dbo_CustomerOrders_dbo_CustomerContracts_CustomerContractID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerOrders_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerOrders] DROP CONSTRAINT [FK_dbo_CustomerOrders_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerOrders_dbo_Services_ServiceID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerOrders] DROP CONSTRAINT [FK_dbo_CustomerOrders_dbo_Services_ServiceID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerSurveyObjects_dbo_CustomerCategories_CustomerCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSurveyObjects] DROP CONSTRAINT [FK_dbo_CustomerSurveyObjects_dbo_CustomerCategories_CustomerCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerSurveyObjects_dbo_CustomerSurveys_CustomerSurveyID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSurveyObjects] DROP CONSTRAINT [FK_dbo_CustomerSurveyObjects_dbo_CustomerSurveys_CustomerSurveyID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerSurveyQuestions_dbo_CustomerSurveys_CustomerSurveyID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSurveyQuestions] DROP CONSTRAINT [FK_dbo_CustomerSurveyQuestions_dbo_CustomerSurveys_CustomerSurveyID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_CustomerSurveyResults_dbo_CustomerSurveyQuestions_CustomerSurveyQuestionID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerSurveyResults] DROP CONSTRAINT [FK_dbo_CustomerSurveyResults_dbo_CustomerSurveyQuestions_CustomerSurveyQuestionID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoDepartments_dbo_DispatchGoes_DispatchGoID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoDepartments] DROP CONSTRAINT [FK_dbo_DispatchGoDepartments_dbo_DispatchGoes_DispatchGoID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoEmployees_dbo_DispatchGoes_DispatchGoID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoEmployees] DROP CONSTRAINT [FK_dbo_DispatchGoEmployees_dbo_DispatchGoes_DispatchGoID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoes_dbo_DispatchCategories_DispatchCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoes] DROP CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchCategories_DispatchCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoes_dbo_DispatchMethods_DispatchMethodID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoes] DROP CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchMethods_DispatchMethodID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoes_dbo_DispatchSecurities_DispatchSecurityID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoes] DROP CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchSecurities_DispatchSecurityID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoes_dbo_DispatchUrgencies_DispatchUrgencyID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoes] DROP CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchUrgencies_DispatchUrgencyID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoOutSides_dbo_DispatchGoes_DispatchGoID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoOutSides] DROP CONSTRAINT [FK_dbo_DispatchGoOutSides_dbo_DispatchGoes_DispatchGoID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoTasks_dbo_DispatchGoes_DispatchGoID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoTasks] DROP CONSTRAINT [FK_dbo_DispatchGoTasks_dbo_DispatchGoes_DispatchGoID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchGoTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoTasks] DROP CONSTRAINT [FK_dbo_DispatchGoTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToAttachments_dbo_DispatchToes_DispatchToID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToAttachmentFiles] DROP CONSTRAINT [FK_dbo_DispatchToAttachments_dbo_DispatchToes_DispatchToID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToDepartments_dbo_DispatchToes_DispatchToID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToDepartments] DROP CONSTRAINT [FK_dbo_DispatchToDepartments_dbo_DispatchToes_DispatchToID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToEmployees_dbo_DispatchToes_DispatchToID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToEmployees] DROP CONSTRAINT [FK_dbo_DispatchToEmployees_dbo_DispatchToes_DispatchToID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToes_dbo_DispatchCategories_DispatchCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToes] DROP CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchCategories_DispatchCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToes_dbo_DispatchMethods_DispatchMethodID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToes] DROP CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchMethods_DispatchMethodID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToes_dbo_DispatchSecurities_DispatchSecurityID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToes] DROP CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchSecurities_DispatchSecurityID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToes_dbo_DispatchUrgencies_DispatchUrgencyID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToes] DROP CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchUrgencies_DispatchUrgencyID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToTasks_dbo_DispatchToes_DispatchToID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToTasks] DROP CONSTRAINT [FK_dbo_DispatchToTasks_dbo_DispatchToes_DispatchToID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DispatchToTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToTasks] DROP CONSTRAINT [FK_dbo_DispatchToTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DocumentAttachments_dbo_Documents_DocumentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentAttachments] DROP CONSTRAINT [FK_dbo_DocumentAttachments_dbo_Documents_DocumentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DocumentDistributions_dbo_Documents_DocumentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentDistributions] DROP CONSTRAINT [FK_dbo_DocumentDistributions_dbo_Documents_DocumentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DocumentRequirements_dbo_Documents_DocumentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentRequirements] DROP CONSTRAINT [FK_dbo_DocumentRequirements_dbo_Documents_DocumentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Documents_dbo_DocumentCategories_DocumentCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_dbo_Documents_dbo_DocumentCategories_DocumentCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Documents_dbo_DocumentSecurities_DocumentSecurityID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Documents] DROP CONSTRAINT [FK_dbo_Documents_dbo_DocumentSecurities_DocumentSecurityID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_DocumentTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentTasks] DROP CONSTRAINT [FK_dbo_DocumentTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanOrganizations_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanOrganizations] DROP CONSTRAINT [FK_dbo_HumanOrganizations_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanOrganizations_dbo_HumanRoles_HumanRoleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanOrganizations] DROP CONSTRAINT [FK_dbo_HumanOrganizations_dbo_HumanRoles_HumanRoleID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanPermissions_dbo_HumanRoles_HumanRoleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanPermissions] DROP CONSTRAINT [FK_dbo_HumanPermissions_dbo_HumanRoles_HumanRoleID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileAssesses_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileAssesses] DROP CONSTRAINT [FK_dbo_HumanProfileAssesses_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileAttachments_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileAttachments] DROP CONSTRAINT [FK_dbo_HumanProfileAttachments_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileCertificates_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileCertificates] DROP CONSTRAINT [FK_dbo_HumanProfileCertificates_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileContracts_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileContracts] DROP CONSTRAINT [FK_dbo_HumanProfileContracts_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileCuriculmViates_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileCuriculmViates] DROP CONSTRAINT [FK_dbo_HumanProfileCuriculmViates_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileDiplomas_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileDiplomas] DROP CONSTRAINT [FK_dbo_HumanProfileDiplomas_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileDisciplines_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileDisciplines] DROP CONSTRAINT [FK_dbo_HumanProfileDisciplines_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileInsurances_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileInsurances] DROP CONSTRAINT [FK_dbo_HumanProfileInsurances_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileRelationships_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileRelationships] DROP CONSTRAINT [FK_dbo_HumanProfileRelationships_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileRewards_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileRewards] DROP CONSTRAINT [FK_dbo_HumanProfileRewards_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileSalaries_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileSalaries] DROP CONSTRAINT [FK_dbo_HumanProfileSalaries_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileTrainings_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileTrainings] DROP CONSTRAINT [FK_dbo_HumanProfileTrainings_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanProfileWorkExperiences_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileWorkExperiences] DROP CONSTRAINT [FK_dbo_HumanProfileWorkExperiences_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentCriterias_dbo_HumanRoles_HumanRoleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentCriterias] DROP CONSTRAINT [FK_dbo_HumanRecruitmentCriterias_dbo_HumanRoles_HumanRoleID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentPlanInterviews_HumanRecruitmentPlanInterviewID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentPlanInterviews_HumanRecruitmentPlanInterviewID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentPlanInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentPlanInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentPlanInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentPlanRequirements] DROP CONSTRAINT [FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentPlanRequirements] DROP CONSTRAINT [FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentProfileResults_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentProfileResults] DROP CONSTRAINT [FK_dbo_HumanRecruitmentProfileResults_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentRequirements_dbo_HumanRoles_HumanRoleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentRequirements] DROP CONSTRAINT [FK_dbo_HumanRecruitmentRequirements_dbo_HumanRoles_HumanRoleID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentCriterias_HumanRecruitmentCriteriaID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentReviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentCriterias_HumanRecruitmentCriteriaID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentReviews] DROP CONSTRAINT [FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentTasks_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentTasks] DROP CONSTRAINT [FK_dbo_HumanRecruitmentTasks_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRecruitmentTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRecruitmentTasks] DROP CONSTRAINT [FK_dbo_HumanRecruitmentTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanRoles_dbo_HumanDepartments_HumanDepartmentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanRoles] DROP CONSTRAINT [FK_dbo_HumanRoles_dbo_HumanDepartments_HumanDepartmentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPlanDetails_dbo_HumanTrainingPlans_HumanTrainingPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPlanDetails] DROP CONSTRAINT [FK_dbo_HumanTrainingPlanDetails_dbo_HumanTrainingPlans_HumanTrainingPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingPlans_HumanTrainingPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPlanRequirements] DROP CONSTRAINT [FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingPlans_HumanTrainingPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPlanRequirements] DROP CONSTRAINT [FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingRequirements_HumanTrainingRequirementID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPlans_dbo_QualityPlans_QualityPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPlans] DROP CONSTRAINT [FK_dbo_HumanTrainingPlans_dbo_QualityPlans_QualityPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPractioners_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPractioners] DROP CONSTRAINT [FK_dbo_HumanTrainingPractioners_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingPractioners_dbo_HumanTrainingPlanDetails_HumanTrainingPlanDetailID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingPractioners] DROP CONSTRAINT [FK_dbo_HumanTrainingPractioners_dbo_HumanTrainingPlanDetails_HumanTrainingPlanDetailID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanTrainingRequirementEmployees_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanTrainingRequirementEmployees] DROP CONSTRAINT [FK_dbo_HumanTrainingRequirementEmployees_dbo_HumanTrainingRequirements_HumanTrainingRequirementID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_HumanUsers_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanUsers] DROP CONSTRAINT [FK_dbo_HumanUsers_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Notifies_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Notifies] DROP CONSTRAINT [FK_dbo_Notifies_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileAttachments_dbo_Profiles_ProfileID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileAttachmentFiles] DROP CONSTRAINT [FK_dbo_ProfileAttachments_dbo_Profiles_ProfileID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileBorrows_dbo_ProfileBorrowCategories_ProfileBorrowCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileBorrows] DROP CONSTRAINT [FK_dbo_ProfileBorrows_dbo_ProfileBorrowCategories_ProfileBorrowCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileBorrows_dbo_Profiles_ProfileID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileBorrows] DROP CONSTRAINT [FK_dbo_ProfileBorrows_dbo_Profiles_ProfileID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileCancelEmployees_dbo_ProfileCancels_ProfileCancelID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileCancelEmployees] DROP CONSTRAINT [FK_dbo_ProfileCancelEmployees_dbo_ProfileCancels_ProfileCancelID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileCancelProfiles_dbo_ProfileCancels_ProfileCancelID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileCancelProfiles] DROP CONSTRAINT [FK_dbo_ProfileCancelProfiles_dbo_ProfileCancels_ProfileCancelID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileCancelProfiles_dbo_Profiles_ProfileID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileCancelProfiles] DROP CONSTRAINT [FK_dbo_ProfileCancelProfiles_dbo_Profiles_ProfileID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ProfileCancels_dbo_ProfileCancelMethods_ProfileCancelMethodID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileCancels] DROP CONSTRAINT [FK_dbo_ProfileCancels_dbo_ProfileCancelMethods_ProfileCancelMethodID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Profiles_dbo_ProfileCategories_ProfileCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Profiles] DROP CONSTRAINT [FK_dbo_Profiles_dbo_ProfileCategories_ProfileCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Profiles_dbo_ProfileSecurities_ProfileSecurityID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Profiles] DROP CONSTRAINT [FK_dbo_Profiles_dbo_ProfileSecurities_ProfileSecurityID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityAuditEmployees_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditProgramEmployees] DROP CONSTRAINT [FK_dbo_QualityAuditEmployees_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityAuditPrograms_dbo_QualityAuditPlans_QualityAuditPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditPrograms] DROP CONSTRAINT [FK_dbo_QualityAuditPrograms_dbo_QualityAuditPlans_QualityAuditPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityCAPARequirements_dbo_QualityNCs_QualityNCID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityCAPARequirements] DROP CONSTRAINT [FK_dbo_QualityCAPARequirements_dbo_QualityNCs_QualityNCID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityCAPAs_dbo_QualityCAPARequirements_QualityCAPARequirementID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityCAPAs] DROP CONSTRAINT [FK_dbo_QualityCAPAs_dbo_QualityCAPARequirements_QualityCAPARequirementID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityCAPATasks_dbo_QualityCAPAs_QualityCAPAID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityCAPATasks] DROP CONSTRAINT [FK_dbo_QualityCAPATasks_dbo_QualityCAPAs_QualityCAPAID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityCAPATasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityCAPATasks] DROP CONSTRAINT [FK_dbo_QualityCAPATasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityMeetingObjects_dbo_QualityMeetings_QualityMeetingID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingObjects] DROP CONSTRAINT [FK_dbo_QualityMeetingObjects_dbo_QualityMeetings_QualityMeetingID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityMeetingProblems_dbo_QualityMeetings_QualityMeetingID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingProblems] DROP CONSTRAINT [FK_dbo_QualityMeetingProblems_dbo_QualityMeetings_QualityMeetingID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityMeetingResults_dbo_QualityMeetings_QualityMeetingID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingResults] DROP CONSTRAINT [FK_dbo_QualityMeetingResults_dbo_QualityMeetings_QualityMeetingID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityMeetingTasks_dbo_QualityMeetingProblems_QualityMeetingProblemID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingTasks] DROP CONSTRAINT [FK_dbo_QualityMeetingTasks_dbo_QualityMeetingProblems_QualityMeetingProblemID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityMeetingTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingTasks] DROP CONSTRAINT [FK_dbo_QualityMeetingTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityNCEmployees_dbo_QualityNCs_QualityNCID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCEmployees] DROP CONSTRAINT [FK_dbo_QualityNCEmployees_dbo_QualityNCs_QualityNCID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityNCRoles_dbo_HumanRoles_HumanRoleID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCRoles] DROP CONSTRAINT [FK_dbo_QualityNCRoles_dbo_HumanRoles_HumanRoleID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityNCRoles_dbo_QualityNCs_QualityNCID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCRoles] DROP CONSTRAINT [FK_dbo_QualityNCRoles_dbo_QualityNCs_QualityNCID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityNCTasks_dbo_QualityNCs_QualityNCID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCTasks] DROP CONSTRAINT [FK_dbo_QualityNCTasks_dbo_QualityNCs_QualityNCID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityNCTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCTasks] DROP CONSTRAINT [FK_dbo_QualityNCTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityPlans_dbo_QualityTargets_QualityTargetID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityPlans] DROP CONSTRAINT [FK_dbo_QualityPlans_dbo_QualityTargets_QualityTargetID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityPlanTasks_dbo_QualityPlans_QualityPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityPlanTasks] DROP CONSTRAINT [FK_dbo_QualityPlanTasks_dbo_QualityPlans_QualityPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_QualityPlanTasks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityPlanTasks] DROP CONSTRAINT [FK_dbo_QualityPlanTasks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServiceCommandContracts_dbo_CustomerContracts_CustomerContractID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceCommandContracts] DROP CONSTRAINT [FK_dbo_ServiceCommandContracts_dbo_CustomerContracts_CustomerContractID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServiceCommandContracts_dbo_ServiceCommands_ServiceCommandID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceCommandContracts] DROP CONSTRAINT [FK_dbo_ServiceCommandContracts_dbo_ServiceCommands_ServiceCommandID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServicePlans_dbo_QualityPlans_QualityPlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServicePlans] DROP CONSTRAINT [FK_dbo_ServicePlans_dbo_QualityPlans_QualityPlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServicePlans_dbo_ServiceCommandContracts_ServiceCommandContractID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServicePlans] DROP CONSTRAINT [FK_dbo_ServicePlans_dbo_ServiceCommandContracts_ServiceCommandContractID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServicePlanStages_dbo_Customers_CustomerID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServicePlanStages] DROP CONSTRAINT [FK_dbo_ServicePlanStages_dbo_Customers_CustomerID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServicePlanStages_dbo_ServicePlans_ServicePlanID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServicePlanStages] DROP CONSTRAINT [FK_dbo_ServicePlanStages_dbo_ServicePlans_ServicePlanID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServicePlanStages_dbo_ServiceStages_ServiceStageID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServicePlanStages] DROP CONSTRAINT [FK_dbo_ServicePlanStages_dbo_ServiceStages_ServiceStageID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Services_dbo_ServiceCategories_ServiceCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Services] DROP CONSTRAINT [FK_dbo_Services_dbo_ServiceCategories_ServiceCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_ServiceStages_dbo_Services_ServiceID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ServiceStages] DROP CONSTRAINT [FK_dbo_ServiceStages_dbo_Services_ServiceID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockAdjustmentDetails_dbo_StockAdjustments_StockAdjustmentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockAdjustmentDetails] DROP CONSTRAINT [FK_dbo_StockAdjustmentDetails_dbo_StockAdjustments_StockAdjustmentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockAdjustmentDetails_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockAdjustmentDetails] DROP CONSTRAINT [FK_dbo_StockAdjustmentDetails_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockBuildDetails_dbo_StockBuilds_StockBuildID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockBuildDetails] DROP CONSTRAINT [FK_dbo_StockBuildDetails_dbo_StockBuilds_StockBuildID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInventories_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInventories] DROP CONSTRAINT [FK_dbo_StockInventories_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInventoryDetails_dbo_StockInventories_StockInventoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInventoryDetails] DROP CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_StockInventories_StockInventoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInventoryDetails_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInventoryDetails] DROP CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInventoryDetails_dbo_Stocks_StockID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInventoryDetails] DROP CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_Stocks_StockID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInwardDetails_dbo_StockInwards_StockInwardID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInwardDetails] DROP CONSTRAINT [FK_dbo_StockInwardDetails_dbo_StockInwards_StockInwardID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockInwardDetails_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockInwardDetails] DROP CONSTRAINT [FK_dbo_StockInwardDetails_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockOutwardDetails_dbo_StockOutwards_StockOutwardID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockOutwardDetails] DROP CONSTRAINT [FK_dbo_StockOutwardDetails_dbo_StockOutwards_StockOutwardID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockOutwardDetails_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockOutwardDetails] DROP CONSTRAINT [FK_dbo_StockOutwardDetails_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockProductBuilds_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockProductBuilds] DROP CONSTRAINT [FK_dbo_StockProductBuilds_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockProducts_dbo_StockProductGroups_StockProductGroupID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockProducts] DROP CONSTRAINT [FK_dbo_StockProducts_dbo_StockProductGroups_StockProductGroupID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockProducts_dbo_Stocks_StockID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockProducts] DROP CONSTRAINT [FK_dbo_StockProducts_dbo_Stocks_StockID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_StockTransferDetails_dbo_StockProducts_StockProductID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockTransferDetails] DROP CONSTRAINT [FK_dbo_StockTransferDetails_dbo_StockProducts_StockProductID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TaskCalendars_dbo_TaskCalendarEvents_TaskCalendarEventID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskCalendars] DROP CONSTRAINT [FK_dbo_TaskCalendars_dbo_TaskCalendarEvents_TaskCalendarEventID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TaskChecks_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskChecks] DROP CONSTRAINT [FK_dbo_TaskChecks_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TaskHistories_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskHistories] DROP CONSTRAINT [FK_dbo_TaskHistories_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TaskPerforms_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPerforms] DROP CONSTRAINT [FK_dbo_TaskPerforms_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_TaskPersonals_dbo_Tasks_TaskID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPersonals] DROP CONSTRAINT [FK_dbo_TaskPersonals_dbo_Tasks_TaskID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Tasks_dbo_Audits_AuditID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_dbo_Tasks_dbo_Audits_AuditID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Tasks_dbo_HumanDepartments_HumanDepartmentID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_dbo_Tasks_dbo_HumanDepartments_HumanDepartmentID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Tasks_dbo_HumanEmployees_HumanEmployeeID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_dbo_Tasks_dbo_HumanEmployees_HumanEmployeeID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Tasks_dbo_TaskCategories_TaskCategoryID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_dbo_Tasks_dbo_TaskCategories_TaskCategoryID];
GO
IF OBJECT_ID(N'[dbo].[FK_dbo_Tasks_dbo_TaskLevels_TaskLevelID]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Tasks] DROP CONSTRAINT [FK_dbo_Tasks_dbo_TaskLevels_TaskLevelID];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentBroads_DepartmentBroadCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentBroads] DROP CONSTRAINT [FK_DepartmentBroads_DepartmentBroadCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentLegalAttachments_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentLegalAttachments] DROP CONSTRAINT [FK_DepartmentLegalAttachments_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentLegalAttachments_DepartmentLegals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentLegalAttachments] DROP CONSTRAINT [FK_DepartmentLegalAttachments_DepartmentLegals];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentLegalAuditResults_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentLegalAuditResults] DROP CONSTRAINT [FK_DepartmentLegalAuditResults_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentLegalDepartments_DepartmentLegals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentLegalAuditResults] DROP CONSTRAINT [FK_DepartmentLegalDepartments_DepartmentLegals];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentLegalDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentLegalAuditResults] DROP CONSTRAINT [FK_DepartmentLegalDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentPolicyAttachments_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentPolicyAttachments] DROP CONSTRAINT [FK_DepartmentPolicyAttachments_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentPolicyAttachments_DepartmentPolicies]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentPolicyAttachments] DROP CONSTRAINT [FK_DepartmentPolicyAttachments_DepartmentPolicies];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRegulatoryAttachments_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRegulatoryAttachments] DROP CONSTRAINT [FK_DepartmentRegulatoryAttachments_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRegulatoryAttachments_DepartmentRegulatories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRegulatoryAttachments] DROP CONSTRAINT [FK_DepartmentRegulatoryAttachments_DepartmentRegulatories];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRegulatoryAuditResults_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults] DROP CONSTRAINT [FK_DepartmentRegulatoryAuditResults_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRegulatoryDepartments_DepartmentRegulatories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults] DROP CONSTRAINT [FK_DepartmentRegulatoryDepartments_DepartmentRegulatories];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRegulatoryDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults] DROP CONSTRAINT [FK_DepartmentRegulatoryDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRequirmentAttachments_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRequirmentAttachments] DROP CONSTRAINT [FK_DepartmentRequirmentAttachments_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRequirmentAttachments_DepartmentRequirments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRequirmentAttachments] DROP CONSTRAINT [FK_DepartmentRequirmentAttachments_DepartmentRequirments];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRequirmentAuditResults_DepartmentRequirments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRequirmentAuditResults] DROP CONSTRAINT [FK_DepartmentRequirmentAuditResults_DepartmentRequirments];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRequirmentAuditResults_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRequirmentAuditResults] DROP CONSTRAINT [FK_DepartmentRequirmentAuditResults_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_DepartmentRequirmentDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentRequirmentAuditResults] DROP CONSTRAINT [FK_DepartmentRequirmentDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchGoAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoAttachmentFiles] DROP CONSTRAINT [FK_DispatchGoAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchGoAttachmentFiles_DispatchGoes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoAttachmentFiles] DROP CONSTRAINT [FK_DispatchGoAttachmentFiles_DispatchGoes];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchGoCCs_DispatchGoes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoCCs] DROP CONSTRAINT [FK_DispatchGoCCs_DispatchGoes];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchGoCCs_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchGoCCs] DROP CONSTRAINT [FK_DispatchGoCCs_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchToAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToAttachmentFiles] DROP CONSTRAINT [FK_DispatchToAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchToCCs_DispatchToes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToCCs] DROP CONSTRAINT [FK_DispatchToCCs_DispatchToes];
GO
IF OBJECT_ID(N'[dbo].[FK_DispatchToCCs_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DispatchToCCs] DROP CONSTRAINT [FK_DispatchToCCs_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentAttachmentFiles] DROP CONSTRAINT [FK_DocumentAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_DocumentAttachmentFiles_Documents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DocumentAttachmentFiles] DROP CONSTRAINT [FK_DocumentAttachmentFiles_Documents];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentCalibrations_EquipmentMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentCalibrations] DROP CONSTRAINT [FK_EquipmentCalibrations_EquipmentMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureAttachmentFiles] DROP CONSTRAINT [FK_EquipmentMeasureAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureAttachmentFiles_EquipmentMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureAttachmentFiles] DROP CONSTRAINT [FK_EquipmentMeasureAttachmentFiles_EquipmentMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureCalibrationResults_EquipmentMeasureCalibrations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureCalibrationResults] DROP CONSTRAINT [FK_EquipmentMeasureCalibrationResults_EquipmentMeasureCalibrations];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureCalibrations_EquipmentCalibrations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureCalibrations] DROP CONSTRAINT [FK_EquipmentMeasureCalibrations_EquipmentCalibrations];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureCalibrations_EquipmentMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureCalibrations] DROP CONSTRAINT [FK_EquipmentMeasureCalibrations_EquipmentMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureCalibrations_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureCalibrations] DROP CONSTRAINT [FK_EquipmentMeasureCalibrations_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureDepartments_EquipmentMeasures]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureDepartments] DROP CONSTRAINT [FK_EquipmentMeasureDepartments_EquipmentMeasures];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasureDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasureDepartments] DROP CONSTRAINT [FK_EquipmentMeasureDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentMeasures_EquipmentMeasureCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentMeasures] DROP CONSTRAINT [FK_EquipmentMeasures_EquipmentMeasureCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionAttachmentFiles] DROP CONSTRAINT [FK_EquipmentProductionAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionAttachmentFiles_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionAttachmentFiles] DROP CONSTRAINT [FK_EquipmentProductionAttachmentFiles_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionAttachs_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionAttaches] DROP CONSTRAINT [FK_EquipmentProductionAttachs_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionDepartments_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionDepartments] DROP CONSTRAINT [FK_EquipmentProductionDepartments_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionDepartments] DROP CONSTRAINT [FK_EquipmentProductionDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionErrors_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionErrors] DROP CONSTRAINT [FK_EquipmentProductionErrors_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionErrors_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionErrors] DROP CONSTRAINT [FK_EquipmentProductionErrors_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionErrors_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionErrors] DROP CONSTRAINT [FK_EquipmentProductionErrors_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionMaintains_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionMaintains] DROP CONSTRAINT [FK_EquipmentProductionMaintains_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionMaintains_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionMaintains] DROP CONSTRAINT [FK_EquipmentProductionMaintains_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductions_EquipmentProductionCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductions] DROP CONSTRAINT [FK_EquipmentProductions_EquipmentProductionCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionTrackings_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionHistories] DROP CONSTRAINT [FK_EquipmentProductionTrackings_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentProductionTrackings_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionHistories] DROP CONSTRAINT [FK_EquipmentProductionTrackings_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_EquipmentTracking_EquipmentProduction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EquipmentProductionHistories] DROP CONSTRAINT [FK_EquipmentTracking_EquipmentProduction];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAbsents_HumanAbsentTypes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAbsents] DROP CONSTRAINT [FK_HumanAbsents_HumanAbsentTypes];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAnswers_HumanQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAnswers] DROP CONSTRAINT [FK_HumanAnswers_HumanQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditDepartments_HumanAuditGradations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditDepartments] DROP CONSTRAINT [FK_HumanAuditDepartments_HumanAuditGradations];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditDepartments] DROP CONSTRAINT [FK_HumanAuditDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditGradationCriteias_HumanAuditGradationRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditGradationCriterias] DROP CONSTRAINT [FK_HumanAuditGradationCriteias_HumanAuditGradationRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditGradationRoles_HumanAuditDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditGradationRoles] DROP CONSTRAINT [FK_HumanAuditGradationRoles_HumanAuditDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditGradationRoles_HumanRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditGradationRoles] DROP CONSTRAINT [FK_HumanAuditGradationRoles_HumanRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditGraditionCriteriaPoints_HumanAuditGradationCriterias]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditGradationCriteriaPoints] DROP CONSTRAINT [FK_HumanAuditGraditionCriteriaPoints_HumanAuditGradationCriterias];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditLevels_HumanAuditGradationRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditLevels] DROP CONSTRAINT [FK_HumanAuditLevels_HumanAuditGradationRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditTickResultBonusScores_HumanAuditTicks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditTickResultBonusScores] DROP CONSTRAINT [FK_HumanAuditTickResultBonusScores_HumanAuditTicks];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditTickResults_HumanAuditTicks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditTickResults] DROP CONSTRAINT [FK_HumanAuditTickResults_HumanAuditTicks];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditTicks_HumanAuditGradationRoles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditTicks] DROP CONSTRAINT [FK_HumanAuditTicks_HumanAuditGradationRoles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditTicks_HumanAuditLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditTicks] DROP CONSTRAINT [FK_HumanAuditTicks_HumanAuditLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanAuditTicks_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditTicks] DROP CONSTRAINT [FK_HumanAuditTicks_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanCriteriaPoints_HumanCriterias]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditCriteriaPoints] DROP CONSTRAINT [FK_HumanCriteriaPoints_HumanCriterias];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanCriterias_HumanCriteriaCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanAuditCriterias] DROP CONSTRAINT [FK_HumanCriterias_HumanCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanEmployeeAudits_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanEmployeeAudits] DROP CONSTRAINT [FK_HumanEmployeeAudits_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanEmployeeAudits_HumanPhaseAudits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanEmployeeAudits] DROP CONSTRAINT [FK_HumanEmployeeAudits_HumanPhaseAudits];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanEmployees_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanEmployees] DROP CONSTRAINT [FK_HumanEmployees_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanPhaseAudits_HumanCategoryQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanPhaseAudits] DROP CONSTRAINT [FK_HumanPhaseAudits_HumanCategoryQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanProfileAttachments_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileAttachments] DROP CONSTRAINT [FK_HumanProfileAttachments_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanProfileWorkTrialResults_HumanProfileWorkTrials]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileWorkTrialResults] DROP CONSTRAINT [FK_HumanProfileWorkTrialResults_HumanProfileWorkTrials];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanProfileWorkTrialResults_QualityCriterias]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileWorkTrialResults] DROP CONSTRAINT [FK_HumanProfileWorkTrialResults_QualityCriterias];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanProfileWorkTrials_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanProfileWorkTrials] DROP CONSTRAINT [FK_HumanProfileWorkTrials_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanQuestions_HumanCategoryQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanQuestions] DROP CONSTRAINT [FK_HumanQuestions_HumanCategoryQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanResultAnswers_HumanAnswers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanResultAnswers] DROP CONSTRAINT [FK_HumanResultAnswers_HumanAnswers];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanResultAnswers_HumanResultQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanResultAnswers] DROP CONSTRAINT [FK_HumanResultAnswers_HumanResultQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanResultQuestions_HumanEmployeeAudits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanResultQuestions] DROP CONSTRAINT [FK_HumanResultQuestions_HumanEmployeeAudits];
GO
IF OBJECT_ID(N'[dbo].[FK_HumanResultQuestions_HumanQuestions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HumanResultQuestions] DROP CONSTRAINT [FK_HumanResultQuestions_HumanQuestions];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDeveloperRequirementPlan_ProductDeveloperRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans] DROP CONSTRAINT [FK_ProductDeveloperRequirementPlan_ProductDeveloperRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDeveloperRequirementPlan_QualityPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans] DROP CONSTRAINT [FK_ProductDeveloperRequirementPlan_QualityPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirementAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles] DROP CONSTRAINT [FK_ProductDevelopmentRequirementAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirementAttachmentFiles_ProductDevelopmentRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles] DROP CONSTRAINT [FK_ProductDevelopmentRequirementAttachmentFiles_ProductDevelopmentRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirementPlanDocument_Documents]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments] DROP CONSTRAINT [FK_ProductDevelopmentRequirementPlanDocument_Documents];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirementPlanDocument_ProductDevelopmentRequirementPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments] DROP CONSTRAINT [FK_ProductDevelopmentRequirementPlanDocument_ProductDevelopmentRequirementPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirementPlans_ProductionRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans] DROP CONSTRAINT [FK_ProductDevelopmentRequirementPlans_ProductionRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductDevelopmentRequirements_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductDevelopmentRequirements] DROP CONSTRAINT [FK_ProductDevelopmentRequirements_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionCommands_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionCommands] DROP CONSTRAINT [FK_ProductionCommands_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionCommands_ProductionPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionCommands] DROP CONSTRAINT [FK_ProductionCommands_ProductionPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionCommands_ProductionRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionCommands] DROP CONSTRAINT [FK_ProductionCommands_ProductionRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionCommands_ProductionStages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionCommands] DROP CONSTRAINT [FK_ProductionCommands_ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerfomrs_ProductionCommands]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerforms] DROP CONSTRAINT [FK_ProductionPerfomrs_ProductionCommands];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerfomrs_ProductionShifts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerforms] DROP CONSTRAINT [FK_ProductionPerfomrs_ProductionShifts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformHistories_ProductionPerfomrs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformHistories] DROP CONSTRAINT [FK_ProductionPerformHistories_ProductionPerfomrs];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformMaterias_ProductionPerfomrs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformMaterias] DROP CONSTRAINT [FK_ProductionPerformMaterias_ProductionPerfomrs];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformMaterias_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformMaterias] DROP CONSTRAINT [FK_ProductionPerformMaterias_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformProductErrors_ProductionPerforms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformProductErrors] DROP CONSTRAINT [FK_ProductionPerformProductErrors_ProductionPerforms];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformProductErrors_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformProductErrors] DROP CONSTRAINT [FK_ProductionPerformProductErrors_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPerformResults_ProductionPerfomrs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPerformResults] DROP CONSTRAINT [FK_ProductionPerformResults_ProductionPerfomrs];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanEquipments_EquipmentProductions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanEquipments] DROP CONSTRAINT [FK_ProductionPlanEquipments_EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanEquipments_ProductionPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanEquipments] DROP CONSTRAINT [FK_ProductionPlanEquipments_ProductionPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanMaterials_ProductionPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanMaterials] DROP CONSTRAINT [FK_ProductionPlanMaterials_ProductionPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanMaterials_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanMaterials] DROP CONSTRAINT [FK_ProductionPlanMaterials_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanProductDetails_ProductionPlanProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanProductDetails] DROP CONSTRAINT [FK_ProductionPlanProductDetails_ProductionPlanProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanProducts_ProductionPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanProducts] DROP CONSTRAINT [FK_ProductionPlanProducts_ProductionPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlanProducts_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlanProducts] DROP CONSTRAINT [FK_ProductionPlanProducts_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionPlans_ProductionRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionPlans] DROP CONSTRAINT [FK_ProductionPlans_ProductionRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionRequirements_ProductionLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionRequirements] DROP CONSTRAINT [FK_ProductionRequirements_ProductionLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionRequirements_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionRequirements] DROP CONSTRAINT [FK_ProductionRequirements_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageCriteriaCategories_ProductionStages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageCriteriaCategories] DROP CONSTRAINT [FK_ProductionStageCriteriaCategories_ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageCriteriaCategories_QualityCriteriaCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageCriteriaCategories] DROP CONSTRAINT [FK_ProductionStageCriteriaCategories_QualityCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageEquipments_EquipmentProductionCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageEquipments] DROP CONSTRAINT [FK_ProductionStageEquipments_EquipmentProductionCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageEquipments_ProductionStages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageEquipments] DROP CONSTRAINT [FK_ProductionStageEquipments_ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageMaterials_ProductionStages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageMaterials] DROP CONSTRAINT [FK_ProductionStageMaterials_ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageMaterials_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageMaterials] DROP CONSTRAINT [FK_ProductionStageMaterials_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageProducts_ProductionStages]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageProducts] DROP CONSTRAINT [FK_ProductionStageProducts_ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStageProducts_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStageProducts] DROP CONSTRAINT [FK_ProductionStageProducts_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductionStages_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductionStages] DROP CONSTRAINT [FK_ProductionStages_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_ProfileAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProfileAttachmentFiles] DROP CONSTRAINT [FK_ProfileAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditProgramDepartments_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditProgramDepartments] DROP CONSTRAINT [FK_QualityAuditProgramDepartments_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditProgramDepartments_QualityAuditPrograms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditProgramDepartments] DROP CONSTRAINT [FK_QualityAuditProgramDepartments_QualityAuditPrograms];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditProgramEmployees_QualityAuditPrograms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditProgramEmployees] DROP CONSTRAINT [FK_QualityAuditProgramEmployees_QualityAuditPrograms];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditProgramISOIndexes_QualityAuditPrograms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditProgramISOes] DROP CONSTRAINT [FK_QualityAuditProgramISOIndexes_QualityAuditPrograms];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditRecordedVotes_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditRecordedVotes] DROP CONSTRAINT [FK_QualityAuditRecordedVotes_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditRecordedVotes_QualityAuditPrograms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditRecordedVotes] DROP CONSTRAINT [FK_QualityAuditRecordedVotes_QualityAuditPrograms];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditResults_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditResults] DROP CONSTRAINT [FK_QualityAuditResults_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditResults_QualityAuditProgramISOIndexes]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditResults] DROP CONSTRAINT [FK_QualityAuditResults_QualityAuditProgramISOIndexes];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityAuditResults_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityAuditResults] DROP CONSTRAINT [FK_QualityAuditResults_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityCriterias_QualityCriteriaCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityCriterias] DROP CONSTRAINT [FK_QualityCriterias_QualityCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityMeetingAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingAttachmentFiles] DROP CONSTRAINT [FK_QualityMeetingAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityMeetingAttachmentFiles_QualityMeetings]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingAttachmentFiles] DROP CONSTRAINT [FK_QualityMeetingAttachmentFiles_QualityMeetings];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityMeetingResultAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingResultAttachmentFiles] DROP CONSTRAINT [FK_QualityMeetingResultAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityMeetingResultAttachmentFiles_QualityMeetingResults]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityMeetingResultAttachmentFiles] DROP CONSTRAINT [FK_QualityMeetingResultAttachmentFiles_QualityMeetingResults];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityNCStockAdjustments_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCStockAdjustments] DROP CONSTRAINT [FK_QualityNCStockAdjustments_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityNCStockAdjustments_StockAdjustments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityNCStockAdjustments] DROP CONSTRAINT [FK_QualityNCStockAdjustments_StockAdjustments];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityTargets_QualityTargetCategories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityTargets] DROP CONSTRAINT [FK_QualityTargets_QualityTargetCategories];
GO
IF OBJECT_ID(N'[dbo].[FK_QualityTargets_QualityTargetLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QualityTargets] DROP CONSTRAINT [FK_QualityTargets_QualityTargetLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_Reports_ReportTemplates]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Reports] DROP CONSTRAINT [FK_Reports_ReportTemplates];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskAudits_QualityNCs]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskAudits] DROP CONSTRAINT [FK_RiskAudits_QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskAudits_RiskControls]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskAudits] DROP CONSTRAINT [FK_RiskAudits_RiskControls];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskContracts_CustomerContracts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskContracts] DROP CONSTRAINT [FK_RiskContracts_CustomerContracts];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskContracts_Risks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskContracts] DROP CONSTRAINT [FK_RiskContracts_Risks];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskControls_Risks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskControls] DROP CONSTRAINT [FK_RiskControls_Risks];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskControlSolutions_RiskControls]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskControlSolutions] DROP CONSTRAINT [FK_RiskControlSolutions_RiskControls];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskControlSolutions_RiskLibrarySolutions]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskControlSolutions] DROP CONSTRAINT [FK_RiskControlSolutions_RiskLibrarySolutions];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskControlTasks_RiskControls]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskControlTasks] DROP CONSTRAINT [FK_RiskControlTasks_RiskControls];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskControlTasks_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskControlTasks] DROP CONSTRAINT [FK_RiskControlTasks_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskIncedents_Risks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskIncedents] DROP CONSTRAINT [FK_RiskIncedents_Risks];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskLegals_DepartmentLegals]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskLegals] DROP CONSTRAINT [FK_RiskLegals_DepartmentLegals];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskLegals_Risks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskLegals] DROP CONSTRAINT [FK_RiskLegals_Risks];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskRegulatories_DepartmentRegulatories]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskRegulatories] DROP CONSTRAINT [FK_RiskRegulatories_DepartmentRegulatories];
GO
IF OBJECT_ID(N'[dbo].[FK_RiskRegulatories_Risks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RiskRegulatories] DROP CONSTRAINT [FK_RiskRegulatories_Risks];
GO
IF OBJECT_ID(N'[dbo].[FK_Risks_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Risks] DROP CONSTRAINT [FK_Risks_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_StockTransferDetails_StockTransfers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StockTransferDetails] DROP CONSTRAINT [FK_StockTransferDetails_StockTransfers];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierAuditPlans_QualityPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierAuditPlans] DROP CONSTRAINT [FK_SupplierAuditPlans_QualityPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierAuditResults_QualityCriterias]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierAuditResults] DROP CONSTRAINT [FK_SupplierAuditResults_QualityCriterias];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierAuditResults_SupplierAudits]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierAuditResults] DROP CONSTRAINT [FK_SupplierAuditResults_SupplierAudits];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierAudits_SupplierAuditPlans]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierAudits] DROP CONSTRAINT [FK_SupplierAudits_SupplierAuditPlans];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierAudits_Suppliers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierAudits] DROP CONSTRAINT [FK_SupplierAudits_Suppliers];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierBidToAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierBidToAttachmentFiles] DROP CONSTRAINT [FK_SupplierBidToAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierBidToAttachmentFiles_SuppliersBidOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierBidToAttachmentFiles] DROP CONSTRAINT [FK_SupplierBidToAttachmentFiles_SuppliersBidOrders];
GO
IF OBJECT_ID(N'[dbo].[FK_Suppliers_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_Suppliers_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_Suppliers_SuppliersGroups]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Suppliers] DROP CONSTRAINT [FK_Suppliers_SuppliersGroups];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersBidOrders_Suppliers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersBidOrders] DROP CONSTRAINT [FK_SuppliersBidOrders_Suppliers];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersBidOrders_SuppliersOrders]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersBidOrders] DROP CONSTRAINT [FK_SuppliersBidOrders_SuppliersOrders];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersBill_SuppliersOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersBills] DROP CONSTRAINT [FK_SuppliersBill_SuppliersOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersOrder_Suppliers]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersOrders] DROP CONSTRAINT [FK_SuppliersOrder_Suppliers];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersOrderDetail_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersOrderDetails] DROP CONSTRAINT [FK_SuppliersOrderDetail_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersOrderDetail_SuppliersOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersOrderDetails] DROP CONSTRAINT [FK_SuppliersOrderDetail_SuppliersOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersOrderRequirementDetails_StockProducts]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersOrderRequirementDetails] DROP CONSTRAINT [FK_SuppliersOrderRequirementDetails_StockProducts];
GO
IF OBJECT_ID(N'[dbo].[FK_SuppliersOrderRequirementDetails_SuppliersOrderRequirements]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SuppliersOrderRequirementDetails] DROP CONSTRAINT [FK_SuppliersOrderRequirementDetails_SuppliersOrderRequirements];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskAttachmentFiles] DROP CONSTRAINT [FK_TaskAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskAttachmentFiles_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskAttachmentFiles] DROP CONSTRAINT [FK_TaskAttachmentFiles_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskCalendars_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskCalendars] DROP CONSTRAINT [FK_TaskCalendars_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskCCs_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskCCs] DROP CONSTRAINT [FK_TaskCCs_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskCCs_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskCCs] DROP CONSTRAINT [FK_TaskCCs_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskComment_Tasks]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskComments] DROP CONSTRAINT [FK_TaskComment_Tasks];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskPerformAttachmentFiles_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPerformAttachmentFiles] DROP CONSTRAINT [FK_TaskPerformAttachmentFiles_AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskPerformAttachmentFiles_TaskPerforms]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskPerformAttachmentFiles] DROP CONSTRAINT [FK_TaskPerformAttachmentFiles_TaskPerforms];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskRequests_HumanDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskRequests] DROP CONSTRAINT [FK_TaskRequests_HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskRequests_HumanEmployees]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskRequests] DROP CONSTRAINT [FK_TaskRequests_HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[FK_TaskRequests_TaskLevels]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaskRequests] DROP CONSTRAINT [FK_TaskRequests_TaskLevels];
GO
IF OBJECT_ID(N'[dbo].[FK_TemplateExports_AttachmentFiles]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TemplateExports] DROP CONSTRAINT [FK_TemplateExports_AttachmentFiles];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[AccountingDebtTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingDebtTypes];
GO
IF OBJECT_ID(N'[dbo].[AccountingPaymentPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingPaymentPlans];
GO
IF OBJECT_ID(N'[dbo].[AccountingPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingPayments];
GO
IF OBJECT_ID(N'[dbo].[AccountingReasonSpendings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingReasonSpendings];
GO
IF OBJECT_ID(N'[dbo].[AccountingSpendingBillDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingSpendingBillDetails];
GO
IF OBJECT_ID(N'[dbo].[AccountingSpendingBills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingSpendingBills];
GO
IF OBJECT_ID(N'[dbo].[AccountingSuggestAdvances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AccountingSuggestAdvances];
GO
IF OBJECT_ID(N'[dbo].[AttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[AuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AuditResults];
GO
IF OBJECT_ID(N'[dbo].[Audits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Audits];
GO
IF OBJECT_ID(N'[dbo].[BusinessActions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessActions];
GO
IF OBJECT_ID(N'[dbo].[BusinessFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessFunctions];
GO
IF OBJECT_ID(N'[dbo].[BusinessModules]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BusinessModules];
GO
IF OBJECT_ID(N'[dbo].[CalendarCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarCategories];
GO
IF OBJECT_ID(N'[dbo].[CalendarOverrides]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarOverrides];
GO
IF OBJECT_ID(N'[dbo].[CalendarTimeOverrides]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CalendarTimeOverrides];
GO
IF OBJECT_ID(N'[dbo].[Chats]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Chats];
GO
IF OBJECT_ID(N'[dbo].[CommonCities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommonCities];
GO
IF OBJECT_ID(N'[dbo].[CommonCountries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CommonCountries];
GO
IF OBJECT_ID(N'[dbo].[CustomerAssesses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAssesses];
GO
IF OBJECT_ID(N'[dbo].[CustomerAssessObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAssessObjects];
GO
IF OBJECT_ID(N'[dbo].[CustomerAssessResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerAssessResults];
GO
IF OBJECT_ID(N'[dbo].[CustomerCampaignAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCampaignAudits];
GO
IF OBJECT_ID(N'[dbo].[CustomerCampaignPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCampaignPlans];
GO
IF OBJECT_ID(N'[dbo].[CustomerCampaigns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCampaigns];
GO
IF OBJECT_ID(N'[dbo].[CustomerCampaignTargets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCampaignTargets];
GO
IF OBJECT_ID(N'[dbo].[CustomerCareObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCareObjects];
GO
IF OBJECT_ID(N'[dbo].[CustomerCareResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCareResults];
GO
IF OBJECT_ID(N'[dbo].[CustomerCares]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCares];
GO
IF OBJECT_ID(N'[dbo].[CustomerCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCategories];
GO
IF OBJECT_ID(N'[dbo].[CustomerCategoryCustomers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCategoryCustomers];
GO
IF OBJECT_ID(N'[dbo].[CustomerCategoryDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCategoryDepartments];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactCalendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactCalendars];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactForms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactForms];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactHistories];
GO
IF OBJECT_ID(N'[dbo].[CustomerContactHistoryAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContactHistoryAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[CustomerContacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContacts];
GO
IF OBJECT_ID(N'[dbo].[CustomerContractAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContractAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[CustomerContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerContracts];
GO
IF OBJECT_ID(N'[dbo].[CustomerCriteriaCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[CustomerCriterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerCriterias];
GO
IF OBJECT_ID(N'[dbo].[CustomerFeedbacks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerFeedbacks];
GO
IF OBJECT_ID(N'[dbo].[CustomerOrderAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerOrderAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[CustomerOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerOrders];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[CustomerSurveyObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSurveyObjects];
GO
IF OBJECT_ID(N'[dbo].[CustomerSurveyQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSurveyQuestions];
GO
IF OBJECT_ID(N'[dbo].[CustomerSurveyResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSurveyResults];
GO
IF OBJECT_ID(N'[dbo].[CustomerSurveys]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerSurveys];
GO
IF OBJECT_ID(N'[dbo].[CustomerTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerTypes];
GO
IF OBJECT_ID(N'[dbo].[DepartmentBroadCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentBroadCategories];
GO
IF OBJECT_ID(N'[dbo].[DepartmentBroads]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentBroads];
GO
IF OBJECT_ID(N'[dbo].[DepartmentLegalAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentLegalAttachments];
GO
IF OBJECT_ID(N'[dbo].[DepartmentLegalAuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentLegalAuditResults];
GO
IF OBJECT_ID(N'[dbo].[DepartmentLegals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentLegals];
GO
IF OBJECT_ID(N'[dbo].[DepartmentPolicies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentPolicies];
GO
IF OBJECT_ID(N'[dbo].[DepartmentPolicyAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentPolicyAttachments];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRegulatories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRegulatories];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRegulatoryAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRegulatoryAttachments];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRegulatoryAuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRegulatoryAuditResults];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRequirmentAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRequirmentAttachments];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRequirmentAuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRequirmentAuditResults];
GO
IF OBJECT_ID(N'[dbo].[DepartmentRequirments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentRequirments];
GO
IF OBJECT_ID(N'[dbo].[DispatchCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchCategories];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoCCs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoCCs];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoDepartments];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoEmployees];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoes];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoOutSides]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoOutSides];
GO
IF OBJECT_ID(N'[dbo].[DispatchGoTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchGoTasks];
GO
IF OBJECT_ID(N'[dbo].[DispatchMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchMethods];
GO
IF OBJECT_ID(N'[dbo].[DispatchSecurities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchSecurities];
GO
IF OBJECT_ID(N'[dbo].[DispatchToAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[DispatchToCCs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToCCs];
GO
IF OBJECT_ID(N'[dbo].[DispatchToDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToDepartments];
GO
IF OBJECT_ID(N'[dbo].[DispatchToEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToEmployees];
GO
IF OBJECT_ID(N'[dbo].[DispatchToes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToes];
GO
IF OBJECT_ID(N'[dbo].[DispatchToTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchToTasks];
GO
IF OBJECT_ID(N'[dbo].[DispatchUrgencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DispatchUrgencies];
GO
IF OBJECT_ID(N'[dbo].[DocumentAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[DocumentAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentAttachments];
GO
IF OBJECT_ID(N'[dbo].[DocumentCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentCategories];
GO
IF OBJECT_ID(N'[dbo].[DocumentDistributions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentDistributions];
GO
IF OBJECT_ID(N'[dbo].[DocumentRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentRequirements];
GO
IF OBJECT_ID(N'[dbo].[Documents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Documents];
GO
IF OBJECT_ID(N'[dbo].[DocumentSecurities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentSecurities];
GO
IF OBJECT_ID(N'[dbo].[DocumentTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DocumentTasks];
GO
IF OBJECT_ID(N'[dbo].[EquipmentCalibrations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentCalibrations];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasureAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasureAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasureCalibrationResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasureCalibrationResults];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasureCalibrations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasureCalibrations];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasureCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasureCategories];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasureDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasureDepartments];
GO
IF OBJECT_ID(N'[dbo].[EquipmentMeasures]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentMeasures];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionAttaches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionAttaches];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionCategories];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionDepartments];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionErrors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionErrors];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionHistories];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductionMaintains]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductionMaintains];
GO
IF OBJECT_ID(N'[dbo].[EquipmentProductions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EquipmentProductions];
GO
IF OBJECT_ID(N'[dbo].[HumanAbsents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAbsents];
GO
IF OBJECT_ID(N'[dbo].[HumanAbsentTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAbsentTypes];
GO
IF OBJECT_ID(N'[dbo].[HumanAnswers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAnswers];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditCriteriaCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditCriteriaPoints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditCriteriaPoints];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditCriterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditCriterias];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditDepartments];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditGradationCriteriaPoints]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditGradationCriteriaPoints];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditGradationCriterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditGradationCriterias];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditGradationRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditGradationRoles];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditGradations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditGradations];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditLevels];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditTickResultBonusScores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditTickResultBonusScores];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditTickResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditTickResults];
GO
IF OBJECT_ID(N'[dbo].[HumanAuditTicks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanAuditTicks];
GO
IF OBJECT_ID(N'[dbo].[HumanCategoryQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanCategoryQuestions];
GO
IF OBJECT_ID(N'[dbo].[HumanDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanDepartments];
GO
IF OBJECT_ID(N'[dbo].[HumanEmployeeAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanEmployeeAudits];
GO
IF OBJECT_ID(N'[dbo].[HumanEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanEmployees];
GO
IF OBJECT_ID(N'[dbo].[HumanEmployeeTimeKeepings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanEmployeeTimeKeepings];
GO
IF OBJECT_ID(N'[dbo].[HumanOrganizations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanOrganizations];
GO
IF OBJECT_ID(N'[dbo].[HumanPermissions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanPermissions];
GO
IF OBJECT_ID(N'[dbo].[HumanPhaseAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanPhaseAudits];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileAssesses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileAssesses];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileAttachments];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileCertificates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileCertificates];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileContracts];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileCuriculmViates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileCuriculmViates];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileDiplomas]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileDiplomas];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileDisciplines]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileDisciplines];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileInsurances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileInsurances];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileRelationships]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileRelationships];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileRewards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileRewards];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileSalaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileSalaries];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileTrainings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileTrainings];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileWorkExperiences]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileWorkExperiences];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileWorkTrialResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileWorkTrialResults];
GO
IF OBJECT_ID(N'[dbo].[HumanProfileWorkTrials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanProfileWorkTrials];
GO
IF OBJECT_ID(N'[dbo].[HumanQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanQuestions];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentCriterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentCriterias];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentInterviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentInterviews];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentPlanInterviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentPlanInterviews];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentPlanRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentPlanRequirements];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentPlans];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentProfileInterviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentProfileInterviews];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentProfileResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentProfileResults];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentProfiles];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentRequirements];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentReviews]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentReviews];
GO
IF OBJECT_ID(N'[dbo].[HumanRecruitmentTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRecruitmentTasks];
GO
IF OBJECT_ID(N'[dbo].[HumanResultAnswers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanResultAnswers];
GO
IF OBJECT_ID(N'[dbo].[HumanResultQuestions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanResultQuestions];
GO
IF OBJECT_ID(N'[dbo].[HumanRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanRoles];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPlanAttachments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPlanAttachments];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPlanDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPlanDetails];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPlanRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPlanRequirements];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPlans];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPractionerRanks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPractionerRanks];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingPractioners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingPractioners];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingRequirementEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingRequirementEmployees];
GO
IF OBJECT_ID(N'[dbo].[HumanTrainingRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanTrainingRequirements];
GO
IF OBJECT_ID(N'[dbo].[HumanUsers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HumanUsers];
GO
IF OBJECT_ID(N'[dbo].[Notifies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Notifies];
GO
IF OBJECT_ID(N'[dbo].[ProductDevelopmentRequirementAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[ProductDevelopmentRequirementPlanDocuments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments];
GO
IF OBJECT_ID(N'[dbo].[ProductDevelopmentRequirementPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDevelopmentRequirementPlans];
GO
IF OBJECT_ID(N'[dbo].[ProductDevelopmentRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductDevelopmentRequirements];
GO
IF OBJECT_ID(N'[dbo].[ProductionCommands]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionCommands];
GO
IF OBJECT_ID(N'[dbo].[ProductionLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionLevels];
GO
IF OBJECT_ID(N'[dbo].[ProductionPerformHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPerformHistories];
GO
IF OBJECT_ID(N'[dbo].[ProductionPerformMaterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPerformMaterias];
GO
IF OBJECT_ID(N'[dbo].[ProductionPerformProductErrors]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPerformProductErrors];
GO
IF OBJECT_ID(N'[dbo].[ProductionPerformResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPerformResults];
GO
IF OBJECT_ID(N'[dbo].[ProductionPerforms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPerforms];
GO
IF OBJECT_ID(N'[dbo].[ProductionPlanEquipments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPlanEquipments];
GO
IF OBJECT_ID(N'[dbo].[ProductionPlanMaterials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPlanMaterials];
GO
IF OBJECT_ID(N'[dbo].[ProductionPlanProductDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPlanProductDetails];
GO
IF OBJECT_ID(N'[dbo].[ProductionPlanProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPlanProducts];
GO
IF OBJECT_ID(N'[dbo].[ProductionPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionPlans];
GO
IF OBJECT_ID(N'[dbo].[ProductionRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionRequirements];
GO
IF OBJECT_ID(N'[dbo].[ProductionShifts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionShifts];
GO
IF OBJECT_ID(N'[dbo].[ProductionStageCriteriaCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionStageCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[ProductionStageEquipments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionStageEquipments];
GO
IF OBJECT_ID(N'[dbo].[ProductionStageMaterials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionStageMaterials];
GO
IF OBJECT_ID(N'[dbo].[ProductionStageProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionStageProducts];
GO
IF OBJECT_ID(N'[dbo].[ProductionStages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductionStages];
GO
IF OBJECT_ID(N'[dbo].[ProfileAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[ProfileBorrowCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileBorrowCategories];
GO
IF OBJECT_ID(N'[dbo].[ProfileBorrows]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileBorrows];
GO
IF OBJECT_ID(N'[dbo].[ProfileCancelEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileCancelEmployees];
GO
IF OBJECT_ID(N'[dbo].[ProfileCancelMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileCancelMethods];
GO
IF OBJECT_ID(N'[dbo].[ProfileCancelProfiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileCancelProfiles];
GO
IF OBJECT_ID(N'[dbo].[ProfileCancels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileCancels];
GO
IF OBJECT_ID(N'[dbo].[ProfileCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileCategories];
GO
IF OBJECT_ID(N'[dbo].[Profiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Profiles];
GO
IF OBJECT_ID(N'[dbo].[ProfileSecurities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProfileSecurities];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditPlans];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditProgramDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditProgramDepartments];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditProgramEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditProgramEmployees];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditProgramISOes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditProgramISOes];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditPrograms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditPrograms];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditRecordedVotes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditRecordedVotes];
GO
IF OBJECT_ID(N'[dbo].[QualityAuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityAuditResults];
GO
IF OBJECT_ID(N'[dbo].[QualityCAPARequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityCAPARequirements];
GO
IF OBJECT_ID(N'[dbo].[QualityCAPAs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityCAPAs];
GO
IF OBJECT_ID(N'[dbo].[QualityCAPATasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityCAPATasks];
GO
IF OBJECT_ID(N'[dbo].[QualityCriteriaCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityCriteriaCategories];
GO
IF OBJECT_ID(N'[dbo].[QualityCriterias]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityCriterias];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingObjects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingObjects];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingProblems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingProblems];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingResultAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingResultAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingResults];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetings];
GO
IF OBJECT_ID(N'[dbo].[QualityMeetingTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityMeetingTasks];
GO
IF OBJECT_ID(N'[dbo].[QualityNCEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityNCEmployees];
GO
IF OBJECT_ID(N'[dbo].[QualityNCRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityNCRoles];
GO
IF OBJECT_ID(N'[dbo].[QualityNCs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityNCs];
GO
IF OBJECT_ID(N'[dbo].[QualityNCStockAdjustments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityNCStockAdjustments];
GO
IF OBJECT_ID(N'[dbo].[QualityNCTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityNCTasks];
GO
IF OBJECT_ID(N'[dbo].[QualityPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityPlans];
GO
IF OBJECT_ID(N'[dbo].[QualityPlanTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityPlanTasks];
GO
IF OBJECT_ID(N'[dbo].[QualityTargetCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityTargetCategories];
GO
IF OBJECT_ID(N'[dbo].[QualityTargetLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityTargetLevels];
GO
IF OBJECT_ID(N'[dbo].[QualityTargets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QualityTargets];
GO
IF OBJECT_ID(N'[dbo].[Reports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Reports];
GO
IF OBJECT_ID(N'[dbo].[ReportTemplates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReportTemplates];
GO
IF OBJECT_ID(N'[dbo].[RiskAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskAudits];
GO
IF OBJECT_ID(N'[dbo].[RiskContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskContracts];
GO
IF OBJECT_ID(N'[dbo].[RiskControls]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskControls];
GO
IF OBJECT_ID(N'[dbo].[RiskControlSolutions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskControlSolutions];
GO
IF OBJECT_ID(N'[dbo].[RiskControlTasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskControlTasks];
GO
IF OBJECT_ID(N'[dbo].[RiskIncedents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskIncedents];
GO
IF OBJECT_ID(N'[dbo].[RiskLegals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskLegals];
GO
IF OBJECT_ID(N'[dbo].[RiskLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskLevels];
GO
IF OBJECT_ID(N'[dbo].[RiskLibrarySolutions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskLibrarySolutions];
GO
IF OBJECT_ID(N'[dbo].[RiskRegulatories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RiskRegulatories];
GO
IF OBJECT_ID(N'[dbo].[Risks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Risks];
GO
IF OBJECT_ID(N'[dbo].[ServiceCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCategories];
GO
IF OBJECT_ID(N'[dbo].[ServiceCommandContracts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCommandContracts];
GO
IF OBJECT_ID(N'[dbo].[ServiceCommands]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceCommands];
GO
IF OBJECT_ID(N'[dbo].[ServicePlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServicePlans];
GO
IF OBJECT_ID(N'[dbo].[ServicePlanStages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServicePlanStages];
GO
IF OBJECT_ID(N'[dbo].[Services]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Services];
GO
IF OBJECT_ID(N'[dbo].[ServiceStages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ServiceStages];
GO
IF OBJECT_ID(N'[dbo].[StockAdjustmentDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockAdjustmentDetails];
GO
IF OBJECT_ID(N'[dbo].[StockAdjustments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockAdjustments];
GO
IF OBJECT_ID(N'[dbo].[StockBuildDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockBuildDetails];
GO
IF OBJECT_ID(N'[dbo].[StockBuilds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockBuilds];
GO
IF OBJECT_ID(N'[dbo].[StockInventories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockInventories];
GO
IF OBJECT_ID(N'[dbo].[StockInventoryDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockInventoryDetails];
GO
IF OBJECT_ID(N'[dbo].[StockInwardDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockInwardDetails];
GO
IF OBJECT_ID(N'[dbo].[StockInwards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockInwards];
GO
IF OBJECT_ID(N'[dbo].[StockListCurrencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockListCurrencies];
GO
IF OBJECT_ID(N'[dbo].[StockOutwardDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockOutwardDetails];
GO
IF OBJECT_ID(N'[dbo].[StockOutwards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockOutwards];
GO
IF OBJECT_ID(N'[dbo].[StockProductBuilds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProductBuilds];
GO
IF OBJECT_ID(N'[dbo].[StockProductGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProductGroups];
GO
IF OBJECT_ID(N'[dbo].[StockProductModels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProductModels];
GO
IF OBJECT_ID(N'[dbo].[StockProductPrices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProductPrices];
GO
IF OBJECT_ID(N'[dbo].[StockProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProducts];
GO
IF OBJECT_ID(N'[dbo].[StockProductTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockProductTypes];
GO
IF OBJECT_ID(N'[dbo].[Stocks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stocks];
GO
IF OBJECT_ID(N'[dbo].[StockTransferDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockTransferDetails];
GO
IF OBJECT_ID(N'[dbo].[StockTransferRefs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockTransferRefs];
GO
IF OBJECT_ID(N'[dbo].[StockTransfers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockTransfers];
GO
IF OBJECT_ID(N'[dbo].[StockUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StockUnits];
GO
IF OBJECT_ID(N'[dbo].[SupplierAuditPlans]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierAuditPlans];
GO
IF OBJECT_ID(N'[dbo].[SupplierAuditResults]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierAuditResults];
GO
IF OBJECT_ID(N'[dbo].[SupplierAudits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierAudits];
GO
IF OBJECT_ID(N'[dbo].[SupplierBidToAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierBidToAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[SuppliersBidOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersBidOrders];
GO
IF OBJECT_ID(N'[dbo].[SuppliersBills]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersBills];
GO
IF OBJECT_ID(N'[dbo].[SuppliersGroups]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersGroups];
GO
IF OBJECT_ID(N'[dbo].[SuppliersOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[SuppliersOrderRequirementDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersOrderRequirementDetails];
GO
IF OBJECT_ID(N'[dbo].[SuppliersOrderRequirements]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersOrderRequirements];
GO
IF OBJECT_ID(N'[dbo].[SuppliersOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SuppliersOrders];
GO
IF OBJECT_ID(N'[dbo].[TaskAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[TaskCalendarEvents]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskCalendarEvents];
GO
IF OBJECT_ID(N'[dbo].[TaskCalendars]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskCalendars];
GO
IF OBJECT_ID(N'[dbo].[TaskCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskCategories];
GO
IF OBJECT_ID(N'[dbo].[TaskCCs]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskCCs];
GO
IF OBJECT_ID(N'[dbo].[TaskChecks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskChecks];
GO
IF OBJECT_ID(N'[dbo].[TaskComments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskComments];
GO
IF OBJECT_ID(N'[dbo].[TaskHistories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskHistories];
GO
IF OBJECT_ID(N'[dbo].[TaskLevels]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskLevels];
GO
IF OBJECT_ID(N'[dbo].[TaskPerformAttachmentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskPerformAttachmentFiles];
GO
IF OBJECT_ID(N'[dbo].[TaskPerforms]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskPerforms];
GO
IF OBJECT_ID(N'[dbo].[TaskPersonals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskPersonals];
GO
IF OBJECT_ID(N'[dbo].[TaskRequests]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaskRequests];
GO
IF OBJECT_ID(N'[dbo].[Tasks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Tasks];
GO
IF OBJECT_ID(N'[dbo].[TemplateExportFields]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TemplateExportFields];
GO
IF OBJECT_ID(N'[dbo].[TemplateExportFieldStyles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TemplateExportFieldStyles];
GO
IF OBJECT_ID(N'[dbo].[TemplateExports]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TemplateExports];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'AccountingDebtTypes'
CREATE TABLE [dbo].[AccountingDebtTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Value] decimal(18,2)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Day] int  NOT NULL
);
GO

-- Creating table 'AccountingPaymentPlans'
CREATE TABLE [dbo].[AccountingPaymentPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [AccountingPaymentID] int  NOT NULL,
    [RatePlan] decimal(18,2)  NULL,
    [RateReal] decimal(18,2)  NULL,
    [ValuePlan] decimal(18,2)  NULL,
    [ValueReal] decimal(18,2)  NULL,
    [TimePlan] datetime  NULL,
    [TimeReal] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AccountingPayments'
CREATE TABLE [dbo].[AccountingPayments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerContractID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Rate] decimal(18,2)  NULL,
    [Value] decimal(18,2)  NULL,
    [IsCustomer] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'AccountingReasonSpendings'
CREATE TABLE [dbo].[AccountingReasonSpendings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsUse] bit  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AccountingSpendingBillDetails'
CREATE TABLE [dbo].[AccountingSpendingBillDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AccountingSpendingBillID] int  NOT NULL,
    [CreditAccount] decimal(18,0)  NOT NULL,
    [DebitAccount] decimal(18,0)  NOT NULL,
    [Account] decimal(18,0)  NOT NULL,
    [HumanEmployeeID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AccountingSpendingBills'
CREATE TABLE [dbo].[AccountingSpendingBills] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [RefDate] datetime  NULL,
    [AccountingSuggestAdvanceID] int  NULL,
    [HumanEmployeeID] int  NULL,
    [AccountingReasonSpendingID] int  NOT NULL,
    [Receiver] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AccountingSuggestAdvances'
CREATE TABLE [dbo].[AccountingSuggestAdvances] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Dear] nvarchar(max)  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Contact] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Amount] decimal(18,0)  NOT NULL,
    [Reason] nvarchar(max)  NOT NULL,
    [TimePayment] nvarchar(max)  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AttachmentFiles'
CREATE TABLE [dbo].[AttachmentFiles] (
    [ID] uniqueidentifier  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Extension] nvarchar(max)  NULL,
    [Size] float  NOT NULL,
    [Type] nvarchar(max)  NULL,
    [Data] varbinary(max)  NULL,
    [IsDeleted] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'AuditResults'
CREATE TABLE [dbo].[AuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AuditID] int  NOT NULL,
    [QualityNCID] int  NULL,
    [QualityCriteriaID] int  NOT NULL,
    [Point] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Audits'
CREATE TABLE [dbo].[Audits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityCriteriaCategoryID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Time] datetime  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'BusinessActions'
CREATE TABLE [dbo].[BusinessActions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SystemCode] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [ModuleCode] nvarchar(max)  NULL,
    [FunctionCode] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'BusinessFunctions'
CREATE TABLE [dbo].[BusinessFunctions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SystemCode] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [ModuleCode] nvarchar(max)  NULL,
    [ParentCode] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsShow] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsGroup] bit  NOT NULL,
    [Position] int  NOT NULL,
    [Url] nvarchar(max)  NULL,
    [Icon] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'BusinessModules'
CREATE TABLE [dbo].[BusinessModules] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SystemCode] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsShow] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Position] int  NOT NULL,
    [Icon] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CalendarCategories'
CREATE TABLE [dbo].[CalendarCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanDepartmentID] int  NULL,
    [Name] nvarchar(200)  NULL,
    [ParentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CalendarOverrides'
CREATE TABLE [dbo].[CalendarOverrides] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskCalendarEventID] int  NULL,
    [CalendarCategoryID] int  NOT NULL,
    [Title] nvarchar(max)  NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [IsDayOverrides] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CalendarTimeOverrides'
CREATE TABLE [dbo].[CalendarTimeOverrides] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CalendarOverrideID] int  NOT NULL,
    [Name] nvarchar(200)  NULL,
    [StartTime] nvarchar(max)  NOT NULL,
    [EndTime] nvarchar(max)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Chats'
CREATE TABLE [dbo].[Chats] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [IsRead] bit  NOT NULL,
    [ReadTime] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CommonCities'
CREATE TABLE [dbo].[CommonCities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [CountryCode] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CommonCountries'
CREATE TABLE [dbo].[CommonCountries] (
    [ID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerAssesses'
CREATE TABLE [dbo].[CustomerAssesses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AuditID] int  NULL,
    [CustomerCriteriaCategoryID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Method] nvarchar(max)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerAssessObjects'
CREATE TABLE [dbo].[CustomerAssessObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerAssessID] int  NOT NULL,
    [CustomerCategoryID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerAssessResults'
CREATE TABLE [dbo].[CustomerAssessResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerAssessID] int  NOT NULL,
    [CustomerCriteriaID] int  NOT NULL,
    [CustomerID] int  NOT NULL,
    [Point] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCampaignAudits'
CREATE TABLE [dbo].[CustomerCampaignAudits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AuditID] int  NOT NULL,
    [CustomerCampaignID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCampaignPlans'
CREATE TABLE [dbo].[CustomerCampaignPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [CustomerCampaignID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCampaigns'
CREATE TABLE [dbo].[CustomerCampaigns] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [IsSuccess] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Income] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerCampaignTargets'
CREATE TABLE [dbo].[CustomerCampaignTargets] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerCampaignID] int  NOT NULL,
    [QualityTargetID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCareObjects'
CREATE TABLE [dbo].[CustomerCareObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerCareID] int  NOT NULL,
    [CustomerCategoryID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCareResults'
CREATE TABLE [dbo].[CustomerCareResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [CustomerCareID] int  NOT NULL,
    [IsSuccess] bit  NOT NULL,
    [Method] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCares'
CREATE TABLE [dbo].[CustomerCares] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [Method] nvarchar(max)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerCategories'
CREATE TABLE [dbo].[CustomerCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ParentID] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerCategoryCustomers'
CREATE TABLE [dbo].[CustomerCategoryCustomers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [CustomerCategoryID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerCategoryDepartments'
CREATE TABLE [dbo].[CustomerCategoryDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerCategoryID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerContactCalendars'
CREATE TABLE [dbo].[CustomerContactCalendars] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [CustomerContactFormID] int  NOT NULL,
    [IsNew] bit  NOT NULL,
    [Time] datetime  NULL,
    [Content] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerContactForms'
CREATE TABLE [dbo].[CustomerContactForms] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [RequirementField] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerContactHistories'
CREATE TABLE [dbo].[CustomerContactHistories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [CustomerContactFormID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Requirment] nvarchar(max)  NOT NULL,
    [Cost] decimal(19,4)  NULL,
    [Content] nvarchar(max)  NULL,
    [IsSuccess] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsPotential] bit  NOT NULL,
    [IsOfficial] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerContactHistoryAttachmentFiles'
CREATE TABLE [dbo].[CustomerContactHistoryAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CustomerContactHistoryID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerContacts'
CREATE TABLE [dbo].[CustomerContacts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [Name] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Sex] bit  NOT NULL,
    [Phone] nvarchar(max)  NULL,
    [Birthday] datetime  NULL,
    [IsDelete] bit  NOT NULL,
    [Role] nvarchar(max)  NULL,
    [Department] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerContractAttachmentFiles'
CREATE TABLE [dbo].[CustomerContractAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CustomerContractID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerContracts'
CREATE TABLE [dbo].[CustomerContracts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NULL,
    [CustomerID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Total] decimal(18,2)  NULL,
    [StatTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [IsSuccess] bit  NOT NULL,
    [IsSignCustomer] bit  NOT NULL,
    [SignCustomerAt] datetime  NULL,
    [IsSignReview] bit  NOT NULL,
    [IsSignCompany] bit  NOT NULL,
    [SignCompanyAt] datetime  NULL,
    [IsSend] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [FinishDate] datetime  NOT NULL,
    [CompleteDate] datetime  NULL,
    [IsCancel] bit  NOT NULL,
    [CancelNote] nvarchar(max)  NULL,
    [IsStart] bit  NOT NULL,
    [StartRealTime] datetime  NULL
);
GO

-- Creating table 'CustomerCriteriaCategories'
CREATE TABLE [dbo].[CustomerCriteriaCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsTerm] bit  NOT NULL,
    [ParentID] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerCriterias'
CREATE TABLE [dbo].[CustomerCriterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerCriteriaCategoryID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [MinPoint] int  NOT NULL,
    [MaxPoint] int  NOT NULL,
    [Factory] decimal(18,2)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerFeedbacks'
CREATE TABLE [dbo].[CustomerFeedbacks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerOrderAttachmentFiles'
CREATE TABLE [dbo].[CustomerOrderAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CustomerOrderID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerOrders'
CREATE TABLE [dbo].[CustomerOrders] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [ServiceID] int  NOT NULL,
    [CustomerContractID] int  NULL,
    [Quantity] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [IsPrice] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [IsSuccess] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerTypeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [TaxCode] nvarchar(max)  NULL,
    [RepresentName] nvarchar(max)  NULL,
    [RepresentRole] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [IsNew] bit  NOT NULL,
    [IsPotential] bit  NOT NULL,
    [IsOfficial] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsPotentialView] bit  NOT NULL,
    [IsNotContract] bit  NOT NULL,
    [TimeNormal] datetime  NULL,
    [TimePotential] datetime  NULL,
    [TimeOfficial] datetime  NULL,
    [RequireContent] nvarchar(max)  NULL,
    [RequireTime] datetime  NULL,
    [EstablishDate] datetime  NULL,
    [Scope] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [Note] nvarchar(max)  NULL,
    [EditFields] nvarchar(max)  NULL,
    [SuccessRate] int  NULL,
    [ReasonNotContract] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerSurveyObjects'
CREATE TABLE [dbo].[CustomerSurveyObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerSurveyID] int  NOT NULL,
    [CustomerCategoryID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerSurveyQuestions'
CREATE TABLE [dbo].[CustomerSurveyQuestions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerSurveyID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsCategory] bit  NOT NULL,
    [IsMulti] bit  NOT NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ParentID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerSurveyResults'
CREATE TABLE [dbo].[CustomerSurveyResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerID] int  NOT NULL,
    [CustomerSurveyQuestionID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'CustomerSurveys'
CREATE TABLE [dbo].[CustomerSurveys] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [Method] nvarchar(max)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'CustomerTypes'
CREATE TABLE [dbo].[CustomerTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentBroadCategories'
CREATE TABLE [dbo].[DepartmentBroadCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [ParentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentBroads'
CREATE TABLE [dbo].[DepartmentBroads] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentBroadCategoryID] int  NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentLegalAttachments'
CREATE TABLE [dbo].[DepartmentLegalAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [DepartmentLegalID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentLegalAuditResults'
CREATE TABLE [dbo].[DepartmentLegalAuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityNCID] int  NULL,
    [HumanDepartmentID] int  NOT NULL,
    [DepartmentLegalID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Evidence] nvarchar(max)  NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentLegals'
CREATE TABLE [dbo].[DepartmentLegals] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ISOID] int  NULL,
    [Description] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [ListApplyDepartment] nvarchar(max)  NULL,
    [IsApplyAll] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentPolicies'
CREATE TABLE [dbo].[DepartmentPolicies] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ISOID] int  NULL,
    [Description] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [Scope] nvarchar(max)  NULL,
    [Object] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentPolicyAttachments'
CREATE TABLE [dbo].[DepartmentPolicyAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [DepartmentPolicyID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRegulatories'
CREATE TABLE [dbo].[DepartmentRegulatories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ISOID] int  NULL,
    [Description] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [Scope] nvarchar(max)  NULL,
    [Object] nvarchar(max)  NULL,
    [ListApplyDepartment] nvarchar(max)  NULL,
    [IsApplyAll] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRegulatoryAttachments'
CREATE TABLE [dbo].[DepartmentRegulatoryAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [DepartmentRegulatoryID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRegulatoryAuditResults'
CREATE TABLE [dbo].[DepartmentRegulatoryAuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityNCID] int  NULL,
    [HumanDepartmentID] int  NOT NULL,
    [DepartmentRegulatoryID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Evidence] nvarchar(max)  NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRequirmentAttachments'
CREATE TABLE [dbo].[DepartmentRequirmentAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [DepartmentRequirmentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRequirmentAuditResults'
CREATE TABLE [dbo].[DepartmentRequirmentAuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityNCID] int  NULL,
    [HumanDepartmentID] int  NOT NULL,
    [DepartmentRequirmentID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Evidence] nvarchar(max)  NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DepartmentRequirments'
CREATE TABLE [dbo].[DepartmentRequirments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ISOID] int  NULL,
    [Description] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [Scope] nvarchar(max)  NULL,
    [Object] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchCategories'
CREATE TABLE [dbo].[DispatchCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoAttachmentFiles'
CREATE TABLE [dbo].[DispatchGoAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoCCs'
CREATE TABLE [dbo].[DispatchGoCCs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL
);
GO

-- Creating table 'DispatchGoDepartments'
CREATE TABLE [dbo].[DispatchGoDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [SendBy] int  NULL,
    [IsVerify] bit  NOT NULL,
    [VerifyAt] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoEmployees'
CREATE TABLE [dbo].[DispatchGoEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [SendBy] int  NULL,
    [IsVerify] bit  NOT NULL,
    [VerifyAt] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoes'
CREATE TABLE [dbo].[DispatchGoes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchCategoryID] int  NULL,
    [DispatchSecurityID] int  NULL,
    [DispatchUrgencyID] int  NULL,
    [DispatchMethodID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Compendia] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Type] bit  NOT NULL,
    [FormHard] bit  NOT NULL,
    [FormSoft] bit  NOT NULL,
    [DestinationAddress] nvarchar(max)  NULL,
    [IsEdit] bit  NOT NULL,
    [AlowNotAproval] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [IsMove] bit  NOT NULL,
    [IsVerify] bit  NOT NULL,
    [MoveAt] datetime  NULL,
    [NoteMove] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoOutSides'
CREATE TABLE [dbo].[DispatchGoOutSides] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Position] nvarchar(max)  NULL,
    [Company] nvarchar(max)  NULL,
    [Type] bit  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [Date] datetime  NULL,
    [Telephone] nvarchar(max)  NULL,
    [PersonEmail] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [CompanyEmail] nvarchar(max)  NULL,
    [IsVerify] bit  NOT NULL,
    [DateVerify] datetime  NULL,
    [NoteVerify] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchGoTasks'
CREATE TABLE [dbo].[DispatchGoTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchGoID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DispatchMethods'
CREATE TABLE [dbo].[DispatchMethods] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DispatchSecurities'
CREATE TABLE [dbo].[DispatchSecurities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DispatchToAttachmentFiles'
CREATE TABLE [dbo].[DispatchToAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchToID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DispatchToCCs'
CREATE TABLE [dbo].[DispatchToCCs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchToID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL
);
GO

-- Creating table 'DispatchToDepartments'
CREATE TABLE [dbo].[DispatchToDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchToID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [SendBy] int  NULL,
    [IsVerify] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchToEmployees'
CREATE TABLE [dbo].[DispatchToEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchToID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [SendBy] int  NULL,
    [IsVerify] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchToes'
CREATE TABLE [dbo].[DispatchToes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchCategoryID] int  NULL,
    [DispatchSecurityID] int  NULL,
    [DispatchUrgencyID] int  NULL,
    [DispatchMethodID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [Compendia] nvarchar(max)  NULL,
    [NumberTo] nvarchar(max)  NULL,
    [FormHard] bit  NOT NULL,
    [FormSoft] bit  NOT NULL,
    [Date] datetime  NOT NULL,
    [SendFrom] nvarchar(max)  NULL,
    [SendTo] nvarchar(max)  NULL,
    [IsMoved] bit  NOT NULL,
    [DateMoved] datetime  NULL,
    [IsVerify] bit  NOT NULL,
    [DateVerifyTime] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [NoteMove] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DispatchToTasks'
CREATE TABLE [dbo].[DispatchToTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DispatchToID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DispatchUrgencies'
CREATE TABLE [dbo].[DispatchUrgencies] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DocumentAttachmentFiles'
CREATE TABLE [dbo].[DocumentAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [DocumentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DocumentAttachments'
CREATE TABLE [dbo].[DocumentAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DocumentID] int  NOT NULL,
    [AttachmentID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DocumentCategories'
CREATE TABLE [dbo].[DocumentCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DocumentDistributions'
CREATE TABLE [dbo].[DocumentDistributions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DocumentID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Number] int  NOT NULL,
    [FormH] bit  NOT NULL,
    [FormS] bit  NOT NULL,
    [NumberObsolete] int  NULL,
    [DateObsolote] datetime  NULL,
    [FormHO] bit  NULL,
    [FormSO] bit  NULL,
    [Note] nvarchar(max)  NULL,
    [NoteObsolete] nvarchar(max)  NULL,
    [IssuedBy] int  NULL,
    [ObsoleteBy] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DocumentRequirements'
CREATE TABLE [dbo].[DocumentRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DocumentID] int  NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Type] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ContentOld] nvarchar(max)  NULL,
    [ContentChange] nvarchar(max)  NULL,
    [ReasonChange] nvarchar(max)  NULL,
    [FinishDate] datetime  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Documents'
CREATE TABLE [dbo].[Documents] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DocumentCategoryID] int  NULL,
    [DocumentSecurityID] int  NOT NULL,
    [DepartmentID] int  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Version] nvarchar(max)  NULL,
    [Type] bit  NOT NULL,
    [FormH] bit  NOT NULL,
    [FormS] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsPublic] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsObsolete] bit  NOT NULL,
    [DateObsolete] datetime  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [PublicDate] datetime  NULL,
    [PublicNumber] int  NULL,
    [ParentID] int  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [NoteIssued] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'DocumentSecurities'
CREATE TABLE [dbo].[DocumentSecurities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'DocumentTasks'
CREATE TABLE [dbo].[DocumentTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DocumentRequirementID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentCalibrations'
CREATE TABLE [dbo].[EquipmentCalibrations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentMeasureID] int  NOT NULL,
    [AccreditationContent] nvarchar(max)  NULL,
    [AccreditationPeriodic] int  NULL,
    [AccreditationLastTime] datetime  NULL,
    [AccreditationNextTime] datetime  NULL,
    [AccreditationBy] nvarchar(max)  NULL,
    [IsPass] bit  NOT NULL,
    [Time] bit  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentMeasureAttachmentFiles'
CREATE TABLE [dbo].[EquipmentMeasureAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [EquipmentMeasureID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'EquipmentMeasureCalibrationResults'
CREATE TABLE [dbo].[EquipmentMeasureCalibrationResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentMeasureCalibrationID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Time] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'EquipmentMeasureCalibrations'
CREATE TABLE [dbo].[EquipmentMeasureCalibrations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentMeasureID] int  NOT NULL,
    [EquipmentCalibrationID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Time] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentMeasureCategories'
CREATE TABLE [dbo].[EquipmentMeasureCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [ParentID] int  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentMeasureDepartments'
CREATE TABLE [dbo].[EquipmentMeasureDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentMeasureID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [IsUsing] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'EquipmentMeasures'
CREATE TABLE [dbo].[EquipmentMeasures] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentMeasureCategoryID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [MadeIn] nvarchar(max)  NULL,
    [MadeYear] datetime  NULL,
    [UseStartTime] datetime  NULL,
    [Specifications] nvarchar(max)  NULL,
    [Features] nvarchar(max)  NULL,
    [SupportInfomation] nvarchar(max)  NULL,
    [ScopeStart] real  NULL,
    [ScopeEnd] real  NULL,
    [ScopeUnit] nvarchar(max)  NULL,
    [Accuracy] real  NULL,
    [CalibrationPeriodic] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsUsing] bit  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsFixed] bit  NOT NULL,
    [IsCalibration] bit  NOT NULL,
    [IsAccreditation] bit  NOT NULL,
    [CalibrationLastTime] datetime  NULL,
    [CalibrationNextTime] datetime  NULL,
    [ExprireTime] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductionAttaches'
CREATE TABLE [dbo].[EquipmentProductionAttaches] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [MadeIn] nvarchar(max)  NULL,
    [MadeYear] datetime  NULL,
    [Specifications] nvarchar(max)  NULL,
    [Features] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsUsing] bit  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsFixed] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductionAttachmentFiles'
CREATE TABLE [dbo].[EquipmentProductionAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'EquipmentProductionCategories'
CREATE TABLE [dbo].[EquipmentProductionCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [ParentID] int  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductionDepartments'
CREATE TABLE [dbo].[EquipmentProductionDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [IsUsing] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'EquipmentProductionErrors'
CREATE TABLE [dbo].[EquipmentProductionErrors] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [HumanDepartmentID] int  NULL,
    [HumanEmployeeID] int  NULL,
    [Time] datetime  NULL,
    [IsNew] bit  NOT NULL,
    [IsFixed] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Cause] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [Problem] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductionHistories'
CREATE TABLE [dbo].[EquipmentProductionHistories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [HumanDepartmentID] int  NULL,
    [HumanEmployeeID] int  NULL,
    [Time] datetime  NOT NULL,
    [IsUsing] bit  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsFixed] bit  NOT NULL,
    [IsMaintain] bit  NOT NULL,
    [IsCalibration] bit  NOT NULL,
    [IsAccreditation] bit  NOT NULL,
    [IsNotUse] bit  NOT NULL,
    [Note] nvarchar(max)  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductionMaintains'
CREATE TABLE [dbo].[EquipmentProductionMaintains] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [HumanDepartmentID] int  NULL,
    [PlanTime] datetime  NULL,
    [RealTime] datetime  NULL,
    [IsComplete] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'EquipmentProductions'
CREATE TABLE [dbo].[EquipmentProductions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EquipmentProductionCategoryID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [MadeIn] nvarchar(max)  NULL,
    [MadeYear] datetime  NULL,
    [UseStartTime] datetime  NULL,
    [Specifications] nvarchar(max)  NULL,
    [MaintainContent] nvarchar(max)  NULL,
    [MaintainOther] nvarchar(max)  NULL,
    [MaintainPeriodic] int  NULL,
    [SupportInfomation] nvarchar(max)  NULL,
    [Features] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsUsing] bit  NOT NULL,
    [IsError] bit  NOT NULL,
    [IsFixed] bit  NOT NULL,
    [IsMaintain] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanAbsents'
CREATE TABLE [dbo].[HumanAbsents] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [HumantAbsentTypeID] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [DayNumber] int  NULL,
    [Contents] nvarchar(500)  NULL,
    [Note] nvarchar(500)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [IsApproval] bit  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [IsEdit] bit  NULL,
    [IsAccept] bit  NULL,
    [HaftDay] bit  NULL
);
GO

-- Creating table 'HumanAbsentTypes'
CREATE TABLE [dbo].[HumanAbsentTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(50)  NOT NULL,
    [Name] nvarchar(100)  NOT NULL,
    [IsPayed] float  NOT NULL,
    [MaxDayAbsent] int  NOT NULL,
    [Note] nchar(10)  NULL
);
GO

-- Creating table 'HumanAnswers'
CREATE TABLE [dbo].[HumanAnswers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanQuestionID] int  NULL,
    [Answer] nvarchar(max)  NULL,
    [IsTrue] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditCriteriaCategories'
CREATE TABLE [dbo].[HumanAuditCriteriaCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditCriteriaPoints'
CREATE TABLE [dbo].[HumanAuditCriteriaPoints] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditCriteriaID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NOT NULL,
    [Point] decimal(18,0)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditCriterias'
CREATE TABLE [dbo].[HumanAuditCriterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditCriteriaCategoryID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [Factory] int  NOT NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditDepartments'
CREATE TABLE [dbo].[HumanAuditDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditGradationID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [EmployeeAuditManagementID] int  NOT NULL,
    [EmployeeAuditLeaderID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditGradationCriteriaPoints'
CREATE TABLE [dbo].[HumanAuditGradationCriteriaPoints] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditGradationCriteriaID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NOT NULL,
    [Point] decimal(18,0)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditGradationCriterias'
CREATE TABLE [dbo].[HumanAuditGradationCriterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditGradationRoleID] int  NOT NULL,
    [HumanAuditCriteriaCategoryName] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [Factory] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditGradationRoles'
CREATE TABLE [dbo].[HumanAuditGradationRoles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditDepartmentID] int  NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [Factory] int  NOT NULL,
    [IsAudit] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditGradations'
CREATE TABLE [dbo].[HumanAuditGradations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsPerform] bit  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditLevels'
CREATE TABLE [dbo].[HumanAuditLevels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditGradationRoleID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [MinPoint] int  NULL,
    [MaxPoint] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditTickResultBonusScores'
CREATE TABLE [dbo].[HumanAuditTickResultBonusScores] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditTickID] int  NOT NULL,
    [Point] int  NOT NULL,
    [Note] nvarchar(500)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditTickResults'
CREATE TABLE [dbo].[HumanAuditTickResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditTickID] int  NOT NULL,
    [AuditEmployeeResult] int  NULL,
    [AuditManagementResult] int  NULL,
    [AuditLeaderResult] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanAuditTicks'
CREATE TABLE [dbo].[HumanAuditTicks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanAuditGradationRoleID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [IsEmployeeAuditted] bit  NOT NULL,
    [Time] datetime  NULL,
    [EmployeeAuditResult] decimal(18,2)  NOT NULL,
    [EmployeeAuditManagementResult] decimal(18,2)  NOT NULL,
    [EmployeeAuditLeaderResult] decimal(18,2)  NOT NULL,
    [IsManagementAuditted] bit  NOT NULL,
    [IsLeaderAuditted] bit  NOT NULL,
    [HumanAuditLevelID] int  NULL,
    [Comments] nvarchar(max)  NULL,
    [IsSave] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanCategoryQuestions'
CREATE TABLE [dbo].[HumanCategoryQuestions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanDepartments'
CREATE TABLE [dbo].[HumanDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [NameE] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Tax] nvarchar(max)  NULL,
    [Website] nvarchar(max)  NULL,
    [EstablishedDate] datetime  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsCancel] bit  NOT NULL,
    [IsMerge] bit  NOT NULL,
    [IsDestroy] bit  NOT NULL,
    [Position] int  NULL,
    [History] nvarchar(max)  NULL,
    [Resposibility] nvarchar(max)  NULL,
    [Authorize] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanEmployeeAudits'
CREATE TABLE [dbo].[HumanEmployeeAudits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanPhaseAuditID] int  NOT NULL,
    [HumanEmployeeID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanEmployees'
CREATE TABLE [dbo].[HumanEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Avatar] nvarchar(max)  NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Birthday] datetime  NULL,
    [WeddingStatus] nvarchar(max)  NULL,
    [Gender] bit  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsTrial] bit  NOT NULL,
    [IsLeave] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanEmployeeTimeKeepings'
CREATE TABLE [dbo].[HumanEmployeeTimeKeepings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanOrganizations'
CREATE TABLE [dbo].[HumanOrganizations] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanPermissions'
CREATE TABLE [dbo].[HumanPermissions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [ModuleCode] nvarchar(max)  NOT NULL,
    [FunctionCode] nvarchar(max)  NOT NULL,
    [ActionCode] nvarchar(max)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanPhaseAudits'
CREATE TABLE [dbo].[HumanPhaseAudits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanCategoryQuestionID] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [StartDate] datetime  NOT NULL,
    [EndDate] datetime  NOT NULL,
    [Contents] nvarchar(max)  NULL,
    [NumberSendInDay] int  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanProfileAssesses'
CREATE TABLE [dbo].[HumanProfileAssesses] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Score] nvarchar(max)  NULL,
    [Result] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileAttachments'
CREATE TABLE [dbo].[HumanProfileAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [Name] nvarchar(max)  NULL,
    [Size] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileCertificates'
CREATE TABLE [dbo].[HumanProfileCertificates] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Type] nvarchar(max)  NULL,
    [Level] nvarchar(max)  NULL,
    [PlaceOfTraining] nvarchar(max)  NULL,
    [DateIssuance] datetime  NULL,
    [DateExpiration] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileContracts'
CREATE TABLE [dbo].[HumanProfileContracts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [NumberOfContracts] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Type] nvarchar(max)  NULL,
    [Condition] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileCuriculmViates'
CREATE TABLE [dbo].[HumanProfileCuriculmViates] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Aliases] nvarchar(max)  NULL,
    [Nationality] nvarchar(max)  NULL,
    [People] nvarchar(max)  NULL,
    [Religion] nvarchar(max)  NULL,
    [PlaceOfBirth] nvarchar(max)  NULL,
    [OfficePhone] nvarchar(max)  NULL,
    [HomePhone] nvarchar(max)  NULL,
    [NumberOfIdentityCard] nvarchar(max)  NULL,
    [DateIssueOfIdentityCard] datetime  NULL,
    [PlaceIssueOfIdentityCard] nvarchar(max)  NULL,
    [DateOnGroup] datetime  NULL,
    [PositionGroup] nvarchar(max)  NULL,
    [PlaceOfLoadedGroup] nvarchar(max)  NULL,
    [DateJoinRevolution] datetime  NULL,
    [DateAtParty] datetime  NULL,
    [DateOfJoinParty] datetime  NULL,
    [PlaceOfLoadedParty] nvarchar(max)  NULL,
    [PosititonParty] nvarchar(max)  NULL,
    [NumberOfPartyCard] nvarchar(max)  NULL,
    [DateOnArmy] datetime  NULL,
    [PositionArmy] nvarchar(max)  NULL,
    [ArmyRank] nvarchar(max)  NULL,
    [Likes] nvarchar(max)  NULL,
    [Forte] nvarchar(max)  NULL,
    [Defect] nvarchar(max)  NULL,
    [TaxCode] nvarchar(max)  NULL,
    [NumberOfBankAccounts] nvarchar(max)  NULL,
    [Bank] nvarchar(max)  NULL,
    [NumberOfPassport] nvarchar(max)  NULL,
    [PlaceOfPassport] nvarchar(max)  NULL,
    [DateOfIssuePassport] datetime  NULL,
    [PassportExpirationDate] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileDiplomas'
CREATE TABLE [dbo].[HumanProfileDiplomas] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Faculty] nvarchar(max)  NULL,
    [Major] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Level] nvarchar(max)  NULL,
    [FormOfTrainning] nvarchar(max)  NULL,
    [Type] nvarchar(max)  NULL,
    [Rank] nvarchar(max)  NULL,
    [Place] nvarchar(max)  NULL,
    [Condition] nvarchar(max)  NULL,
    [DateOfGraduation] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileDisciplines'
CREATE TABLE [dbo].[HumanProfileDisciplines] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [NumberOfDecision] nvarchar(max)  NULL,
    [DateOfDecision] datetime  NULL,
    [Reason] nvarchar(max)  NULL,
    [Form] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileInsurances'
CREATE TABLE [dbo].[HumanProfileInsurances] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Number] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Type] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [InSuranceNorms] decimal(18,0)  NULL
);
GO

-- Creating table 'HumanProfileRelationships'
CREATE TABLE [dbo].[HumanProfileRelationships] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Age] smallint  NULL,
    [IsMale] bit  NULL,
    [Relationship] nvarchar(max)  NULL,
    [Job] nvarchar(max)  NULL,
    [PlaceOfJob] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [Phone] nvarchar(50)  NULL,
    [Adress] nvarchar(500)  NULL
);
GO

-- Creating table 'HumanProfileRewards'
CREATE TABLE [dbo].[HumanProfileRewards] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [NumberOfDecision] nvarchar(max)  NULL,
    [DateOfDecision] datetime  NULL,
    [Reason] nvarchar(max)  NULL,
    [Form] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileSalaries'
CREATE TABLE [dbo].[HumanProfileSalaries] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Level] nvarchar(max)  NULL,
    [Wage] nvarchar(max)  NULL,
    [DateOfApp] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'HumanProfileTrainings'
CREATE TABLE [dbo].[HumanProfileTrainings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Form] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Certificate] nvarchar(max)  NULL,
    [Result] nvarchar(max)  NULL,
    [Reviews] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileWorkExperiences'
CREATE TABLE [dbo].[HumanProfileWorkExperiences] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Position] nvarchar(max)  NULL,
    [JobDescription] nvarchar(max)  NULL,
    [Department] nvarchar(max)  NULL,
    [PlaceOfWork] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanProfileWorkTrialResults'
CREATE TABLE [dbo].[HumanProfileWorkTrialResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanProfileWorkTrialID] int  NOT NULL,
    [QualityCriteriaID] int  NOT NULL,
    [EmployeePoint] int  NULL,
    [ManagerPoint] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Note] nvarchar(500)  NULL
);
GO

-- Creating table 'HumanProfileWorkTrials'
CREATE TABLE [dbo].[HumanProfileWorkTrials] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [HumanRoleID] int  NULL,
    [FromDate] datetime  NULL,
    [ToDate] datetime  NULL,
    [Note] nvarchar(500)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(500)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [DirectorApproval] int  NULL,
    [DirectorApprovalAt] datetime  NULL,
    [ContractType] int  NULL,
    [ContractStartTime] datetime  NULL,
    [ManagerID] int  NULL
);
GO

-- Creating table 'HumanQuestions'
CREATE TABLE [dbo].[HumanQuestions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanCategoryQuestionID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRecruitmentCriterias'
CREATE TABLE [dbo].[HumanRecruitmentCriterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [MinPoint] int  NOT NULL,
    [MaxPoint] int  NOT NULL,
    [Factor] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanRecruitmentInterviews'
CREATE TABLE [dbo].[HumanRecruitmentInterviews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentProfileInterviewID] int  NOT NULL,
    [HumanRecruitmentPlanInterviewID] int  NOT NULL,
    [Result] nvarchar(max)  NULL,
    [Time] datetime  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRecruitmentPlanInterviews'
CREATE TABLE [dbo].[HumanRecruitmentPlanInterviews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentPlanID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanRecruitmentPlanRequirements'
CREATE TABLE [dbo].[HumanRecruitmentPlanRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentRequirementID] int  NOT NULL,
    [HumanRecruitmentPlanID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanRecruitmentPlans'
CREATE TABLE [dbo].[HumanRecruitmentPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [TotalCost] decimal(18,2)  NULL,
    [Content] nvarchar(max)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanRecruitmentProfileInterviews'
CREATE TABLE [dbo].[HumanRecruitmentProfileInterviews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentProfileID] int  NOT NULL,
    [HumanRecruitmentPlanID] int  NOT NULL,
    [HumanRecruitmentRequirementID] int  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRecruitmentProfileResults'
CREATE TABLE [dbo].[HumanRecruitmentProfileResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentProfileInterviewID] int  NOT NULL,
    [TotalPoint] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Salary] decimal(18,2)  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsPass] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRecruitmentProfiles'
CREATE TABLE [dbo].[HumanRecruitmentProfiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Avatar] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Birthday] datetime  NULL,
    [Gender] bit  NOT NULL,
    [Nationality] nvarchar(max)  NULL,
    [People] nvarchar(max)  NULL,
    [Religion] nvarchar(max)  NULL,
    [PlaceOfBirth] nvarchar(max)  NULL,
    [NumberOfIdentityCard] nvarchar(max)  NULL,
    [DateIssueOfIdentityCard] datetime  NULL,
    [PlaceIssueOfIdentityCard] nvarchar(max)  NULL,
    [HomePhone] nvarchar(max)  NULL,
    [PlaceOfTranning] nvarchar(max)  NULL,
    [Specicalization] nvarchar(max)  NULL,
    [LevelOfComputerization] nvarchar(max)  NULL,
    [ForeignLanguage] nvarchar(max)  NULL,
    [Literacy] nvarchar(max)  NULL,
    [Qualifications] nvarchar(max)  NULL,
    [ListOfCertificates] nvarchar(max)  NULL,
    [Experience] nvarchar(max)  NULL,
    [Salary] decimal(18,2)  NULL,
    [YearsOfExperience] smallint  NULL,
    [IsEmployee] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [EmployeeID] int  NOT NULL
);
GO

-- Creating table 'HumanRecruitmentRequirements'
CREATE TABLE [dbo].[HumanRecruitmentRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Number] int  NOT NULL,
    [DateRequired] datetime  NOT NULL,
    [Form] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanRecruitmentReviews'
CREATE TABLE [dbo].[HumanRecruitmentReviews] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentProfileID] int  NOT NULL,
    [HumanRecruitmentCriteriaID] int  NOT NULL,
    [Point] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRecruitmentTasks'
CREATE TABLE [dbo].[HumanRecruitmentTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRecruitmentPlanID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanResultAnswers'
CREATE TABLE [dbo].[HumanResultAnswers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanResultQuestionID] int  NOT NULL,
    [HumanAnswerID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanResultQuestions'
CREATE TABLE [dbo].[HumanResultQuestions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeAuditID] int  NOT NULL,
    [HumanQuestionID] int  NOT NULL,
    [IsResult] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanRoles'
CREATE TABLE [dbo].[HumanRoles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsDestroy] bit  NOT NULL,
    [Position] int  NULL,
    [Resposibility] nvarchar(max)  NULL,
    [Authorize] nvarchar(max)  NULL,
    [Competence] nvarchar(max)  NULL,
    [ReportTo] nvarchar(max)  NULL,
    [ReplaceBy] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanTrainingPlanAttachments'
CREATE TABLE [dbo].[HumanTrainingPlanAttachments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanTrainingPlanID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Size] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [AttachmentFileID] uniqueidentifier  NULL
);
GO

-- Creating table 'HumanTrainingPlanDetails'
CREATE TABLE [dbo].[HumanTrainingPlanDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanTrainingPlanID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Number] int  NOT NULL,
    [Reason] nvarchar(max)  NULL,
    [Type] bit  NOT NULL,
    [ExpectedCost] decimal(18,2)  NULL,
    [Certificate] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsCommit] bit  NULL,
    [CommitContent] nvarchar(max)  NULL,
    [FromDate] datetime  NOT NULL,
    [ToDate] datetime  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [IsCancel] bit  NOT NULL,
    [ReasonCancel] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanTrainingPlanRequirements'
CREATE TABLE [dbo].[HumanTrainingPlanRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanTrainingRequirementID] int  NOT NULL,
    [HumanTrainingPlanID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanTrainingPlans'
CREATE TABLE [dbo].[HumanTrainingPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanTrainingPractionerRanks'
CREATE TABLE [dbo].[HumanTrainingPractionerRanks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RankName] nvarchar(200)  NOT NULL,
    [Descripson] nvarchar(500)  NULL,
    [IsUse] bit  NOT NULL,
    [UpdateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'HumanTrainingPractioners'
CREATE TABLE [dbo].[HumanTrainingPractioners] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Rank] int  NULL,
    [IsRegister] bit  NOT NULL,
    [IsAcceptCommit] bit  NOT NULL,
    [TimeRegister] datetime  NOT NULL,
    [IsJoin] bit  NOT NULL,
    [ResonUnJoin] nvarchar(max)  NULL,
    [NumberPresence] int  NULL,
    [NumberAbsence] int  NULL,
    [TotalPoint] decimal(18,2)  NULL,
    [CommentOfTeacher] nvarchar(max)  NULL,
    [IsInProfile] bit  NOT NULL,
    [UpdateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [HumanTrainingPlanDetailID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL
);
GO

-- Creating table 'HumanTrainingRequirementEmployees'
CREATE TABLE [dbo].[HumanTrainingRequirementEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanTrainingRequirementID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanTrainingRequirements'
CREATE TABLE [dbo].[HumanTrainingRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NOT NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [Content] nvarchar(max)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'HumanUsers'
CREATE TABLE [dbo].[HumanUsers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NULL,
    [Account] nvarchar(max)  NULL,
    [Password] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsLocked] bit  NOT NULL,
    [IsChangePass] bit  NOT NULL,
    [IsProtected] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Notifies'
CREATE TABLE [dbo].[Notifies] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [ModuleCode] nvarchar(max)  NULL,
    [FunctionName] nvarchar(max)  NULL,
    [Param] nvarchar(max)  NULL,
    [Title] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [IsRead] bit  NOT NULL,
    [ReadTime] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductDevelopmentRequirementAttachmentFiles'
CREATE TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [ProductDevelopmentRequirementID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductDevelopmentRequirementPlanDocuments'
CREATE TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductDevelopmentRequirementPlanID] int  NOT NULL,
    [DocumentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductDevelopmentRequirementPlans'
CREATE TABLE [dbo].[ProductDevelopmentRequirementPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [ProductDevelopmentRequirementID] int  NULL,
    [ProductionRequirementID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NOT NULL
);
GO

-- Creating table 'ProductDevelopmentRequirements'
CREATE TABLE [dbo].[ProductDevelopmentRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockProductID] int  NOT NULL,
    [DevelopmentFromProduct] int  NULL,
    [Reason] nvarchar(max)  NULL,
    [Contents] nvarchar(max)  NULL,
    [IsWork] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionCommands'
CREATE TABLE [dbo].[ProductionCommands] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [BatchCode] nvarchar(max)  NULL,
    [ProductionPlanID] int  NULL,
    [ProductionRequirementID] int  NULL,
    [ProductionStageID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [FinishTime] datetime  NULL,
    [IsStart] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionLevels'
CREATE TABLE [dbo].[ProductionLevels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPerformHistories'
CREATE TABLE [dbo].[ProductionPerformHistories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPerformID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [StockProductID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Error] nvarchar(max)  NULL,
    [Cause] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPerformMaterias'
CREATE TABLE [dbo].[ProductionPerformMaterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPerformID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [Quantity] real  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPerformProductErrors'
CREATE TABLE [dbo].[ProductionPerformProductErrors] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPerformID] int  NOT NULL,
    [StockProductID] int  NULL,
    [Quantity] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPerformResults'
CREATE TABLE [dbo].[ProductionPerformResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [ProductionPerformID] int  NOT NULL,
    [IsAbsent] bit  NOT NULL,
    [Quantity] int  NULL,
    [StockProductID] int  NULL,
    [MaterialNumber] real  NULL,
    [MaterialExitsNumber] real  NULL,
    [ProductionPerformProductResultID] int  NULL,
    [Cause] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPerforms'
CREATE TABLE [dbo].[ProductionPerforms] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [ProductionCommandID] int  NOT NULL,
    [ProductionShiftID] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPlanEquipments'
CREATE TABLE [dbo].[ProductionPlanEquipments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPlanID] int  NOT NULL,
    [EquipmentProductionID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPlanMaterials'
CREATE TABLE [dbo].[ProductionPlanMaterials] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPlanID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [Quantity] real  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPlanProductDetails'
CREATE TABLE [dbo].[ProductionPlanProductDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPlanProductID] int  NOT NULL,
    [Date] datetime  NULL,
    [HumanDepartmentID] int  NULL,
    [Quantity] int  NOT NULL,
    [CalculatorQuantity] int  NOT NULL,
    [Menday] real  NULL,
    [CalculatorMenday] real  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPlanProducts'
CREATE TABLE [dbo].[ProductionPlanProducts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionPlanID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [QuantityCalculator] real  NOT NULL,
    [Quantity] int  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionPlans'
CREATE TABLE [dbo].[ProductionPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionRequirementID] int  NOT NULL,
    [Quantity] real  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionRequirements'
CREATE TABLE [dbo].[ProductionRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [ProductionLevelID] int  NULL,
    [StockProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [FinishTime] datetime  NULL,
    [IsNew] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionShifts'
CREATE TABLE [dbo].[ProductionShifts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] time  NULL,
    [EndTime] time  NULL,
    [IsUse] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionStageCriteriaCategories'
CREATE TABLE [dbo].[ProductionStageCriteriaCategories] (
    [ID] int  NOT NULL,
    [ProductionStageID] int  NOT NULL,
    [QualityCriteriaCategoryID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionStageEquipments'
CREATE TABLE [dbo].[ProductionStageEquipments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionStageID] int  NOT NULL,
    [EquipmentProductionCategoryID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionStageMaterials'
CREATE TABLE [dbo].[ProductionStageMaterials] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionStageID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [Quantity] real  NOT NULL,
    [Unit] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionStageProducts'
CREATE TABLE [dbo].[ProductionStageProducts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProductionStageID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProductionStages'
CREATE TABLE [dbo].[ProductionStages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [StockProductID] int  NOT NULL,
    [ResultProductID] int  NOT NULL,
    [Quantity] int  NOT NULL,
    [ResultQuantity] real  NULL,
    [MenDay] real  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Position] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileAttachmentFiles'
CREATE TABLE [dbo].[ProfileAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProfileID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileBorrowCategories'
CREATE TABLE [dbo].[ProfileBorrowCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileBorrows'
CREATE TABLE [dbo].[ProfileBorrows] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProfileBorrowCategoryID] int  NOT NULL,
    [ProfileID] int  NOT NULL,
    [BorrowBy] int  NOT NULL,
    [BorrowAt] datetime  NOT NULL,
    [LimitAt] datetime  NOT NULL,
    [IsReturn] bit  NOT NULL,
    [ReturnAt] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileCancelEmployees'
CREATE TABLE [dbo].[ProfileCancelEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProfileCancelID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileCancelMethods'
CREATE TABLE [dbo].[ProfileCancelMethods] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileCancelProfiles'
CREATE TABLE [dbo].[ProfileCancelProfiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProfileCancelID] int  NOT NULL,
    [ProfileID] int  NOT NULL,
    [PageTotal] int  NOT NULL,
    [Reason] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileCancels'
CREATE TABLE [dbo].[ProfileCancels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [ProfileCancelMethodID] int  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsCanceled] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileCategories'
CREATE TABLE [dbo].[ProfileCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Profiles'
CREATE TABLE [dbo].[Profiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ProfileCategoryID] int  NOT NULL,
    [ProfileSecurityID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [FormH] bit  NOT NULL,
    [FormS] bit  NOT NULL,
    [ReceivedAt] datetime  NULL,
    [NotUseAt] datetime  NULL,
    [StoreTime] int  NULL,
    [StoreRoomTime] int  NULL,
    [StoreRoomPosition] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ProfileSecurities'
CREATE TABLE [dbo].[ProfileSecurities] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityAuditPlans'
CREATE TABLE [dbo].[QualityAuditPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [ISOID] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApporvalBy] int  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [Scope] nvarchar(max)  NULL,
    [Requirement] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsPass] bit  NOT NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditProgramDepartments'
CREATE TABLE [dbo].[QualityAuditProgramDepartments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityAuditProgramID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditProgramEmployees'
CREATE TABLE [dbo].[QualityAuditProgramEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityAuditProgramID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [IsCaptain] bit  NOT NULL,
    [IsAuditor] bit  NOT NULL,
    [IsAbsent] bit  NOT NULL,
    [AbsentReason] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditProgramISOes'
CREATE TABLE [dbo].[QualityAuditProgramISOes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityAuditProgramID] int  NOT NULL,
    [ISOIndexID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditPrograms'
CREATE TABLE [dbo].[QualityAuditPrograms] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityAuditPlanID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [IsNew] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditRecordedVotes'
CREATE TABLE [dbo].[QualityAuditRecordedVotes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityAuditProgramID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [QualityAuditProgramISOID] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditBy] int  NOT NULL,
    [ISOIndexID] int  NULL,
    [ModuleCode] nvarchar(max)  NULL,
    [FunctionCode] nvarchar(max)  NULL,
    [IsMaximum] bit  NOT NULL,
    [IsMedium] bit  NOT NULL,
    [IsObs] bit  NOT NULL,
    [IsVerify] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [Contents] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityAuditResults'
CREATE TABLE [dbo].[QualityAuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [QualityAuditProgramISOID] int  NOT NULL,
    [QualityNCID] int  NULL,
    [ISOIndexCriteriaID] int  NOT NULL,
    [IsPass] bit  NOT NULL,
    [AuditBy] int  NULL,
    [AuditAt] datetime  NULL,
    [AuditNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityCAPARequirements'
CREATE TABLE [dbo].[QualityCAPARequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityNCID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [EmployeeID] int  NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [CompleteTime] datetime  NOT NULL,
    [CompleteRealTime] datetime  NULL,
    [IsCompleteAuto] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [IsAuditBack] bit  NULL,
    [AuditBackPass] bit  NULL,
    [AuditBackNote] nvarchar(max)  NULL,
    [AuditBackBy] int  NULL,
    [AuditBackTime] datetime  NULL,
    [ParentID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityCAPAs'
CREATE TABLE [dbo].[QualityCAPAs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityCAPARequirementID] int  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [IsPass] bit  NOT NULL,
    [CompleteTime] datetime  NULL,
    [CompleteRealTime] datetime  NULL,
    [Cause] nvarchar(max)  NULL,
    [Solution] nvarchar(max)  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityCAPATasks'
CREATE TABLE [dbo].[QualityCAPATasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [QualityCAPAID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityCriteriaCategories'
CREATE TABLE [dbo].[QualityCriteriaCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsProduction] bit  NOT NULL,
    [ParentID] int  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityCriterias'
CREATE TABLE [dbo].[QualityCriterias] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [MinPoint] int  NOT NULL,
    [MaxPoint] int  NOT NULL,
    [Factory] decimal(18,2)  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [QualityCriteriaCategoryID] int  NULL
);
GO

-- Creating table 'QualityMeetingAttachmentFiles'
CREATE TABLE [dbo].[QualityMeetingAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetingObjects'
CREATE TABLE [dbo].[QualityMeetingObjects] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [IsMeeting] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetingProblems'
CREATE TABLE [dbo].[QualityMeetingProblems] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingID] int  NOT NULL,
    [ISOMeetingID] int  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetingResultAttachmentFiles'
CREATE TABLE [dbo].[QualityMeetingResultAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingResultID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetingResults'
CREATE TABLE [dbo].[QualityMeetingResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingID] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetings'
CREATE TABLE [dbo].[QualityMeetings] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsFinish] bit  NOT NULL,
    [Address] nvarchar(max)  NULL,
    [TaskPrepare] nvarchar(max)  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityMeetingTasks'
CREATE TABLE [dbo].[QualityMeetingTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityMeetingProblemID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityNCEmployees'
CREATE TABLE [dbo].[QualityNCEmployees] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityNCID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityNCRoles'
CREATE TABLE [dbo].[QualityNCRoles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanRoleID] int  NOT NULL,
    [QualityNCID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityNCs'
CREATE TABLE [dbo].[QualityNCs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [DepartmentID] int  NOT NULL,
    [Source] nvarchar(max)  NULL,
    [Time] datetime  NOT NULL,
    [Content] nvarchar(max)  NULL,
    [IsMaximum] bit  NOT NULL,
    [IsMedium] bit  NOT NULL,
    [IsObs] bit  NOT NULL,
    [IsVerify] bit  NOT NULL,
    [IsTrue] bit  NOT NULL,
    [IsCompleteAuto] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [IsCAPA] bit  NOT NULL,
    [IsLock] bit  NOT NULL,
    [CheckAt] datetime  NULL,
    [CheckNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityNCStockAdjustments'
CREATE TABLE [dbo].[QualityNCStockAdjustments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockAdjustmentID] int  NOT NULL,
    [QualityNCID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityNCTasks'
CREATE TABLE [dbo].[QualityNCTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [QualityNCID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityPlans'
CREATE TABLE [dbo].[QualityPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityTargetID] int  NULL,
    [DepartmentID] int  NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [Type] bit  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Cost] decimal(18,2)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsProduction] bit  NOT NULL,
    [IsEnd] bit  NOT NULL,
    [EndAt] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityPlanTasks'
CREATE TABLE [dbo].[QualityPlanTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityTargetCategories'
CREATE TABLE [dbo].[QualityTargetCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'QualityTargetLevels'
CREATE TABLE [dbo].[QualityTargetLevels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'QualityTargets'
CREATE TABLE [dbo].[QualityTargets] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityTargetCategoryID] int  NULL,
    [ParentID] int  NULL,
    [DepartmentID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Value] decimal(18,2)  NOT NULL,
    [Unit] nvarchar(max)  NULL,
    [CompleteAt] datetime  NOT NULL,
    [Type] bit  NOT NULL,
    [QualityTargetLevelID] int  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [IsAccept] bit  NOT NULL,
    [IsSuccess] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [Description] nvarchar(max)  NULL,
    [IsEnd] bit  NOT NULL,
    [EndAt] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Reports'
CREATE TABLE [dbo].[Reports] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [ReportTemplateID] int  NULL,
    [Type] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [ValueMap] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ReportTemplates'
CREATE TABLE [dbo].[ReportTemplates] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ModuleCode] nvarchar(max)  NULL,
    [FunctionCode] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [File] nvarchar(max)  NULL,
    [PathUpload] nvarchar(max)  NULL,
    [PathReport] nvarchar(max)  NULL,
    [Type] nvarchar(max)  NULL,
    [Size] float  NULL,
    [IsMap] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'RiskAudits'
CREATE TABLE [dbo].[RiskAudits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskControlID] int  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [AuditTime] datetime  NULL,
    [QualityNCID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'RiskContracts'
CREATE TABLE [dbo].[RiskContracts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CustomerContractID] int  NOT NULL,
    [RiskID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskControls'
CREATE TABLE [dbo].[RiskControls] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskID] int  NOT NULL,
    [Controls] nvarchar(max)  NULL,
    [LikeLiHood] real  NOT NULL,
    [RiskTreatmentID] int  NOT NULL,
    [CompleteTime] datetime  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [NowLikeLiHood] real  NOT NULL,
    [NowImpact] real  NOT NULL,
    [NowConsequence] real  NOT NULL
);
GO

-- Creating table 'RiskControlSolutions'
CREATE TABLE [dbo].[RiskControlSolutions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskControlID] int  NOT NULL,
    [RiskLibrarySolutionID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskControlTasks'
CREATE TABLE [dbo].[RiskControlTasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [RiskControlID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskIncedents'
CREATE TABLE [dbo].[RiskIncedents] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskID] int  NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [Time] datetime  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskLegals'
CREATE TABLE [dbo].[RiskLegals] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskID] int  NOT NULL,
    [DepartmentLegalID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskLevels'
CREATE TABLE [dbo].[RiskLevels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CenterRiskMethodID] int  NOT NULL,
    [PointMin] real  NOT NULL,
    [PointMax] real  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskLibrarySolutions'
CREATE TABLE [dbo].[RiskLibrarySolutions] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskTempControlID] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'RiskRegulatories'
CREATE TABLE [dbo].[RiskRegulatories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RiskID] int  NOT NULL,
    [DepartmentRegulatoryID] int  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'Risks'
CREATE TABLE [dbo].[Risks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [RiskCategoryID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Weakness] nvarchar(max)  NULL,
    [Action] nvarchar(max)  NULL,
    [HumanEmployeeID] int  NOT NULL,
    [IsFromRequire] bit  NOT NULL,
    [IsFromAction] bit  NOT NULL,
    [IsFromLegal] bit  NOT NULL,
    [IsFromRegulatory] bit  NOT NULL,
    [IsSend] bit  NOT NULL,
    [LikeLiHood] real  NOT NULL,
    [Impact] real  NOT NULL,
    [Consequence] real  NOT NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'ServiceCategories'
CREATE TABLE [dbo].[ServiceCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ServiceCommandContracts'
CREATE TABLE [dbo].[ServiceCommandContracts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ServiceCommandID] int  NOT NULL,
    [CustomerContractID] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ServiceCommands'
CREATE TABLE [dbo].[ServiceCommands] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NULL,
    [EndTime] datetime  NULL,
    [PerformBy] int  NULL,
    [IsEdit] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ServicePlans'
CREATE TABLE [dbo].[ServicePlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [ServiceCommandContractID] int  NULL
);
GO

-- Creating table 'ServicePlanStages'
CREATE TABLE [dbo].[ServicePlanStages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ServicePlanID] int  NOT NULL,
    [ServiceStageID] int  NOT NULL,
    [CustomerID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Services'
CREATE TABLE [dbo].[Services] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ServiceCategoryID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Code] nvarchar(max)  NULL,
    [IsUse] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Cost] decimal(18,2)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'ServiceStages'
CREATE TABLE [dbo].[ServiceStages] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ServiceID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Time] real  NULL,
    [TimeUnit] int  NULL,
    [Order] int  NULL,
    [IsUse] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockAdjustmentDetails'
CREATE TABLE [dbo].[StockAdjustmentDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockAdjustmentID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Stock_ID] int  NULL,
    [Unit_ID] int  NULL,
    [UnitConvert] decimal(18,2)  NULL,
    [Width] decimal(18,2)  NULL,
    [Height] decimal(18,2)  NULL,
    [Orgin] nvarchar(max)  NULL,
    [CurrentQty] decimal(18,2)  NULL,
    [NewQty] decimal(18,2)  NULL,
    [QtyDiff] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [QtyConvert] decimal(18,2)  NULL,
    [StoreID] bigint  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockAdjustments'
CREATE TABLE [dbo].[StockAdjustments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NOT NULL,
    [Ref_OrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Employee_ID] int  NULL,
    [StockID] int  NULL,
    [Amount] decimal(18,2)  NULL,
    [Accept] bit  NOT NULL,
    [IsClose] bit  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockBuildDetails'
CREATE TABLE [dbo].[StockBuildDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockBuildID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [ProductName] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [RefTypeSub] int  NULL,
    [Stock_ID] int  NULL,
    [Unit] nvarchar(max)  NULL,
    [UnitConvert] decimal(18,2)  NULL,
    [Vat] int  NULL,
    [CurrentQty] decimal(18,2)  NULL,
    [QuantityDefault] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [QtyConvert] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [Limit] datetime  NULL,
    [Serial] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockBuilds'
CREATE TABLE [dbo].[StockBuilds] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NULL,
    [Ref_OrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Barcode] nvarchar(max)  NULL,
    [Department_ID] int  NULL,
    [Employee_ID] int  NULL,
    [Contact] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [Vat] int  NULL,
    [Amount] decimal(18,2)  NULL,
    [FAmount] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockInventories'
CREATE TABLE [dbo].[StockInventories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefID] nvarchar(max)  NULL,
    [RefDate] datetime  NULL,
    [RefType] int  NULL,
    [Stock_ID] int  NULL,
    [StockProductID] int  NULL,
    [Customer_ID] int  NULL,
    [Currency_ID] int  NULL,
    [Limit] datetime  NULL,
    [Quantity] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [ChassyNo] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [Location] nvarchar(max)  NULL,
    [Width] decimal(18,2)  NULL,
    [Height] decimal(18,2)  NULL,
    [Orgin] nvarchar(max)  NULL,
    [Size] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockInventoryDetails'
CREATE TABLE [dbo].[StockInventoryDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockInventoryID] int  NULL,
    [RefNo] nvarchar(max)  NULL,
    [RefDate] datetime  NULL,
    [RefDetailNo] uniqueidentifier  NULL,
    [RefType] int  NULL,
    [RefStatus] int  NULL,
    [StoreID] int  NULL,
    [StockID] int  NULL,
    [StockProductID] int  NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Supplier_ID] int  NULL,
    [Customer_ID] int  NULL,
    [Employee_ID] int  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [Unit] nvarchar(max)  NULL,
    [Price] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [E_Qty] decimal(18,2)  NULL,
    [E_Amt] decimal(18,2)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [Book_ID] int  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockInwardDetails'
CREATE TABLE [dbo].[StockInwardDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockInwardID] int  NOT NULL,
    [StockProductID] int  NOT NULL,
    [ProductName] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Stock_ID] int  NULL,
    [Lev1] decimal(18,2)  NULL,
    [Lev2] decimal(18,2)  NULL,
    [Lev3] decimal(18,2)  NULL,
    [Lev4] decimal(18,2)  NULL,
    [Unit] nvarchar(max)  NULL,
    [UnitConvert] decimal(18,2)  NULL,
    [Vat] int  NULL,
    [CurrentQty] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [QtyConvert] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [Limit] datetime  NULL,
    [Width] decimal(18,2)  NULL,
    [Height] decimal(18,2)  NULL,
    [Orgin] nvarchar(max)  NULL,
    [Size] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [ChassyNo] nvarchar(max)  NULL,
    [IME] nvarchar(max)  NULL,
    [StoreID] bigint  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NOT NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockInwards'
CREATE TABLE [dbo].[StockInwards] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NULL,
    [Ref_OrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Barcode] nvarchar(max)  NULL,
    [Department_ID] int  NULL,
    [Employee_ID] int  NULL,
    [Stock_ID] int  NULL,
    [Branch_ID] int  NULL,
    [Contract_ID] int  NULL,
    [Customer_ID] int  NULL,
    [CustomerName] nvarchar(max)  NULL,
    [CustomerAddress] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Payment] smallint  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [Vat] int  NULL,
    [Amount] decimal(18,2)  NULL,
    [FAmount] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [IsClose] bit  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockListCurrencies'
CREATE TABLE [dbo].[StockListCurrencies] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Exchange] decimal(18,2)  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockOutwardDetails'
CREATE TABLE [dbo].[StockOutwardDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockOutwardID] int  NOT NULL,
    [Stock_ID] int  NULL,
    [RefType] int  NULL,
    [StockProductID] int  NOT NULL,
    [ProductName] nvarchar(max)  NULL,
    [Vat] int  NULL,
    [Lev1] decimal(18,2)  NULL,
    [Lev2] decimal(18,2)  NULL,
    [Lev3] decimal(18,2)  NULL,
    [Lev4] decimal(18,2)  NULL,
    [Unit] nvarchar(max)  NULL,
    [UnitConvert] decimal(18,2)  NULL,
    [CurrentQty] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [QtyConvert] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Profit] decimal(18,2)  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [ChassyNo] nvarchar(max)  NULL,
    [IME] nvarchar(max)  NULL,
    [Width] decimal(18,2)  NULL,
    [Height] decimal(18,2)  NULL,
    [Orgin] nvarchar(max)  NULL,
    [Size] nvarchar(max)  NULL,
    [StoreID] bigint  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NOT NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockOutwards'
CREATE TABLE [dbo].[StockOutwards] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NULL,
    [Ref_OrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Barcode] nvarchar(max)  NULL,
    [Department_ID] int  NULL,
    [Employee_ID] int  NULL,
    [Stock_ID] int  NULL,
    [Branch_ID] int  NULL,
    [Contract_ID] int  NULL,
    [IsInside] bit  NOT NULL,
    [Customer_ID] int  NULL,
    [CustomerName] nvarchar(max)  NULL,
    [CustomerAddress] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Payment] smallint  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [FAmount] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Charge] decimal(18,2)  NULL,
    [Cost] decimal(18,2)  NULL,
    [Profit] decimal(18,2)  NULL,
    [IsClose] bit  NOT NULL,
    [Sorted] bigint  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockProductBuilds'
CREATE TABLE [dbo].[StockProductBuilds] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockProductID] int  NOT NULL,
    [UnitName] nvarchar(max)  NULL,
    [Build_ID] int  NOT NULL,
    [Quantity] decimal(18,2)  NULL,
    [Price] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL
);
GO

-- Creating table 'StockProductGroups'
CREATE TABLE [dbo].[StockProductGroups] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [IsMain] bit  NOT NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockProductModels'
CREATE TABLE [dbo].[StockProductModels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockProductPrices'
CREATE TABLE [dbo].[StockProductPrices] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NULL,
    [RefStatus] smallint  NULL,
    [BeginDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Product_ID] int  NULL,
    [Customer_ID] int  NULL,
    [Price] decimal(18,2)  NULL,
    [DiscountRate] decimal(18,2)  NULL,
    [CommissionRate] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [BeginAmount] decimal(18,2)  NULL,
    [EndAmount] decimal(18,2)  NULL,
    [IsPublic] bit  NOT NULL,
    [CreatedBy] int  NULL,
    [CreatedAt] datetime  NULL,
    [ModifiedBy] nvarchar(max)  NULL,
    [ModifiedDate] datetime  NULL,
    [OwnerID] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NULL,
    [Active] bit  NOT NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'StockProducts'
CREATE TABLE [dbo].[StockProducts] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [NameEN] nvarchar(max)  NULL,
    [Type_ID] int  NULL,
    [Manufacturer_ID] int  NULL,
    [Model_ID] int  NULL,
    [StockProductGroupID] int  NULL,
    [Provider_ID] int  NULL,
    [Origin] nvarchar(max)  NULL,
    [Barcode] nvarchar(max)  NULL,
    [Unit_ID] int  NULL,
    [UnitConvert] nvarchar(max)  NULL,
    [UnitRate] decimal(18,2)  NULL,
    [Org_Price] decimal(18,2)  NULL,
    [Sale_Price] decimal(18,2)  NULL,
    [Retail_Price] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [CurrentCost] decimal(18,2)  NULL,
    [AverageCost] decimal(18,2)  NULL,
    [Warranty] int  NULL,
    [VAT_ID] decimal(18,2)  NULL,
    [ImportTax_ID] decimal(18,2)  NULL,
    [ExportTax_ID] decimal(18,2)  NULL,
    [LuxuryTax_ID] decimal(18,2)  NULL,
    [Customer_ID] nvarchar(max)  NULL,
    [Customer_Name] nvarchar(max)  NULL,
    [CostMethod] smallint  NULL,
    [MinStock] decimal(18,2)  NULL,
    [MaxStock] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [Commission] decimal(18,2)  NULL,
    [Description] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [Expiry] bit  NOT NULL,
    [LimitOrders] decimal(18,2)  NULL,
    [ExpiryValue] decimal(18,2)  NULL,
    [Batch] bit  NOT NULL,
    [Depth] decimal(18,2)  NULL,
    [Height] decimal(18,2)  NULL,
    [Width] decimal(18,2)  NULL,
    [Thickness] decimal(18,2)  NULL,
    [Size] nvarchar(max)  NULL,
    [ImageProduct] varbinary(max)  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [StockID] int  NULL,
    [Serial] bit  NOT NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockProductTypes'
CREATE TABLE [dbo].[StockProductTypes] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL
);
GO

-- Creating table 'Stocks'
CREATE TABLE [dbo].[Stocks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [Contact] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Telephone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Mobi] nvarchar(max)  NULL,
    [Manager] nvarchar(max)  NULL,
    [ImageMap] varbinary(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockTransferDetails'
CREATE TABLE [dbo].[StockTransferDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [StockTransferID] int  NULL,
    [RefType] int  NULL,
    [StockProductID] int  NOT NULL,
    [Product_Name] nvarchar(max)  NULL,
    [OutStock] int  NULL,
    [InStock] int  NULL,
    [Lev1] decimal(18,2)  NULL,
    [Lev2] decimal(18,2)  NULL,
    [Lev3] decimal(18,2)  NULL,
    [Lev4] decimal(18,2)  NULL,
    [Unit] nvarchar(max)  NULL,
    [UnitConvert] decimal(18,2)  NULL,
    [UnitPrice] decimal(18,2)  NULL,
    [Quantity] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [QtyConvert] decimal(18,2)  NULL,
    [StoreID] bigint  NULL,
    [Batch] nvarchar(max)  NULL,
    [Serial] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Sorted] bigint  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockTransferRefs'
CREATE TABLE [dbo].[StockTransferRefs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefID] int  NULL,
    [RefOrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [RefDate] datetime  NULL,
    [TransFrom] nvarchar(max)  NULL,
    [TransTo] nvarchar(max)  NULL,
    [Department_ID] int  NULL,
    [Employee_ID] int  NULL,
    [Stock_ID] int  NULL,
    [Branch_ID] int  NULL,
    [Contract_ID] int  NULL,
    [Contract] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [Amount] decimal(18,2)  NULL,
    [Discount] decimal(18,2)  NULL,
    [FAmount] decimal(18,2)  NULL,
    [FDiscount] decimal(18,2)  NULL,
    [IsClose] bit  NOT NULL,
    [Sorted] bigint  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockTransfers'
CREATE TABLE [dbo].[StockTransfers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [RefDate] datetime  NULL,
    [Ref_OrgNo] nvarchar(max)  NULL,
    [RefType] int  NULL,
    [Department_ID] int  NULL,
    [Employee_ID] int  NULL,
    [FromStock_ID] nvarchar(max)  NULL,
    [Sender_ID] int  NULL,
    [ToStock_ID] nvarchar(max)  NULL,
    [Receiver_ID] int  NULL,
    [Branch_ID] int  NULL,
    [Contract_ID] int  NULL,
    [Currency_ID] int  NULL,
    [ExchangeRate] decimal(18,2)  NULL,
    [Barcode] nvarchar(max)  NULL,
    [Amount] decimal(18,2)  NULL,
    [IsReview] bit  NOT NULL,
    [IsClose] bit  NOT NULL,
    [Sorted] bigint  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'StockUnits'
CREATE TABLE [dbo].[StockUnits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'SupplierAuditPlans'
CREATE TABLE [dbo].[SupplierAuditPlans] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [QualityPlanID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'SupplierAuditResults'
CREATE TABLE [dbo].[SupplierAuditResults] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SupplierAuditID] int  NOT NULL,
    [QualityCriteria] int  NOT NULL,
    [Point] decimal(18,0)  NULL,
    [Factory] decimal(18,0)  NULL,
    [TotalPoint] decimal(18,0)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NOT NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'SupplierAudits'
CREATE TABLE [dbo].[SupplierAudits] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SupplierID] int  NOT NULL,
    [SupplierAuditPlanID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [IsPass] bit  NULL
);
GO

-- Creating table 'SupplierBidToAttachmentFiles'
CREATE TABLE [dbo].[SupplierBidToAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SupplierBidOrderID] int  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SuppliersGroupID] int  NOT NULL,
    [Code] nvarchar(max)  NULL,
    [Name] nvarchar(max)  NULL,
    [BrandName] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NULL,
    [Website] nvarchar(max)  NULL,
    [Phone] nvarchar(max)  NULL,
    [Fax] nvarchar(max)  NULL,
    [Tax] nvarchar(max)  NULL,
    [AcountNumber] nvarchar(max)  NULL,
    [BankName] nvarchar(max)  NULL,
    [Address] nvarchar(max)  NULL,
    [CountryId] int  NULL,
    [ProvinceId] int  NULL,
    [Representative] nvarchar(max)  NULL,
    [TotalImportValue] float  NULL,
    [IsOwedOn] float  NULL,
    [Position] nvarchar(max)  NULL,
    [IsCustomer] bit  NOT NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [Description] nvarchar(max)  NULL,
    [Status] int  NULL,
    [CreateAt] datetime  NOT NULL,
    [CreateBy] int  NOT NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Commodity] nvarchar(500)  NULL
);
GO

-- Creating table 'SuppliersBidOrders'
CREATE TABLE [dbo].[SuppliersBidOrders] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SuppliersOrderID] int  NOT NULL,
    [SupplierID] int  NOT NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [ReceiveDate] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Status] bit  NULL,
    [Contents] nvarchar(max)  NULL
);
GO

-- Creating table 'SuppliersBills'
CREATE TABLE [dbo].[SuppliersBills] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SuppliersOrderID] int  NOT NULL,
    [PayedMoney] decimal(18,0)  NULL,
    [PayDate] datetime  NULL,
    [BillType] tinyint  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Note] nvarchar(max)  NULL
);
GO

-- Creating table 'SuppliersGroups'
CREATE TABLE [dbo].[SuppliersGroups] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [Active] bit  NOT NULL,
    [ParentID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'SuppliersOrderDetails'
CREATE TABLE [dbo].[SuppliersOrderDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SuppliersOrderID] int  NULL,
    [StocksProductID] int  NULL,
    [Quantity] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Price] decimal(18,2)  NULL,
    [Note] nvarchar(500)  NULL,
    [ReciveQuantity] int  NULL,
    [ReciveQuality] int  NULL,
    [SuppliersOrderRequirementDetailID] int  NULL
);
GO

-- Creating table 'SuppliersOrderRequirementDetails'
CREATE TABLE [dbo].[SuppliersOrderRequirementDetails] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [SuppliersOrderRequirementID] int  NULL,
    [StocksProductID] int  NULL,
    [Quantity] int  NULL,
    [Status] int  NULL,
    [Note] nvarchar(500)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'SuppliersOrderRequirements'
CREATE TABLE [dbo].[SuppliersOrderRequirements] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CODE] varchar(50)  NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [Status] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [IsApproval] bit  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [IsEdit] bit  NULL,
    [IsAccept] bit  NULL
);
GO

-- Creating table 'SuppliersOrders'
CREATE TABLE [dbo].[SuppliersOrders] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [CODE] varchar(50)  NULL,
    [SupplierID] int  NULL,
    [OrderDate] datetime  NULL,
    [ShipDate] datetime  NULL,
    [ReciepDate] datetime  NULL,
    [Payment] tinyint  NULL,
    [ReciepPlace] nvarchar(500)  NULL,
    [Status] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [IsApproval] bit  NULL,
    [ApprovalAt] datetime  NULL,
    [ApprovalBy] int  NULL,
    [ApprovalNote] nvarchar(max)  NULL,
    [IsEdit] bit  NULL,
    [IsAccept] bit  NULL,
    [Discount] int  NULL,
    [Amount] decimal(18,0)  NULL,
    [ShipType] nvarchar(500)  NULL
);
GO

-- Creating table 'TaskAttachmentFiles'
CREATE TABLE [dbo].[TaskAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [TaskID] int  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [IsChange] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskCalendarEvents'
CREATE TABLE [dbo].[TaskCalendarEvents] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsWorking] bit  NOT NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'TaskCalendars'
CREATE TABLE [dbo].[TaskCalendars] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskCalendarEventID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [Title] nvarchar(max)  NULL,
    [Year] int  NOT NULL,
    [Month] int  NOT NULL,
    [Day] int  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskCategories'
CREATE TABLE [dbo].[TaskCategories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskCCs'
CREATE TABLE [dbo].[TaskCCs] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL
);
GO

-- Creating table 'TaskChecks'
CREATE TABLE [dbo].[TaskChecks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Rate] decimal(18,2)  NOT NULL,
    [IsPass] bit  NOT NULL,
    [Feedback] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskComments'
CREATE TABLE [dbo].[TaskComments] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NOT NULL,
    [TaskID] int  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskHistories'
CREATE TABLE [dbo].[TaskHistories] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [ParentID] int  NOT NULL,
    [DepartmentID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [CategoryID] int  NOT NULL,
    [LevelID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsPrivate] bit  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [IsPass] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [Rate] decimal(18,2)  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [CompleteTime] datetime  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [IsChange] bit  NOT NULL,
    [ChangeNote] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskLevels'
CREATE TABLE [dbo].[TaskLevels] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Color] nvarchar(max)  NULL,
    [IsActive] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [Note] nvarchar(max)  NULL,
    [CreateBy] int  NULL,
    [CreateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [UpdateAt] datetime  NULL
);
GO

-- Creating table 'TaskPerformAttachmentFiles'
CREATE TABLE [dbo].[TaskPerformAttachmentFiles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [AttachmentFileID] uniqueidentifier  NOT NULL,
    [TaskPerformID] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskPerforms'
CREATE TABLE [dbo].[TaskPerforms] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [Time] datetime  NOT NULL,
    [Rate] decimal(18,2)  NOT NULL,
    [IsCheck] bit  NOT NULL,
    [Feedback] nvarchar(max)  NULL,
    [Content] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskPersonals'
CREATE TABLE [dbo].[TaskPersonals] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TaskID] int  NOT NULL,
    [PerformBy] int  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsCreate] bit  NOT NULL,
    [IsPerform] bit  NOT NULL,
    [IsClosed] bit  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TaskRequests'
CREATE TABLE [dbo].[TaskRequests] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NULL,
    [HumanDepartmentID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [TaskCategoryID] int  NOT NULL,
    [TaskLevelID] int  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [Contents] nvarchar(max)  NULL,
    [Reason] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [IsEdit] bit  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsApproval] bit  NOT NULL,
    [IsAccept] bit  NOT NULL,
    [IsDelete] bit  NOT NULL,
    [ApprovalBy] int  NULL,
    [ApprovalAt] datetime  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'Tasks'
CREATE TABLE [dbo].[Tasks] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [ParentID] int  NOT NULL,
    [HumanDepartmentID] int  NOT NULL,
    [HumanEmployeeID] int  NOT NULL,
    [TaskCategoryID] int  NOT NULL,
    [TaskLevelID] int  NOT NULL,
    [AuditID] int  NULL,
    [Name] nvarchar(max)  NULL,
    [StartTime] datetime  NOT NULL,
    [EndTime] datetime  NOT NULL,
    [IsPrivate] bit  NOT NULL,
    [IsNew] bit  NOT NULL,
    [IsEdit] bit  NOT NULL,
    [IsComplete] bit  NOT NULL,
    [IsPass] bit  NOT NULL,
    [IsPause] bit  NOT NULL,
    [Rate] decimal(18,2)  NOT NULL,
    [Cost] decimal(18,2)  NOT NULL,
    [CompleteTime] datetime  NULL,
    [Content] nvarchar(max)  NULL,
    [Note] nvarchar(max)  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TemplateExportFields'
CREATE TABLE [dbo].[TemplateExportFields] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TemplateExportID] int  NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Field] nvarchar(500)  NULL,
    [Value] nvarchar(500)  NULL,
    [Postition] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TemplateExportFieldStyles'
CREATE TABLE [dbo].[TemplateExportFieldStyles] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [TemplateExportFieldID] int  NOT NULL,
    [StyleIndex] int  NOT NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL
);
GO

-- Creating table 'TemplateExports'
CREATE TABLE [dbo].[TemplateExports] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(500)  NOT NULL,
    [Font] varchar(50)  NULL,
    [FontSize] int  NULL,
    [CreateAt] datetime  NULL,
    [CreateBy] int  NULL,
    [UpdateAt] datetime  NULL,
    [UpdateBy] int  NULL,
    [Type] int  NULL,
    [AttachmentFileID] uniqueidentifier  NULL,
    [FunctionCode] varchar(500)  NOT NULL,
    [Default] bit  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [ID] in table 'AccountingDebtTypes'
ALTER TABLE [dbo].[AccountingDebtTypes]
ADD CONSTRAINT [PK_AccountingDebtTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingPaymentPlans'
ALTER TABLE [dbo].[AccountingPaymentPlans]
ADD CONSTRAINT [PK_AccountingPaymentPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingPayments'
ALTER TABLE [dbo].[AccountingPayments]
ADD CONSTRAINT [PK_AccountingPayments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingReasonSpendings'
ALTER TABLE [dbo].[AccountingReasonSpendings]
ADD CONSTRAINT [PK_AccountingReasonSpendings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingSpendingBillDetails'
ALTER TABLE [dbo].[AccountingSpendingBillDetails]
ADD CONSTRAINT [PK_AccountingSpendingBillDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingSpendingBills'
ALTER TABLE [dbo].[AccountingSpendingBills]
ADD CONSTRAINT [PK_AccountingSpendingBills]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AccountingSuggestAdvances'
ALTER TABLE [dbo].[AccountingSuggestAdvances]
ADD CONSTRAINT [PK_AccountingSuggestAdvances]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AttachmentFiles'
ALTER TABLE [dbo].[AttachmentFiles]
ADD CONSTRAINT [PK_AttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'AuditResults'
ALTER TABLE [dbo].[AuditResults]
ADD CONSTRAINT [PK_AuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Audits'
ALTER TABLE [dbo].[Audits]
ADD CONSTRAINT [PK_Audits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BusinessActions'
ALTER TABLE [dbo].[BusinessActions]
ADD CONSTRAINT [PK_BusinessActions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BusinessFunctions'
ALTER TABLE [dbo].[BusinessFunctions]
ADD CONSTRAINT [PK_BusinessFunctions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'BusinessModules'
ALTER TABLE [dbo].[BusinessModules]
ADD CONSTRAINT [PK_BusinessModules]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CalendarCategories'
ALTER TABLE [dbo].[CalendarCategories]
ADD CONSTRAINT [PK_CalendarCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CalendarOverrides'
ALTER TABLE [dbo].[CalendarOverrides]
ADD CONSTRAINT [PK_CalendarOverrides]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CalendarTimeOverrides'
ALTER TABLE [dbo].[CalendarTimeOverrides]
ADD CONSTRAINT [PK_CalendarTimeOverrides]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Chats'
ALTER TABLE [dbo].[Chats]
ADD CONSTRAINT [PK_Chats]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CommonCities'
ALTER TABLE [dbo].[CommonCities]
ADD CONSTRAINT [PK_CommonCities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CommonCountries'
ALTER TABLE [dbo].[CommonCountries]
ADD CONSTRAINT [PK_CommonCountries]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerAssesses'
ALTER TABLE [dbo].[CustomerAssesses]
ADD CONSTRAINT [PK_CustomerAssesses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerAssessObjects'
ALTER TABLE [dbo].[CustomerAssessObjects]
ADD CONSTRAINT [PK_CustomerAssessObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerAssessResults'
ALTER TABLE [dbo].[CustomerAssessResults]
ADD CONSTRAINT [PK_CustomerAssessResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCampaignAudits'
ALTER TABLE [dbo].[CustomerCampaignAudits]
ADD CONSTRAINT [PK_CustomerCampaignAudits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCampaignPlans'
ALTER TABLE [dbo].[CustomerCampaignPlans]
ADD CONSTRAINT [PK_CustomerCampaignPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCampaigns'
ALTER TABLE [dbo].[CustomerCampaigns]
ADD CONSTRAINT [PK_CustomerCampaigns]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCampaignTargets'
ALTER TABLE [dbo].[CustomerCampaignTargets]
ADD CONSTRAINT [PK_CustomerCampaignTargets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCareObjects'
ALTER TABLE [dbo].[CustomerCareObjects]
ADD CONSTRAINT [PK_CustomerCareObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCareResults'
ALTER TABLE [dbo].[CustomerCareResults]
ADD CONSTRAINT [PK_CustomerCareResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCares'
ALTER TABLE [dbo].[CustomerCares]
ADD CONSTRAINT [PK_CustomerCares]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCategories'
ALTER TABLE [dbo].[CustomerCategories]
ADD CONSTRAINT [PK_CustomerCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCategoryCustomers'
ALTER TABLE [dbo].[CustomerCategoryCustomers]
ADD CONSTRAINT [PK_CustomerCategoryCustomers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCategoryDepartments'
ALTER TABLE [dbo].[CustomerCategoryDepartments]
ADD CONSTRAINT [PK_CustomerCategoryDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContactCalendars'
ALTER TABLE [dbo].[CustomerContactCalendars]
ADD CONSTRAINT [PK_CustomerContactCalendars]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContactForms'
ALTER TABLE [dbo].[CustomerContactForms]
ADD CONSTRAINT [PK_CustomerContactForms]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContactHistories'
ALTER TABLE [dbo].[CustomerContactHistories]
ADD CONSTRAINT [PK_CustomerContactHistories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContactHistoryAttachmentFiles'
ALTER TABLE [dbo].[CustomerContactHistoryAttachmentFiles]
ADD CONSTRAINT [PK_CustomerContactHistoryAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContacts'
ALTER TABLE [dbo].[CustomerContacts]
ADD CONSTRAINT [PK_CustomerContacts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContractAttachmentFiles'
ALTER TABLE [dbo].[CustomerContractAttachmentFiles]
ADD CONSTRAINT [PK_CustomerContractAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerContracts'
ALTER TABLE [dbo].[CustomerContracts]
ADD CONSTRAINT [PK_CustomerContracts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCriteriaCategories'
ALTER TABLE [dbo].[CustomerCriteriaCategories]
ADD CONSTRAINT [PK_CustomerCriteriaCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerCriterias'
ALTER TABLE [dbo].[CustomerCriterias]
ADD CONSTRAINT [PK_CustomerCriterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerFeedbacks'
ALTER TABLE [dbo].[CustomerFeedbacks]
ADD CONSTRAINT [PK_CustomerFeedbacks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerOrderAttachmentFiles'
ALTER TABLE [dbo].[CustomerOrderAttachmentFiles]
ADD CONSTRAINT [PK_CustomerOrderAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerOrders'
ALTER TABLE [dbo].[CustomerOrders]
ADD CONSTRAINT [PK_CustomerOrders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerSurveyObjects'
ALTER TABLE [dbo].[CustomerSurveyObjects]
ADD CONSTRAINT [PK_CustomerSurveyObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerSurveyQuestions'
ALTER TABLE [dbo].[CustomerSurveyQuestions]
ADD CONSTRAINT [PK_CustomerSurveyQuestions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerSurveyResults'
ALTER TABLE [dbo].[CustomerSurveyResults]
ADD CONSTRAINT [PK_CustomerSurveyResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerSurveys'
ALTER TABLE [dbo].[CustomerSurveys]
ADD CONSTRAINT [PK_CustomerSurveys]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'CustomerTypes'
ALTER TABLE [dbo].[CustomerTypes]
ADD CONSTRAINT [PK_CustomerTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentBroadCategories'
ALTER TABLE [dbo].[DepartmentBroadCategories]
ADD CONSTRAINT [PK_DepartmentBroadCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentBroads'
ALTER TABLE [dbo].[DepartmentBroads]
ADD CONSTRAINT [PK_DepartmentBroads]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentLegalAttachments'
ALTER TABLE [dbo].[DepartmentLegalAttachments]
ADD CONSTRAINT [PK_DepartmentLegalAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentLegalAuditResults'
ALTER TABLE [dbo].[DepartmentLegalAuditResults]
ADD CONSTRAINT [PK_DepartmentLegalAuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentLegals'
ALTER TABLE [dbo].[DepartmentLegals]
ADD CONSTRAINT [PK_DepartmentLegals]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentPolicies'
ALTER TABLE [dbo].[DepartmentPolicies]
ADD CONSTRAINT [PK_DepartmentPolicies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentPolicyAttachments'
ALTER TABLE [dbo].[DepartmentPolicyAttachments]
ADD CONSTRAINT [PK_DepartmentPolicyAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRegulatories'
ALTER TABLE [dbo].[DepartmentRegulatories]
ADD CONSTRAINT [PK_DepartmentRegulatories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRegulatoryAttachments'
ALTER TABLE [dbo].[DepartmentRegulatoryAttachments]
ADD CONSTRAINT [PK_DepartmentRegulatoryAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRegulatoryAuditResults'
ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults]
ADD CONSTRAINT [PK_DepartmentRegulatoryAuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRequirmentAttachments'
ALTER TABLE [dbo].[DepartmentRequirmentAttachments]
ADD CONSTRAINT [PK_DepartmentRequirmentAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRequirmentAuditResults'
ALTER TABLE [dbo].[DepartmentRequirmentAuditResults]
ADD CONSTRAINT [PK_DepartmentRequirmentAuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DepartmentRequirments'
ALTER TABLE [dbo].[DepartmentRequirments]
ADD CONSTRAINT [PK_DepartmentRequirments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchCategories'
ALTER TABLE [dbo].[DispatchCategories]
ADD CONSTRAINT [PK_DispatchCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoAttachmentFiles'
ALTER TABLE [dbo].[DispatchGoAttachmentFiles]
ADD CONSTRAINT [PK_DispatchGoAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoCCs'
ALTER TABLE [dbo].[DispatchGoCCs]
ADD CONSTRAINT [PK_DispatchGoCCs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoDepartments'
ALTER TABLE [dbo].[DispatchGoDepartments]
ADD CONSTRAINT [PK_DispatchGoDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoEmployees'
ALTER TABLE [dbo].[DispatchGoEmployees]
ADD CONSTRAINT [PK_DispatchGoEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoes'
ALTER TABLE [dbo].[DispatchGoes]
ADD CONSTRAINT [PK_DispatchGoes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoOutSides'
ALTER TABLE [dbo].[DispatchGoOutSides]
ADD CONSTRAINT [PK_DispatchGoOutSides]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchGoTasks'
ALTER TABLE [dbo].[DispatchGoTasks]
ADD CONSTRAINT [PK_DispatchGoTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchMethods'
ALTER TABLE [dbo].[DispatchMethods]
ADD CONSTRAINT [PK_DispatchMethods]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchSecurities'
ALTER TABLE [dbo].[DispatchSecurities]
ADD CONSTRAINT [PK_DispatchSecurities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToAttachmentFiles'
ALTER TABLE [dbo].[DispatchToAttachmentFiles]
ADD CONSTRAINT [PK_DispatchToAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToCCs'
ALTER TABLE [dbo].[DispatchToCCs]
ADD CONSTRAINT [PK_DispatchToCCs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToDepartments'
ALTER TABLE [dbo].[DispatchToDepartments]
ADD CONSTRAINT [PK_DispatchToDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToEmployees'
ALTER TABLE [dbo].[DispatchToEmployees]
ADD CONSTRAINT [PK_DispatchToEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToes'
ALTER TABLE [dbo].[DispatchToes]
ADD CONSTRAINT [PK_DispatchToes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchToTasks'
ALTER TABLE [dbo].[DispatchToTasks]
ADD CONSTRAINT [PK_DispatchToTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DispatchUrgencies'
ALTER TABLE [dbo].[DispatchUrgencies]
ADD CONSTRAINT [PK_DispatchUrgencies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentAttachmentFiles'
ALTER TABLE [dbo].[DocumentAttachmentFiles]
ADD CONSTRAINT [PK_DocumentAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentAttachments'
ALTER TABLE [dbo].[DocumentAttachments]
ADD CONSTRAINT [PK_DocumentAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentCategories'
ALTER TABLE [dbo].[DocumentCategories]
ADD CONSTRAINT [PK_DocumentCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentDistributions'
ALTER TABLE [dbo].[DocumentDistributions]
ADD CONSTRAINT [PK_DocumentDistributions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentRequirements'
ALTER TABLE [dbo].[DocumentRequirements]
ADD CONSTRAINT [PK_DocumentRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [PK_Documents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentSecurities'
ALTER TABLE [dbo].[DocumentSecurities]
ADD CONSTRAINT [PK_DocumentSecurities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'DocumentTasks'
ALTER TABLE [dbo].[DocumentTasks]
ADD CONSTRAINT [PK_DocumentTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentCalibrations'
ALTER TABLE [dbo].[EquipmentCalibrations]
ADD CONSTRAINT [PK_EquipmentCalibrations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasureAttachmentFiles'
ALTER TABLE [dbo].[EquipmentMeasureAttachmentFiles]
ADD CONSTRAINT [PK_EquipmentMeasureAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasureCalibrationResults'
ALTER TABLE [dbo].[EquipmentMeasureCalibrationResults]
ADD CONSTRAINT [PK_EquipmentMeasureCalibrationResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasureCalibrations'
ALTER TABLE [dbo].[EquipmentMeasureCalibrations]
ADD CONSTRAINT [PK_EquipmentMeasureCalibrations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasureCategories'
ALTER TABLE [dbo].[EquipmentMeasureCategories]
ADD CONSTRAINT [PK_EquipmentMeasureCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasureDepartments'
ALTER TABLE [dbo].[EquipmentMeasureDepartments]
ADD CONSTRAINT [PK_EquipmentMeasureDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentMeasures'
ALTER TABLE [dbo].[EquipmentMeasures]
ADD CONSTRAINT [PK_EquipmentMeasures]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionAttaches'
ALTER TABLE [dbo].[EquipmentProductionAttaches]
ADD CONSTRAINT [PK_EquipmentProductionAttaches]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionAttachmentFiles'
ALTER TABLE [dbo].[EquipmentProductionAttachmentFiles]
ADD CONSTRAINT [PK_EquipmentProductionAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionCategories'
ALTER TABLE [dbo].[EquipmentProductionCategories]
ADD CONSTRAINT [PK_EquipmentProductionCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionDepartments'
ALTER TABLE [dbo].[EquipmentProductionDepartments]
ADD CONSTRAINT [PK_EquipmentProductionDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionErrors'
ALTER TABLE [dbo].[EquipmentProductionErrors]
ADD CONSTRAINT [PK_EquipmentProductionErrors]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionHistories'
ALTER TABLE [dbo].[EquipmentProductionHistories]
ADD CONSTRAINT [PK_EquipmentProductionHistories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductionMaintains'
ALTER TABLE [dbo].[EquipmentProductionMaintains]
ADD CONSTRAINT [PK_EquipmentProductionMaintains]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'EquipmentProductions'
ALTER TABLE [dbo].[EquipmentProductions]
ADD CONSTRAINT [PK_EquipmentProductions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAbsents'
ALTER TABLE [dbo].[HumanAbsents]
ADD CONSTRAINT [PK_HumanAbsents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAbsentTypes'
ALTER TABLE [dbo].[HumanAbsentTypes]
ADD CONSTRAINT [PK_HumanAbsentTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAnswers'
ALTER TABLE [dbo].[HumanAnswers]
ADD CONSTRAINT [PK_HumanAnswers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditCriteriaCategories'
ALTER TABLE [dbo].[HumanAuditCriteriaCategories]
ADD CONSTRAINT [PK_HumanAuditCriteriaCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditCriteriaPoints'
ALTER TABLE [dbo].[HumanAuditCriteriaPoints]
ADD CONSTRAINT [PK_HumanAuditCriteriaPoints]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditCriterias'
ALTER TABLE [dbo].[HumanAuditCriterias]
ADD CONSTRAINT [PK_HumanAuditCriterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditDepartments'
ALTER TABLE [dbo].[HumanAuditDepartments]
ADD CONSTRAINT [PK_HumanAuditDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditGradationCriteriaPoints'
ALTER TABLE [dbo].[HumanAuditGradationCriteriaPoints]
ADD CONSTRAINT [PK_HumanAuditGradationCriteriaPoints]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditGradationCriterias'
ALTER TABLE [dbo].[HumanAuditGradationCriterias]
ADD CONSTRAINT [PK_HumanAuditGradationCriterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditGradationRoles'
ALTER TABLE [dbo].[HumanAuditGradationRoles]
ADD CONSTRAINT [PK_HumanAuditGradationRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditGradations'
ALTER TABLE [dbo].[HumanAuditGradations]
ADD CONSTRAINT [PK_HumanAuditGradations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditLevels'
ALTER TABLE [dbo].[HumanAuditLevels]
ADD CONSTRAINT [PK_HumanAuditLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditTickResultBonusScores'
ALTER TABLE [dbo].[HumanAuditTickResultBonusScores]
ADD CONSTRAINT [PK_HumanAuditTickResultBonusScores]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditTickResults'
ALTER TABLE [dbo].[HumanAuditTickResults]
ADD CONSTRAINT [PK_HumanAuditTickResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanAuditTicks'
ALTER TABLE [dbo].[HumanAuditTicks]
ADD CONSTRAINT [PK_HumanAuditTicks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanCategoryQuestions'
ALTER TABLE [dbo].[HumanCategoryQuestions]
ADD CONSTRAINT [PK_HumanCategoryQuestions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanDepartments'
ALTER TABLE [dbo].[HumanDepartments]
ADD CONSTRAINT [PK_HumanDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanEmployeeAudits'
ALTER TABLE [dbo].[HumanEmployeeAudits]
ADD CONSTRAINT [PK_HumanEmployeeAudits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanEmployees'
ALTER TABLE [dbo].[HumanEmployees]
ADD CONSTRAINT [PK_HumanEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanEmployeeTimeKeepings'
ALTER TABLE [dbo].[HumanEmployeeTimeKeepings]
ADD CONSTRAINT [PK_HumanEmployeeTimeKeepings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanOrganizations'
ALTER TABLE [dbo].[HumanOrganizations]
ADD CONSTRAINT [PK_HumanOrganizations]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanPermissions'
ALTER TABLE [dbo].[HumanPermissions]
ADD CONSTRAINT [PK_HumanPermissions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanPhaseAudits'
ALTER TABLE [dbo].[HumanPhaseAudits]
ADD CONSTRAINT [PK_HumanPhaseAudits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileAssesses'
ALTER TABLE [dbo].[HumanProfileAssesses]
ADD CONSTRAINT [PK_HumanProfileAssesses]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileAttachments'
ALTER TABLE [dbo].[HumanProfileAttachments]
ADD CONSTRAINT [PK_HumanProfileAttachments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileCertificates'
ALTER TABLE [dbo].[HumanProfileCertificates]
ADD CONSTRAINT [PK_HumanProfileCertificates]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileContracts'
ALTER TABLE [dbo].[HumanProfileContracts]
ADD CONSTRAINT [PK_HumanProfileContracts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileCuriculmViates'
ALTER TABLE [dbo].[HumanProfileCuriculmViates]
ADD CONSTRAINT [PK_HumanProfileCuriculmViates]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileDiplomas'
ALTER TABLE [dbo].[HumanProfileDiplomas]
ADD CONSTRAINT [PK_HumanProfileDiplomas]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileDisciplines'
ALTER TABLE [dbo].[HumanProfileDisciplines]
ADD CONSTRAINT [PK_HumanProfileDisciplines]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileInsurances'
ALTER TABLE [dbo].[HumanProfileInsurances]
ADD CONSTRAINT [PK_HumanProfileInsurances]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileRelationships'
ALTER TABLE [dbo].[HumanProfileRelationships]
ADD CONSTRAINT [PK_HumanProfileRelationships]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileRewards'
ALTER TABLE [dbo].[HumanProfileRewards]
ADD CONSTRAINT [PK_HumanProfileRewards]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileSalaries'
ALTER TABLE [dbo].[HumanProfileSalaries]
ADD CONSTRAINT [PK_HumanProfileSalaries]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileTrainings'
ALTER TABLE [dbo].[HumanProfileTrainings]
ADD CONSTRAINT [PK_HumanProfileTrainings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileWorkExperiences'
ALTER TABLE [dbo].[HumanProfileWorkExperiences]
ADD CONSTRAINT [PK_HumanProfileWorkExperiences]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileWorkTrialResults'
ALTER TABLE [dbo].[HumanProfileWorkTrialResults]
ADD CONSTRAINT [PK_HumanProfileWorkTrialResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanProfileWorkTrials'
ALTER TABLE [dbo].[HumanProfileWorkTrials]
ADD CONSTRAINT [PK_HumanProfileWorkTrials]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanQuestions'
ALTER TABLE [dbo].[HumanQuestions]
ADD CONSTRAINT [PK_HumanQuestions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentCriterias'
ALTER TABLE [dbo].[HumanRecruitmentCriterias]
ADD CONSTRAINT [PK_HumanRecruitmentCriterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentInterviews'
ALTER TABLE [dbo].[HumanRecruitmentInterviews]
ADD CONSTRAINT [PK_HumanRecruitmentInterviews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentPlanInterviews'
ALTER TABLE [dbo].[HumanRecruitmentPlanInterviews]
ADD CONSTRAINT [PK_HumanRecruitmentPlanInterviews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentPlanRequirements'
ALTER TABLE [dbo].[HumanRecruitmentPlanRequirements]
ADD CONSTRAINT [PK_HumanRecruitmentPlanRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentPlans'
ALTER TABLE [dbo].[HumanRecruitmentPlans]
ADD CONSTRAINT [PK_HumanRecruitmentPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentProfileInterviews'
ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews]
ADD CONSTRAINT [PK_HumanRecruitmentProfileInterviews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentProfileResults'
ALTER TABLE [dbo].[HumanRecruitmentProfileResults]
ADD CONSTRAINT [PK_HumanRecruitmentProfileResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentProfiles'
ALTER TABLE [dbo].[HumanRecruitmentProfiles]
ADD CONSTRAINT [PK_HumanRecruitmentProfiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentRequirements'
ALTER TABLE [dbo].[HumanRecruitmentRequirements]
ADD CONSTRAINT [PK_HumanRecruitmentRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentReviews'
ALTER TABLE [dbo].[HumanRecruitmentReviews]
ADD CONSTRAINT [PK_HumanRecruitmentReviews]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRecruitmentTasks'
ALTER TABLE [dbo].[HumanRecruitmentTasks]
ADD CONSTRAINT [PK_HumanRecruitmentTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanResultAnswers'
ALTER TABLE [dbo].[HumanResultAnswers]
ADD CONSTRAINT [PK_HumanResultAnswers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanResultQuestions'
ALTER TABLE [dbo].[HumanResultQuestions]
ADD CONSTRAINT [PK_HumanResultQuestions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanRoles'
ALTER TABLE [dbo].[HumanRoles]
ADD CONSTRAINT [PK_HumanRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID], [HumanTrainingPlanID] in table 'HumanTrainingPlanAttachments'
ALTER TABLE [dbo].[HumanTrainingPlanAttachments]
ADD CONSTRAINT [PK_HumanTrainingPlanAttachments]
    PRIMARY KEY CLUSTERED ([ID], [HumanTrainingPlanID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingPlanDetails'
ALTER TABLE [dbo].[HumanTrainingPlanDetails]
ADD CONSTRAINT [PK_HumanTrainingPlanDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingPlanRequirements'
ALTER TABLE [dbo].[HumanTrainingPlanRequirements]
ADD CONSTRAINT [PK_HumanTrainingPlanRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingPlans'
ALTER TABLE [dbo].[HumanTrainingPlans]
ADD CONSTRAINT [PK_HumanTrainingPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingPractionerRanks'
ALTER TABLE [dbo].[HumanTrainingPractionerRanks]
ADD CONSTRAINT [PK_HumanTrainingPractionerRanks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingPractioners'
ALTER TABLE [dbo].[HumanTrainingPractioners]
ADD CONSTRAINT [PK_HumanTrainingPractioners]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingRequirementEmployees'
ALTER TABLE [dbo].[HumanTrainingRequirementEmployees]
ADD CONSTRAINT [PK_HumanTrainingRequirementEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanTrainingRequirements'
ALTER TABLE [dbo].[HumanTrainingRequirements]
ADD CONSTRAINT [PK_HumanTrainingRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'HumanUsers'
ALTER TABLE [dbo].[HumanUsers]
ADD CONSTRAINT [PK_HumanUsers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Notifies'
ALTER TABLE [dbo].[Notifies]
ADD CONSTRAINT [PK_Notifies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductDevelopmentRequirementAttachmentFiles'
ALTER TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles]
ADD CONSTRAINT [PK_ProductDevelopmentRequirementAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductDevelopmentRequirementPlanDocuments'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments]
ADD CONSTRAINT [PK_ProductDevelopmentRequirementPlanDocuments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductDevelopmentRequirementPlans'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans]
ADD CONSTRAINT [PK_ProductDevelopmentRequirementPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductDevelopmentRequirements'
ALTER TABLE [dbo].[ProductDevelopmentRequirements]
ADD CONSTRAINT [PK_ProductDevelopmentRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionCommands'
ALTER TABLE [dbo].[ProductionCommands]
ADD CONSTRAINT [PK_ProductionCommands]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionLevels'
ALTER TABLE [dbo].[ProductionLevels]
ADD CONSTRAINT [PK_ProductionLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPerformHistories'
ALTER TABLE [dbo].[ProductionPerformHistories]
ADD CONSTRAINT [PK_ProductionPerformHistories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPerformMaterias'
ALTER TABLE [dbo].[ProductionPerformMaterias]
ADD CONSTRAINT [PK_ProductionPerformMaterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPerformProductErrors'
ALTER TABLE [dbo].[ProductionPerformProductErrors]
ADD CONSTRAINT [PK_ProductionPerformProductErrors]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPerformResults'
ALTER TABLE [dbo].[ProductionPerformResults]
ADD CONSTRAINT [PK_ProductionPerformResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPerforms'
ALTER TABLE [dbo].[ProductionPerforms]
ADD CONSTRAINT [PK_ProductionPerforms]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPlanEquipments'
ALTER TABLE [dbo].[ProductionPlanEquipments]
ADD CONSTRAINT [PK_ProductionPlanEquipments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPlanMaterials'
ALTER TABLE [dbo].[ProductionPlanMaterials]
ADD CONSTRAINT [PK_ProductionPlanMaterials]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPlanProductDetails'
ALTER TABLE [dbo].[ProductionPlanProductDetails]
ADD CONSTRAINT [PK_ProductionPlanProductDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPlanProducts'
ALTER TABLE [dbo].[ProductionPlanProducts]
ADD CONSTRAINT [PK_ProductionPlanProducts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionPlans'
ALTER TABLE [dbo].[ProductionPlans]
ADD CONSTRAINT [PK_ProductionPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionRequirements'
ALTER TABLE [dbo].[ProductionRequirements]
ADD CONSTRAINT [PK_ProductionRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionShifts'
ALTER TABLE [dbo].[ProductionShifts]
ADD CONSTRAINT [PK_ProductionShifts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionStageCriteriaCategories'
ALTER TABLE [dbo].[ProductionStageCriteriaCategories]
ADD CONSTRAINT [PK_ProductionStageCriteriaCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionStageEquipments'
ALTER TABLE [dbo].[ProductionStageEquipments]
ADD CONSTRAINT [PK_ProductionStageEquipments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionStageMaterials'
ALTER TABLE [dbo].[ProductionStageMaterials]
ADD CONSTRAINT [PK_ProductionStageMaterials]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionStageProducts'
ALTER TABLE [dbo].[ProductionStageProducts]
ADD CONSTRAINT [PK_ProductionStageProducts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProductionStages'
ALTER TABLE [dbo].[ProductionStages]
ADD CONSTRAINT [PK_ProductionStages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileAttachmentFiles'
ALTER TABLE [dbo].[ProfileAttachmentFiles]
ADD CONSTRAINT [PK_ProfileAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileBorrowCategories'
ALTER TABLE [dbo].[ProfileBorrowCategories]
ADD CONSTRAINT [PK_ProfileBorrowCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileBorrows'
ALTER TABLE [dbo].[ProfileBorrows]
ADD CONSTRAINT [PK_ProfileBorrows]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileCancelEmployees'
ALTER TABLE [dbo].[ProfileCancelEmployees]
ADD CONSTRAINT [PK_ProfileCancelEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileCancelMethods'
ALTER TABLE [dbo].[ProfileCancelMethods]
ADD CONSTRAINT [PK_ProfileCancelMethods]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileCancelProfiles'
ALTER TABLE [dbo].[ProfileCancelProfiles]
ADD CONSTRAINT [PK_ProfileCancelProfiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileCancels'
ALTER TABLE [dbo].[ProfileCancels]
ADD CONSTRAINT [PK_ProfileCancels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileCategories'
ALTER TABLE [dbo].[ProfileCategories]
ADD CONSTRAINT [PK_ProfileCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Profiles'
ALTER TABLE [dbo].[Profiles]
ADD CONSTRAINT [PK_Profiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ProfileSecurities'
ALTER TABLE [dbo].[ProfileSecurities]
ADD CONSTRAINT [PK_ProfileSecurities]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditPlans'
ALTER TABLE [dbo].[QualityAuditPlans]
ADD CONSTRAINT [PK_QualityAuditPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditProgramDepartments'
ALTER TABLE [dbo].[QualityAuditProgramDepartments]
ADD CONSTRAINT [PK_QualityAuditProgramDepartments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditProgramEmployees'
ALTER TABLE [dbo].[QualityAuditProgramEmployees]
ADD CONSTRAINT [PK_QualityAuditProgramEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditProgramISOes'
ALTER TABLE [dbo].[QualityAuditProgramISOes]
ADD CONSTRAINT [PK_QualityAuditProgramISOes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditPrograms'
ALTER TABLE [dbo].[QualityAuditPrograms]
ADD CONSTRAINT [PK_QualityAuditPrograms]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditRecordedVotes'
ALTER TABLE [dbo].[QualityAuditRecordedVotes]
ADD CONSTRAINT [PK_QualityAuditRecordedVotes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityAuditResults'
ALTER TABLE [dbo].[QualityAuditResults]
ADD CONSTRAINT [PK_QualityAuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityCAPARequirements'
ALTER TABLE [dbo].[QualityCAPARequirements]
ADD CONSTRAINT [PK_QualityCAPARequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityCAPAs'
ALTER TABLE [dbo].[QualityCAPAs]
ADD CONSTRAINT [PK_QualityCAPAs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityCAPATasks'
ALTER TABLE [dbo].[QualityCAPATasks]
ADD CONSTRAINT [PK_QualityCAPATasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityCriteriaCategories'
ALTER TABLE [dbo].[QualityCriteriaCategories]
ADD CONSTRAINT [PK_QualityCriteriaCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityCriterias'
ALTER TABLE [dbo].[QualityCriterias]
ADD CONSTRAINT [PK_QualityCriterias]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingAttachmentFiles]
ADD CONSTRAINT [PK_QualityMeetingAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingObjects'
ALTER TABLE [dbo].[QualityMeetingObjects]
ADD CONSTRAINT [PK_QualityMeetingObjects]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingProblems'
ALTER TABLE [dbo].[QualityMeetingProblems]
ADD CONSTRAINT [PK_QualityMeetingProblems]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingResultAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingResultAttachmentFiles]
ADD CONSTRAINT [PK_QualityMeetingResultAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingResults'
ALTER TABLE [dbo].[QualityMeetingResults]
ADD CONSTRAINT [PK_QualityMeetingResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetings'
ALTER TABLE [dbo].[QualityMeetings]
ADD CONSTRAINT [PK_QualityMeetings]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityMeetingTasks'
ALTER TABLE [dbo].[QualityMeetingTasks]
ADD CONSTRAINT [PK_QualityMeetingTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityNCEmployees'
ALTER TABLE [dbo].[QualityNCEmployees]
ADD CONSTRAINT [PK_QualityNCEmployees]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityNCRoles'
ALTER TABLE [dbo].[QualityNCRoles]
ADD CONSTRAINT [PK_QualityNCRoles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityNCs'
ALTER TABLE [dbo].[QualityNCs]
ADD CONSTRAINT [PK_QualityNCs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityNCStockAdjustments'
ALTER TABLE [dbo].[QualityNCStockAdjustments]
ADD CONSTRAINT [PK_QualityNCStockAdjustments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityNCTasks'
ALTER TABLE [dbo].[QualityNCTasks]
ADD CONSTRAINT [PK_QualityNCTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityPlans'
ALTER TABLE [dbo].[QualityPlans]
ADD CONSTRAINT [PK_QualityPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityPlanTasks'
ALTER TABLE [dbo].[QualityPlanTasks]
ADD CONSTRAINT [PK_QualityPlanTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityTargetCategories'
ALTER TABLE [dbo].[QualityTargetCategories]
ADD CONSTRAINT [PK_QualityTargetCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityTargetLevels'
ALTER TABLE [dbo].[QualityTargetLevels]
ADD CONSTRAINT [PK_QualityTargetLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'QualityTargets'
ALTER TABLE [dbo].[QualityTargets]
ADD CONSTRAINT [PK_QualityTargets]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [PK_Reports]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ReportTemplates'
ALTER TABLE [dbo].[ReportTemplates]
ADD CONSTRAINT [PK_ReportTemplates]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskAudits'
ALTER TABLE [dbo].[RiskAudits]
ADD CONSTRAINT [PK_RiskAudits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskContracts'
ALTER TABLE [dbo].[RiskContracts]
ADD CONSTRAINT [PK_RiskContracts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskControls'
ALTER TABLE [dbo].[RiskControls]
ADD CONSTRAINT [PK_RiskControls]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskControlSolutions'
ALTER TABLE [dbo].[RiskControlSolutions]
ADD CONSTRAINT [PK_RiskControlSolutions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskControlTasks'
ALTER TABLE [dbo].[RiskControlTasks]
ADD CONSTRAINT [PK_RiskControlTasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskIncedents'
ALTER TABLE [dbo].[RiskIncedents]
ADD CONSTRAINT [PK_RiskIncedents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskLegals'
ALTER TABLE [dbo].[RiskLegals]
ADD CONSTRAINT [PK_RiskLegals]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskLevels'
ALTER TABLE [dbo].[RiskLevels]
ADD CONSTRAINT [PK_RiskLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskLibrarySolutions'
ALTER TABLE [dbo].[RiskLibrarySolutions]
ADD CONSTRAINT [PK_RiskLibrarySolutions]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'RiskRegulatories'
ALTER TABLE [dbo].[RiskRegulatories]
ADD CONSTRAINT [PK_RiskRegulatories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Risks'
ALTER TABLE [dbo].[Risks]
ADD CONSTRAINT [PK_Risks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceCategories'
ALTER TABLE [dbo].[ServiceCategories]
ADD CONSTRAINT [PK_ServiceCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceCommandContracts'
ALTER TABLE [dbo].[ServiceCommandContracts]
ADD CONSTRAINT [PK_ServiceCommandContracts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceCommands'
ALTER TABLE [dbo].[ServiceCommands]
ADD CONSTRAINT [PK_ServiceCommands]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServicePlans'
ALTER TABLE [dbo].[ServicePlans]
ADD CONSTRAINT [PK_ServicePlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServicePlanStages'
ALTER TABLE [dbo].[ServicePlanStages]
ADD CONSTRAINT [PK_ServicePlanStages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [PK_Services]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'ServiceStages'
ALTER TABLE [dbo].[ServiceStages]
ADD CONSTRAINT [PK_ServiceStages]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockAdjustmentDetails'
ALTER TABLE [dbo].[StockAdjustmentDetails]
ADD CONSTRAINT [PK_StockAdjustmentDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockAdjustments'
ALTER TABLE [dbo].[StockAdjustments]
ADD CONSTRAINT [PK_StockAdjustments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockBuildDetails'
ALTER TABLE [dbo].[StockBuildDetails]
ADD CONSTRAINT [PK_StockBuildDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockBuilds'
ALTER TABLE [dbo].[StockBuilds]
ADD CONSTRAINT [PK_StockBuilds]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockInventories'
ALTER TABLE [dbo].[StockInventories]
ADD CONSTRAINT [PK_StockInventories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockInventoryDetails'
ALTER TABLE [dbo].[StockInventoryDetails]
ADD CONSTRAINT [PK_StockInventoryDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockInwardDetails'
ALTER TABLE [dbo].[StockInwardDetails]
ADD CONSTRAINT [PK_StockInwardDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockInwards'
ALTER TABLE [dbo].[StockInwards]
ADD CONSTRAINT [PK_StockInwards]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockListCurrencies'
ALTER TABLE [dbo].[StockListCurrencies]
ADD CONSTRAINT [PK_StockListCurrencies]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockOutwardDetails'
ALTER TABLE [dbo].[StockOutwardDetails]
ADD CONSTRAINT [PK_StockOutwardDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockOutwards'
ALTER TABLE [dbo].[StockOutwards]
ADD CONSTRAINT [PK_StockOutwards]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProductBuilds'
ALTER TABLE [dbo].[StockProductBuilds]
ADD CONSTRAINT [PK_StockProductBuilds]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProductGroups'
ALTER TABLE [dbo].[StockProductGroups]
ADD CONSTRAINT [PK_StockProductGroups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProductModels'
ALTER TABLE [dbo].[StockProductModels]
ADD CONSTRAINT [PK_StockProductModels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProductPrices'
ALTER TABLE [dbo].[StockProductPrices]
ADD CONSTRAINT [PK_StockProductPrices]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProducts'
ALTER TABLE [dbo].[StockProducts]
ADD CONSTRAINT [PK_StockProducts]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockProductTypes'
ALTER TABLE [dbo].[StockProductTypes]
ADD CONSTRAINT [PK_StockProductTypes]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Stocks'
ALTER TABLE [dbo].[Stocks]
ADD CONSTRAINT [PK_Stocks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockTransferDetails'
ALTER TABLE [dbo].[StockTransferDetails]
ADD CONSTRAINT [PK_StockTransferDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockTransferRefs'
ALTER TABLE [dbo].[StockTransferRefs]
ADD CONSTRAINT [PK_StockTransferRefs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockTransfers'
ALTER TABLE [dbo].[StockTransfers]
ADD CONSTRAINT [PK_StockTransfers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'StockUnits'
ALTER TABLE [dbo].[StockUnits]
ADD CONSTRAINT [PK_StockUnits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SupplierAuditPlans'
ALTER TABLE [dbo].[SupplierAuditPlans]
ADD CONSTRAINT [PK_SupplierAuditPlans]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SupplierAuditResults'
ALTER TABLE [dbo].[SupplierAuditResults]
ADD CONSTRAINT [PK_SupplierAuditResults]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SupplierAudits'
ALTER TABLE [dbo].[SupplierAudits]
ADD CONSTRAINT [PK_SupplierAudits]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SupplierBidToAttachmentFiles'
ALTER TABLE [dbo].[SupplierBidToAttachmentFiles]
ADD CONSTRAINT [PK_SupplierBidToAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersBidOrders'
ALTER TABLE [dbo].[SuppliersBidOrders]
ADD CONSTRAINT [PK_SuppliersBidOrders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersBills'
ALTER TABLE [dbo].[SuppliersBills]
ADD CONSTRAINT [PK_SuppliersBills]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersGroups'
ALTER TABLE [dbo].[SuppliersGroups]
ADD CONSTRAINT [PK_SuppliersGroups]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersOrderDetails'
ALTER TABLE [dbo].[SuppliersOrderDetails]
ADD CONSTRAINT [PK_SuppliersOrderDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersOrderRequirementDetails'
ALTER TABLE [dbo].[SuppliersOrderRequirementDetails]
ADD CONSTRAINT [PK_SuppliersOrderRequirementDetails]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersOrderRequirements'
ALTER TABLE [dbo].[SuppliersOrderRequirements]
ADD CONSTRAINT [PK_SuppliersOrderRequirements]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'SuppliersOrders'
ALTER TABLE [dbo].[SuppliersOrders]
ADD CONSTRAINT [PK_SuppliersOrders]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskAttachmentFiles'
ALTER TABLE [dbo].[TaskAttachmentFiles]
ADD CONSTRAINT [PK_TaskAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskCalendarEvents'
ALTER TABLE [dbo].[TaskCalendarEvents]
ADD CONSTRAINT [PK_TaskCalendarEvents]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskCalendars'
ALTER TABLE [dbo].[TaskCalendars]
ADD CONSTRAINT [PK_TaskCalendars]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskCategories'
ALTER TABLE [dbo].[TaskCategories]
ADD CONSTRAINT [PK_TaskCategories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskCCs'
ALTER TABLE [dbo].[TaskCCs]
ADD CONSTRAINT [PK_TaskCCs]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskChecks'
ALTER TABLE [dbo].[TaskChecks]
ADD CONSTRAINT [PK_TaskChecks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskComments'
ALTER TABLE [dbo].[TaskComments]
ADD CONSTRAINT [PK_TaskComments]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskHistories'
ALTER TABLE [dbo].[TaskHistories]
ADD CONSTRAINT [PK_TaskHistories]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskLevels'
ALTER TABLE [dbo].[TaskLevels]
ADD CONSTRAINT [PK_TaskLevels]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskPerformAttachmentFiles'
ALTER TABLE [dbo].[TaskPerformAttachmentFiles]
ADD CONSTRAINT [PK_TaskPerformAttachmentFiles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskPerforms'
ALTER TABLE [dbo].[TaskPerforms]
ADD CONSTRAINT [PK_TaskPerforms]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskPersonals'
ALTER TABLE [dbo].[TaskPersonals]
ADD CONSTRAINT [PK_TaskPersonals]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TaskRequests'
ALTER TABLE [dbo].[TaskRequests]
ADD CONSTRAINT [PK_TaskRequests]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [PK_Tasks]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TemplateExportFields'
ALTER TABLE [dbo].[TemplateExportFields]
ADD CONSTRAINT [PK_TemplateExportFields]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TemplateExportFieldStyles'
ALTER TABLE [dbo].[TemplateExportFieldStyles]
ADD CONSTRAINT [PK_TemplateExportFieldStyles]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [ID] in table 'TemplateExports'
ALTER TABLE [dbo].[TemplateExports]
ADD CONSTRAINT [PK_TemplateExports]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [AccountingPaymentID] in table 'AccountingPaymentPlans'
ALTER TABLE [dbo].[AccountingPaymentPlans]
ADD CONSTRAINT [FK_dbo_AccountingPaymentPlans_dbo_AccountingPayments_AccountingPaymentID]
    FOREIGN KEY ([AccountingPaymentID])
    REFERENCES [dbo].[AccountingPayments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountingPaymentPlans_dbo_AccountingPayments_AccountingPaymentID'
CREATE INDEX [IX_FK_dbo_AccountingPaymentPlans_dbo_AccountingPayments_AccountingPaymentID]
ON [dbo].[AccountingPaymentPlans]
    ([AccountingPaymentID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'AccountingPaymentPlans'
ALTER TABLE [dbo].[AccountingPaymentPlans]
ADD CONSTRAINT [FK_dbo_AccountingPaymentPlans_dbo_QualityPlans_QualityPlanID]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountingPaymentPlans_dbo_QualityPlans_QualityPlanID'
CREATE INDEX [IX_FK_dbo_AccountingPaymentPlans_dbo_QualityPlans_QualityPlanID]
ON [dbo].[AccountingPaymentPlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [CustomerContractID] in table 'AccountingPayments'
ALTER TABLE [dbo].[AccountingPayments]
ADD CONSTRAINT [FK_dbo_AccountingPayments_dbo_CustomerContracts_CustomerContractID]
    FOREIGN KEY ([CustomerContractID])
    REFERENCES [dbo].[CustomerContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AccountingPayments_dbo_CustomerContracts_CustomerContractID'
CREATE INDEX [IX_FK_dbo_AccountingPayments_dbo_CustomerContracts_CustomerContractID]
ON [dbo].[AccountingPayments]
    ([CustomerContractID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'CustomerContactHistoryAttachmentFiles'
ALTER TABLE [dbo].[CustomerContactHistoryAttachmentFiles]
ADD CONSTRAINT [FK_CustomerContactHistoryAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContactHistoryAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_CustomerContactHistoryAttachmentFiles_AttachmentFiles]
ON [dbo].[CustomerContactHistoryAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'CustomerContacts'
ALTER TABLE [dbo].[CustomerContacts]
ADD CONSTRAINT [FK_CustomerContacts_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContacts_AttachmentFiles'
CREATE INDEX [IX_FK_CustomerContacts_AttachmentFiles]
ON [dbo].[CustomerContacts]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'CustomerContractAttachmentFiles'
ALTER TABLE [dbo].[CustomerContractAttachmentFiles]
ADD CONSTRAINT [FK_CustomerContractAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContractAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_CustomerContractAttachmentFiles_AttachmentFiles]
ON [dbo].[CustomerContractAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'CustomerOrderAttachmentFiles'
ALTER TABLE [dbo].[CustomerOrderAttachmentFiles]
ADD CONSTRAINT [FK_CustomerOrderAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerOrderAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_CustomerOrderAttachmentFiles_AttachmentFiles]
ON [dbo].[CustomerOrderAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DepartmentLegalAttachments'
ALTER TABLE [dbo].[DepartmentLegalAttachments]
ADD CONSTRAINT [FK_DepartmentLegalAttachments_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentLegalAttachments_AttachmentFiles'
CREATE INDEX [IX_FK_DepartmentLegalAttachments_AttachmentFiles]
ON [dbo].[DepartmentLegalAttachments]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DepartmentPolicyAttachments'
ALTER TABLE [dbo].[DepartmentPolicyAttachments]
ADD CONSTRAINT [FK_DepartmentPolicyAttachments_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentPolicyAttachments_AttachmentFiles'
CREATE INDEX [IX_FK_DepartmentPolicyAttachments_AttachmentFiles]
ON [dbo].[DepartmentPolicyAttachments]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DepartmentRegulatoryAttachments'
ALTER TABLE [dbo].[DepartmentRegulatoryAttachments]
ADD CONSTRAINT [FK_DepartmentRegulatoryAttachments_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRegulatoryAttachments_AttachmentFiles'
CREATE INDEX [IX_FK_DepartmentRegulatoryAttachments_AttachmentFiles]
ON [dbo].[DepartmentRegulatoryAttachments]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DepartmentRequirmentAttachments'
ALTER TABLE [dbo].[DepartmentRequirmentAttachments]
ADD CONSTRAINT [FK_DepartmentRequirmentAttachments_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRequirmentAttachments_AttachmentFiles'
CREATE INDEX [IX_FK_DepartmentRequirmentAttachments_AttachmentFiles]
ON [dbo].[DepartmentRequirmentAttachments]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DispatchGoAttachmentFiles'
ALTER TABLE [dbo].[DispatchGoAttachmentFiles]
ADD CONSTRAINT [FK_DispatchGoAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchGoAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_DispatchGoAttachmentFiles_AttachmentFiles]
ON [dbo].[DispatchGoAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DispatchToAttachmentFiles'
ALTER TABLE [dbo].[DispatchToAttachmentFiles]
ADD CONSTRAINT [FK_DispatchToAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchToAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_DispatchToAttachmentFiles_AttachmentFiles]
ON [dbo].[DispatchToAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'DocumentAttachmentFiles'
ALTER TABLE [dbo].[DocumentAttachmentFiles]
ADD CONSTRAINT [FK_DocumentAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_DocumentAttachmentFiles_AttachmentFiles]
ON [dbo].[DocumentAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'EquipmentMeasureAttachmentFiles'
ALTER TABLE [dbo].[EquipmentMeasureAttachmentFiles]
ADD CONSTRAINT [FK_EquipmentMeasureAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_EquipmentMeasureAttachmentFiles_AttachmentFiles]
ON [dbo].[EquipmentMeasureAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'EquipmentProductionAttachmentFiles'
ALTER TABLE [dbo].[EquipmentProductionAttachmentFiles]
ADD CONSTRAINT [FK_EquipmentProductionAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_EquipmentProductionAttachmentFiles_AttachmentFiles]
ON [dbo].[EquipmentProductionAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'HumanEmployees'
ALTER TABLE [dbo].[HumanEmployees]
ADD CONSTRAINT [FK_HumanEmployees_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanEmployees_AttachmentFiles'
CREATE INDEX [IX_FK_HumanEmployees_AttachmentFiles]
ON [dbo].[HumanEmployees]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'HumanProfileAttachments'
ALTER TABLE [dbo].[HumanProfileAttachments]
ADD CONSTRAINT [FK_HumanProfileAttachments_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanProfileAttachments_AttachmentFiles'
CREATE INDEX [IX_FK_HumanProfileAttachments_AttachmentFiles]
ON [dbo].[HumanProfileAttachments]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'ProductDevelopmentRequirementAttachmentFiles'
ALTER TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles]
ADD CONSTRAINT [FK_ProductDevelopmentRequirementAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirementAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_ProductDevelopmentRequirementAttachmentFiles_AttachmentFiles]
ON [dbo].[ProductDevelopmentRequirementAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'ProfileAttachmentFiles'
ALTER TABLE [dbo].[ProfileAttachmentFiles]
ADD CONSTRAINT [FK_ProfileAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProfileAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_ProfileAttachmentFiles_AttachmentFiles]
ON [dbo].[ProfileAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'QualityMeetingAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingAttachmentFiles]
ADD CONSTRAINT [FK_QualityMeetingAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityMeetingAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_QualityMeetingAttachmentFiles_AttachmentFiles]
ON [dbo].[QualityMeetingAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'QualityMeetingResultAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingResultAttachmentFiles]
ADD CONSTRAINT [FK_QualityMeetingResultAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityMeetingResultAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_QualityMeetingResultAttachmentFiles_AttachmentFiles]
ON [dbo].[QualityMeetingResultAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'SupplierBidToAttachmentFiles'
ALTER TABLE [dbo].[SupplierBidToAttachmentFiles]
ADD CONSTRAINT [FK_SupplierBidToAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierBidToAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_SupplierBidToAttachmentFiles_AttachmentFiles]
ON [dbo].[SupplierBidToAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_Suppliers_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Suppliers_AttachmentFiles'
CREATE INDEX [IX_FK_Suppliers_AttachmentFiles]
ON [dbo].[Suppliers]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'TaskAttachmentFiles'
ALTER TABLE [dbo].[TaskAttachmentFiles]
ADD CONSTRAINT [FK_TaskAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_TaskAttachmentFiles_AttachmentFiles]
ON [dbo].[TaskAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'TaskPerformAttachmentFiles'
ALTER TABLE [dbo].[TaskPerformAttachmentFiles]
ADD CONSTRAINT [FK_TaskPerformAttachmentFiles_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskPerformAttachmentFiles_AttachmentFiles'
CREATE INDEX [IX_FK_TaskPerformAttachmentFiles_AttachmentFiles]
ON [dbo].[TaskPerformAttachmentFiles]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AttachmentFileID] in table 'TemplateExports'
ALTER TABLE [dbo].[TemplateExports]
ADD CONSTRAINT [FK_TemplateExports_AttachmentFiles]
    FOREIGN KEY ([AttachmentFileID])
    REFERENCES [dbo].[AttachmentFiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TemplateExports_AttachmentFiles'
CREATE INDEX [IX_FK_TemplateExports_AttachmentFiles]
ON [dbo].[TemplateExports]
    ([AttachmentFileID]);
GO

-- Creating foreign key on [AuditID] in table 'AuditResults'
ALTER TABLE [dbo].[AuditResults]
ADD CONSTRAINT [FK_dbo_AuditResults_dbo_Audits_AuditID]
    FOREIGN KEY ([AuditID])
    REFERENCES [dbo].[Audits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AuditResults_dbo_Audits_AuditID'
CREATE INDEX [IX_FK_dbo_AuditResults_dbo_Audits_AuditID]
ON [dbo].[AuditResults]
    ([AuditID]);
GO

-- Creating foreign key on [QualityCriteriaID] in table 'AuditResults'
ALTER TABLE [dbo].[AuditResults]
ADD CONSTRAINT [FK_dbo_AuditResults_dbo_QualityCriterias_QualityCriteriaID]
    FOREIGN KEY ([QualityCriteriaID])
    REFERENCES [dbo].[QualityCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AuditResults_dbo_QualityCriterias_QualityCriteriaID'
CREATE INDEX [IX_FK_dbo_AuditResults_dbo_QualityCriterias_QualityCriteriaID]
ON [dbo].[AuditResults]
    ([QualityCriteriaID]);
GO

-- Creating foreign key on [QualityNCID] in table 'AuditResults'
ALTER TABLE [dbo].[AuditResults]
ADD CONSTRAINT [FK_dbo_AuditResults_dbo_QualityNCs_QualityNCID]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AuditResults_dbo_QualityNCs_QualityNCID'
CREATE INDEX [IX_FK_dbo_AuditResults_dbo_QualityNCs_QualityNCID]
ON [dbo].[AuditResults]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityCriteriaCategoryID] in table 'Audits'
ALTER TABLE [dbo].[Audits]
ADD CONSTRAINT [FK_dbo_Audits_dbo_QualityCriteriaCategories_QualityCriteriaCategoryID]
    FOREIGN KEY ([QualityCriteriaCategoryID])
    REFERENCES [dbo].[QualityCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Audits_dbo_QualityCriteriaCategories_QualityCriteriaCategoryID'
CREATE INDEX [IX_FK_dbo_Audits_dbo_QualityCriteriaCategories_QualityCriteriaCategoryID]
ON [dbo].[Audits]
    ([QualityCriteriaCategoryID]);
GO

-- Creating foreign key on [AuditID] in table 'CustomerAssesses'
ALTER TABLE [dbo].[CustomerAssesses]
ADD CONSTRAINT [FK_dbo_CustomerAssesses_dbo_Audits_AuditID]
    FOREIGN KEY ([AuditID])
    REFERENCES [dbo].[Audits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerAssesses_dbo_Audits_AuditID'
CREATE INDEX [IX_FK_dbo_CustomerAssesses_dbo_Audits_AuditID]
ON [dbo].[CustomerAssesses]
    ([AuditID]);
GO

-- Creating foreign key on [AuditID] in table 'CustomerCampaignAudits'
ALTER TABLE [dbo].[CustomerCampaignAudits]
ADD CONSTRAINT [FK_dbo_CustomerCampaignAudits_dbo_Audits_AuditID]
    FOREIGN KEY ([AuditID])
    REFERENCES [dbo].[Audits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignAudits_dbo_Audits_AuditID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignAudits_dbo_Audits_AuditID]
ON [dbo].[CustomerCampaignAudits]
    ([AuditID]);
GO

-- Creating foreign key on [AuditID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_dbo_Tasks_dbo_Audits_AuditID]
    FOREIGN KEY ([AuditID])
    REFERENCES [dbo].[Audits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Tasks_dbo_Audits_AuditID'
CREATE INDEX [IX_FK_dbo_Tasks_dbo_Audits_AuditID]
ON [dbo].[Tasks]
    ([AuditID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'CalendarCategories'
ALTER TABLE [dbo].[CalendarCategories]
ADD CONSTRAINT [FK_CalendarCategories_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarCategories_HumanDepartments'
CREATE INDEX [IX_FK_CalendarCategories_HumanDepartments]
ON [dbo].[CalendarCategories]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [CalendarCategoryID] in table 'CalendarOverrides'
ALTER TABLE [dbo].[CalendarOverrides]
ADD CONSTRAINT [FK_CalendarOverrides_CalendarCategories]
    FOREIGN KEY ([CalendarCategoryID])
    REFERENCES [dbo].[CalendarCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarOverrides_CalendarCategories'
CREATE INDEX [IX_FK_CalendarOverrides_CalendarCategories]
ON [dbo].[CalendarOverrides]
    ([CalendarCategoryID]);
GO

-- Creating foreign key on [TaskCalendarEventID] in table 'CalendarOverrides'
ALTER TABLE [dbo].[CalendarOverrides]
ADD CONSTRAINT [FK_CalendarOverrides_TaskCalendarEvents]
    FOREIGN KEY ([TaskCalendarEventID])
    REFERENCES [dbo].[TaskCalendarEvents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarOverrides_TaskCalendarEvents'
CREATE INDEX [IX_FK_CalendarOverrides_TaskCalendarEvents]
ON [dbo].[CalendarOverrides]
    ([TaskCalendarEventID]);
GO

-- Creating foreign key on [CalendarOverrideID] in table 'CalendarTimeOverrides'
ALTER TABLE [dbo].[CalendarTimeOverrides]
ADD CONSTRAINT [FK_CalendarTimeOverrides_CalendarOverrides]
    FOREIGN KEY ([CalendarOverrideID])
    REFERENCES [dbo].[CalendarOverrides]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CalendarTimeOverrides_CalendarOverrides'
CREATE INDEX [IX_FK_CalendarTimeOverrides_CalendarOverrides]
ON [dbo].[CalendarTimeOverrides]
    ([CalendarOverrideID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'Chats'
ALTER TABLE [dbo].[Chats]
ADD CONSTRAINT [FK_dbo_Chats_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Chats_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_Chats_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[Chats]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [CustomerCriteriaCategoryID] in table 'CustomerAssesses'
ALTER TABLE [dbo].[CustomerAssesses]
ADD CONSTRAINT [FK_CustomerAssesses_CustomerCriteriaCategories]
    FOREIGN KEY ([CustomerCriteriaCategoryID])
    REFERENCES [dbo].[CustomerCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerAssesses_CustomerCriteriaCategories'
CREATE INDEX [IX_FK_CustomerAssesses_CustomerCriteriaCategories]
ON [dbo].[CustomerAssesses]
    ([CustomerCriteriaCategoryID]);
GO

-- Creating foreign key on [CustomerAssessID] in table 'CustomerAssessObjects'
ALTER TABLE [dbo].[CustomerAssessObjects]
ADD CONSTRAINT [FK_dbo_CustomerAssessObjects_dbo_CustomerAssesses_CustomerAssessID]
    FOREIGN KEY ([CustomerAssessID])
    REFERENCES [dbo].[CustomerAssesses]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerAssessObjects_dbo_CustomerAssesses_CustomerAssessID'
CREATE INDEX [IX_FK_dbo_CustomerAssessObjects_dbo_CustomerAssesses_CustomerAssessID]
ON [dbo].[CustomerAssessObjects]
    ([CustomerAssessID]);
GO

-- Creating foreign key on [CustomerAssessID] in table 'CustomerAssessResults'
ALTER TABLE [dbo].[CustomerAssessResults]
ADD CONSTRAINT [FK_dbo_CustomerAssessResults_dbo_CustomerAssesses_CustomerAssessID]
    FOREIGN KEY ([CustomerAssessID])
    REFERENCES [dbo].[CustomerAssesses]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerAssessResults_dbo_CustomerAssesses_CustomerAssessID'
CREATE INDEX [IX_FK_dbo_CustomerAssessResults_dbo_CustomerAssesses_CustomerAssessID]
ON [dbo].[CustomerAssessResults]
    ([CustomerAssessID]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'CustomerAssessObjects'
ALTER TABLE [dbo].[CustomerAssessObjects]
ADD CONSTRAINT [FK_dbo_CustomerAssessObjects_dbo_CustomerCategories_CustomerCategoryID]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[CustomerCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerAssessObjects_dbo_CustomerCategories_CustomerCategoryID'
CREATE INDEX [IX_FK_dbo_CustomerAssessObjects_dbo_CustomerCategories_CustomerCategoryID]
ON [dbo].[CustomerAssessObjects]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerCriteriaID] in table 'CustomerAssessResults'
ALTER TABLE [dbo].[CustomerAssessResults]
ADD CONSTRAINT [FK_dbo_CustomerAssessResults_dbo_CustomerCriterias_CustomerCriteriaID]
    FOREIGN KEY ([CustomerCriteriaID])
    REFERENCES [dbo].[CustomerCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerAssessResults_dbo_CustomerCriterias_CustomerCriteriaID'
CREATE INDEX [IX_FK_dbo_CustomerAssessResults_dbo_CustomerCriterias_CustomerCriteriaID]
ON [dbo].[CustomerAssessResults]
    ([CustomerCriteriaID]);
GO

-- Creating foreign key on [CustomerCampaignID] in table 'CustomerCampaignAudits'
ALTER TABLE [dbo].[CustomerCampaignAudits]
ADD CONSTRAINT [FK_dbo_CustomerCampaignAudits_dbo_CustomerCampaigns_CustomerCampaignID]
    FOREIGN KEY ([CustomerCampaignID])
    REFERENCES [dbo].[CustomerCampaigns]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignAudits_dbo_CustomerCampaigns_CustomerCampaignID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignAudits_dbo_CustomerCampaigns_CustomerCampaignID]
ON [dbo].[CustomerCampaignAudits]
    ([CustomerCampaignID]);
GO

-- Creating foreign key on [CustomerCampaignID] in table 'CustomerCampaignPlans'
ALTER TABLE [dbo].[CustomerCampaignPlans]
ADD CONSTRAINT [FK_dbo_CustomerCampaignPlans_dbo_CustomerCampaigns_CustomerCampaignID]
    FOREIGN KEY ([CustomerCampaignID])
    REFERENCES [dbo].[CustomerCampaigns]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignPlans_dbo_CustomerCampaigns_CustomerCampaignID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignPlans_dbo_CustomerCampaigns_CustomerCampaignID]
ON [dbo].[CustomerCampaignPlans]
    ([CustomerCampaignID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'CustomerCampaignPlans'
ALTER TABLE [dbo].[CustomerCampaignPlans]
ADD CONSTRAINT [FK_dbo_CustomerCampaignPlans_dbo_QualityPlans_QualityPlanID]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignPlans_dbo_QualityPlans_QualityPlanID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignPlans_dbo_QualityPlans_QualityPlanID]
ON [dbo].[CustomerCampaignPlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [CustomerCampaignID] in table 'CustomerCampaignTargets'
ALTER TABLE [dbo].[CustomerCampaignTargets]
ADD CONSTRAINT [FK_dbo_CustomerCampaignTargets_dbo_CustomerCampaigns_CustomerCampaignID]
    FOREIGN KEY ([CustomerCampaignID])
    REFERENCES [dbo].[CustomerCampaigns]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignTargets_dbo_CustomerCampaigns_CustomerCampaignID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignTargets_dbo_CustomerCampaigns_CustomerCampaignID]
ON [dbo].[CustomerCampaignTargets]
    ([CustomerCampaignID]);
GO

-- Creating foreign key on [QualityTargetID] in table 'CustomerCampaignTargets'
ALTER TABLE [dbo].[CustomerCampaignTargets]
ADD CONSTRAINT [FK_dbo_CustomerCampaignTargets_dbo_QualityTargets_QualityTargetID]
    FOREIGN KEY ([QualityTargetID])
    REFERENCES [dbo].[QualityTargets]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCampaignTargets_dbo_QualityTargets_QualityTargetID'
CREATE INDEX [IX_FK_dbo_CustomerCampaignTargets_dbo_QualityTargets_QualityTargetID]
ON [dbo].[CustomerCampaignTargets]
    ([QualityTargetID]);
GO

-- Creating foreign key on [CustomerCareID] in table 'CustomerCareObjects'
ALTER TABLE [dbo].[CustomerCareObjects]
ADD CONSTRAINT [FK_dbo_CustomerCareObjects_dbo_CustomerCares_CustomerCareID]
    FOREIGN KEY ([CustomerCareID])
    REFERENCES [dbo].[CustomerCares]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCareObjects_dbo_CustomerCares_CustomerCareID'
CREATE INDEX [IX_FK_dbo_CustomerCareObjects_dbo_CustomerCares_CustomerCareID]
ON [dbo].[CustomerCareObjects]
    ([CustomerCareID]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'CustomerCareObjects'
ALTER TABLE [dbo].[CustomerCareObjects]
ADD CONSTRAINT [FK_dbo_CustomerCareObjects_dbo_CustomerCategories_CustomerCategoryID]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[CustomerCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCareObjects_dbo_CustomerCategories_CustomerCategoryID'
CREATE INDEX [IX_FK_dbo_CustomerCareObjects_dbo_CustomerCategories_CustomerCategoryID]
ON [dbo].[CustomerCareObjects]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerCareID] in table 'CustomerCareResults'
ALTER TABLE [dbo].[CustomerCareResults]
ADD CONSTRAINT [FK_dbo_CustomerCareResults_dbo_CustomerCares_CustomerCareID]
    FOREIGN KEY ([CustomerCareID])
    REFERENCES [dbo].[CustomerCares]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCareResults_dbo_CustomerCares_CustomerCareID'
CREATE INDEX [IX_FK_dbo_CustomerCareResults_dbo_CustomerCares_CustomerCareID]
ON [dbo].[CustomerCareResults]
    ([CustomerCareID]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'CustomerCategoryDepartments'
ALTER TABLE [dbo].[CustomerCategoryDepartments]
ADD CONSTRAINT [FK_CustomerCategoryDepartments_CustomerCategories]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[CustomerCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCategoryDepartments_CustomerCategories'
CREATE INDEX [IX_FK_CustomerCategoryDepartments_CustomerCategories]
ON [dbo].[CustomerCategoryDepartments]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'CustomerCategoryCustomers'
ALTER TABLE [dbo].[CustomerCategoryCustomers]
ADD CONSTRAINT [FK_dbo_CustomerCategoryCustomers_dbo_CustomerCategories_CustomerCategoryID]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[CustomerCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCategoryCustomers_dbo_CustomerCategories_CustomerCategoryID'
CREATE INDEX [IX_FK_dbo_CustomerCategoryCustomers_dbo_CustomerCategories_CustomerCategoryID]
ON [dbo].[CustomerCategoryCustomers]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerCategoryID] in table 'CustomerSurveyObjects'
ALTER TABLE [dbo].[CustomerSurveyObjects]
ADD CONSTRAINT [FK_dbo_CustomerSurveyObjects_dbo_CustomerCategories_CustomerCategoryID]
    FOREIGN KEY ([CustomerCategoryID])
    REFERENCES [dbo].[CustomerCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerSurveyObjects_dbo_CustomerCategories_CustomerCategoryID'
CREATE INDEX [IX_FK_dbo_CustomerSurveyObjects_dbo_CustomerCategories_CustomerCategoryID]
ON [dbo].[CustomerSurveyObjects]
    ([CustomerCategoryID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerCategoryCustomers'
ALTER TABLE [dbo].[CustomerCategoryCustomers]
ADD CONSTRAINT [FK_dbo_CustomerCategoryCustomers_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCategoryCustomers_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerCategoryCustomers_dbo_Customers_CustomerID]
ON [dbo].[CustomerCategoryCustomers]
    ([CustomerID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'CustomerCategoryDepartments'
ALTER TABLE [dbo].[CustomerCategoryDepartments]
ADD CONSTRAINT [FK_CustomerCategoryDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCategoryDepartments_HumanDepartments'
CREATE INDEX [IX_FK_CustomerCategoryDepartments_HumanDepartments]
ON [dbo].[CustomerCategoryDepartments]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [CustomerContactFormID] in table 'CustomerContactCalendars'
ALTER TABLE [dbo].[CustomerContactCalendars]
ADD CONSTRAINT [FK_dbo_CustomerContactCalendars_dbo_CustomerContactForms_CustomerContactFormID]
    FOREIGN KEY ([CustomerContactFormID])
    REFERENCES [dbo].[CustomerContactForms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContactCalendars_dbo_CustomerContactForms_CustomerContactFormID'
CREATE INDEX [IX_FK_dbo_CustomerContactCalendars_dbo_CustomerContactForms_CustomerContactFormID]
ON [dbo].[CustomerContactCalendars]
    ([CustomerContactFormID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerContactCalendars'
ALTER TABLE [dbo].[CustomerContactCalendars]
ADD CONSTRAINT [FK_dbo_CustomerContactCalendars_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContactCalendars_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerContactCalendars_dbo_Customers_CustomerID]
ON [dbo].[CustomerContactCalendars]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerContactFormID] in table 'CustomerContactHistories'
ALTER TABLE [dbo].[CustomerContactHistories]
ADD CONSTRAINT [FK_dbo_CustomerContactHistories_dbo_CustomerContactForms_CustomerContactFormID]
    FOREIGN KEY ([CustomerContactFormID])
    REFERENCES [dbo].[CustomerContactForms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContactHistories_dbo_CustomerContactForms_CustomerContactFormID'
CREATE INDEX [IX_FK_dbo_CustomerContactHistories_dbo_CustomerContactForms_CustomerContactFormID]
ON [dbo].[CustomerContactHistories]
    ([CustomerContactFormID]);
GO

-- Creating foreign key on [CustomerContactHistoryID] in table 'CustomerContactHistoryAttachmentFiles'
ALTER TABLE [dbo].[CustomerContactHistoryAttachmentFiles]
ADD CONSTRAINT [FK_CustomerContactHistoryAttachmentFiles_CustomerContactHistories]
    FOREIGN KEY ([CustomerContactHistoryID])
    REFERENCES [dbo].[CustomerContactHistories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContactHistoryAttachmentFiles_CustomerContactHistories'
CREATE INDEX [IX_FK_CustomerContactHistoryAttachmentFiles_CustomerContactHistories]
ON [dbo].[CustomerContactHistoryAttachmentFiles]
    ([CustomerContactHistoryID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerContactHistories'
ALTER TABLE [dbo].[CustomerContactHistories]
ADD CONSTRAINT [FK_dbo_CustomerContactHistories_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContactHistories_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerContactHistories_dbo_Customers_CustomerID]
ON [dbo].[CustomerContactHistories]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerContacts'
ALTER TABLE [dbo].[CustomerContacts]
ADD CONSTRAINT [FK_dbo_CustomerContacts_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContacts_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerContacts_dbo_Customers_CustomerID]
ON [dbo].[CustomerContacts]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerContractID] in table 'CustomerContractAttachmentFiles'
ALTER TABLE [dbo].[CustomerContractAttachmentFiles]
ADD CONSTRAINT [FK_CustomerContractAttachmentFiles_CustomerContracts]
    FOREIGN KEY ([CustomerContractID])
    REFERENCES [dbo].[CustomerContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContractAttachmentFiles_CustomerContracts'
CREATE INDEX [IX_FK_CustomerContractAttachmentFiles_CustomerContracts]
ON [dbo].[CustomerContractAttachmentFiles]
    ([CustomerContractID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerContracts'
ALTER TABLE [dbo].[CustomerContracts]
ADD CONSTRAINT [FK_dbo_CustomerContracts_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerContracts_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerContracts_dbo_Customers_CustomerID]
ON [dbo].[CustomerContracts]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerContractID] in table 'CustomerOrders'
ALTER TABLE [dbo].[CustomerOrders]
ADD CONSTRAINT [FK_dbo_CustomerOrders_dbo_CustomerContracts_CustomerContractID]
    FOREIGN KEY ([CustomerContractID])
    REFERENCES [dbo].[CustomerContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerOrders_dbo_CustomerContracts_CustomerContractID'
CREATE INDEX [IX_FK_dbo_CustomerOrders_dbo_CustomerContracts_CustomerContractID]
ON [dbo].[CustomerOrders]
    ([CustomerContractID]);
GO

-- Creating foreign key on [CustomerContractID] in table 'ServiceCommandContracts'
ALTER TABLE [dbo].[ServiceCommandContracts]
ADD CONSTRAINT [FK_dbo_ServiceCommandContracts_dbo_CustomerContracts_CustomerContractID]
    FOREIGN KEY ([CustomerContractID])
    REFERENCES [dbo].[CustomerContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServiceCommandContracts_dbo_CustomerContracts_CustomerContractID'
CREATE INDEX [IX_FK_dbo_ServiceCommandContracts_dbo_CustomerContracts_CustomerContractID]
ON [dbo].[ServiceCommandContracts]
    ([CustomerContractID]);
GO

-- Creating foreign key on [CustomerContractID] in table 'RiskContracts'
ALTER TABLE [dbo].[RiskContracts]
ADD CONSTRAINT [FK_RiskContracts_CustomerContracts]
    FOREIGN KEY ([CustomerContractID])
    REFERENCES [dbo].[CustomerContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskContracts_CustomerContracts'
CREATE INDEX [IX_FK_RiskContracts_CustomerContracts]
ON [dbo].[RiskContracts]
    ([CustomerContractID]);
GO

-- Creating foreign key on [CustomerCriteriaCategoryID] in table 'CustomerCriterias'
ALTER TABLE [dbo].[CustomerCriterias]
ADD CONSTRAINT [FK_dbo_CustomerCriterias_dbo_CustomerCriterionCategories_CustomerCriterionCategoryID]
    FOREIGN KEY ([CustomerCriteriaCategoryID])
    REFERENCES [dbo].[CustomerCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerCriterias_dbo_CustomerCriterionCategories_CustomerCriterionCategoryID'
CREATE INDEX [IX_FK_dbo_CustomerCriterias_dbo_CustomerCriterionCategories_CustomerCriterionCategoryID]
ON [dbo].[CustomerCriterias]
    ([CustomerCriteriaCategoryID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerFeedbacks'
ALTER TABLE [dbo].[CustomerFeedbacks]
ADD CONSTRAINT [FK_dbo_CustomerFeedbacks_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerFeedbacks_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerFeedbacks_dbo_Customers_CustomerID]
ON [dbo].[CustomerFeedbacks]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerOrderID] in table 'CustomerOrderAttachmentFiles'
ALTER TABLE [dbo].[CustomerOrderAttachmentFiles]
ADD CONSTRAINT [FK_CustomerOrderAttachmentFiles_CustomerOrders]
    FOREIGN KEY ([CustomerOrderID])
    REFERENCES [dbo].[CustomerOrders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerOrderAttachmentFiles_CustomerOrders'
CREATE INDEX [IX_FK_CustomerOrderAttachmentFiles_CustomerOrders]
ON [dbo].[CustomerOrderAttachmentFiles]
    ([CustomerOrderID]);
GO

-- Creating foreign key on [CustomerID] in table 'CustomerOrders'
ALTER TABLE [dbo].[CustomerOrders]
ADD CONSTRAINT [FK_dbo_CustomerOrders_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerOrders_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_CustomerOrders_dbo_Customers_CustomerID]
ON [dbo].[CustomerOrders]
    ([CustomerID]);
GO

-- Creating foreign key on [ServiceID] in table 'CustomerOrders'
ALTER TABLE [dbo].[CustomerOrders]
ADD CONSTRAINT [FK_dbo_CustomerOrders_dbo_Services_ServiceID]
    FOREIGN KEY ([ServiceID])
    REFERENCES [dbo].[Services]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerOrders_dbo_Services_ServiceID'
CREATE INDEX [IX_FK_dbo_CustomerOrders_dbo_Services_ServiceID]
ON [dbo].[CustomerOrders]
    ([ServiceID]);
GO

-- Creating foreign key on [CustomerID] in table 'ServicePlanStages'
ALTER TABLE [dbo].[ServicePlanStages]
ADD CONSTRAINT [FK_dbo_ServicePlanStages_dbo_Customers_CustomerID]
    FOREIGN KEY ([CustomerID])
    REFERENCES [dbo].[Customers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServicePlanStages_dbo_Customers_CustomerID'
CREATE INDEX [IX_FK_dbo_ServicePlanStages_dbo_Customers_CustomerID]
ON [dbo].[ServicePlanStages]
    ([CustomerID]);
GO

-- Creating foreign key on [CustomerSurveyID] in table 'CustomerSurveyObjects'
ALTER TABLE [dbo].[CustomerSurveyObjects]
ADD CONSTRAINT [FK_dbo_CustomerSurveyObjects_dbo_CustomerSurveys_CustomerSurveyID]
    FOREIGN KEY ([CustomerSurveyID])
    REFERENCES [dbo].[CustomerSurveys]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerSurveyObjects_dbo_CustomerSurveys_CustomerSurveyID'
CREATE INDEX [IX_FK_dbo_CustomerSurveyObjects_dbo_CustomerSurveys_CustomerSurveyID]
ON [dbo].[CustomerSurveyObjects]
    ([CustomerSurveyID]);
GO

-- Creating foreign key on [CustomerSurveyID] in table 'CustomerSurveyQuestions'
ALTER TABLE [dbo].[CustomerSurveyQuestions]
ADD CONSTRAINT [FK_dbo_CustomerSurveyQuestions_dbo_CustomerSurveys_CustomerSurveyID]
    FOREIGN KEY ([CustomerSurveyID])
    REFERENCES [dbo].[CustomerSurveys]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerSurveyQuestions_dbo_CustomerSurveys_CustomerSurveyID'
CREATE INDEX [IX_FK_dbo_CustomerSurveyQuestions_dbo_CustomerSurveys_CustomerSurveyID]
ON [dbo].[CustomerSurveyQuestions]
    ([CustomerSurveyID]);
GO

-- Creating foreign key on [CustomerSurveyQuestionID] in table 'CustomerSurveyResults'
ALTER TABLE [dbo].[CustomerSurveyResults]
ADD CONSTRAINT [FK_dbo_CustomerSurveyResults_dbo_CustomerSurveyQuestions_CustomerSurveyQuestionID]
    FOREIGN KEY ([CustomerSurveyQuestionID])
    REFERENCES [dbo].[CustomerSurveyQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_CustomerSurveyResults_dbo_CustomerSurveyQuestions_CustomerSurveyQuestionID'
CREATE INDEX [IX_FK_dbo_CustomerSurveyResults_dbo_CustomerSurveyQuestions_CustomerSurveyQuestionID]
ON [dbo].[CustomerSurveyResults]
    ([CustomerSurveyQuestionID]);
GO

-- Creating foreign key on [DepartmentBroadCategoryID] in table 'DepartmentBroads'
ALTER TABLE [dbo].[DepartmentBroads]
ADD CONSTRAINT [FK_DepartmentBroads_DepartmentBroadCategories]
    FOREIGN KEY ([DepartmentBroadCategoryID])
    REFERENCES [dbo].[DepartmentBroadCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentBroads_DepartmentBroadCategories'
CREATE INDEX [IX_FK_DepartmentBroads_DepartmentBroadCategories]
ON [dbo].[DepartmentBroads]
    ([DepartmentBroadCategoryID]);
GO

-- Creating foreign key on [DepartmentLegalID] in table 'DepartmentLegalAttachments'
ALTER TABLE [dbo].[DepartmentLegalAttachments]
ADD CONSTRAINT [FK_DepartmentLegalAttachments_DepartmentLegals]
    FOREIGN KEY ([DepartmentLegalID])
    REFERENCES [dbo].[DepartmentLegals]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentLegalAttachments_DepartmentLegals'
CREATE INDEX [IX_FK_DepartmentLegalAttachments_DepartmentLegals]
ON [dbo].[DepartmentLegalAttachments]
    ([DepartmentLegalID]);
GO

-- Creating foreign key on [QualityNCID] in table 'DepartmentLegalAuditResults'
ALTER TABLE [dbo].[DepartmentLegalAuditResults]
ADD CONSTRAINT [FK_DepartmentLegalAuditResults_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentLegalAuditResults_QualityNCs'
CREATE INDEX [IX_FK_DepartmentLegalAuditResults_QualityNCs]
ON [dbo].[DepartmentLegalAuditResults]
    ([QualityNCID]);
GO

-- Creating foreign key on [DepartmentLegalID] in table 'DepartmentLegalAuditResults'
ALTER TABLE [dbo].[DepartmentLegalAuditResults]
ADD CONSTRAINT [FK_DepartmentLegalDepartments_DepartmentLegals]
    FOREIGN KEY ([DepartmentLegalID])
    REFERENCES [dbo].[DepartmentLegals]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentLegalDepartments_DepartmentLegals'
CREATE INDEX [IX_FK_DepartmentLegalDepartments_DepartmentLegals]
ON [dbo].[DepartmentLegalAuditResults]
    ([DepartmentLegalID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'DepartmentLegalAuditResults'
ALTER TABLE [dbo].[DepartmentLegalAuditResults]
ADD CONSTRAINT [FK_DepartmentLegalDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentLegalDepartments_HumanDepartments'
CREATE INDEX [IX_FK_DepartmentLegalDepartments_HumanDepartments]
ON [dbo].[DepartmentLegalAuditResults]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [DepartmentLegalID] in table 'RiskLegals'
ALTER TABLE [dbo].[RiskLegals]
ADD CONSTRAINT [FK_RiskLegals_DepartmentLegals]
    FOREIGN KEY ([DepartmentLegalID])
    REFERENCES [dbo].[DepartmentLegals]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskLegals_DepartmentLegals'
CREATE INDEX [IX_FK_RiskLegals_DepartmentLegals]
ON [dbo].[RiskLegals]
    ([DepartmentLegalID]);
GO

-- Creating foreign key on [DepartmentPolicyID] in table 'DepartmentPolicyAttachments'
ALTER TABLE [dbo].[DepartmentPolicyAttachments]
ADD CONSTRAINT [FK_DepartmentPolicyAttachments_DepartmentPolicies]
    FOREIGN KEY ([DepartmentPolicyID])
    REFERENCES [dbo].[DepartmentPolicies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentPolicyAttachments_DepartmentPolicies'
CREATE INDEX [IX_FK_DepartmentPolicyAttachments_DepartmentPolicies]
ON [dbo].[DepartmentPolicyAttachments]
    ([DepartmentPolicyID]);
GO

-- Creating foreign key on [DepartmentRegulatoryID] in table 'DepartmentRegulatoryAttachments'
ALTER TABLE [dbo].[DepartmentRegulatoryAttachments]
ADD CONSTRAINT [FK_DepartmentRegulatoryAttachments_DepartmentRegulatories]
    FOREIGN KEY ([DepartmentRegulatoryID])
    REFERENCES [dbo].[DepartmentRegulatories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRegulatoryAttachments_DepartmentRegulatories'
CREATE INDEX [IX_FK_DepartmentRegulatoryAttachments_DepartmentRegulatories]
ON [dbo].[DepartmentRegulatoryAttachments]
    ([DepartmentRegulatoryID]);
GO

-- Creating foreign key on [DepartmentRegulatoryID] in table 'DepartmentRegulatoryAuditResults'
ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults]
ADD CONSTRAINT [FK_DepartmentRegulatoryDepartments_DepartmentRegulatories]
    FOREIGN KEY ([DepartmentRegulatoryID])
    REFERENCES [dbo].[DepartmentRegulatories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRegulatoryDepartments_DepartmentRegulatories'
CREATE INDEX [IX_FK_DepartmentRegulatoryDepartments_DepartmentRegulatories]
ON [dbo].[DepartmentRegulatoryAuditResults]
    ([DepartmentRegulatoryID]);
GO

-- Creating foreign key on [DepartmentRegulatoryID] in table 'RiskRegulatories'
ALTER TABLE [dbo].[RiskRegulatories]
ADD CONSTRAINT [FK_RiskRegulatories_DepartmentRegulatories]
    FOREIGN KEY ([DepartmentRegulatoryID])
    REFERENCES [dbo].[DepartmentRegulatories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskRegulatories_DepartmentRegulatories'
CREATE INDEX [IX_FK_RiskRegulatories_DepartmentRegulatories]
ON [dbo].[RiskRegulatories]
    ([DepartmentRegulatoryID]);
GO

-- Creating foreign key on [QualityNCID] in table 'DepartmentRegulatoryAuditResults'
ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults]
ADD CONSTRAINT [FK_DepartmentRegulatoryAuditResults_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRegulatoryAuditResults_QualityNCs'
CREATE INDEX [IX_FK_DepartmentRegulatoryAuditResults_QualityNCs]
ON [dbo].[DepartmentRegulatoryAuditResults]
    ([QualityNCID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'DepartmentRegulatoryAuditResults'
ALTER TABLE [dbo].[DepartmentRegulatoryAuditResults]
ADD CONSTRAINT [FK_DepartmentRegulatoryDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRegulatoryDepartments_HumanDepartments'
CREATE INDEX [IX_FK_DepartmentRegulatoryDepartments_HumanDepartments]
ON [dbo].[DepartmentRegulatoryAuditResults]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [DepartmentRequirmentID] in table 'DepartmentRequirmentAttachments'
ALTER TABLE [dbo].[DepartmentRequirmentAttachments]
ADD CONSTRAINT [FK_DepartmentRequirmentAttachments_DepartmentRequirments]
    FOREIGN KEY ([DepartmentRequirmentID])
    REFERENCES [dbo].[DepartmentRequirments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRequirmentAttachments_DepartmentRequirments'
CREATE INDEX [IX_FK_DepartmentRequirmentAttachments_DepartmentRequirments]
ON [dbo].[DepartmentRequirmentAttachments]
    ([DepartmentRequirmentID]);
GO

-- Creating foreign key on [DepartmentRequirmentID] in table 'DepartmentRequirmentAuditResults'
ALTER TABLE [dbo].[DepartmentRequirmentAuditResults]
ADD CONSTRAINT [FK_DepartmentRequirmentAuditResults_DepartmentRequirments]
    FOREIGN KEY ([DepartmentRequirmentID])
    REFERENCES [dbo].[DepartmentRequirments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRequirmentAuditResults_DepartmentRequirments'
CREATE INDEX [IX_FK_DepartmentRequirmentAuditResults_DepartmentRequirments]
ON [dbo].[DepartmentRequirmentAuditResults]
    ([DepartmentRequirmentID]);
GO

-- Creating foreign key on [QualityNCID] in table 'DepartmentRequirmentAuditResults'
ALTER TABLE [dbo].[DepartmentRequirmentAuditResults]
ADD CONSTRAINT [FK_DepartmentRequirmentAuditResults_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRequirmentAuditResults_QualityNCs'
CREATE INDEX [IX_FK_DepartmentRequirmentAuditResults_QualityNCs]
ON [dbo].[DepartmentRequirmentAuditResults]
    ([QualityNCID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'DepartmentRequirmentAuditResults'
ALTER TABLE [dbo].[DepartmentRequirmentAuditResults]
ADD CONSTRAINT [FK_DepartmentRequirmentDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DepartmentRequirmentDepartments_HumanDepartments'
CREATE INDEX [IX_FK_DepartmentRequirmentDepartments_HumanDepartments]
ON [dbo].[DepartmentRequirmentAuditResults]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [DispatchCategoryID] in table 'DispatchGoes'
ALTER TABLE [dbo].[DispatchGoes]
ADD CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchCategories_DispatchCategoryID]
    FOREIGN KEY ([DispatchCategoryID])
    REFERENCES [dbo].[DispatchCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoes_dbo_DispatchCategories_DispatchCategoryID'
CREATE INDEX [IX_FK_dbo_DispatchGoes_dbo_DispatchCategories_DispatchCategoryID]
ON [dbo].[DispatchGoes]
    ([DispatchCategoryID]);
GO

-- Creating foreign key on [DispatchCategoryID] in table 'DispatchToes'
ALTER TABLE [dbo].[DispatchToes]
ADD CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchCategories_DispatchCategoryID]
    FOREIGN KEY ([DispatchCategoryID])
    REFERENCES [dbo].[DispatchCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToes_dbo_DispatchCategories_DispatchCategoryID'
CREATE INDEX [IX_FK_dbo_DispatchToes_dbo_DispatchCategories_DispatchCategoryID]
ON [dbo].[DispatchToes]
    ([DispatchCategoryID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoAttachmentFiles'
ALTER TABLE [dbo].[DispatchGoAttachmentFiles]
ADD CONSTRAINT [FK_DispatchGoAttachmentFiles_DispatchGoes]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchGoAttachmentFiles_DispatchGoes'
CREATE INDEX [IX_FK_DispatchGoAttachmentFiles_DispatchGoes]
ON [dbo].[DispatchGoAttachmentFiles]
    ([DispatchGoID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoCCs'
ALTER TABLE [dbo].[DispatchGoCCs]
ADD CONSTRAINT [FK_DispatchGoCCs_DispatchGoes]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchGoCCs_DispatchGoes'
CREATE INDEX [IX_FK_DispatchGoCCs_DispatchGoes]
ON [dbo].[DispatchGoCCs]
    ([DispatchGoID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'DispatchGoCCs'
ALTER TABLE [dbo].[DispatchGoCCs]
ADD CONSTRAINT [FK_DispatchGoCCs_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchGoCCs_HumanEmployees'
CREATE INDEX [IX_FK_DispatchGoCCs_HumanEmployees]
ON [dbo].[DispatchGoCCs]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoDepartments'
ALTER TABLE [dbo].[DispatchGoDepartments]
ADD CONSTRAINT [FK_dbo_DispatchGoDepartments_dbo_DispatchGoes_DispatchGoID]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoDepartments_dbo_DispatchGoes_DispatchGoID'
CREATE INDEX [IX_FK_dbo_DispatchGoDepartments_dbo_DispatchGoes_DispatchGoID]
ON [dbo].[DispatchGoDepartments]
    ([DispatchGoID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoEmployees'
ALTER TABLE [dbo].[DispatchGoEmployees]
ADD CONSTRAINT [FK_dbo_DispatchGoEmployees_dbo_DispatchGoes_DispatchGoID]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoEmployees_dbo_DispatchGoes_DispatchGoID'
CREATE INDEX [IX_FK_dbo_DispatchGoEmployees_dbo_DispatchGoes_DispatchGoID]
ON [dbo].[DispatchGoEmployees]
    ([DispatchGoID]);
GO

-- Creating foreign key on [DispatchMethodID] in table 'DispatchGoes'
ALTER TABLE [dbo].[DispatchGoes]
ADD CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchMethods_DispatchMethodID]
    FOREIGN KEY ([DispatchMethodID])
    REFERENCES [dbo].[DispatchMethods]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoes_dbo_DispatchMethods_DispatchMethodID'
CREATE INDEX [IX_FK_dbo_DispatchGoes_dbo_DispatchMethods_DispatchMethodID]
ON [dbo].[DispatchGoes]
    ([DispatchMethodID]);
GO

-- Creating foreign key on [DispatchSecurityID] in table 'DispatchGoes'
ALTER TABLE [dbo].[DispatchGoes]
ADD CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchSecurities_DispatchSecurityID]
    FOREIGN KEY ([DispatchSecurityID])
    REFERENCES [dbo].[DispatchSecurities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoes_dbo_DispatchSecurities_DispatchSecurityID'
CREATE INDEX [IX_FK_dbo_DispatchGoes_dbo_DispatchSecurities_DispatchSecurityID]
ON [dbo].[DispatchGoes]
    ([DispatchSecurityID]);
GO

-- Creating foreign key on [DispatchUrgencyID] in table 'DispatchGoes'
ALTER TABLE [dbo].[DispatchGoes]
ADD CONSTRAINT [FK_dbo_DispatchGoes_dbo_DispatchUrgencies_DispatchUrgencyID]
    FOREIGN KEY ([DispatchUrgencyID])
    REFERENCES [dbo].[DispatchUrgencies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoes_dbo_DispatchUrgencies_DispatchUrgencyID'
CREATE INDEX [IX_FK_dbo_DispatchGoes_dbo_DispatchUrgencies_DispatchUrgencyID]
ON [dbo].[DispatchGoes]
    ([DispatchUrgencyID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoOutSides'
ALTER TABLE [dbo].[DispatchGoOutSides]
ADD CONSTRAINT [FK_dbo_DispatchGoOutSides_dbo_DispatchGoes_DispatchGoID]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoOutSides_dbo_DispatchGoes_DispatchGoID'
CREATE INDEX [IX_FK_dbo_DispatchGoOutSides_dbo_DispatchGoes_DispatchGoID]
ON [dbo].[DispatchGoOutSides]
    ([DispatchGoID]);
GO

-- Creating foreign key on [DispatchGoID] in table 'DispatchGoTasks'
ALTER TABLE [dbo].[DispatchGoTasks]
ADD CONSTRAINT [FK_dbo_DispatchGoTasks_dbo_DispatchGoes_DispatchGoID]
    FOREIGN KEY ([DispatchGoID])
    REFERENCES [dbo].[DispatchGoes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoTasks_dbo_DispatchGoes_DispatchGoID'
CREATE INDEX [IX_FK_dbo_DispatchGoTasks_dbo_DispatchGoes_DispatchGoID]
ON [dbo].[DispatchGoTasks]
    ([DispatchGoID]);
GO

-- Creating foreign key on [TaskID] in table 'DispatchGoTasks'
ALTER TABLE [dbo].[DispatchGoTasks]
ADD CONSTRAINT [FK_dbo_DispatchGoTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchGoTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_DispatchGoTasks_dbo_Tasks_TaskID]
ON [dbo].[DispatchGoTasks]
    ([TaskID]);
GO

-- Creating foreign key on [DispatchMethodID] in table 'DispatchToes'
ALTER TABLE [dbo].[DispatchToes]
ADD CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchMethods_DispatchMethodID]
    FOREIGN KEY ([DispatchMethodID])
    REFERENCES [dbo].[DispatchMethods]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToes_dbo_DispatchMethods_DispatchMethodID'
CREATE INDEX [IX_FK_dbo_DispatchToes_dbo_DispatchMethods_DispatchMethodID]
ON [dbo].[DispatchToes]
    ([DispatchMethodID]);
GO

-- Creating foreign key on [DispatchSecurityID] in table 'DispatchToes'
ALTER TABLE [dbo].[DispatchToes]
ADD CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchSecurities_DispatchSecurityID]
    FOREIGN KEY ([DispatchSecurityID])
    REFERENCES [dbo].[DispatchSecurities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToes_dbo_DispatchSecurities_DispatchSecurityID'
CREATE INDEX [IX_FK_dbo_DispatchToes_dbo_DispatchSecurities_DispatchSecurityID]
ON [dbo].[DispatchToes]
    ([DispatchSecurityID]);
GO

-- Creating foreign key on [DispatchToID] in table 'DispatchToAttachmentFiles'
ALTER TABLE [dbo].[DispatchToAttachmentFiles]
ADD CONSTRAINT [FK_dbo_DispatchToAttachments_dbo_DispatchToes_DispatchToID]
    FOREIGN KEY ([DispatchToID])
    REFERENCES [dbo].[DispatchToes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToAttachments_dbo_DispatchToes_DispatchToID'
CREATE INDEX [IX_FK_dbo_DispatchToAttachments_dbo_DispatchToes_DispatchToID]
ON [dbo].[DispatchToAttachmentFiles]
    ([DispatchToID]);
GO

-- Creating foreign key on [DispatchToID] in table 'DispatchToCCs'
ALTER TABLE [dbo].[DispatchToCCs]
ADD CONSTRAINT [FK_DispatchToCCs_DispatchToes]
    FOREIGN KEY ([DispatchToID])
    REFERENCES [dbo].[DispatchToes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchToCCs_DispatchToes'
CREATE INDEX [IX_FK_DispatchToCCs_DispatchToes]
ON [dbo].[DispatchToCCs]
    ([DispatchToID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'DispatchToCCs'
ALTER TABLE [dbo].[DispatchToCCs]
ADD CONSTRAINT [FK_DispatchToCCs_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DispatchToCCs_HumanEmployees'
CREATE INDEX [IX_FK_DispatchToCCs_HumanEmployees]
ON [dbo].[DispatchToCCs]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [DispatchToID] in table 'DispatchToDepartments'
ALTER TABLE [dbo].[DispatchToDepartments]
ADD CONSTRAINT [FK_dbo_DispatchToDepartments_dbo_DispatchToes_DispatchToID]
    FOREIGN KEY ([DispatchToID])
    REFERENCES [dbo].[DispatchToes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToDepartments_dbo_DispatchToes_DispatchToID'
CREATE INDEX [IX_FK_dbo_DispatchToDepartments_dbo_DispatchToes_DispatchToID]
ON [dbo].[DispatchToDepartments]
    ([DispatchToID]);
GO

-- Creating foreign key on [DispatchToID] in table 'DispatchToEmployees'
ALTER TABLE [dbo].[DispatchToEmployees]
ADD CONSTRAINT [FK_dbo_DispatchToEmployees_dbo_DispatchToes_DispatchToID]
    FOREIGN KEY ([DispatchToID])
    REFERENCES [dbo].[DispatchToes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToEmployees_dbo_DispatchToes_DispatchToID'
CREATE INDEX [IX_FK_dbo_DispatchToEmployees_dbo_DispatchToes_DispatchToID]
ON [dbo].[DispatchToEmployees]
    ([DispatchToID]);
GO

-- Creating foreign key on [DispatchUrgencyID] in table 'DispatchToes'
ALTER TABLE [dbo].[DispatchToes]
ADD CONSTRAINT [FK_dbo_DispatchToes_dbo_DispatchUrgencies_DispatchUrgencyID]
    FOREIGN KEY ([DispatchUrgencyID])
    REFERENCES [dbo].[DispatchUrgencies]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToes_dbo_DispatchUrgencies_DispatchUrgencyID'
CREATE INDEX [IX_FK_dbo_DispatchToes_dbo_DispatchUrgencies_DispatchUrgencyID]
ON [dbo].[DispatchToes]
    ([DispatchUrgencyID]);
GO

-- Creating foreign key on [DispatchToID] in table 'DispatchToTasks'
ALTER TABLE [dbo].[DispatchToTasks]
ADD CONSTRAINT [FK_dbo_DispatchToTasks_dbo_DispatchToes_DispatchToID]
    FOREIGN KEY ([DispatchToID])
    REFERENCES [dbo].[DispatchToes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToTasks_dbo_DispatchToes_DispatchToID'
CREATE INDEX [IX_FK_dbo_DispatchToTasks_dbo_DispatchToes_DispatchToID]
ON [dbo].[DispatchToTasks]
    ([DispatchToID]);
GO

-- Creating foreign key on [TaskID] in table 'DispatchToTasks'
ALTER TABLE [dbo].[DispatchToTasks]
ADD CONSTRAINT [FK_dbo_DispatchToTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DispatchToTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_DispatchToTasks_dbo_Tasks_TaskID]
ON [dbo].[DispatchToTasks]
    ([TaskID]);
GO

-- Creating foreign key on [DocumentID] in table 'DocumentAttachmentFiles'
ALTER TABLE [dbo].[DocumentAttachmentFiles]
ADD CONSTRAINT [FK_DocumentAttachmentFiles_Documents]
    FOREIGN KEY ([DocumentID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_DocumentAttachmentFiles_Documents'
CREATE INDEX [IX_FK_DocumentAttachmentFiles_Documents]
ON [dbo].[DocumentAttachmentFiles]
    ([DocumentID]);
GO

-- Creating foreign key on [DocumentID] in table 'DocumentAttachments'
ALTER TABLE [dbo].[DocumentAttachments]
ADD CONSTRAINT [FK_dbo_DocumentAttachments_dbo_Documents_DocumentID]
    FOREIGN KEY ([DocumentID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DocumentAttachments_dbo_Documents_DocumentID'
CREATE INDEX [IX_FK_dbo_DocumentAttachments_dbo_Documents_DocumentID]
ON [dbo].[DocumentAttachments]
    ([DocumentID]);
GO

-- Creating foreign key on [DocumentCategoryID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_dbo_Documents_dbo_DocumentCategories_DocumentCategoryID]
    FOREIGN KEY ([DocumentCategoryID])
    REFERENCES [dbo].[DocumentCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Documents_dbo_DocumentCategories_DocumentCategoryID'
CREATE INDEX [IX_FK_dbo_Documents_dbo_DocumentCategories_DocumentCategoryID]
ON [dbo].[Documents]
    ([DocumentCategoryID]);
GO

-- Creating foreign key on [DocumentID] in table 'DocumentDistributions'
ALTER TABLE [dbo].[DocumentDistributions]
ADD CONSTRAINT [FK_dbo_DocumentDistributions_dbo_Documents_DocumentID]
    FOREIGN KEY ([DocumentID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DocumentDistributions_dbo_Documents_DocumentID'
CREATE INDEX [IX_FK_dbo_DocumentDistributions_dbo_Documents_DocumentID]
ON [dbo].[DocumentDistributions]
    ([DocumentID]);
GO

-- Creating foreign key on [DocumentID] in table 'DocumentRequirements'
ALTER TABLE [dbo].[DocumentRequirements]
ADD CONSTRAINT [FK_dbo_DocumentRequirements_dbo_Documents_DocumentID]
    FOREIGN KEY ([DocumentID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DocumentRequirements_dbo_Documents_DocumentID'
CREATE INDEX [IX_FK_dbo_DocumentRequirements_dbo_Documents_DocumentID]
ON [dbo].[DocumentRequirements]
    ([DocumentID]);
GO

-- Creating foreign key on [DocumentSecurityID] in table 'Documents'
ALTER TABLE [dbo].[Documents]
ADD CONSTRAINT [FK_dbo_Documents_dbo_DocumentSecurities_DocumentSecurityID]
    FOREIGN KEY ([DocumentSecurityID])
    REFERENCES [dbo].[DocumentSecurities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Documents_dbo_DocumentSecurities_DocumentSecurityID'
CREATE INDEX [IX_FK_dbo_Documents_dbo_DocumentSecurities_DocumentSecurityID]
ON [dbo].[Documents]
    ([DocumentSecurityID]);
GO

-- Creating foreign key on [DocumentID] in table 'ProductDevelopmentRequirementPlanDocuments'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments]
ADD CONSTRAINT [FK_ProductDevelopmentRequirementPlanDocument_Documents]
    FOREIGN KEY ([DocumentID])
    REFERENCES [dbo].[Documents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirementPlanDocument_Documents'
CREATE INDEX [IX_FK_ProductDevelopmentRequirementPlanDocument_Documents]
ON [dbo].[ProductDevelopmentRequirementPlanDocuments]
    ([DocumentID]);
GO

-- Creating foreign key on [TaskID] in table 'DocumentTasks'
ALTER TABLE [dbo].[DocumentTasks]
ADD CONSTRAINT [FK_dbo_DocumentTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_DocumentTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_DocumentTasks_dbo_Tasks_TaskID]
ON [dbo].[DocumentTasks]
    ([TaskID]);
GO

-- Creating foreign key on [EquipmentMeasureID] in table 'EquipmentCalibrations'
ALTER TABLE [dbo].[EquipmentCalibrations]
ADD CONSTRAINT [FK_EquipmentCalibrations_EquipmentMeasures]
    FOREIGN KEY ([EquipmentMeasureID])
    REFERENCES [dbo].[EquipmentMeasures]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentCalibrations_EquipmentMeasures'
CREATE INDEX [IX_FK_EquipmentCalibrations_EquipmentMeasures]
ON [dbo].[EquipmentCalibrations]
    ([EquipmentMeasureID]);
GO

-- Creating foreign key on [EquipmentCalibrationID] in table 'EquipmentMeasureCalibrations'
ALTER TABLE [dbo].[EquipmentMeasureCalibrations]
ADD CONSTRAINT [FK_EquipmentMeasureCalibrations_EquipmentCalibrations]
    FOREIGN KEY ([EquipmentCalibrationID])
    REFERENCES [dbo].[EquipmentCalibrations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureCalibrations_EquipmentCalibrations'
CREATE INDEX [IX_FK_EquipmentMeasureCalibrations_EquipmentCalibrations]
ON [dbo].[EquipmentMeasureCalibrations]
    ([EquipmentCalibrationID]);
GO

-- Creating foreign key on [EquipmentMeasureID] in table 'EquipmentMeasureAttachmentFiles'
ALTER TABLE [dbo].[EquipmentMeasureAttachmentFiles]
ADD CONSTRAINT [FK_EquipmentMeasureAttachmentFiles_EquipmentMeasures]
    FOREIGN KEY ([EquipmentMeasureID])
    REFERENCES [dbo].[EquipmentMeasures]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureAttachmentFiles_EquipmentMeasures'
CREATE INDEX [IX_FK_EquipmentMeasureAttachmentFiles_EquipmentMeasures]
ON [dbo].[EquipmentMeasureAttachmentFiles]
    ([EquipmentMeasureID]);
GO

-- Creating foreign key on [EquipmentMeasureCalibrationID] in table 'EquipmentMeasureCalibrationResults'
ALTER TABLE [dbo].[EquipmentMeasureCalibrationResults]
ADD CONSTRAINT [FK_EquipmentMeasureCalibrationResults_EquipmentMeasureCalibrations]
    FOREIGN KEY ([EquipmentMeasureCalibrationID])
    REFERENCES [dbo].[EquipmentMeasureCalibrations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureCalibrationResults_EquipmentMeasureCalibrations'
CREATE INDEX [IX_FK_EquipmentMeasureCalibrationResults_EquipmentMeasureCalibrations]
ON [dbo].[EquipmentMeasureCalibrationResults]
    ([EquipmentMeasureCalibrationID]);
GO

-- Creating foreign key on [EquipmentMeasureID] in table 'EquipmentMeasureCalibrations'
ALTER TABLE [dbo].[EquipmentMeasureCalibrations]
ADD CONSTRAINT [FK_EquipmentMeasureCalibrations_EquipmentMeasures]
    FOREIGN KEY ([EquipmentMeasureID])
    REFERENCES [dbo].[EquipmentMeasures]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureCalibrations_EquipmentMeasures'
CREATE INDEX [IX_FK_EquipmentMeasureCalibrations_EquipmentMeasures]
ON [dbo].[EquipmentMeasureCalibrations]
    ([EquipmentMeasureID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'EquipmentMeasureCalibrations'
ALTER TABLE [dbo].[EquipmentMeasureCalibrations]
ADD CONSTRAINT [FK_EquipmentMeasureCalibrations_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureCalibrations_HumanEmployees'
CREATE INDEX [IX_FK_EquipmentMeasureCalibrations_HumanEmployees]
ON [dbo].[EquipmentMeasureCalibrations]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [EquipmentMeasureCategoryID] in table 'EquipmentMeasures'
ALTER TABLE [dbo].[EquipmentMeasures]
ADD CONSTRAINT [FK_EquipmentMeasures_EquipmentMeasureCategories]
    FOREIGN KEY ([EquipmentMeasureCategoryID])
    REFERENCES [dbo].[EquipmentMeasureCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasures_EquipmentMeasureCategories'
CREATE INDEX [IX_FK_EquipmentMeasures_EquipmentMeasureCategories]
ON [dbo].[EquipmentMeasures]
    ([EquipmentMeasureCategoryID]);
GO

-- Creating foreign key on [EquipmentMeasureID] in table 'EquipmentMeasureDepartments'
ALTER TABLE [dbo].[EquipmentMeasureDepartments]
ADD CONSTRAINT [FK_EquipmentMeasureDepartments_EquipmentMeasures]
    FOREIGN KEY ([EquipmentMeasureID])
    REFERENCES [dbo].[EquipmentMeasures]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureDepartments_EquipmentMeasures'
CREATE INDEX [IX_FK_EquipmentMeasureDepartments_EquipmentMeasures]
ON [dbo].[EquipmentMeasureDepartments]
    ([EquipmentMeasureID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'EquipmentMeasureDepartments'
ALTER TABLE [dbo].[EquipmentMeasureDepartments]
ADD CONSTRAINT [FK_EquipmentMeasureDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentMeasureDepartments_HumanDepartments'
CREATE INDEX [IX_FK_EquipmentMeasureDepartments_HumanDepartments]
ON [dbo].[EquipmentMeasureDepartments]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionAttaches'
ALTER TABLE [dbo].[EquipmentProductionAttaches]
ADD CONSTRAINT [FK_EquipmentProductionAttachs_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionAttachs_EquipmentProductions'
CREATE INDEX [IX_FK_EquipmentProductionAttachs_EquipmentProductions]
ON [dbo].[EquipmentProductionAttaches]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionAttachmentFiles'
ALTER TABLE [dbo].[EquipmentProductionAttachmentFiles]
ADD CONSTRAINT [FK_EquipmentProductionAttachmentFiles_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionAttachmentFiles_EquipmentProductions'
CREATE INDEX [IX_FK_EquipmentProductionAttachmentFiles_EquipmentProductions]
ON [dbo].[EquipmentProductionAttachmentFiles]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [EquipmentProductionCategoryID] in table 'EquipmentProductions'
ALTER TABLE [dbo].[EquipmentProductions]
ADD CONSTRAINT [FK_EquipmentProductions_EquipmentProductionCategories]
    FOREIGN KEY ([EquipmentProductionCategoryID])
    REFERENCES [dbo].[EquipmentProductionCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductions_EquipmentProductionCategories'
CREATE INDEX [IX_FK_EquipmentProductions_EquipmentProductionCategories]
ON [dbo].[EquipmentProductions]
    ([EquipmentProductionCategoryID]);
GO

-- Creating foreign key on [EquipmentProductionCategoryID] in table 'ProductionStageEquipments'
ALTER TABLE [dbo].[ProductionStageEquipments]
ADD CONSTRAINT [FK_ProductionStageEquipments_EquipmentProductionCategories]
    FOREIGN KEY ([EquipmentProductionCategoryID])
    REFERENCES [dbo].[EquipmentProductionCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageEquipments_EquipmentProductionCategories'
CREATE INDEX [IX_FK_ProductionStageEquipments_EquipmentProductionCategories]
ON [dbo].[ProductionStageEquipments]
    ([EquipmentProductionCategoryID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionDepartments'
ALTER TABLE [dbo].[EquipmentProductionDepartments]
ADD CONSTRAINT [FK_EquipmentProductionDepartments_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionDepartments_EquipmentProductions'
CREATE INDEX [IX_FK_EquipmentProductionDepartments_EquipmentProductions]
ON [dbo].[EquipmentProductionDepartments]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'EquipmentProductionDepartments'
ALTER TABLE [dbo].[EquipmentProductionDepartments]
ADD CONSTRAINT [FK_EquipmentProductionDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionDepartments_HumanDepartments'
CREATE INDEX [IX_FK_EquipmentProductionDepartments_HumanDepartments]
ON [dbo].[EquipmentProductionDepartments]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionErrors'
ALTER TABLE [dbo].[EquipmentProductionErrors]
ADD CONSTRAINT [FK_EquipmentProductionErrors_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionErrors_EquipmentProductions'
CREATE INDEX [IX_FK_EquipmentProductionErrors_EquipmentProductions]
ON [dbo].[EquipmentProductionErrors]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'EquipmentProductionErrors'
ALTER TABLE [dbo].[EquipmentProductionErrors]
ADD CONSTRAINT [FK_EquipmentProductionErrors_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionErrors_HumanDepartments'
CREATE INDEX [IX_FK_EquipmentProductionErrors_HumanDepartments]
ON [dbo].[EquipmentProductionErrors]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'EquipmentProductionErrors'
ALTER TABLE [dbo].[EquipmentProductionErrors]
ADD CONSTRAINT [FK_EquipmentProductionErrors_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionErrors_HumanEmployees'
CREATE INDEX [IX_FK_EquipmentProductionErrors_HumanEmployees]
ON [dbo].[EquipmentProductionErrors]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'EquipmentProductionHistories'
ALTER TABLE [dbo].[EquipmentProductionHistories]
ADD CONSTRAINT [FK_EquipmentProductionTrackings_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionTrackings_HumanDepartments'
CREATE INDEX [IX_FK_EquipmentProductionTrackings_HumanDepartments]
ON [dbo].[EquipmentProductionHistories]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'EquipmentProductionHistories'
ALTER TABLE [dbo].[EquipmentProductionHistories]
ADD CONSTRAINT [FK_EquipmentProductionTrackings_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionTrackings_HumanEmployees'
CREATE INDEX [IX_FK_EquipmentProductionTrackings_HumanEmployees]
ON [dbo].[EquipmentProductionHistories]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionHistories'
ALTER TABLE [dbo].[EquipmentProductionHistories]
ADD CONSTRAINT [FK_EquipmentTracking_EquipmentProduction]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentTracking_EquipmentProduction'
CREATE INDEX [IX_FK_EquipmentTracking_EquipmentProduction]
ON [dbo].[EquipmentProductionHistories]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'EquipmentProductionMaintains'
ALTER TABLE [dbo].[EquipmentProductionMaintains]
ADD CONSTRAINT [FK_EquipmentProductionMaintains_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionMaintains_EquipmentProductions'
CREATE INDEX [IX_FK_EquipmentProductionMaintains_EquipmentProductions]
ON [dbo].[EquipmentProductionMaintains]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'EquipmentProductionMaintains'
ALTER TABLE [dbo].[EquipmentProductionMaintains]
ADD CONSTRAINT [FK_EquipmentProductionMaintains_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_EquipmentProductionMaintains_HumanDepartments'
CREATE INDEX [IX_FK_EquipmentProductionMaintains_HumanDepartments]
ON [dbo].[EquipmentProductionMaintains]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [EquipmentProductionID] in table 'ProductionPlanEquipments'
ALTER TABLE [dbo].[ProductionPlanEquipments]
ADD CONSTRAINT [FK_ProductionPlanEquipments_EquipmentProductions]
    FOREIGN KEY ([EquipmentProductionID])
    REFERENCES [dbo].[EquipmentProductions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanEquipments_EquipmentProductions'
CREATE INDEX [IX_FK_ProductionPlanEquipments_EquipmentProductions]
ON [dbo].[ProductionPlanEquipments]
    ([EquipmentProductionID]);
GO

-- Creating foreign key on [HumantAbsentTypeID] in table 'HumanAbsents'
ALTER TABLE [dbo].[HumanAbsents]
ADD CONSTRAINT [FK_HumanAbsents_HumanAbsentTypes]
    FOREIGN KEY ([HumantAbsentTypeID])
    REFERENCES [dbo].[HumanAbsentTypes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAbsents_HumanAbsentTypes'
CREATE INDEX [IX_FK_HumanAbsents_HumanAbsentTypes]
ON [dbo].[HumanAbsents]
    ([HumantAbsentTypeID]);
GO

-- Creating foreign key on [HumanQuestionID] in table 'HumanAnswers'
ALTER TABLE [dbo].[HumanAnswers]
ADD CONSTRAINT [FK_HumanAnswers_HumanQuestions]
    FOREIGN KEY ([HumanQuestionID])
    REFERENCES [dbo].[HumanQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAnswers_HumanQuestions'
CREATE INDEX [IX_FK_HumanAnswers_HumanQuestions]
ON [dbo].[HumanAnswers]
    ([HumanQuestionID]);
GO

-- Creating foreign key on [HumanAnswerID] in table 'HumanResultAnswers'
ALTER TABLE [dbo].[HumanResultAnswers]
ADD CONSTRAINT [FK_HumanResultAnswers_HumanAnswers]
    FOREIGN KEY ([HumanAnswerID])
    REFERENCES [dbo].[HumanAnswers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanResultAnswers_HumanAnswers'
CREATE INDEX [IX_FK_HumanResultAnswers_HumanAnswers]
ON [dbo].[HumanResultAnswers]
    ([HumanAnswerID]);
GO

-- Creating foreign key on [HumanAuditCriteriaCategoryID] in table 'HumanAuditCriterias'
ALTER TABLE [dbo].[HumanAuditCriterias]
ADD CONSTRAINT [FK_HumanCriterias_HumanCriteriaCategories]
    FOREIGN KEY ([HumanAuditCriteriaCategoryID])
    REFERENCES [dbo].[HumanAuditCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanCriterias_HumanCriteriaCategories'
CREATE INDEX [IX_FK_HumanCriterias_HumanCriteriaCategories]
ON [dbo].[HumanAuditCriterias]
    ([HumanAuditCriteriaCategoryID]);
GO

-- Creating foreign key on [HumanAuditCriteriaID] in table 'HumanAuditCriteriaPoints'
ALTER TABLE [dbo].[HumanAuditCriteriaPoints]
ADD CONSTRAINT [FK_HumanCriteriaPoints_HumanCriterias]
    FOREIGN KEY ([HumanAuditCriteriaID])
    REFERENCES [dbo].[HumanAuditCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanCriteriaPoints_HumanCriterias'
CREATE INDEX [IX_FK_HumanCriteriaPoints_HumanCriterias]
ON [dbo].[HumanAuditCriteriaPoints]
    ([HumanAuditCriteriaID]);
GO

-- Creating foreign key on [HumanAuditGradationID] in table 'HumanAuditDepartments'
ALTER TABLE [dbo].[HumanAuditDepartments]
ADD CONSTRAINT [FK_HumanAuditDepartments_HumanAuditGradations]
    FOREIGN KEY ([HumanAuditGradationID])
    REFERENCES [dbo].[HumanAuditGradations]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditDepartments_HumanAuditGradations'
CREATE INDEX [IX_FK_HumanAuditDepartments_HumanAuditGradations]
ON [dbo].[HumanAuditDepartments]
    ([HumanAuditGradationID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'HumanAuditDepartments'
ALTER TABLE [dbo].[HumanAuditDepartments]
ADD CONSTRAINT [FK_HumanAuditDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditDepartments_HumanDepartments'
CREATE INDEX [IX_FK_HumanAuditDepartments_HumanDepartments]
ON [dbo].[HumanAuditDepartments]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanAuditDepartmentID] in table 'HumanAuditGradationRoles'
ALTER TABLE [dbo].[HumanAuditGradationRoles]
ADD CONSTRAINT [FK_HumanAuditGradationRoles_HumanAuditDepartments]
    FOREIGN KEY ([HumanAuditDepartmentID])
    REFERENCES [dbo].[HumanAuditDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditGradationRoles_HumanAuditDepartments'
CREATE INDEX [IX_FK_HumanAuditGradationRoles_HumanAuditDepartments]
ON [dbo].[HumanAuditGradationRoles]
    ([HumanAuditDepartmentID]);
GO

-- Creating foreign key on [HumanAuditGradationCriteriaID] in table 'HumanAuditGradationCriteriaPoints'
ALTER TABLE [dbo].[HumanAuditGradationCriteriaPoints]
ADD CONSTRAINT [FK_HumanAuditGraditionCriteriaPoints_HumanAuditGradationCriterias]
    FOREIGN KEY ([HumanAuditGradationCriteriaID])
    REFERENCES [dbo].[HumanAuditGradationCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditGraditionCriteriaPoints_HumanAuditGradationCriterias'
CREATE INDEX [IX_FK_HumanAuditGraditionCriteriaPoints_HumanAuditGradationCriterias]
ON [dbo].[HumanAuditGradationCriteriaPoints]
    ([HumanAuditGradationCriteriaID]);
GO

-- Creating foreign key on [HumanAuditGradationRoleID] in table 'HumanAuditGradationCriterias'
ALTER TABLE [dbo].[HumanAuditGradationCriterias]
ADD CONSTRAINT [FK_HumanAuditGradationCriteias_HumanAuditGradationRoles]
    FOREIGN KEY ([HumanAuditGradationRoleID])
    REFERENCES [dbo].[HumanAuditGradationRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditGradationCriteias_HumanAuditGradationRoles'
CREATE INDEX [IX_FK_HumanAuditGradationCriteias_HumanAuditGradationRoles]
ON [dbo].[HumanAuditGradationCriterias]
    ([HumanAuditGradationRoleID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'HumanAuditGradationRoles'
ALTER TABLE [dbo].[HumanAuditGradationRoles]
ADD CONSTRAINT [FK_HumanAuditGradationRoles_HumanRoles]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditGradationRoles_HumanRoles'
CREATE INDEX [IX_FK_HumanAuditGradationRoles_HumanRoles]
ON [dbo].[HumanAuditGradationRoles]
    ([HumanRoleID]);
GO

-- Creating foreign key on [HumanAuditGradationRoleID] in table 'HumanAuditLevels'
ALTER TABLE [dbo].[HumanAuditLevels]
ADD CONSTRAINT [FK_HumanAuditLevels_HumanAuditGradationRoles]
    FOREIGN KEY ([HumanAuditGradationRoleID])
    REFERENCES [dbo].[HumanAuditGradationRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditLevels_HumanAuditGradationRoles'
CREATE INDEX [IX_FK_HumanAuditLevels_HumanAuditGradationRoles]
ON [dbo].[HumanAuditLevels]
    ([HumanAuditGradationRoleID]);
GO

-- Creating foreign key on [HumanAuditGradationRoleID] in table 'HumanAuditTicks'
ALTER TABLE [dbo].[HumanAuditTicks]
ADD CONSTRAINT [FK_HumanAuditTicks_HumanAuditGradationRoles]
    FOREIGN KEY ([HumanAuditGradationRoleID])
    REFERENCES [dbo].[HumanAuditGradationRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditTicks_HumanAuditGradationRoles'
CREATE INDEX [IX_FK_HumanAuditTicks_HumanAuditGradationRoles]
ON [dbo].[HumanAuditTicks]
    ([HumanAuditGradationRoleID]);
GO

-- Creating foreign key on [HumanAuditLevelID] in table 'HumanAuditTicks'
ALTER TABLE [dbo].[HumanAuditTicks]
ADD CONSTRAINT [FK_HumanAuditTicks_HumanAuditLevels]
    FOREIGN KEY ([HumanAuditLevelID])
    REFERENCES [dbo].[HumanAuditLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditTicks_HumanAuditLevels'
CREATE INDEX [IX_FK_HumanAuditTicks_HumanAuditLevels]
ON [dbo].[HumanAuditTicks]
    ([HumanAuditLevelID]);
GO

-- Creating foreign key on [HumanAuditTickID] in table 'HumanAuditTickResultBonusScores'
ALTER TABLE [dbo].[HumanAuditTickResultBonusScores]
ADD CONSTRAINT [FK_HumanAuditTickResultBonusScores_HumanAuditTicks]
    FOREIGN KEY ([HumanAuditTickID])
    REFERENCES [dbo].[HumanAuditTicks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditTickResultBonusScores_HumanAuditTicks'
CREATE INDEX [IX_FK_HumanAuditTickResultBonusScores_HumanAuditTicks]
ON [dbo].[HumanAuditTickResultBonusScores]
    ([HumanAuditTickID]);
GO

-- Creating foreign key on [HumanAuditTickID] in table 'HumanAuditTickResults'
ALTER TABLE [dbo].[HumanAuditTickResults]
ADD CONSTRAINT [FK_HumanAuditTickResults_HumanAuditTicks]
    FOREIGN KEY ([HumanAuditTickID])
    REFERENCES [dbo].[HumanAuditTicks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditTickResults_HumanAuditTicks'
CREATE INDEX [IX_FK_HumanAuditTickResults_HumanAuditTicks]
ON [dbo].[HumanAuditTickResults]
    ([HumanAuditTickID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanAuditTicks'
ALTER TABLE [dbo].[HumanAuditTicks]
ADD CONSTRAINT [FK_HumanAuditTicks_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanAuditTicks_HumanEmployees'
CREATE INDEX [IX_FK_HumanAuditTicks_HumanEmployees]
ON [dbo].[HumanAuditTicks]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanCategoryQuestionID] in table 'HumanPhaseAudits'
ALTER TABLE [dbo].[HumanPhaseAudits]
ADD CONSTRAINT [FK_HumanPhaseAudits_HumanCategoryQuestions]
    FOREIGN KEY ([HumanCategoryQuestionID])
    REFERENCES [dbo].[HumanCategoryQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanPhaseAudits_HumanCategoryQuestions'
CREATE INDEX [IX_FK_HumanPhaseAudits_HumanCategoryQuestions]
ON [dbo].[HumanPhaseAudits]
    ([HumanCategoryQuestionID]);
GO

-- Creating foreign key on [HumanCategoryQuestionID] in table 'HumanQuestions'
ALTER TABLE [dbo].[HumanQuestions]
ADD CONSTRAINT [FK_HumanQuestions_HumanCategoryQuestions]
    FOREIGN KEY ([HumanCategoryQuestionID])
    REFERENCES [dbo].[HumanCategoryQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanQuestions_HumanCategoryQuestions'
CREATE INDEX [IX_FK_HumanQuestions_HumanCategoryQuestions]
ON [dbo].[HumanQuestions]
    ([HumanCategoryQuestionID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'HumanRoles'
ALTER TABLE [dbo].[HumanRoles]
ADD CONSTRAINT [FK_dbo_HumanRoles_dbo_HumanDepartments_HumanDepartmentID]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRoles_dbo_HumanDepartments_HumanDepartmentID'
CREATE INDEX [IX_FK_dbo_HumanRoles_dbo_HumanDepartments_HumanDepartmentID]
ON [dbo].[HumanRoles]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_dbo_Tasks_dbo_HumanDepartments_HumanDepartmentID]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Tasks_dbo_HumanDepartments_HumanDepartmentID'
CREATE INDEX [IX_FK_dbo_Tasks_dbo_HumanDepartments_HumanDepartmentID]
ON [dbo].[Tasks]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'ProductionCommands'
ALTER TABLE [dbo].[ProductionCommands]
ADD CONSTRAINT [FK_ProductionCommands_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionCommands_HumanDepartments'
CREATE INDEX [IX_FK_ProductionCommands_HumanDepartments]
ON [dbo].[ProductionCommands]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'QualityAuditProgramDepartments'
ALTER TABLE [dbo].[QualityAuditProgramDepartments]
ADD CONSTRAINT [FK_QualityAuditProgramDepartments_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditProgramDepartments_HumanDepartments'
CREATE INDEX [IX_FK_QualityAuditProgramDepartments_HumanDepartments]
ON [dbo].[QualityAuditProgramDepartments]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'QualityAuditRecordedVotes'
ALTER TABLE [dbo].[QualityAuditRecordedVotes]
ADD CONSTRAINT [FK_QualityAuditRecordedVotes_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditRecordedVotes_HumanDepartments'
CREATE INDEX [IX_FK_QualityAuditRecordedVotes_HumanDepartments]
ON [dbo].[QualityAuditRecordedVotes]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'QualityAuditResults'
ALTER TABLE [dbo].[QualityAuditResults]
ADD CONSTRAINT [FK_QualityAuditResults_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditResults_HumanDepartments'
CREATE INDEX [IX_FK_QualityAuditResults_HumanDepartments]
ON [dbo].[QualityAuditResults]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'Risks'
ALTER TABLE [dbo].[Risks]
ADD CONSTRAINT [FK_Risks_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Risks_HumanDepartments'
CREATE INDEX [IX_FK_Risks_HumanDepartments]
ON [dbo].[Risks]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'TaskCalendars'
ALTER TABLE [dbo].[TaskCalendars]
ADD CONSTRAINT [FK_TaskCalendars_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskCalendars_HumanDepartments'
CREATE INDEX [IX_FK_TaskCalendars_HumanDepartments]
ON [dbo].[TaskCalendars]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanDepartmentID] in table 'TaskRequests'
ALTER TABLE [dbo].[TaskRequests]
ADD CONSTRAINT [FK_TaskRequests_HumanDepartments]
    FOREIGN KEY ([HumanDepartmentID])
    REFERENCES [dbo].[HumanDepartments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskRequests_HumanDepartments'
CREATE INDEX [IX_FK_TaskRequests_HumanDepartments]
ON [dbo].[TaskRequests]
    ([HumanDepartmentID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanEmployeeAudits'
ALTER TABLE [dbo].[HumanEmployeeAudits]
ADD CONSTRAINT [FK_HumanEmployeeAudits_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanEmployeeAudits_HumanEmployees'
CREATE INDEX [IX_FK_HumanEmployeeAudits_HumanEmployees]
ON [dbo].[HumanEmployeeAudits]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanPhaseAuditID] in table 'HumanEmployeeAudits'
ALTER TABLE [dbo].[HumanEmployeeAudits]
ADD CONSTRAINT [FK_HumanEmployeeAudits_HumanPhaseAudits]
    FOREIGN KEY ([HumanPhaseAuditID])
    REFERENCES [dbo].[HumanPhaseAudits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanEmployeeAudits_HumanPhaseAudits'
CREATE INDEX [IX_FK_HumanEmployeeAudits_HumanPhaseAudits]
ON [dbo].[HumanEmployeeAudits]
    ([HumanPhaseAuditID]);
GO

-- Creating foreign key on [HumanEmployeeAuditID] in table 'HumanResultQuestions'
ALTER TABLE [dbo].[HumanResultQuestions]
ADD CONSTRAINT [FK_HumanResultQuestions_HumanEmployeeAudits]
    FOREIGN KEY ([HumanEmployeeAuditID])
    REFERENCES [dbo].[HumanEmployeeAudits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanResultQuestions_HumanEmployeeAudits'
CREATE INDEX [IX_FK_HumanResultQuestions_HumanEmployeeAudits]
ON [dbo].[HumanResultQuestions]
    ([HumanEmployeeAuditID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanOrganizations'
ALTER TABLE [dbo].[HumanOrganizations]
ADD CONSTRAINT [FK_dbo_HumanOrganizations_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanOrganizations_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanOrganizations_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanOrganizations]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileAssesses'
ALTER TABLE [dbo].[HumanProfileAssesses]
ADD CONSTRAINT [FK_dbo_HumanProfileAssesses_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileAssesses_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileAssesses_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileAssesses]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileAttachments'
ALTER TABLE [dbo].[HumanProfileAttachments]
ADD CONSTRAINT [FK_dbo_HumanProfileAttachments_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileAttachments_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileAttachments_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileAttachments]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileCertificates'
ALTER TABLE [dbo].[HumanProfileCertificates]
ADD CONSTRAINT [FK_dbo_HumanProfileCertificates_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileCertificates_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileCertificates_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileCertificates]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileContracts'
ALTER TABLE [dbo].[HumanProfileContracts]
ADD CONSTRAINT [FK_dbo_HumanProfileContracts_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileContracts_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileContracts_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileContracts]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileCuriculmViates'
ALTER TABLE [dbo].[HumanProfileCuriculmViates]
ADD CONSTRAINT [FK_dbo_HumanProfileCuriculmViates_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileCuriculmViates_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileCuriculmViates_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileCuriculmViates]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileDiplomas'
ALTER TABLE [dbo].[HumanProfileDiplomas]
ADD CONSTRAINT [FK_dbo_HumanProfileDiplomas_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileDiplomas_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileDiplomas_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileDiplomas]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileDisciplines'
ALTER TABLE [dbo].[HumanProfileDisciplines]
ADD CONSTRAINT [FK_dbo_HumanProfileDisciplines_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileDisciplines_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileDisciplines_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileDisciplines]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileInsurances'
ALTER TABLE [dbo].[HumanProfileInsurances]
ADD CONSTRAINT [FK_dbo_HumanProfileInsurances_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileInsurances_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileInsurances_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileInsurances]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileRelationships'
ALTER TABLE [dbo].[HumanProfileRelationships]
ADD CONSTRAINT [FK_dbo_HumanProfileRelationships_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileRelationships_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileRelationships_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileRelationships]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileRewards'
ALTER TABLE [dbo].[HumanProfileRewards]
ADD CONSTRAINT [FK_dbo_HumanProfileRewards_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileRewards_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileRewards_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileRewards]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileSalaries'
ALTER TABLE [dbo].[HumanProfileSalaries]
ADD CONSTRAINT [FK_dbo_HumanProfileSalaries_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileSalaries_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileSalaries_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileSalaries]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileTrainings'
ALTER TABLE [dbo].[HumanProfileTrainings]
ADD CONSTRAINT [FK_dbo_HumanProfileTrainings_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileTrainings_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileTrainings_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileTrainings]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileWorkExperiences'
ALTER TABLE [dbo].[HumanProfileWorkExperiences]
ADD CONSTRAINT [FK_dbo_HumanProfileWorkExperiences_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanProfileWorkExperiences_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanProfileWorkExperiences_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanProfileWorkExperiences]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanTrainingPractioners'
ALTER TABLE [dbo].[HumanTrainingPractioners]
ADD CONSTRAINT [FK_dbo_HumanTrainingPractioners_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPractioners_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPractioners_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanTrainingPractioners]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanUsers'
ALTER TABLE [dbo].[HumanUsers]
ADD CONSTRAINT [FK_dbo_HumanUsers_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanUsers_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_HumanUsers_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[HumanUsers]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'Notifies'
ALTER TABLE [dbo].[Notifies]
ADD CONSTRAINT [FK_dbo_Notifies_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Notifies_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_Notifies_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[Notifies]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'QualityAuditProgramEmployees'
ALTER TABLE [dbo].[QualityAuditProgramEmployees]
ADD CONSTRAINT [FK_dbo_QualityAuditEmployees_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityAuditEmployees_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_QualityAuditEmployees_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[QualityAuditProgramEmployees]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_dbo_Tasks_dbo_HumanEmployees_HumanEmployeeID]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Tasks_dbo_HumanEmployees_HumanEmployeeID'
CREATE INDEX [IX_FK_dbo_Tasks_dbo_HumanEmployees_HumanEmployeeID]
ON [dbo].[Tasks]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'HumanProfileWorkTrials'
ALTER TABLE [dbo].[HumanProfileWorkTrials]
ADD CONSTRAINT [FK_HumanProfileWorkTrials_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanProfileWorkTrials_HumanEmployees'
CREATE INDEX [IX_FK_HumanProfileWorkTrials_HumanEmployees]
ON [dbo].[HumanProfileWorkTrials]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'TaskCCs'
ALTER TABLE [dbo].[TaskCCs]
ADD CONSTRAINT [FK_TaskCCs_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskCCs_HumanEmployees'
CREATE INDEX [IX_FK_TaskCCs_HumanEmployees]
ON [dbo].[TaskCCs]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanEmployeeID] in table 'TaskRequests'
ALTER TABLE [dbo].[TaskRequests]
ADD CONSTRAINT [FK_TaskRequests_HumanEmployees]
    FOREIGN KEY ([HumanEmployeeID])
    REFERENCES [dbo].[HumanEmployees]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskRequests_HumanEmployees'
CREATE INDEX [IX_FK_TaskRequests_HumanEmployees]
ON [dbo].[TaskRequests]
    ([HumanEmployeeID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'HumanOrganizations'
ALTER TABLE [dbo].[HumanOrganizations]
ADD CONSTRAINT [FK_dbo_HumanOrganizations_dbo_HumanRoles_HumanRoleID]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanOrganizations_dbo_HumanRoles_HumanRoleID'
CREATE INDEX [IX_FK_dbo_HumanOrganizations_dbo_HumanRoles_HumanRoleID]
ON [dbo].[HumanOrganizations]
    ([HumanRoleID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'HumanPermissions'
ALTER TABLE [dbo].[HumanPermissions]
ADD CONSTRAINT [FK_dbo_HumanPermissions_dbo_HumanRoles_HumanRoleID]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanPermissions_dbo_HumanRoles_HumanRoleID'
CREATE INDEX [IX_FK_dbo_HumanPermissions_dbo_HumanRoles_HumanRoleID]
ON [dbo].[HumanPermissions]
    ([HumanRoleID]);
GO

-- Creating foreign key on [HumanProfileWorkTrialID] in table 'HumanProfileWorkTrialResults'
ALTER TABLE [dbo].[HumanProfileWorkTrialResults]
ADD CONSTRAINT [FK_HumanProfileWorkTrialResults_HumanProfileWorkTrials]
    FOREIGN KEY ([HumanProfileWorkTrialID])
    REFERENCES [dbo].[HumanProfileWorkTrials]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanProfileWorkTrialResults_HumanProfileWorkTrials'
CREATE INDEX [IX_FK_HumanProfileWorkTrialResults_HumanProfileWorkTrials]
ON [dbo].[HumanProfileWorkTrialResults]
    ([HumanProfileWorkTrialID]);
GO

-- Creating foreign key on [QualityCriteriaID] in table 'HumanProfileWorkTrialResults'
ALTER TABLE [dbo].[HumanProfileWorkTrialResults]
ADD CONSTRAINT [FK_HumanProfileWorkTrialResults_QualityCriterias]
    FOREIGN KEY ([QualityCriteriaID])
    REFERENCES [dbo].[QualityCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanProfileWorkTrialResults_QualityCriterias'
CREATE INDEX [IX_FK_HumanProfileWorkTrialResults_QualityCriterias]
ON [dbo].[HumanProfileWorkTrialResults]
    ([QualityCriteriaID]);
GO

-- Creating foreign key on [HumanQuestionID] in table 'HumanResultQuestions'
ALTER TABLE [dbo].[HumanResultQuestions]
ADD CONSTRAINT [FK_HumanResultQuestions_HumanQuestions]
    FOREIGN KEY ([HumanQuestionID])
    REFERENCES [dbo].[HumanQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanResultQuestions_HumanQuestions'
CREATE INDEX [IX_FK_HumanResultQuestions_HumanQuestions]
ON [dbo].[HumanResultQuestions]
    ([HumanQuestionID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'HumanRecruitmentCriterias'
ALTER TABLE [dbo].[HumanRecruitmentCriterias]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentCriterias_dbo_HumanRoles_HumanRoleID]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentCriterias_dbo_HumanRoles_HumanRoleID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentCriterias_dbo_HumanRoles_HumanRoleID]
ON [dbo].[HumanRecruitmentCriterias]
    ([HumanRoleID]);
GO

-- Creating foreign key on [HumanRecruitmentCriteriaID] in table 'HumanRecruitmentReviews'
ALTER TABLE [dbo].[HumanRecruitmentReviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentCriterias_HumanRecruitmentCriteriaID]
    FOREIGN KEY ([HumanRecruitmentCriteriaID])
    REFERENCES [dbo].[HumanRecruitmentCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentCriterias_HumanRecruitmentCriteriaID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentCriterias_HumanRecruitmentCriteriaID]
ON [dbo].[HumanRecruitmentReviews]
    ([HumanRecruitmentCriteriaID]);
GO

-- Creating foreign key on [HumanRecruitmentPlanInterviewID] in table 'HumanRecruitmentInterviews'
ALTER TABLE [dbo].[HumanRecruitmentInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentPlanInterviews_HumanRecruitmentPlanInterviewID]
    FOREIGN KEY ([HumanRecruitmentPlanInterviewID])
    REFERENCES [dbo].[HumanRecruitmentPlanInterviews]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentPlanInterviews_HumanRecruitmentPlanInterviewID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentPlanInterviews_HumanRecruitmentPlanInterviewID]
ON [dbo].[HumanRecruitmentInterviews]
    ([HumanRecruitmentPlanInterviewID]);
GO

-- Creating foreign key on [HumanRecruitmentProfileInterviewID] in table 'HumanRecruitmentInterviews'
ALTER TABLE [dbo].[HumanRecruitmentInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]
    FOREIGN KEY ([HumanRecruitmentProfileInterviewID])
    REFERENCES [dbo].[HumanRecruitmentProfileInterviews]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentInterviews_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]
ON [dbo].[HumanRecruitmentInterviews]
    ([HumanRecruitmentProfileInterviewID]);
GO

-- Creating foreign key on [HumanRecruitmentPlanID] in table 'HumanRecruitmentPlanInterviews'
ALTER TABLE [dbo].[HumanRecruitmentPlanInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentPlanInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
    FOREIGN KEY ([HumanRecruitmentPlanID])
    REFERENCES [dbo].[HumanRecruitmentPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentPlanInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentPlanInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
ON [dbo].[HumanRecruitmentPlanInterviews]
    ([HumanRecruitmentPlanID]);
GO

-- Creating foreign key on [HumanRecruitmentPlanID] in table 'HumanRecruitmentPlanRequirements'
ALTER TABLE [dbo].[HumanRecruitmentPlanRequirements]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
    FOREIGN KEY ([HumanRecruitmentPlanID])
    REFERENCES [dbo].[HumanRecruitmentPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
ON [dbo].[HumanRecruitmentPlanRequirements]
    ([HumanRecruitmentPlanID]);
GO

-- Creating foreign key on [HumanRecruitmentRequirementID] in table 'HumanRecruitmentPlanRequirements'
ALTER TABLE [dbo].[HumanRecruitmentPlanRequirements]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]
    FOREIGN KEY ([HumanRecruitmentRequirementID])
    REFERENCES [dbo].[HumanRecruitmentRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentPlanRequirements_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]
ON [dbo].[HumanRecruitmentPlanRequirements]
    ([HumanRecruitmentRequirementID]);
GO

-- Creating foreign key on [HumanRecruitmentPlanID] in table 'HumanRecruitmentProfileInterviews'
ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
    FOREIGN KEY ([HumanRecruitmentPlanID])
    REFERENCES [dbo].[HumanRecruitmentPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
ON [dbo].[HumanRecruitmentProfileInterviews]
    ([HumanRecruitmentPlanID]);
GO

-- Creating foreign key on [HumanRecruitmentPlanID] in table 'HumanRecruitmentTasks'
ALTER TABLE [dbo].[HumanRecruitmentTasks]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentTasks_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
    FOREIGN KEY ([HumanRecruitmentPlanID])
    REFERENCES [dbo].[HumanRecruitmentPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentTasks_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentTasks_dbo_HumanRecruitmentPlans_HumanRecruitmentPlanID]
ON [dbo].[HumanRecruitmentTasks]
    ([HumanRecruitmentPlanID]);
GO

-- Creating foreign key on [HumanRecruitmentProfileID] in table 'HumanRecruitmentProfileInterviews'
ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]
    FOREIGN KEY ([HumanRecruitmentProfileID])
    REFERENCES [dbo].[HumanRecruitmentProfiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]
ON [dbo].[HumanRecruitmentProfileInterviews]
    ([HumanRecruitmentProfileID]);
GO

-- Creating foreign key on [HumanRecruitmentRequirementID] in table 'HumanRecruitmentProfileInterviews'
ALTER TABLE [dbo].[HumanRecruitmentProfileInterviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]
    FOREIGN KEY ([HumanRecruitmentRequirementID])
    REFERENCES [dbo].[HumanRecruitmentRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentProfileInterviews_dbo_HumanRecruitmentRequirements_HumanRecruitmentRequirementID]
ON [dbo].[HumanRecruitmentProfileInterviews]
    ([HumanRecruitmentRequirementID]);
GO

-- Creating foreign key on [HumanRecruitmentProfileInterviewID] in table 'HumanRecruitmentProfileResults'
ALTER TABLE [dbo].[HumanRecruitmentProfileResults]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentProfileResults_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]
    FOREIGN KEY ([HumanRecruitmentProfileInterviewID])
    REFERENCES [dbo].[HumanRecruitmentProfileInterviews]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentProfileResults_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentProfileResults_dbo_HumanRecruitmentProfileInterviews_HumanRecruitmentProfileInterviewID]
ON [dbo].[HumanRecruitmentProfileResults]
    ([HumanRecruitmentProfileInterviewID]);
GO

-- Creating foreign key on [HumanRecruitmentProfileID] in table 'HumanRecruitmentReviews'
ALTER TABLE [dbo].[HumanRecruitmentReviews]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]
    FOREIGN KEY ([HumanRecruitmentProfileID])
    REFERENCES [dbo].[HumanRecruitmentProfiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentReviews_dbo_HumanRecruitmentProfiles_HumanRecruitmentProfileID]
ON [dbo].[HumanRecruitmentReviews]
    ([HumanRecruitmentProfileID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'HumanRecruitmentRequirements'
ALTER TABLE [dbo].[HumanRecruitmentRequirements]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentRequirements_dbo_HumanRoles_HumanRoleID]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentRequirements_dbo_HumanRoles_HumanRoleID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentRequirements_dbo_HumanRoles_HumanRoleID]
ON [dbo].[HumanRecruitmentRequirements]
    ([HumanRoleID]);
GO

-- Creating foreign key on [TaskID] in table 'HumanRecruitmentTasks'
ALTER TABLE [dbo].[HumanRecruitmentTasks]
ADD CONSTRAINT [FK_dbo_HumanRecruitmentTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanRecruitmentTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_HumanRecruitmentTasks_dbo_Tasks_TaskID]
ON [dbo].[HumanRecruitmentTasks]
    ([TaskID]);
GO

-- Creating foreign key on [HumanResultQuestionID] in table 'HumanResultAnswers'
ALTER TABLE [dbo].[HumanResultAnswers]
ADD CONSTRAINT [FK_HumanResultAnswers_HumanResultQuestions]
    FOREIGN KEY ([HumanResultQuestionID])
    REFERENCES [dbo].[HumanResultQuestions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_HumanResultAnswers_HumanResultQuestions'
CREATE INDEX [IX_FK_HumanResultAnswers_HumanResultQuestions]
ON [dbo].[HumanResultAnswers]
    ([HumanResultQuestionID]);
GO

-- Creating foreign key on [HumanRoleID] in table 'QualityNCRoles'
ALTER TABLE [dbo].[QualityNCRoles]
ADD CONSTRAINT [FK_dbo_QualityNCRoles_dbo_HumanRoles_HumanRoleID]
    FOREIGN KEY ([HumanRoleID])
    REFERENCES [dbo].[HumanRoles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityNCRoles_dbo_HumanRoles_HumanRoleID'
CREATE INDEX [IX_FK_dbo_QualityNCRoles_dbo_HumanRoles_HumanRoleID]
ON [dbo].[QualityNCRoles]
    ([HumanRoleID]);
GO

-- Creating foreign key on [HumanTrainingPlanID] in table 'HumanTrainingPlanDetails'
ALTER TABLE [dbo].[HumanTrainingPlanDetails]
ADD CONSTRAINT [FK_dbo_HumanTrainingPlanDetails_dbo_HumanTrainingPlans_HumanTrainingPlanID]
    FOREIGN KEY ([HumanTrainingPlanID])
    REFERENCES [dbo].[HumanTrainingPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPlanDetails_dbo_HumanTrainingPlans_HumanTrainingPlanID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPlanDetails_dbo_HumanTrainingPlans_HumanTrainingPlanID]
ON [dbo].[HumanTrainingPlanDetails]
    ([HumanTrainingPlanID]);
GO

-- Creating foreign key on [HumanTrainingPlanDetailID] in table 'HumanTrainingPractioners'
ALTER TABLE [dbo].[HumanTrainingPractioners]
ADD CONSTRAINT [FK_dbo_HumanTrainingPractioners_dbo_HumanTrainingPlanDetails_HumanTrainingPlanDetailID]
    FOREIGN KEY ([HumanTrainingPlanDetailID])
    REFERENCES [dbo].[HumanTrainingPlanDetails]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPractioners_dbo_HumanTrainingPlanDetails_HumanTrainingPlanDetailID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPractioners_dbo_HumanTrainingPlanDetails_HumanTrainingPlanDetailID]
ON [dbo].[HumanTrainingPractioners]
    ([HumanTrainingPlanDetailID]);
GO

-- Creating foreign key on [HumanTrainingPlanID] in table 'HumanTrainingPlanRequirements'
ALTER TABLE [dbo].[HumanTrainingPlanRequirements]
ADD CONSTRAINT [FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingPlans_HumanTrainingPlanID]
    FOREIGN KEY ([HumanTrainingPlanID])
    REFERENCES [dbo].[HumanTrainingPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingPlans_HumanTrainingPlanID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingPlans_HumanTrainingPlanID]
ON [dbo].[HumanTrainingPlanRequirements]
    ([HumanTrainingPlanID]);
GO

-- Creating foreign key on [HumanTrainingRequirementID] in table 'HumanTrainingPlanRequirements'
ALTER TABLE [dbo].[HumanTrainingPlanRequirements]
ADD CONSTRAINT [FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]
    FOREIGN KEY ([HumanTrainingRequirementID])
    REFERENCES [dbo].[HumanTrainingRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingRequirements_HumanTrainingRequirementID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPlanRequirements_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]
ON [dbo].[HumanTrainingPlanRequirements]
    ([HumanTrainingRequirementID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'HumanTrainingPlans'
ALTER TABLE [dbo].[HumanTrainingPlans]
ADD CONSTRAINT [FK_dbo_HumanTrainingPlans_dbo_QualityPlans_QualityPlanID]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingPlans_dbo_QualityPlans_QualityPlanID'
CREATE INDEX [IX_FK_dbo_HumanTrainingPlans_dbo_QualityPlans_QualityPlanID]
ON [dbo].[HumanTrainingPlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [HumanTrainingRequirementID] in table 'HumanTrainingRequirementEmployees'
ALTER TABLE [dbo].[HumanTrainingRequirementEmployees]
ADD CONSTRAINT [FK_dbo_HumanTrainingRequirementEmployees_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]
    FOREIGN KEY ([HumanTrainingRequirementID])
    REFERENCES [dbo].[HumanTrainingRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_HumanTrainingRequirementEmployees_dbo_HumanTrainingRequirements_HumanTrainingRequirementID'
CREATE INDEX [IX_FK_dbo_HumanTrainingRequirementEmployees_dbo_HumanTrainingRequirements_HumanTrainingRequirementID]
ON [dbo].[HumanTrainingRequirementEmployees]
    ([HumanTrainingRequirementID]);
GO

-- Creating foreign key on [ProductDevelopmentRequirementID] in table 'ProductDevelopmentRequirementAttachmentFiles'
ALTER TABLE [dbo].[ProductDevelopmentRequirementAttachmentFiles]
ADD CONSTRAINT [FK_ProductDevelopmentRequirementAttachmentFiles_ProductDevelopmentRequirements]
    FOREIGN KEY ([ProductDevelopmentRequirementID])
    REFERENCES [dbo].[ProductDevelopmentRequirements]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirementAttachmentFiles_ProductDevelopmentRequirements'
CREATE INDEX [IX_FK_ProductDevelopmentRequirementAttachmentFiles_ProductDevelopmentRequirements]
ON [dbo].[ProductDevelopmentRequirementAttachmentFiles]
    ([ProductDevelopmentRequirementID]);
GO

-- Creating foreign key on [ProductDevelopmentRequirementPlanID] in table 'ProductDevelopmentRequirementPlanDocuments'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlanDocuments]
ADD CONSTRAINT [FK_ProductDevelopmentRequirementPlanDocument_ProductDevelopmentRequirementPlans]
    FOREIGN KEY ([ProductDevelopmentRequirementPlanID])
    REFERENCES [dbo].[ProductDevelopmentRequirementPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirementPlanDocument_ProductDevelopmentRequirementPlans'
CREATE INDEX [IX_FK_ProductDevelopmentRequirementPlanDocument_ProductDevelopmentRequirementPlans]
ON [dbo].[ProductDevelopmentRequirementPlanDocuments]
    ([ProductDevelopmentRequirementPlanID]);
GO

-- Creating foreign key on [ProductDevelopmentRequirementID] in table 'ProductDevelopmentRequirementPlans'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans]
ADD CONSTRAINT [FK_ProductDeveloperRequirementPlan_ProductDeveloperRequirements]
    FOREIGN KEY ([ProductDevelopmentRequirementID])
    REFERENCES [dbo].[ProductDevelopmentRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDeveloperRequirementPlan_ProductDeveloperRequirements'
CREATE INDEX [IX_FK_ProductDeveloperRequirementPlan_ProductDeveloperRequirements]
ON [dbo].[ProductDevelopmentRequirementPlans]
    ([ProductDevelopmentRequirementID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'ProductDevelopmentRequirementPlans'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans]
ADD CONSTRAINT [FK_ProductDeveloperRequirementPlan_QualityPlans]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE CASCADE ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDeveloperRequirementPlan_QualityPlans'
CREATE INDEX [IX_FK_ProductDeveloperRequirementPlan_QualityPlans]
ON [dbo].[ProductDevelopmentRequirementPlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [ProductionRequirementID] in table 'ProductDevelopmentRequirementPlans'
ALTER TABLE [dbo].[ProductDevelopmentRequirementPlans]
ADD CONSTRAINT [FK_ProductDevelopmentRequirementPlans_ProductionRequirements]
    FOREIGN KEY ([ProductionRequirementID])
    REFERENCES [dbo].[ProductionRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirementPlans_ProductionRequirements'
CREATE INDEX [IX_FK_ProductDevelopmentRequirementPlans_ProductionRequirements]
ON [dbo].[ProductDevelopmentRequirementPlans]
    ([ProductionRequirementID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductDevelopmentRequirements'
ALTER TABLE [dbo].[ProductDevelopmentRequirements]
ADD CONSTRAINT [FK_ProductDevelopmentRequirements_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductDevelopmentRequirements_StockProducts'
CREATE INDEX [IX_FK_ProductDevelopmentRequirements_StockProducts]
ON [dbo].[ProductDevelopmentRequirements]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionPlanID] in table 'ProductionCommands'
ALTER TABLE [dbo].[ProductionCommands]
ADD CONSTRAINT [FK_ProductionCommands_ProductionPlans]
    FOREIGN KEY ([ProductionPlanID])
    REFERENCES [dbo].[ProductionPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionCommands_ProductionPlans'
CREATE INDEX [IX_FK_ProductionCommands_ProductionPlans]
ON [dbo].[ProductionCommands]
    ([ProductionPlanID]);
GO

-- Creating foreign key on [ProductionRequirementID] in table 'ProductionCommands'
ALTER TABLE [dbo].[ProductionCommands]
ADD CONSTRAINT [FK_ProductionCommands_ProductionRequirements]
    FOREIGN KEY ([ProductionRequirementID])
    REFERENCES [dbo].[ProductionRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionCommands_ProductionRequirements'
CREATE INDEX [IX_FK_ProductionCommands_ProductionRequirements]
ON [dbo].[ProductionCommands]
    ([ProductionRequirementID]);
GO

-- Creating foreign key on [ProductionStageID] in table 'ProductionCommands'
ALTER TABLE [dbo].[ProductionCommands]
ADD CONSTRAINT [FK_ProductionCommands_ProductionStages]
    FOREIGN KEY ([ProductionStageID])
    REFERENCES [dbo].[ProductionStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionCommands_ProductionStages'
CREATE INDEX [IX_FK_ProductionCommands_ProductionStages]
ON [dbo].[ProductionCommands]
    ([ProductionStageID]);
GO

-- Creating foreign key on [ProductionCommandID] in table 'ProductionPerforms'
ALTER TABLE [dbo].[ProductionPerforms]
ADD CONSTRAINT [FK_ProductionPerfomrs_ProductionCommands]
    FOREIGN KEY ([ProductionCommandID])
    REFERENCES [dbo].[ProductionCommands]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerfomrs_ProductionCommands'
CREATE INDEX [IX_FK_ProductionPerfomrs_ProductionCommands]
ON [dbo].[ProductionPerforms]
    ([ProductionCommandID]);
GO

-- Creating foreign key on [ProductionLevelID] in table 'ProductionRequirements'
ALTER TABLE [dbo].[ProductionRequirements]
ADD CONSTRAINT [FK_ProductionRequirements_ProductionLevels]
    FOREIGN KEY ([ProductionLevelID])
    REFERENCES [dbo].[ProductionLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionRequirements_ProductionLevels'
CREATE INDEX [IX_FK_ProductionRequirements_ProductionLevels]
ON [dbo].[ProductionRequirements]
    ([ProductionLevelID]);
GO

-- Creating foreign key on [ProductionPerformID] in table 'ProductionPerformHistories'
ALTER TABLE [dbo].[ProductionPerformHistories]
ADD CONSTRAINT [FK_ProductionPerformHistories_ProductionPerfomrs]
    FOREIGN KEY ([ProductionPerformID])
    REFERENCES [dbo].[ProductionPerforms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformHistories_ProductionPerfomrs'
CREATE INDEX [IX_FK_ProductionPerformHistories_ProductionPerfomrs]
ON [dbo].[ProductionPerformHistories]
    ([ProductionPerformID]);
GO

-- Creating foreign key on [ProductionPerformID] in table 'ProductionPerformMaterias'
ALTER TABLE [dbo].[ProductionPerformMaterias]
ADD CONSTRAINT [FK_ProductionPerformMaterias_ProductionPerfomrs]
    FOREIGN KEY ([ProductionPerformID])
    REFERENCES [dbo].[ProductionPerforms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformMaterias_ProductionPerfomrs'
CREATE INDEX [IX_FK_ProductionPerformMaterias_ProductionPerfomrs]
ON [dbo].[ProductionPerformMaterias]
    ([ProductionPerformID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionPerformMaterias'
ALTER TABLE [dbo].[ProductionPerformMaterias]
ADD CONSTRAINT [FK_ProductionPerformMaterias_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformMaterias_StockProducts'
CREATE INDEX [IX_FK_ProductionPerformMaterias_StockProducts]
ON [dbo].[ProductionPerformMaterias]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionPerformID] in table 'ProductionPerformProductErrors'
ALTER TABLE [dbo].[ProductionPerformProductErrors]
ADD CONSTRAINT [FK_ProductionPerformProductErrors_ProductionPerforms]
    FOREIGN KEY ([ProductionPerformID])
    REFERENCES [dbo].[ProductionPerforms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformProductErrors_ProductionPerforms'
CREATE INDEX [IX_FK_ProductionPerformProductErrors_ProductionPerforms]
ON [dbo].[ProductionPerformProductErrors]
    ([ProductionPerformID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionPerformProductErrors'
ALTER TABLE [dbo].[ProductionPerformProductErrors]
ADD CONSTRAINT [FK_ProductionPerformProductErrors_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformProductErrors_StockProducts'
CREATE INDEX [IX_FK_ProductionPerformProductErrors_StockProducts]
ON [dbo].[ProductionPerformProductErrors]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionPerformID] in table 'ProductionPerformResults'
ALTER TABLE [dbo].[ProductionPerformResults]
ADD CONSTRAINT [FK_ProductionPerformResults_ProductionPerfomrs]
    FOREIGN KEY ([ProductionPerformID])
    REFERENCES [dbo].[ProductionPerforms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerformResults_ProductionPerfomrs'
CREATE INDEX [IX_FK_ProductionPerformResults_ProductionPerfomrs]
ON [dbo].[ProductionPerformResults]
    ([ProductionPerformID]);
GO

-- Creating foreign key on [ProductionShiftID] in table 'ProductionPerforms'
ALTER TABLE [dbo].[ProductionPerforms]
ADD CONSTRAINT [FK_ProductionPerfomrs_ProductionShifts]
    FOREIGN KEY ([ProductionShiftID])
    REFERENCES [dbo].[ProductionShifts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPerfomrs_ProductionShifts'
CREATE INDEX [IX_FK_ProductionPerfomrs_ProductionShifts]
ON [dbo].[ProductionPerforms]
    ([ProductionShiftID]);
GO

-- Creating foreign key on [ProductionPlanID] in table 'ProductionPlanEquipments'
ALTER TABLE [dbo].[ProductionPlanEquipments]
ADD CONSTRAINT [FK_ProductionPlanEquipments_ProductionPlans]
    FOREIGN KEY ([ProductionPlanID])
    REFERENCES [dbo].[ProductionPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanEquipments_ProductionPlans'
CREATE INDEX [IX_FK_ProductionPlanEquipments_ProductionPlans]
ON [dbo].[ProductionPlanEquipments]
    ([ProductionPlanID]);
GO

-- Creating foreign key on [ProductionPlanID] in table 'ProductionPlanMaterials'
ALTER TABLE [dbo].[ProductionPlanMaterials]
ADD CONSTRAINT [FK_ProductionPlanMaterials_ProductionPlans]
    FOREIGN KEY ([ProductionPlanID])
    REFERENCES [dbo].[ProductionPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanMaterials_ProductionPlans'
CREATE INDEX [IX_FK_ProductionPlanMaterials_ProductionPlans]
ON [dbo].[ProductionPlanMaterials]
    ([ProductionPlanID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionPlanMaterials'
ALTER TABLE [dbo].[ProductionPlanMaterials]
ADD CONSTRAINT [FK_ProductionPlanMaterials_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanMaterials_StockProducts'
CREATE INDEX [IX_FK_ProductionPlanMaterials_StockProducts]
ON [dbo].[ProductionPlanMaterials]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionPlanProductID] in table 'ProductionPlanProductDetails'
ALTER TABLE [dbo].[ProductionPlanProductDetails]
ADD CONSTRAINT [FK_ProductionPlanProductDetails_ProductionPlanProducts]
    FOREIGN KEY ([ProductionPlanProductID])
    REFERENCES [dbo].[ProductionPlanProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanProductDetails_ProductionPlanProducts'
CREATE INDEX [IX_FK_ProductionPlanProductDetails_ProductionPlanProducts]
ON [dbo].[ProductionPlanProductDetails]
    ([ProductionPlanProductID]);
GO

-- Creating foreign key on [ProductionPlanID] in table 'ProductionPlanProducts'
ALTER TABLE [dbo].[ProductionPlanProducts]
ADD CONSTRAINT [FK_ProductionPlanProducts_ProductionPlans]
    FOREIGN KEY ([ProductionPlanID])
    REFERENCES [dbo].[ProductionPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanProducts_ProductionPlans'
CREATE INDEX [IX_FK_ProductionPlanProducts_ProductionPlans]
ON [dbo].[ProductionPlanProducts]
    ([ProductionPlanID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionPlanProducts'
ALTER TABLE [dbo].[ProductionPlanProducts]
ADD CONSTRAINT [FK_ProductionPlanProducts_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlanProducts_StockProducts'
CREATE INDEX [IX_FK_ProductionPlanProducts_StockProducts]
ON [dbo].[ProductionPlanProducts]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionRequirementID] in table 'ProductionPlans'
ALTER TABLE [dbo].[ProductionPlans]
ADD CONSTRAINT [FK_ProductionPlans_ProductionRequirements]
    FOREIGN KEY ([ProductionRequirementID])
    REFERENCES [dbo].[ProductionRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionPlans_ProductionRequirements'
CREATE INDEX [IX_FK_ProductionPlans_ProductionRequirements]
ON [dbo].[ProductionPlans]
    ([ProductionRequirementID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionRequirements'
ALTER TABLE [dbo].[ProductionRequirements]
ADD CONSTRAINT [FK_ProductionRequirements_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionRequirements_StockProducts'
CREATE INDEX [IX_FK_ProductionRequirements_StockProducts]
ON [dbo].[ProductionRequirements]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionStageID] in table 'ProductionStageCriteriaCategories'
ALTER TABLE [dbo].[ProductionStageCriteriaCategories]
ADD CONSTRAINT [FK_ProductionStageCriteriaCategories_ProductionStages]
    FOREIGN KEY ([ProductionStageID])
    REFERENCES [dbo].[ProductionStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageCriteriaCategories_ProductionStages'
CREATE INDEX [IX_FK_ProductionStageCriteriaCategories_ProductionStages]
ON [dbo].[ProductionStageCriteriaCategories]
    ([ProductionStageID]);
GO

-- Creating foreign key on [QualityCriteriaCategoryID] in table 'ProductionStageCriteriaCategories'
ALTER TABLE [dbo].[ProductionStageCriteriaCategories]
ADD CONSTRAINT [FK_ProductionStageCriteriaCategories_QualityCriteriaCategories]
    FOREIGN KEY ([QualityCriteriaCategoryID])
    REFERENCES [dbo].[QualityCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageCriteriaCategories_QualityCriteriaCategories'
CREATE INDEX [IX_FK_ProductionStageCriteriaCategories_QualityCriteriaCategories]
ON [dbo].[ProductionStageCriteriaCategories]
    ([QualityCriteriaCategoryID]);
GO

-- Creating foreign key on [ProductionStageID] in table 'ProductionStageEquipments'
ALTER TABLE [dbo].[ProductionStageEquipments]
ADD CONSTRAINT [FK_ProductionStageEquipments_ProductionStages]
    FOREIGN KEY ([ProductionStageID])
    REFERENCES [dbo].[ProductionStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageEquipments_ProductionStages'
CREATE INDEX [IX_FK_ProductionStageEquipments_ProductionStages]
ON [dbo].[ProductionStageEquipments]
    ([ProductionStageID]);
GO

-- Creating foreign key on [ProductionStageID] in table 'ProductionStageMaterials'
ALTER TABLE [dbo].[ProductionStageMaterials]
ADD CONSTRAINT [FK_ProductionStageMaterials_ProductionStages]
    FOREIGN KEY ([ProductionStageID])
    REFERENCES [dbo].[ProductionStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageMaterials_ProductionStages'
CREATE INDEX [IX_FK_ProductionStageMaterials_ProductionStages]
ON [dbo].[ProductionStageMaterials]
    ([ProductionStageID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionStageMaterials'
ALTER TABLE [dbo].[ProductionStageMaterials]
ADD CONSTRAINT [FK_ProductionStageMaterials_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageMaterials_StockProducts'
CREATE INDEX [IX_FK_ProductionStageMaterials_StockProducts]
ON [dbo].[ProductionStageMaterials]
    ([StockProductID]);
GO

-- Creating foreign key on [ProductionStageID] in table 'ProductionStageProducts'
ALTER TABLE [dbo].[ProductionStageProducts]
ADD CONSTRAINT [FK_ProductionStageProducts_ProductionStages]
    FOREIGN KEY ([ProductionStageID])
    REFERENCES [dbo].[ProductionStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageProducts_ProductionStages'
CREATE INDEX [IX_FK_ProductionStageProducts_ProductionStages]
ON [dbo].[ProductionStageProducts]
    ([ProductionStageID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionStageProducts'
ALTER TABLE [dbo].[ProductionStageProducts]
ADD CONSTRAINT [FK_ProductionStageProducts_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStageProducts_StockProducts'
CREATE INDEX [IX_FK_ProductionStageProducts_StockProducts]
ON [dbo].[ProductionStageProducts]
    ([StockProductID]);
GO

-- Creating foreign key on [StockProductID] in table 'ProductionStages'
ALTER TABLE [dbo].[ProductionStages]
ADD CONSTRAINT [FK_ProductionStages_StockProducts]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductionStages_StockProducts'
CREATE INDEX [IX_FK_ProductionStages_StockProducts]
ON [dbo].[ProductionStages]
    ([StockProductID]);
GO

-- Creating foreign key on [ProfileID] in table 'ProfileAttachmentFiles'
ALTER TABLE [dbo].[ProfileAttachmentFiles]
ADD CONSTRAINT [FK_dbo_ProfileAttachments_dbo_Profiles_ProfileID]
    FOREIGN KEY ([ProfileID])
    REFERENCES [dbo].[Profiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileAttachments_dbo_Profiles_ProfileID'
CREATE INDEX [IX_FK_dbo_ProfileAttachments_dbo_Profiles_ProfileID]
ON [dbo].[ProfileAttachmentFiles]
    ([ProfileID]);
GO

-- Creating foreign key on [ProfileBorrowCategoryID] in table 'ProfileBorrows'
ALTER TABLE [dbo].[ProfileBorrows]
ADD CONSTRAINT [FK_dbo_ProfileBorrows_dbo_ProfileBorrowCategories_ProfileBorrowCategoryID]
    FOREIGN KEY ([ProfileBorrowCategoryID])
    REFERENCES [dbo].[ProfileBorrowCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileBorrows_dbo_ProfileBorrowCategories_ProfileBorrowCategoryID'
CREATE INDEX [IX_FK_dbo_ProfileBorrows_dbo_ProfileBorrowCategories_ProfileBorrowCategoryID]
ON [dbo].[ProfileBorrows]
    ([ProfileBorrowCategoryID]);
GO

-- Creating foreign key on [ProfileID] in table 'ProfileBorrows'
ALTER TABLE [dbo].[ProfileBorrows]
ADD CONSTRAINT [FK_dbo_ProfileBorrows_dbo_Profiles_ProfileID]
    FOREIGN KEY ([ProfileID])
    REFERENCES [dbo].[Profiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileBorrows_dbo_Profiles_ProfileID'
CREATE INDEX [IX_FK_dbo_ProfileBorrows_dbo_Profiles_ProfileID]
ON [dbo].[ProfileBorrows]
    ([ProfileID]);
GO

-- Creating foreign key on [ProfileCancelID] in table 'ProfileCancelEmployees'
ALTER TABLE [dbo].[ProfileCancelEmployees]
ADD CONSTRAINT [FK_dbo_ProfileCancelEmployees_dbo_ProfileCancels_ProfileCancelID]
    FOREIGN KEY ([ProfileCancelID])
    REFERENCES [dbo].[ProfileCancels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileCancelEmployees_dbo_ProfileCancels_ProfileCancelID'
CREATE INDEX [IX_FK_dbo_ProfileCancelEmployees_dbo_ProfileCancels_ProfileCancelID]
ON [dbo].[ProfileCancelEmployees]
    ([ProfileCancelID]);
GO

-- Creating foreign key on [ProfileCancelMethodID] in table 'ProfileCancels'
ALTER TABLE [dbo].[ProfileCancels]
ADD CONSTRAINT [FK_dbo_ProfileCancels_dbo_ProfileCancelMethods_ProfileCancelMethodID]
    FOREIGN KEY ([ProfileCancelMethodID])
    REFERENCES [dbo].[ProfileCancelMethods]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileCancels_dbo_ProfileCancelMethods_ProfileCancelMethodID'
CREATE INDEX [IX_FK_dbo_ProfileCancels_dbo_ProfileCancelMethods_ProfileCancelMethodID]
ON [dbo].[ProfileCancels]
    ([ProfileCancelMethodID]);
GO

-- Creating foreign key on [ProfileCancelID] in table 'ProfileCancelProfiles'
ALTER TABLE [dbo].[ProfileCancelProfiles]
ADD CONSTRAINT [FK_dbo_ProfileCancelProfiles_dbo_ProfileCancels_ProfileCancelID]
    FOREIGN KEY ([ProfileCancelID])
    REFERENCES [dbo].[ProfileCancels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileCancelProfiles_dbo_ProfileCancels_ProfileCancelID'
CREATE INDEX [IX_FK_dbo_ProfileCancelProfiles_dbo_ProfileCancels_ProfileCancelID]
ON [dbo].[ProfileCancelProfiles]
    ([ProfileCancelID]);
GO

-- Creating foreign key on [ProfileID] in table 'ProfileCancelProfiles'
ALTER TABLE [dbo].[ProfileCancelProfiles]
ADD CONSTRAINT [FK_dbo_ProfileCancelProfiles_dbo_Profiles_ProfileID]
    FOREIGN KEY ([ProfileID])
    REFERENCES [dbo].[Profiles]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ProfileCancelProfiles_dbo_Profiles_ProfileID'
CREATE INDEX [IX_FK_dbo_ProfileCancelProfiles_dbo_Profiles_ProfileID]
ON [dbo].[ProfileCancelProfiles]
    ([ProfileID]);
GO

-- Creating foreign key on [ProfileCategoryID] in table 'Profiles'
ALTER TABLE [dbo].[Profiles]
ADD CONSTRAINT [FK_dbo_Profiles_dbo_ProfileCategories_ProfileCategoryID]
    FOREIGN KEY ([ProfileCategoryID])
    REFERENCES [dbo].[ProfileCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Profiles_dbo_ProfileCategories_ProfileCategoryID'
CREATE INDEX [IX_FK_dbo_Profiles_dbo_ProfileCategories_ProfileCategoryID]
ON [dbo].[Profiles]
    ([ProfileCategoryID]);
GO

-- Creating foreign key on [ProfileSecurityID] in table 'Profiles'
ALTER TABLE [dbo].[Profiles]
ADD CONSTRAINT [FK_dbo_Profiles_dbo_ProfileSecurities_ProfileSecurityID]
    FOREIGN KEY ([ProfileSecurityID])
    REFERENCES [dbo].[ProfileSecurities]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Profiles_dbo_ProfileSecurities_ProfileSecurityID'
CREATE INDEX [IX_FK_dbo_Profiles_dbo_ProfileSecurities_ProfileSecurityID]
ON [dbo].[Profiles]
    ([ProfileSecurityID]);
GO

-- Creating foreign key on [QualityAuditPlanID] in table 'QualityAuditPrograms'
ALTER TABLE [dbo].[QualityAuditPrograms]
ADD CONSTRAINT [FK_dbo_QualityAuditPrograms_dbo_QualityAuditPlans_QualityAuditPlanID]
    FOREIGN KEY ([QualityAuditPlanID])
    REFERENCES [dbo].[QualityAuditPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityAuditPrograms_dbo_QualityAuditPlans_QualityAuditPlanID'
CREATE INDEX [IX_FK_dbo_QualityAuditPrograms_dbo_QualityAuditPlans_QualityAuditPlanID]
ON [dbo].[QualityAuditPrograms]
    ([QualityAuditPlanID]);
GO

-- Creating foreign key on [QualityAuditProgramID] in table 'QualityAuditProgramDepartments'
ALTER TABLE [dbo].[QualityAuditProgramDepartments]
ADD CONSTRAINT [FK_QualityAuditProgramDepartments_QualityAuditPrograms]
    FOREIGN KEY ([QualityAuditProgramID])
    REFERENCES [dbo].[QualityAuditPrograms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditProgramDepartments_QualityAuditPrograms'
CREATE INDEX [IX_FK_QualityAuditProgramDepartments_QualityAuditPrograms]
ON [dbo].[QualityAuditProgramDepartments]
    ([QualityAuditProgramID]);
GO

-- Creating foreign key on [QualityAuditProgramID] in table 'QualityAuditProgramEmployees'
ALTER TABLE [dbo].[QualityAuditProgramEmployees]
ADD CONSTRAINT [FK_QualityAuditProgramEmployees_QualityAuditPrograms]
    FOREIGN KEY ([QualityAuditProgramID])
    REFERENCES [dbo].[QualityAuditPrograms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditProgramEmployees_QualityAuditPrograms'
CREATE INDEX [IX_FK_QualityAuditProgramEmployees_QualityAuditPrograms]
ON [dbo].[QualityAuditProgramEmployees]
    ([QualityAuditProgramID]);
GO

-- Creating foreign key on [QualityAuditProgramID] in table 'QualityAuditProgramISOes'
ALTER TABLE [dbo].[QualityAuditProgramISOes]
ADD CONSTRAINT [FK_QualityAuditProgramISOIndexes_QualityAuditPrograms]
    FOREIGN KEY ([QualityAuditProgramID])
    REFERENCES [dbo].[QualityAuditPrograms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditProgramISOIndexes_QualityAuditPrograms'
CREATE INDEX [IX_FK_QualityAuditProgramISOIndexes_QualityAuditPrograms]
ON [dbo].[QualityAuditProgramISOes]
    ([QualityAuditProgramID]);
GO

-- Creating foreign key on [QualityAuditProgramISOID] in table 'QualityAuditResults'
ALTER TABLE [dbo].[QualityAuditResults]
ADD CONSTRAINT [FK_QualityAuditResults_QualityAuditProgramISOIndexes]
    FOREIGN KEY ([QualityAuditProgramISOID])
    REFERENCES [dbo].[QualityAuditProgramISOes]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditResults_QualityAuditProgramISOIndexes'
CREATE INDEX [IX_FK_QualityAuditResults_QualityAuditProgramISOIndexes]
ON [dbo].[QualityAuditResults]
    ([QualityAuditProgramISOID]);
GO

-- Creating foreign key on [QualityAuditProgramID] in table 'QualityAuditRecordedVotes'
ALTER TABLE [dbo].[QualityAuditRecordedVotes]
ADD CONSTRAINT [FK_QualityAuditRecordedVotes_QualityAuditPrograms]
    FOREIGN KEY ([QualityAuditProgramID])
    REFERENCES [dbo].[QualityAuditPrograms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditRecordedVotes_QualityAuditPrograms'
CREATE INDEX [IX_FK_QualityAuditRecordedVotes_QualityAuditPrograms]
ON [dbo].[QualityAuditRecordedVotes]
    ([QualityAuditProgramID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityAuditResults'
ALTER TABLE [dbo].[QualityAuditResults]
ADD CONSTRAINT [FK_QualityAuditResults_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityAuditResults_QualityNCs'
CREATE INDEX [IX_FK_QualityAuditResults_QualityNCs]
ON [dbo].[QualityAuditResults]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityCAPARequirements'
ALTER TABLE [dbo].[QualityCAPARequirements]
ADD CONSTRAINT [FK_dbo_QualityCAPARequirements_dbo_QualityNCs_QualityNCID]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityCAPARequirements_dbo_QualityNCs_QualityNCID'
CREATE INDEX [IX_FK_dbo_QualityCAPARequirements_dbo_QualityNCs_QualityNCID]
ON [dbo].[QualityCAPARequirements]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityCAPARequirementID] in table 'QualityCAPAs'
ALTER TABLE [dbo].[QualityCAPAs]
ADD CONSTRAINT [FK_dbo_QualityCAPAs_dbo_QualityCAPARequirements_QualityCAPARequirementID]
    FOREIGN KEY ([QualityCAPARequirementID])
    REFERENCES [dbo].[QualityCAPARequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityCAPAs_dbo_QualityCAPARequirements_QualityCAPARequirementID'
CREATE INDEX [IX_FK_dbo_QualityCAPAs_dbo_QualityCAPARequirements_QualityCAPARequirementID]
ON [dbo].[QualityCAPAs]
    ([QualityCAPARequirementID]);
GO

-- Creating foreign key on [QualityCAPAID] in table 'QualityCAPATasks'
ALTER TABLE [dbo].[QualityCAPATasks]
ADD CONSTRAINT [FK_dbo_QualityCAPATasks_dbo_QualityCAPAs_QualityCAPAID]
    FOREIGN KEY ([QualityCAPAID])
    REFERENCES [dbo].[QualityCAPAs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityCAPATasks_dbo_QualityCAPAs_QualityCAPAID'
CREATE INDEX [IX_FK_dbo_QualityCAPATasks_dbo_QualityCAPAs_QualityCAPAID]
ON [dbo].[QualityCAPATasks]
    ([QualityCAPAID]);
GO

-- Creating foreign key on [TaskID] in table 'QualityCAPATasks'
ALTER TABLE [dbo].[QualityCAPATasks]
ADD CONSTRAINT [FK_dbo_QualityCAPATasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityCAPATasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_QualityCAPATasks_dbo_Tasks_TaskID]
ON [dbo].[QualityCAPATasks]
    ([TaskID]);
GO

-- Creating foreign key on [QualityCriteriaCategoryID] in table 'QualityCriterias'
ALTER TABLE [dbo].[QualityCriterias]
ADD CONSTRAINT [FK_QualityCriterias_QualityCriteriaCategories]
    FOREIGN KEY ([QualityCriteriaCategoryID])
    REFERENCES [dbo].[QualityCriteriaCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityCriterias_QualityCriteriaCategories'
CREATE INDEX [IX_FK_QualityCriterias_QualityCriteriaCategories]
ON [dbo].[QualityCriterias]
    ([QualityCriteriaCategoryID]);
GO

-- Creating foreign key on [QualityCriteria] in table 'SupplierAuditResults'
ALTER TABLE [dbo].[SupplierAuditResults]
ADD CONSTRAINT [FK_SupplierAuditResults_QualityCriterias]
    FOREIGN KEY ([QualityCriteria])
    REFERENCES [dbo].[QualityCriterias]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierAuditResults_QualityCriterias'
CREATE INDEX [IX_FK_SupplierAuditResults_QualityCriterias]
ON [dbo].[SupplierAuditResults]
    ([QualityCriteria]);
GO

-- Creating foreign key on [QualityMeetingID] in table 'QualityMeetingAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingAttachmentFiles]
ADD CONSTRAINT [FK_QualityMeetingAttachmentFiles_QualityMeetings]
    FOREIGN KEY ([QualityMeetingID])
    REFERENCES [dbo].[QualityMeetings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityMeetingAttachmentFiles_QualityMeetings'
CREATE INDEX [IX_FK_QualityMeetingAttachmentFiles_QualityMeetings]
ON [dbo].[QualityMeetingAttachmentFiles]
    ([QualityMeetingID]);
GO

-- Creating foreign key on [QualityMeetingID] in table 'QualityMeetingObjects'
ALTER TABLE [dbo].[QualityMeetingObjects]
ADD CONSTRAINT [FK_dbo_QualityMeetingObjects_dbo_QualityMeetings_QualityMeetingID]
    FOREIGN KEY ([QualityMeetingID])
    REFERENCES [dbo].[QualityMeetings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityMeetingObjects_dbo_QualityMeetings_QualityMeetingID'
CREATE INDEX [IX_FK_dbo_QualityMeetingObjects_dbo_QualityMeetings_QualityMeetingID]
ON [dbo].[QualityMeetingObjects]
    ([QualityMeetingID]);
GO

-- Creating foreign key on [QualityMeetingID] in table 'QualityMeetingProblems'
ALTER TABLE [dbo].[QualityMeetingProblems]
ADD CONSTRAINT [FK_dbo_QualityMeetingProblems_dbo_QualityMeetings_QualityMeetingID]
    FOREIGN KEY ([QualityMeetingID])
    REFERENCES [dbo].[QualityMeetings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityMeetingProblems_dbo_QualityMeetings_QualityMeetingID'
CREATE INDEX [IX_FK_dbo_QualityMeetingProblems_dbo_QualityMeetings_QualityMeetingID]
ON [dbo].[QualityMeetingProblems]
    ([QualityMeetingID]);
GO

-- Creating foreign key on [QualityMeetingProblemID] in table 'QualityMeetingTasks'
ALTER TABLE [dbo].[QualityMeetingTasks]
ADD CONSTRAINT [FK_dbo_QualityMeetingTasks_dbo_QualityMeetingProblems_QualityMeetingProblemID]
    FOREIGN KEY ([QualityMeetingProblemID])
    REFERENCES [dbo].[QualityMeetingProblems]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityMeetingTasks_dbo_QualityMeetingProblems_QualityMeetingProblemID'
CREATE INDEX [IX_FK_dbo_QualityMeetingTasks_dbo_QualityMeetingProblems_QualityMeetingProblemID]
ON [dbo].[QualityMeetingTasks]
    ([QualityMeetingProblemID]);
GO

-- Creating foreign key on [QualityMeetingResultID] in table 'QualityMeetingResultAttachmentFiles'
ALTER TABLE [dbo].[QualityMeetingResultAttachmentFiles]
ADD CONSTRAINT [FK_QualityMeetingResultAttachmentFiles_QualityMeetingResults]
    FOREIGN KEY ([QualityMeetingResultID])
    REFERENCES [dbo].[QualityMeetingResults]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityMeetingResultAttachmentFiles_QualityMeetingResults'
CREATE INDEX [IX_FK_QualityMeetingResultAttachmentFiles_QualityMeetingResults]
ON [dbo].[QualityMeetingResultAttachmentFiles]
    ([QualityMeetingResultID]);
GO

-- Creating foreign key on [QualityMeetingID] in table 'QualityMeetingResults'
ALTER TABLE [dbo].[QualityMeetingResults]
ADD CONSTRAINT [FK_dbo_QualityMeetingResults_dbo_QualityMeetings_QualityMeetingID]
    FOREIGN KEY ([QualityMeetingID])
    REFERENCES [dbo].[QualityMeetings]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityMeetingResults_dbo_QualityMeetings_QualityMeetingID'
CREATE INDEX [IX_FK_dbo_QualityMeetingResults_dbo_QualityMeetings_QualityMeetingID]
ON [dbo].[QualityMeetingResults]
    ([QualityMeetingID]);
GO

-- Creating foreign key on [TaskID] in table 'QualityMeetingTasks'
ALTER TABLE [dbo].[QualityMeetingTasks]
ADD CONSTRAINT [FK_dbo_QualityMeetingTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityMeetingTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_QualityMeetingTasks_dbo_Tasks_TaskID]
ON [dbo].[QualityMeetingTasks]
    ([TaskID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityNCEmployees'
ALTER TABLE [dbo].[QualityNCEmployees]
ADD CONSTRAINT [FK_dbo_QualityNCEmployees_dbo_QualityNCs_QualityNCID]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityNCEmployees_dbo_QualityNCs_QualityNCID'
CREATE INDEX [IX_FK_dbo_QualityNCEmployees_dbo_QualityNCs_QualityNCID]
ON [dbo].[QualityNCEmployees]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityNCRoles'
ALTER TABLE [dbo].[QualityNCRoles]
ADD CONSTRAINT [FK_dbo_QualityNCRoles_dbo_QualityNCs_QualityNCID]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityNCRoles_dbo_QualityNCs_QualityNCID'
CREATE INDEX [IX_FK_dbo_QualityNCRoles_dbo_QualityNCs_QualityNCID]
ON [dbo].[QualityNCRoles]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityNCTasks'
ALTER TABLE [dbo].[QualityNCTasks]
ADD CONSTRAINT [FK_dbo_QualityNCTasks_dbo_QualityNCs_QualityNCID]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityNCTasks_dbo_QualityNCs_QualityNCID'
CREATE INDEX [IX_FK_dbo_QualityNCTasks_dbo_QualityNCs_QualityNCID]
ON [dbo].[QualityNCTasks]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityNCID] in table 'QualityNCStockAdjustments'
ALTER TABLE [dbo].[QualityNCStockAdjustments]
ADD CONSTRAINT [FK_QualityNCStockAdjustments_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityNCStockAdjustments_QualityNCs'
CREATE INDEX [IX_FK_QualityNCStockAdjustments_QualityNCs]
ON [dbo].[QualityNCStockAdjustments]
    ([QualityNCID]);
GO

-- Creating foreign key on [QualityNCID] in table 'RiskAudits'
ALTER TABLE [dbo].[RiskAudits]
ADD CONSTRAINT [FK_RiskAudits_QualityNCs]
    FOREIGN KEY ([QualityNCID])
    REFERENCES [dbo].[QualityNCs]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskAudits_QualityNCs'
CREATE INDEX [IX_FK_RiskAudits_QualityNCs]
ON [dbo].[RiskAudits]
    ([QualityNCID]);
GO

-- Creating foreign key on [StockAdjustmentID] in table 'QualityNCStockAdjustments'
ALTER TABLE [dbo].[QualityNCStockAdjustments]
ADD CONSTRAINT [FK_QualityNCStockAdjustments_StockAdjustments]
    FOREIGN KEY ([StockAdjustmentID])
    REFERENCES [dbo].[StockAdjustments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityNCStockAdjustments_StockAdjustments'
CREATE INDEX [IX_FK_QualityNCStockAdjustments_StockAdjustments]
ON [dbo].[QualityNCStockAdjustments]
    ([StockAdjustmentID]);
GO

-- Creating foreign key on [TaskID] in table 'QualityNCTasks'
ALTER TABLE [dbo].[QualityNCTasks]
ADD CONSTRAINT [FK_dbo_QualityNCTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityNCTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_QualityNCTasks_dbo_Tasks_TaskID]
ON [dbo].[QualityNCTasks]
    ([TaskID]);
GO

-- Creating foreign key on [QualityTargetID] in table 'QualityPlans'
ALTER TABLE [dbo].[QualityPlans]
ADD CONSTRAINT [FK_dbo_QualityPlans_dbo_QualityTargets_QualityTargetID]
    FOREIGN KEY ([QualityTargetID])
    REFERENCES [dbo].[QualityTargets]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityPlans_dbo_QualityTargets_QualityTargetID'
CREATE INDEX [IX_FK_dbo_QualityPlans_dbo_QualityTargets_QualityTargetID]
ON [dbo].[QualityPlans]
    ([QualityTargetID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'QualityPlanTasks'
ALTER TABLE [dbo].[QualityPlanTasks]
ADD CONSTRAINT [FK_dbo_QualityPlanTasks_dbo_QualityPlans_QualityPlanID]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityPlanTasks_dbo_QualityPlans_QualityPlanID'
CREATE INDEX [IX_FK_dbo_QualityPlanTasks_dbo_QualityPlans_QualityPlanID]
ON [dbo].[QualityPlanTasks]
    ([QualityPlanID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'ServicePlans'
ALTER TABLE [dbo].[ServicePlans]
ADD CONSTRAINT [FK_dbo_ServicePlans_dbo_QualityPlans_QualityPlanID]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServicePlans_dbo_QualityPlans_QualityPlanID'
CREATE INDEX [IX_FK_dbo_ServicePlans_dbo_QualityPlans_QualityPlanID]
ON [dbo].[ServicePlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [QualityPlanID] in table 'SupplierAuditPlans'
ALTER TABLE [dbo].[SupplierAuditPlans]
ADD CONSTRAINT [FK_SupplierAuditPlans_QualityPlans]
    FOREIGN KEY ([QualityPlanID])
    REFERENCES [dbo].[QualityPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierAuditPlans_QualityPlans'
CREATE INDEX [IX_FK_SupplierAuditPlans_QualityPlans]
ON [dbo].[SupplierAuditPlans]
    ([QualityPlanID]);
GO

-- Creating foreign key on [TaskID] in table 'QualityPlanTasks'
ALTER TABLE [dbo].[QualityPlanTasks]
ADD CONSTRAINT [FK_dbo_QualityPlanTasks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_QualityPlanTasks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_QualityPlanTasks_dbo_Tasks_TaskID]
ON [dbo].[QualityPlanTasks]
    ([TaskID]);
GO

-- Creating foreign key on [QualityTargetCategoryID] in table 'QualityTargets'
ALTER TABLE [dbo].[QualityTargets]
ADD CONSTRAINT [FK_QualityTargets_QualityTargetCategories]
    FOREIGN KEY ([QualityTargetCategoryID])
    REFERENCES [dbo].[QualityTargetCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityTargets_QualityTargetCategories'
CREATE INDEX [IX_FK_QualityTargets_QualityTargetCategories]
ON [dbo].[QualityTargets]
    ([QualityTargetCategoryID]);
GO

-- Creating foreign key on [QualityTargetLevelID] in table 'QualityTargets'
ALTER TABLE [dbo].[QualityTargets]
ADD CONSTRAINT [FK_QualityTargets_QualityTargetLevels]
    FOREIGN KEY ([QualityTargetLevelID])
    REFERENCES [dbo].[QualityTargetLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_QualityTargets_QualityTargetLevels'
CREATE INDEX [IX_FK_QualityTargets_QualityTargetLevels]
ON [dbo].[QualityTargets]
    ([QualityTargetLevelID]);
GO

-- Creating foreign key on [ReportTemplateID] in table 'Reports'
ALTER TABLE [dbo].[Reports]
ADD CONSTRAINT [FK_Reports_ReportTemplates]
    FOREIGN KEY ([ReportTemplateID])
    REFERENCES [dbo].[ReportTemplates]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Reports_ReportTemplates'
CREATE INDEX [IX_FK_Reports_ReportTemplates]
ON [dbo].[Reports]
    ([ReportTemplateID]);
GO

-- Creating foreign key on [RiskControlID] in table 'RiskAudits'
ALTER TABLE [dbo].[RiskAudits]
ADD CONSTRAINT [FK_RiskAudits_RiskControls]
    FOREIGN KEY ([RiskControlID])
    REFERENCES [dbo].[RiskControls]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskAudits_RiskControls'
CREATE INDEX [IX_FK_RiskAudits_RiskControls]
ON [dbo].[RiskAudits]
    ([RiskControlID]);
GO

-- Creating foreign key on [RiskID] in table 'RiskContracts'
ALTER TABLE [dbo].[RiskContracts]
ADD CONSTRAINT [FK_RiskContracts_Risks]
    FOREIGN KEY ([RiskID])
    REFERENCES [dbo].[Risks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskContracts_Risks'
CREATE INDEX [IX_FK_RiskContracts_Risks]
ON [dbo].[RiskContracts]
    ([RiskID]);
GO

-- Creating foreign key on [RiskID] in table 'RiskControls'
ALTER TABLE [dbo].[RiskControls]
ADD CONSTRAINT [FK_RiskControls_Risks]
    FOREIGN KEY ([RiskID])
    REFERENCES [dbo].[Risks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskControls_Risks'
CREATE INDEX [IX_FK_RiskControls_Risks]
ON [dbo].[RiskControls]
    ([RiskID]);
GO

-- Creating foreign key on [RiskControlID] in table 'RiskControlSolutions'
ALTER TABLE [dbo].[RiskControlSolutions]
ADD CONSTRAINT [FK_RiskControlSolutions_RiskControls]
    FOREIGN KEY ([RiskControlID])
    REFERENCES [dbo].[RiskControls]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskControlSolutions_RiskControls'
CREATE INDEX [IX_FK_RiskControlSolutions_RiskControls]
ON [dbo].[RiskControlSolutions]
    ([RiskControlID]);
GO

-- Creating foreign key on [RiskControlID] in table 'RiskControlTasks'
ALTER TABLE [dbo].[RiskControlTasks]
ADD CONSTRAINT [FK_RiskControlTasks_RiskControls]
    FOREIGN KEY ([RiskControlID])
    REFERENCES [dbo].[RiskControls]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskControlTasks_RiskControls'
CREATE INDEX [IX_FK_RiskControlTasks_RiskControls]
ON [dbo].[RiskControlTasks]
    ([RiskControlID]);
GO

-- Creating foreign key on [RiskLibrarySolutionID] in table 'RiskControlSolutions'
ALTER TABLE [dbo].[RiskControlSolutions]
ADD CONSTRAINT [FK_RiskControlSolutions_RiskLibrarySolutions]
    FOREIGN KEY ([RiskLibrarySolutionID])
    REFERENCES [dbo].[RiskLibrarySolutions]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskControlSolutions_RiskLibrarySolutions'
CREATE INDEX [IX_FK_RiskControlSolutions_RiskLibrarySolutions]
ON [dbo].[RiskControlSolutions]
    ([RiskLibrarySolutionID]);
GO

-- Creating foreign key on [TaskID] in table 'RiskControlTasks'
ALTER TABLE [dbo].[RiskControlTasks]
ADD CONSTRAINT [FK_RiskControlTasks_Tasks]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskControlTasks_Tasks'
CREATE INDEX [IX_FK_RiskControlTasks_Tasks]
ON [dbo].[RiskControlTasks]
    ([TaskID]);
GO

-- Creating foreign key on [RiskID] in table 'RiskIncedents'
ALTER TABLE [dbo].[RiskIncedents]
ADD CONSTRAINT [FK_RiskIncedents_Risks]
    FOREIGN KEY ([RiskID])
    REFERENCES [dbo].[Risks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskIncedents_Risks'
CREATE INDEX [IX_FK_RiskIncedents_Risks]
ON [dbo].[RiskIncedents]
    ([RiskID]);
GO

-- Creating foreign key on [RiskID] in table 'RiskLegals'
ALTER TABLE [dbo].[RiskLegals]
ADD CONSTRAINT [FK_RiskLegals_Risks]
    FOREIGN KEY ([RiskID])
    REFERENCES [dbo].[Risks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskLegals_Risks'
CREATE INDEX [IX_FK_RiskLegals_Risks]
ON [dbo].[RiskLegals]
    ([RiskID]);
GO

-- Creating foreign key on [RiskID] in table 'RiskRegulatories'
ALTER TABLE [dbo].[RiskRegulatories]
ADD CONSTRAINT [FK_RiskRegulatories_Risks]
    FOREIGN KEY ([RiskID])
    REFERENCES [dbo].[Risks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_RiskRegulatories_Risks'
CREATE INDEX [IX_FK_RiskRegulatories_Risks]
ON [dbo].[RiskRegulatories]
    ([RiskID]);
GO

-- Creating foreign key on [ServiceCategoryID] in table 'Services'
ALTER TABLE [dbo].[Services]
ADD CONSTRAINT [FK_dbo_Services_dbo_ServiceCategories_ServiceCategoryID]
    FOREIGN KEY ([ServiceCategoryID])
    REFERENCES [dbo].[ServiceCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Services_dbo_ServiceCategories_ServiceCategoryID'
CREATE INDEX [IX_FK_dbo_Services_dbo_ServiceCategories_ServiceCategoryID]
ON [dbo].[Services]
    ([ServiceCategoryID]);
GO

-- Creating foreign key on [ServiceCommandID] in table 'ServiceCommandContracts'
ALTER TABLE [dbo].[ServiceCommandContracts]
ADD CONSTRAINT [FK_dbo_ServiceCommandContracts_dbo_ServiceCommands_ServiceCommandID]
    FOREIGN KEY ([ServiceCommandID])
    REFERENCES [dbo].[ServiceCommands]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServiceCommandContracts_dbo_ServiceCommands_ServiceCommandID'
CREATE INDEX [IX_FK_dbo_ServiceCommandContracts_dbo_ServiceCommands_ServiceCommandID]
ON [dbo].[ServiceCommandContracts]
    ([ServiceCommandID]);
GO

-- Creating foreign key on [ServiceCommandContractID] in table 'ServicePlans'
ALTER TABLE [dbo].[ServicePlans]
ADD CONSTRAINT [FK_dbo_ServicePlans_dbo_ServiceCommandContracts_ServiceCommandContractID]
    FOREIGN KEY ([ServiceCommandContractID])
    REFERENCES [dbo].[ServiceCommandContracts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServicePlans_dbo_ServiceCommandContracts_ServiceCommandContractID'
CREATE INDEX [IX_FK_dbo_ServicePlans_dbo_ServiceCommandContracts_ServiceCommandContractID]
ON [dbo].[ServicePlans]
    ([ServiceCommandContractID]);
GO

-- Creating foreign key on [ServicePlanID] in table 'ServicePlanStages'
ALTER TABLE [dbo].[ServicePlanStages]
ADD CONSTRAINT [FK_dbo_ServicePlanStages_dbo_ServicePlans_ServicePlanID]
    FOREIGN KEY ([ServicePlanID])
    REFERENCES [dbo].[ServicePlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServicePlanStages_dbo_ServicePlans_ServicePlanID'
CREATE INDEX [IX_FK_dbo_ServicePlanStages_dbo_ServicePlans_ServicePlanID]
ON [dbo].[ServicePlanStages]
    ([ServicePlanID]);
GO

-- Creating foreign key on [ServiceStageID] in table 'ServicePlanStages'
ALTER TABLE [dbo].[ServicePlanStages]
ADD CONSTRAINT [FK_dbo_ServicePlanStages_dbo_ServiceStages_ServiceStageID]
    FOREIGN KEY ([ServiceStageID])
    REFERENCES [dbo].[ServiceStages]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServicePlanStages_dbo_ServiceStages_ServiceStageID'
CREATE INDEX [IX_FK_dbo_ServicePlanStages_dbo_ServiceStages_ServiceStageID]
ON [dbo].[ServicePlanStages]
    ([ServiceStageID]);
GO

-- Creating foreign key on [ServiceID] in table 'ServiceStages'
ALTER TABLE [dbo].[ServiceStages]
ADD CONSTRAINT [FK_dbo_ServiceStages_dbo_Services_ServiceID]
    FOREIGN KEY ([ServiceID])
    REFERENCES [dbo].[Services]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_ServiceStages_dbo_Services_ServiceID'
CREATE INDEX [IX_FK_dbo_ServiceStages_dbo_Services_ServiceID]
ON [dbo].[ServiceStages]
    ([ServiceID]);
GO

-- Creating foreign key on [StockAdjustmentID] in table 'StockAdjustmentDetails'
ALTER TABLE [dbo].[StockAdjustmentDetails]
ADD CONSTRAINT [FK_dbo_StockAdjustmentDetails_dbo_StockAdjustments_StockAdjustmentID]
    FOREIGN KEY ([StockAdjustmentID])
    REFERENCES [dbo].[StockAdjustments]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockAdjustmentDetails_dbo_StockAdjustments_StockAdjustmentID'
CREATE INDEX [IX_FK_dbo_StockAdjustmentDetails_dbo_StockAdjustments_StockAdjustmentID]
ON [dbo].[StockAdjustmentDetails]
    ([StockAdjustmentID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockAdjustmentDetails'
ALTER TABLE [dbo].[StockAdjustmentDetails]
ADD CONSTRAINT [FK_dbo_StockAdjustmentDetails_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockAdjustmentDetails_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockAdjustmentDetails_dbo_StockProducts_StockProductID]
ON [dbo].[StockAdjustmentDetails]
    ([StockProductID]);
GO

-- Creating foreign key on [StockBuildID] in table 'StockBuildDetails'
ALTER TABLE [dbo].[StockBuildDetails]
ADD CONSTRAINT [FK_dbo_StockBuildDetails_dbo_StockBuilds_StockBuildID]
    FOREIGN KEY ([StockBuildID])
    REFERENCES [dbo].[StockBuilds]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockBuildDetails_dbo_StockBuilds_StockBuildID'
CREATE INDEX [IX_FK_dbo_StockBuildDetails_dbo_StockBuilds_StockBuildID]
ON [dbo].[StockBuildDetails]
    ([StockBuildID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockInventories'
ALTER TABLE [dbo].[StockInventories]
ADD CONSTRAINT [FK_dbo_StockInventories_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInventories_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockInventories_dbo_StockProducts_StockProductID]
ON [dbo].[StockInventories]
    ([StockProductID]);
GO

-- Creating foreign key on [StockInventoryID] in table 'StockInventoryDetails'
ALTER TABLE [dbo].[StockInventoryDetails]
ADD CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_StockInventories_StockInventoryID]
    FOREIGN KEY ([StockInventoryID])
    REFERENCES [dbo].[StockInventories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInventoryDetails_dbo_StockInventories_StockInventoryID'
CREATE INDEX [IX_FK_dbo_StockInventoryDetails_dbo_StockInventories_StockInventoryID]
ON [dbo].[StockInventoryDetails]
    ([StockInventoryID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockInventoryDetails'
ALTER TABLE [dbo].[StockInventoryDetails]
ADD CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInventoryDetails_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockInventoryDetails_dbo_StockProducts_StockProductID]
ON [dbo].[StockInventoryDetails]
    ([StockProductID]);
GO

-- Creating foreign key on [StockID] in table 'StockInventoryDetails'
ALTER TABLE [dbo].[StockInventoryDetails]
ADD CONSTRAINT [FK_dbo_StockInventoryDetails_dbo_Stocks_StockID]
    FOREIGN KEY ([StockID])
    REFERENCES [dbo].[Stocks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInventoryDetails_dbo_Stocks_StockID'
CREATE INDEX [IX_FK_dbo_StockInventoryDetails_dbo_Stocks_StockID]
ON [dbo].[StockInventoryDetails]
    ([StockID]);
GO

-- Creating foreign key on [StockInwardID] in table 'StockInwardDetails'
ALTER TABLE [dbo].[StockInwardDetails]
ADD CONSTRAINT [FK_dbo_StockInwardDetails_dbo_StockInwards_StockInwardID]
    FOREIGN KEY ([StockInwardID])
    REFERENCES [dbo].[StockInwards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInwardDetails_dbo_StockInwards_StockInwardID'
CREATE INDEX [IX_FK_dbo_StockInwardDetails_dbo_StockInwards_StockInwardID]
ON [dbo].[StockInwardDetails]
    ([StockInwardID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockInwardDetails'
ALTER TABLE [dbo].[StockInwardDetails]
ADD CONSTRAINT [FK_dbo_StockInwardDetails_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockInwardDetails_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockInwardDetails_dbo_StockProducts_StockProductID]
ON [dbo].[StockInwardDetails]
    ([StockProductID]);
GO

-- Creating foreign key on [StockOutwardID] in table 'StockOutwardDetails'
ALTER TABLE [dbo].[StockOutwardDetails]
ADD CONSTRAINT [FK_dbo_StockOutwardDetails_dbo_StockOutwards_StockOutwardID]
    FOREIGN KEY ([StockOutwardID])
    REFERENCES [dbo].[StockOutwards]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockOutwardDetails_dbo_StockOutwards_StockOutwardID'
CREATE INDEX [IX_FK_dbo_StockOutwardDetails_dbo_StockOutwards_StockOutwardID]
ON [dbo].[StockOutwardDetails]
    ([StockOutwardID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockOutwardDetails'
ALTER TABLE [dbo].[StockOutwardDetails]
ADD CONSTRAINT [FK_dbo_StockOutwardDetails_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockOutwardDetails_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockOutwardDetails_dbo_StockProducts_StockProductID]
ON [dbo].[StockOutwardDetails]
    ([StockProductID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockProductBuilds'
ALTER TABLE [dbo].[StockProductBuilds]
ADD CONSTRAINT [FK_dbo_StockProductBuilds_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockProductBuilds_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockProductBuilds_dbo_StockProducts_StockProductID]
ON [dbo].[StockProductBuilds]
    ([StockProductID]);
GO

-- Creating foreign key on [StockProductGroupID] in table 'StockProducts'
ALTER TABLE [dbo].[StockProducts]
ADD CONSTRAINT [FK_dbo_StockProducts_dbo_StockProductGroups_StockProductGroupID]
    FOREIGN KEY ([StockProductGroupID])
    REFERENCES [dbo].[StockProductGroups]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockProducts_dbo_StockProductGroups_StockProductGroupID'
CREATE INDEX [IX_FK_dbo_StockProducts_dbo_StockProductGroups_StockProductGroupID]
ON [dbo].[StockProducts]
    ([StockProductGroupID]);
GO

-- Creating foreign key on [StockID] in table 'StockProducts'
ALTER TABLE [dbo].[StockProducts]
ADD CONSTRAINT [FK_dbo_StockProducts_dbo_Stocks_StockID]
    FOREIGN KEY ([StockID])
    REFERENCES [dbo].[Stocks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockProducts_dbo_Stocks_StockID'
CREATE INDEX [IX_FK_dbo_StockProducts_dbo_Stocks_StockID]
ON [dbo].[StockProducts]
    ([StockID]);
GO

-- Creating foreign key on [StockProductID] in table 'StockTransferDetails'
ALTER TABLE [dbo].[StockTransferDetails]
ADD CONSTRAINT [FK_dbo_StockTransferDetails_dbo_StockProducts_StockProductID]
    FOREIGN KEY ([StockProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_StockTransferDetails_dbo_StockProducts_StockProductID'
CREATE INDEX [IX_FK_dbo_StockTransferDetails_dbo_StockProducts_StockProductID]
ON [dbo].[StockTransferDetails]
    ([StockProductID]);
GO

-- Creating foreign key on [StocksProductID] in table 'SuppliersOrderDetails'
ALTER TABLE [dbo].[SuppliersOrderDetails]
ADD CONSTRAINT [FK_SuppliersOrderDetail_StockProducts]
    FOREIGN KEY ([StocksProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersOrderDetail_StockProducts'
CREATE INDEX [IX_FK_SuppliersOrderDetail_StockProducts]
ON [dbo].[SuppliersOrderDetails]
    ([StocksProductID]);
GO

-- Creating foreign key on [StocksProductID] in table 'SuppliersOrderRequirementDetails'
ALTER TABLE [dbo].[SuppliersOrderRequirementDetails]
ADD CONSTRAINT [FK_SuppliersOrderRequirementDetails_StockProducts]
    FOREIGN KEY ([StocksProductID])
    REFERENCES [dbo].[StockProducts]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersOrderRequirementDetails_StockProducts'
CREATE INDEX [IX_FK_SuppliersOrderRequirementDetails_StockProducts]
ON [dbo].[SuppliersOrderRequirementDetails]
    ([StocksProductID]);
GO

-- Creating foreign key on [StockTransferID] in table 'StockTransferDetails'
ALTER TABLE [dbo].[StockTransferDetails]
ADD CONSTRAINT [FK_StockTransferDetails_StockTransfers]
    FOREIGN KEY ([StockTransferID])
    REFERENCES [dbo].[StockTransfers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_StockTransferDetails_StockTransfers'
CREATE INDEX [IX_FK_StockTransferDetails_StockTransfers]
ON [dbo].[StockTransferDetails]
    ([StockTransferID]);
GO

-- Creating foreign key on [SupplierAuditPlanID] in table 'SupplierAudits'
ALTER TABLE [dbo].[SupplierAudits]
ADD CONSTRAINT [FK_SupplierAudits_SupplierAuditPlans]
    FOREIGN KEY ([SupplierAuditPlanID])
    REFERENCES [dbo].[SupplierAuditPlans]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierAudits_SupplierAuditPlans'
CREATE INDEX [IX_FK_SupplierAudits_SupplierAuditPlans]
ON [dbo].[SupplierAudits]
    ([SupplierAuditPlanID]);
GO

-- Creating foreign key on [SupplierAuditID] in table 'SupplierAuditResults'
ALTER TABLE [dbo].[SupplierAuditResults]
ADD CONSTRAINT [FK_SupplierAuditResults_SupplierAudits]
    FOREIGN KEY ([SupplierAuditID])
    REFERENCES [dbo].[SupplierAudits]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierAuditResults_SupplierAudits'
CREATE INDEX [IX_FK_SupplierAuditResults_SupplierAudits]
ON [dbo].[SupplierAuditResults]
    ([SupplierAuditID]);
GO

-- Creating foreign key on [SupplierID] in table 'SupplierAudits'
ALTER TABLE [dbo].[SupplierAudits]
ADD CONSTRAINT [FK_SupplierAudits_Suppliers]
    FOREIGN KEY ([SupplierID])
    REFERENCES [dbo].[Suppliers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierAudits_Suppliers'
CREATE INDEX [IX_FK_SupplierAudits_Suppliers]
ON [dbo].[SupplierAudits]
    ([SupplierID]);
GO

-- Creating foreign key on [SupplierBidOrderID] in table 'SupplierBidToAttachmentFiles'
ALTER TABLE [dbo].[SupplierBidToAttachmentFiles]
ADD CONSTRAINT [FK_SupplierBidToAttachmentFiles_SuppliersBidOrders]
    FOREIGN KEY ([SupplierBidOrderID])
    REFERENCES [dbo].[SuppliersBidOrders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierBidToAttachmentFiles_SuppliersBidOrders'
CREATE INDEX [IX_FK_SupplierBidToAttachmentFiles_SuppliersBidOrders]
ON [dbo].[SupplierBidToAttachmentFiles]
    ([SupplierBidOrderID]);
GO

-- Creating foreign key on [SuppliersGroupID] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [FK_Suppliers_SuppliersGroups]
    FOREIGN KEY ([SuppliersGroupID])
    REFERENCES [dbo].[SuppliersGroups]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_Suppliers_SuppliersGroups'
CREATE INDEX [IX_FK_Suppliers_SuppliersGroups]
ON [dbo].[Suppliers]
    ([SuppliersGroupID]);
GO

-- Creating foreign key on [SupplierID] in table 'SuppliersBidOrders'
ALTER TABLE [dbo].[SuppliersBidOrders]
ADD CONSTRAINT [FK_SuppliersBidOrders_Suppliers]
    FOREIGN KEY ([SupplierID])
    REFERENCES [dbo].[Suppliers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersBidOrders_Suppliers'
CREATE INDEX [IX_FK_SuppliersBidOrders_Suppliers]
ON [dbo].[SuppliersBidOrders]
    ([SupplierID]);
GO

-- Creating foreign key on [SupplierID] in table 'SuppliersOrders'
ALTER TABLE [dbo].[SuppliersOrders]
ADD CONSTRAINT [FK_SuppliersOrder_Suppliers]
    FOREIGN KEY ([SupplierID])
    REFERENCES [dbo].[Suppliers]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersOrder_Suppliers'
CREATE INDEX [IX_FK_SuppliersOrder_Suppliers]
ON [dbo].[SuppliersOrders]
    ([SupplierID]);
GO

-- Creating foreign key on [SuppliersOrderID] in table 'SuppliersBidOrders'
ALTER TABLE [dbo].[SuppliersBidOrders]
ADD CONSTRAINT [FK_SuppliersBidOrders_SuppliersOrders]
    FOREIGN KEY ([SuppliersOrderID])
    REFERENCES [dbo].[SuppliersOrders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersBidOrders_SuppliersOrders'
CREATE INDEX [IX_FK_SuppliersBidOrders_SuppliersOrders]
ON [dbo].[SuppliersBidOrders]
    ([SuppliersOrderID]);
GO

-- Creating foreign key on [SuppliersOrderID] in table 'SuppliersBills'
ALTER TABLE [dbo].[SuppliersBills]
ADD CONSTRAINT [FK_SuppliersBill_SuppliersOrder]
    FOREIGN KEY ([SuppliersOrderID])
    REFERENCES [dbo].[SuppliersOrders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersBill_SuppliersOrder'
CREATE INDEX [IX_FK_SuppliersBill_SuppliersOrder]
ON [dbo].[SuppliersBills]
    ([SuppliersOrderID]);
GO

-- Creating foreign key on [SuppliersOrderID] in table 'SuppliersOrderDetails'
ALTER TABLE [dbo].[SuppliersOrderDetails]
ADD CONSTRAINT [FK_SuppliersOrderDetail_SuppliersOrder]
    FOREIGN KEY ([SuppliersOrderID])
    REFERENCES [dbo].[SuppliersOrders]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersOrderDetail_SuppliersOrder'
CREATE INDEX [IX_FK_SuppliersOrderDetail_SuppliersOrder]
ON [dbo].[SuppliersOrderDetails]
    ([SuppliersOrderID]);
GO

-- Creating foreign key on [SuppliersOrderRequirementID] in table 'SuppliersOrderRequirementDetails'
ALTER TABLE [dbo].[SuppliersOrderRequirementDetails]
ADD CONSTRAINT [FK_SuppliersOrderRequirementDetails_SuppliersOrderRequirements]
    FOREIGN KEY ([SuppliersOrderRequirementID])
    REFERENCES [dbo].[SuppliersOrderRequirements]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_SuppliersOrderRequirementDetails_SuppliersOrderRequirements'
CREATE INDEX [IX_FK_SuppliersOrderRequirementDetails_SuppliersOrderRequirements]
ON [dbo].[SuppliersOrderRequirementDetails]
    ([SuppliersOrderRequirementID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskAttachmentFiles'
ALTER TABLE [dbo].[TaskAttachmentFiles]
ADD CONSTRAINT [FK_TaskAttachmentFiles_Tasks]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskAttachmentFiles_Tasks'
CREATE INDEX [IX_FK_TaskAttachmentFiles_Tasks]
ON [dbo].[TaskAttachmentFiles]
    ([TaskID]);
GO

-- Creating foreign key on [TaskCalendarEventID] in table 'TaskCalendars'
ALTER TABLE [dbo].[TaskCalendars]
ADD CONSTRAINT [FK_dbo_TaskCalendars_dbo_TaskCalendarEvents_TaskCalendarEventID]
    FOREIGN KEY ([TaskCalendarEventID])
    REFERENCES [dbo].[TaskCalendarEvents]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TaskCalendars_dbo_TaskCalendarEvents_TaskCalendarEventID'
CREATE INDEX [IX_FK_dbo_TaskCalendars_dbo_TaskCalendarEvents_TaskCalendarEventID]
ON [dbo].[TaskCalendars]
    ([TaskCalendarEventID]);
GO

-- Creating foreign key on [TaskCategoryID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_dbo_Tasks_dbo_TaskCategories_TaskCategoryID]
    FOREIGN KEY ([TaskCategoryID])
    REFERENCES [dbo].[TaskCategories]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Tasks_dbo_TaskCategories_TaskCategoryID'
CREATE INDEX [IX_FK_dbo_Tasks_dbo_TaskCategories_TaskCategoryID]
ON [dbo].[Tasks]
    ([TaskCategoryID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskCCs'
ALTER TABLE [dbo].[TaskCCs]
ADD CONSTRAINT [FK_TaskCCs_Tasks]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskCCs_Tasks'
CREATE INDEX [IX_FK_TaskCCs_Tasks]
ON [dbo].[TaskCCs]
    ([TaskID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskChecks'
ALTER TABLE [dbo].[TaskChecks]
ADD CONSTRAINT [FK_dbo_TaskChecks_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TaskChecks_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_TaskChecks_dbo_Tasks_TaskID]
ON [dbo].[TaskChecks]
    ([TaskID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskComments'
ALTER TABLE [dbo].[TaskComments]
ADD CONSTRAINT [FK_TaskComment_Tasks]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskComment_Tasks'
CREATE INDEX [IX_FK_TaskComment_Tasks]
ON [dbo].[TaskComments]
    ([TaskID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskHistories'
ALTER TABLE [dbo].[TaskHistories]
ADD CONSTRAINT [FK_dbo_TaskHistories_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TaskHistories_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_TaskHistories_dbo_Tasks_TaskID]
ON [dbo].[TaskHistories]
    ([TaskID]);
GO

-- Creating foreign key on [TaskLevelID] in table 'Tasks'
ALTER TABLE [dbo].[Tasks]
ADD CONSTRAINT [FK_dbo_Tasks_dbo_TaskLevels_TaskLevelID]
    FOREIGN KEY ([TaskLevelID])
    REFERENCES [dbo].[TaskLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_Tasks_dbo_TaskLevels_TaskLevelID'
CREATE INDEX [IX_FK_dbo_Tasks_dbo_TaskLevels_TaskLevelID]
ON [dbo].[Tasks]
    ([TaskLevelID]);
GO

-- Creating foreign key on [TaskLevelID] in table 'TaskRequests'
ALTER TABLE [dbo].[TaskRequests]
ADD CONSTRAINT [FK_TaskRequests_TaskLevels]
    FOREIGN KEY ([TaskLevelID])
    REFERENCES [dbo].[TaskLevels]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskRequests_TaskLevels'
CREATE INDEX [IX_FK_TaskRequests_TaskLevels]
ON [dbo].[TaskRequests]
    ([TaskLevelID]);
GO

-- Creating foreign key on [TaskPerformID] in table 'TaskPerformAttachmentFiles'
ALTER TABLE [dbo].[TaskPerformAttachmentFiles]
ADD CONSTRAINT [FK_TaskPerformAttachmentFiles_TaskPerforms]
    FOREIGN KEY ([TaskPerformID])
    REFERENCES [dbo].[TaskPerforms]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_TaskPerformAttachmentFiles_TaskPerforms'
CREATE INDEX [IX_FK_TaskPerformAttachmentFiles_TaskPerforms]
ON [dbo].[TaskPerformAttachmentFiles]
    ([TaskPerformID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskPerforms'
ALTER TABLE [dbo].[TaskPerforms]
ADD CONSTRAINT [FK_dbo_TaskPerforms_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TaskPerforms_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_TaskPerforms_dbo_Tasks_TaskID]
ON [dbo].[TaskPerforms]
    ([TaskID]);
GO

-- Creating foreign key on [TaskID] in table 'TaskPersonals'
ALTER TABLE [dbo].[TaskPersonals]
ADD CONSTRAINT [FK_dbo_TaskPersonals_dbo_Tasks_TaskID]
    FOREIGN KEY ([TaskID])
    REFERENCES [dbo].[Tasks]
        ([ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_TaskPersonals_dbo_Tasks_TaskID'
CREATE INDEX [IX_FK_dbo_TaskPersonals_dbo_Tasks_TaskID]
ON [dbo].[TaskPersonals]
    ([TaskID]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------