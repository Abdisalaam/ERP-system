
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 04/24/2024 21:12:47
-- Generated from EDMX file: E:\Development\Sutumatic Dark\23-04-2024-Byanaty-Restaurant\Accountingsystem\Accounting.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [FTERP502];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_ProductProductBatches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductBatches] DROP CONSTRAINT [FK_ProductProductBatches];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductSlicesProduct]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SlicesProducts] DROP CONSTRAINT [FK_ProductSlicesProduct];
GO
IF OBJECT_ID(N'[dbo].[FK_BranchBranchDepartment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[BranchDepartments] DROP CONSTRAINT [FK_BranchBranchDepartment];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyBranch]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Branches] DROP CONSTRAINT [FK_CompanyBranch];
GO
IF OBJECT_ID(N'[dbo].[FK_PartnerPartnerFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PartnerFileDetails] DROP CONSTRAINT [FK_PartnerPartnerFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCompanyDepartments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyDepartments] DROP CONSTRAINT [FK_CompanyCompanyDepartments];
GO
IF OBJECT_ID(N'[dbo].[FK_CountryProvince]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Provinces] DROP CONSTRAINT [FK_CountryProvince];
GO
IF OBJECT_ID(N'[dbo].[FK_ProvinceCities]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Cities] DROP CONSTRAINT [FK_ProvinceCities];
GO
IF OBJECT_ID(N'[dbo].[FK_ExpenseExpenseFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExpenseFileDetails] DROP CONSTRAINT [FK_ExpenseExpenseFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_GLMasterGLDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GLDetails] DROP CONSTRAINT [FK_GLMasterGLDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ChartOfAccountingChartOfAccounting]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ChartOfAccountings] DROP CONSTRAINT [FK_ChartOfAccountingChartOfAccounting];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerCustomerFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CustomerFileDetails] DROP CONSTRAINT [FK_CustomerCustomerFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerContentPersonDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContentPersonDetails] DROP CONSTRAINT [FK_CustomerContentPersonDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeEmployeeFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeFileDetails] DROP CONSTRAINT [FK_EmployeeEmployeeFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalary_Components]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Salary_Components] DROP CONSTRAINT [FK_EmployeeSalary_Components];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeVacations]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vacations] DROP CONSTRAINT [FK_EmployeeVacations];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeEmp_Evaluation]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Emp_Evaluation] DROP CONSTRAINT [FK_EmployeeEmp_Evaluation];
GO
IF OBJECT_ID(N'[dbo].[FK_SupplierSupplierFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SupplierFileDetails] DROP CONSTRAINT [FK_SupplierSupplierFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceMasterInvoiceDetails1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseDetails] DROP CONSTRAINT [FK_InvoiceMasterInvoiceDetails1];
GO
IF OBJECT_ID(N'[dbo].[FK_FixedAssetFixedAssetImage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FixedAssetImages] DROP CONSTRAINT [FK_FixedAssetFixedAssetImage];
GO
IF OBJECT_ID(N'[dbo].[FK_RecalPurchInvoiceMasterRecalPurchInvoiceDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecalPurchInvoiceDetails] DROP CONSTRAINT [FK_RecalPurchInvoiceMasterRecalPurchInvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_InventoryTransfer_HdInventoryTransfer_Detail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryTransfer_Detail] DROP CONSTRAINT [FK_InventoryTransfer_HdInventoryTransfer_Detail];
GO
IF OBJECT_ID(N'[dbo].[FK_ReceiptMatching]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Matchings] DROP CONSTRAINT [FK_ReceiptMatching];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentMatchingPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MatchingPayments] DROP CONSTRAINT [FK_PaymentMatchingPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_OfferPriceMasterOfferPriceDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfferPriceDetails] DROP CONSTRAINT [FK_OfferPriceMasterOfferPriceDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CostDistributionCostDistributionDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CostDistributionDetails] DROP CONSTRAINT [FK_CostDistributionCostDistributionDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ReceiptReceiptFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceiptFileDetails] DROP CONSTRAINT [FK_ReceiptReceiptFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentPaymentFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentFileDetails] DROP CONSTRAINT [FK_PaymentPaymentFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_FixedAssetFixedAssetFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FixedAssetFileDetails] DROP CONSTRAINT [FK_FixedAssetFixedAssetFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectProjectFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectFileDetails] DROP CONSTRAINT [FK_ProjectProjectFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceMasterInvoiceDetails3]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceDetailsTemps] DROP CONSTRAINT [FK_InvoiceMasterInvoiceDetails3];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceMasterSalesFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesFileDetails] DROP CONSTRAINT [FK_InvoiceMasterSalesFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_PurInvoiceMasterTempPurchaseDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseDetailsTemps] DROP CONSTRAINT [FK_PurInvoiceMasterTempPurchaseDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_PurInvoiceMasterPurchaseFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseFileDetails] DROP CONSTRAINT [FK_PurInvoiceMasterPurchaseFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_RecallPurchInvoicMasterTempRecallPurchInvDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecallPurchInvDetailsTemps] DROP CONSTRAINT [FK_RecallPurchInvoicMasterTempRecallPurchInvDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_RecalPurchInvoiceMasterRecallPurchFileDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RecallPurchFileDetails] DROP CONSTRAINT [FK_RecalPurchInvoiceMasterRecallPurchFileDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseSalesInvoiceMasterReversSalesFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReversSalesFileDetails] DROP CONSTRAINT [FK_ReverseSalesInvoiceMasterReversSalesFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseSalesInvoiceMasterTempReveInvoiceDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReveInvoiceDetailsTemps] DROP CONSTRAINT [FK_ReverseSalesInvoiceMasterTempReveInvoiceDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_InventoryTransfer_HdTempInventoryTransfer_DetailTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InventoryTransfer_DetailTemp] DROP CONSTRAINT [FK_InventoryTransfer_HdTempInventoryTransfer_DetailTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesReceiptMasterTempSalesReceiptDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesReceiptDetailsTemps] DROP CONSTRAINT [FK_SalesReceiptMasterTempSalesReceiptDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesReceiptMasterSalesReceiptFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesReceiptFiles] DROP CONSTRAINT [FK_SalesReceiptMasterSalesReceiptFile];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesReceiptMasterSalesReceiptDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesReceiptDetails] DROP CONSTRAINT [FK_SalesReceiptMasterSalesReceiptDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrderMasterPurchaseOrderDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseOrderDetails] DROP CONSTRAINT [FK_PurchaseOrderMasterPurchaseOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_PurchaseOrderMasterPurchaseFileOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PurchaseFileOrders] DROP CONSTRAINT [FK_PurchaseOrderMasterPurchaseFileOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToProjectMasterTransferToprojectFile1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferToprojectFile1] DROP CONSTRAINT [FK_TransferToProjectMasterTransferToprojectFile1];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToProjectTempTransferToProjectDetailTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferToProjectDetailTemps] DROP CONSTRAINT [FK_TransferToProjectTempTransferToProjectDetailTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeTempSalary_ComponentsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Salary_ComponentsTemp] DROP CONSTRAINT [FK_EmployeTempSalary_ComponentsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectInvoiceMasterProjectInvoiceDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectInvoiceDetails] DROP CONSTRAINT [FK_ProjectInvoiceMasterProjectInvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_Maintenance_Invoice_MasterMaintenance_Invoice_Detail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Maintenance_Invoice_Detail] DROP CONSTRAINT [FK_Maintenance_Invoice_MasterMaintenance_Invoice_Detail];
GO
IF OBJECT_ID(N'[dbo].[FK_Maintenance_Invoice_Master_TempMaintenance_Invoice_Detail_Temp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Maintenance_Invoice_Detail_Temp] DROP CONSTRAINT [FK_Maintenance_Invoice_Master_TempMaintenance_Invoice_Detail_Temp];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesReceiptMasterTempSalesReceiptDetailsTemp2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseSalesReceiptDetailsTemps] DROP CONSTRAINT [FK_SalesReceiptMasterTempSalesReceiptDetailsTemp2];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseSalesReceiptMasterReverseSalesReceiptDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseSalesReceiptDetails] DROP CONSTRAINT [FK_ReverseSalesReceiptMasterReverseSalesReceiptDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseSalesReceiptMasterReverseSalesReceiptFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseSalesReceiptFiles] DROP CONSTRAINT [FK_ReverseSalesReceiptMasterReverseSalesReceiptFile];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductTemSlicesTem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SlicesTems] DROP CONSTRAINT [FK_ProductTemSlicesTem];
GO
IF OBJECT_ID(N'[dbo].[FK_SubLedgerSubLedgerFileDetail1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubLedgerFileDetail1] DROP CONSTRAINT [FK_SubLedgerSubLedgerFileDetail1];
GO
IF OBJECT_ID(N'[dbo].[FK_BranchDepartmentDepartmentBussinesUnit]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DepartmentBussinesUnits] DROP CONSTRAINT [FK_BranchDepartmentDepartmentBussinesUnit];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToMaintenanceTempTransferToMaintenanceDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferToMaintenanceDetailsTemps] DROP CONSTRAINT [FK_TransferToMaintenanceTempTransferToMaintenanceDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToMaintenanceTransferToMaintenanceDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferToMaintenanceDetails] DROP CONSTRAINT [FK_TransferToMaintenanceTransferToMaintenanceDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCompanyFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyFileDetails] DROP CONSTRAINT [FK_CompanyCompanyFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CompanyCompanyPhotoDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CompanyPhotoDetails] DROP CONSTRAINT [FK_CompanyCompanyPhotoDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CatchReceiptCatchReceiptFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[CatchReceiptFileDetails] DROP CONSTRAINT [FK_CatchReceiptCatchReceiptFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_CustomerTempContentPersonTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ContentPersonDetailsTemps] DROP CONSTRAINT [FK_CustomerTempContentPersonTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_SpentReciptSpentReciptFileDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpentReciptFileDetails] DROP CONSTRAINT [FK_SpentReciptSpentReciptFileDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_Sales_SettingsSales_Setting_Slice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sales_Setting_Slice] DROP CONSTRAINT [FK_Sales_SettingsSales_Setting_Slice];
GO
IF OBJECT_ID(N'[dbo].[FK_SubLedgerSubLedger]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubLedgers] DROP CONSTRAINT [FK_SubLedgerSubLedger];
GO
IF OBJECT_ID(N'[dbo].[FK_ReveInvoiceDetailsReverseSalesInvoiceMaster]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReveInvoiceDetails] DROP CONSTRAINT [FK_ReveInvoiceDetailsReverseSalesInvoiceMaster];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceDetailsInvoiceMaster]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceDetails] DROP CONSTRAINT [FK_InvoiceDetailsInvoiceMaster];
GO
IF OBJECT_ID(N'[dbo].[FK_TransferToProjectDetailTransferToProjectMaster]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TransferToProjectDetails] DROP CONSTRAINT [FK_TransferToProjectDetailTransferToProjectMaster];
GO
IF OBJECT_ID(N'[dbo].[FK_SlicesProductSlices]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SlicesProducts] DROP CONSTRAINT [FK_SlicesProductSlices];
GO
IF OBJECT_ID(N'[dbo].[FK_ChartOfAccountingGLDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GLDetails] DROP CONSTRAINT [FK_ChartOfAccountingGLDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectInvoiceMasterTempProjectInvoiceDetailTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectInvoiceDetailTemps] DROP CONSTRAINT [FK_ProjectInvoiceMasterTempProjectInvoiceDetailTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalaryMonthEmpSal_Detail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpSal_Detail] DROP CONSTRAINT [FK_EmployeeSalaryMonthEmpSal_Detail];
GO
IF OBJECT_ID(N'[dbo].[FK_FixedAssetInformationFixedAssetFileDetailInfo]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FixedAssetFileDetailInfoes] DROP CONSTRAINT [FK_FixedAssetInformationFixedAssetFileDetailInfo];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreProductBatches]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductBatches] DROP CONSTRAINT [FK_StoreProductBatches];
GO
IF OBJECT_ID(N'[dbo].[FK_SettlementOfStoresSettlementOfStoresDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SettlementOfStoresDetails] DROP CONSTRAINT [FK_SettlementOfStoresSettlementOfStoresDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_SettlementOfStoresTempSettlementOfStoresDetailsTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SettlementOfStoresDetailsTemps] DROP CONSTRAINT [FK_SettlementOfStoresTempSettlementOfStoresDetailsTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_PointOfSalePOSDetailPayment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[POSDetailPayments] DROP CONSTRAINT [FK_PointOfSalePOSDetailPayment];
GO
IF OBJECT_ID(N'[dbo].[FK_PointOfSaleMasterPOSInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MasterPOSInvoices] DROP CONSTRAINT [FK_PointOfSaleMasterPOSInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceDetailPOSInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetailPOSInvoices] DROP CONSTRAINT [FK_MasterPOSInvoiceDetailPOSInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoicePosPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PosPaymentMethods] DROP CONSTRAINT [FK_MasterPOSInvoicePosPaymentMethod];
GO
IF OBJECT_ID(N'[dbo].[FK_ReversePOSInvoiceReversePOSDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReversePOSDetails] DROP CONSTRAINT [FK_ReversePOSInvoiceReversePOSDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceReversePOSInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReversePOSInvoices] DROP CONSTRAINT [FK_MasterPOSInvoiceReversePOSInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_PointOfSaleTempPOSDetailPaymentTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[POSDetailPaymentTemps] DROP CONSTRAINT [FK_PointOfSaleTempPOSDetailPaymentTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceTempDetailPOSInvoiceTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetailPOSInvoiceTemps] DROP CONSTRAINT [FK_MasterPOSInvoiceTempDetailPOSInvoiceTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceTempPosPaymentMethodTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PosPaymentMethodTemps] DROP CONSTRAINT [FK_MasterPOSInvoiceTempPosPaymentMethodTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ReversePOSInvoiceTempReversePOSDetailTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReversePOSDetailTemps] DROP CONSTRAINT [FK_ReversePOSInvoiceTempReversePOSDetailTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectProject_Details]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectDetails] DROP CONSTRAINT [FK_ProjectProject_Details];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectTempProjectDetailTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectDetailTemps] DROP CONSTRAINT [FK_ProjectTempProjectDetailTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectPaymentProjectPaymentFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectPaymentFiles] DROP CONSTRAINT [FK_ProjectPaymentProjectPaymentFile];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectReciptProjectReceiptFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectReceiptFiles] DROP CONSTRAINT [FK_ProjectReciptProjectReceiptFile];
GO
IF OBJECT_ID(N'[dbo].[FK_SubscriptionInvoiceSubscriptionInvoiceFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubscriptionInvoiceFiles] DROP CONSTRAINT [FK_SubscriptionInvoiceSubscriptionInvoiceFile];
GO
IF OBJECT_ID(N'[dbo].[FK_GLMasterGLMasterFile]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[GLMasterFiles] DROP CONSTRAINT [FK_GLMasterGLMasterFile];
GO
IF OBJECT_ID(N'[dbo].[FK_ClientPackage]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Clients] DROP CONSTRAINT [FK_ClientPackage];
GO
IF OBJECT_ID(N'[dbo].[FK_VacationsVacation_Types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Vacations] DROP CONSTRAINT [FK_VacationsVacation_Types];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpVacationVacation_Types]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpVacations] DROP CONSTRAINT [FK_EmpVacationVacation_Types];
GO
IF OBJECT_ID(N'[dbo].[FK_EmpVacationEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpVacations] DROP CONSTRAINT [FK_EmpVacationEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeTempEmpVacationTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmpVacationTemps] DROP CONSTRAINT [FK_EmployeTempEmpVacationTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_InvoiceMasterInvoiceTax]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[InvoiceTaxes] DROP CONSTRAINT [FK_InvoiceMasterInvoiceTax];
GO
IF OBJECT_ID(N'[dbo].[FK_SalesReceiptMasterSalesReceiptTax]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SalesReceiptTaxes] DROP CONSTRAINT [FK_SalesReceiptMasterSalesReceiptTax];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceTaxPOSInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaxPOSInvoices] DROP CONSTRAINT [FK_MasterPOSInvoiceTaxPOSInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_OfferPriceMasterOfferTax]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OfferTaxes] DROP CONSTRAINT [FK_OfferPriceMasterOfferTax];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductFileDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductFileDetails] DROP CONSTRAINT [FK_ProductProductFileDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductTemProductCompoundTemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductCompoundTemps] DROP CONSTRAINT [FK_ProductTemProductCompoundTemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProductProductCompound]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProductCompounds] DROP CONSTRAINT [FK_ProductProductCompound];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceOrderOperationalDetails] DROP CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceOrderOperationalStatus] DROP CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceOrderOperationalDetailTemps] DROP CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail1];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail2]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceOrderDetails] DROP CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail2];
GO
IF OBJECT_ID(N'[dbo].[FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[MaintenanceOrderStatus] DROP CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus1];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderOrderDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[OrderDetails] DROP CONSTRAINT [FK_OrderOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_TempOrderTempOrderDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TempOrderDetails] DROP CONSTRAINT [FK_TempOrderTempOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_HoldedOrderHoldedOrderDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[HoldedOrderDetails] DROP CONSTRAINT [FK_HoldedOrderHoldedOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_OrderReverseOrder]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseOrders] DROP CONSTRAINT [FK_OrderReverseOrder];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseOrderReverseOrderDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseOrderDetails] DROP CONSTRAINT [FK_ReverseOrderReverseOrderDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterQRInvoiceQRPaymentMethod]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[QRPaymentMethods] DROP CONSTRAINT [FK_MasterQRInvoiceQRPaymentMethod];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterQRInvoiceDetailQRInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[DetailQRInvoices] DROP CONSTRAINT [FK_MasterQRInvoiceDetailQRInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterQRInvoiceTaxQRInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[TaxQRInvoices] DROP CONSTRAINT [FK_MasterQRInvoiceTaxQRInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_StoreStoreEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[StoreEmployees] DROP CONSTRAINT [FK_StoreStoreEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_ExtrnalSalesEmployeeEmployeeSalesDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[EmployeeSalesDetails] DROP CONSTRAINT [FK_ExtrnalSalesEmployeeEmployeeSalesDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_ExtrnalSalesEmployeeExternalEmployeePayments]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ExternalEmployeePayments] DROP CONSTRAINT [FK_ExtrnalSalesEmployeeExternalEmployeePayments];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemRoleRoleFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunctions] DROP CONSTRAINT [FK_SystemRoleRoleFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_RoleFunctionSystemFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RoleFunctions] DROP CONSTRAINT [FK_RoleFunctionSystemFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_SystemFunctionSystemFunction]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SystemFunctions] DROP CONSTRAINT [FK_SystemFunctionSystemFunction];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSystemRole]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Employees] DROP CONSTRAINT [FK_EmployeeSystemRole];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectProjectSub]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectClauses] DROP CONSTRAINT [FK_ProjectProjectSub];
GO
IF OBJECT_ID(N'[dbo].[FK_SubLedgerSubLedger1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubLedgertemps] DROP CONSTRAINT [FK_SubLedgerSubLedger1];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectTempProjectClausetemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ProjectClausetemps] DROP CONSTRAINT [FK_ProjectTempProjectClausetemp];
GO
IF OBJECT_ID(N'[dbo].[FK_ProjectTempSubLedgertemp]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SubLedgertemps] DROP CONSTRAINT [FK_ProjectTempSubLedgertemp];
GO
IF OBJECT_ID(N'[dbo].[FK_SpentReciptOrderSpentReciptOrderType]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpentReciptOrders] DROP CONSTRAINT [FK_SpentReciptOrderSpentReciptOrderType];
GO
IF OBJECT_ID(N'[dbo].[FK_SpentReciptOrderSpentRecipt]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SpentRecipts] DROP CONSTRAINT [FK_SpentReciptOrderSpentRecipt];
GO
IF OBJECT_ID(N'[dbo].[FK_FixedAssetFixedAssetDetails]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FixedAssetDetails] DROP CONSTRAINT [FK_FixedAssetFixedAssetDetails];
GO
IF OBJECT_ID(N'[dbo].[FK_FixedAssetDetailsGLMaster]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[FixedAssetDetails] DROP CONSTRAINT [FK_FixedAssetDetailsGLMaster];
GO
IF OBJECT_ID(N'[dbo].[FK_ReceiptReceiptFileDetail1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReceiptPOSFileDetails] DROP CONSTRAINT [FK_ReceiptReceiptFileDetail1];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoicePOSReceipt]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[POSReceipts] DROP CONSTRAINT [FK_MasterPOSInvoicePOSReceipt];
GO
IF OBJECT_ID(N'[dbo].[FK_ReversePOSInvoiceReversePOSDetail1]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseQRDetails] DROP CONSTRAINT [FK_ReversePOSInvoiceReversePOSDetail1];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseQRInvoiceMasterQRInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseQRInvoices] DROP CONSTRAINT [FK_ReverseQRInvoiceMasterQRInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_ReverseQRDetailDetailQRInvoice]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[ReverseQRDetails] DROP CONSTRAINT [FK_ReverseQRDetailDetailQRInvoice];
GO
IF OBJECT_ID(N'[dbo].[FK_PaymentMethodPaymentMethodEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[PaymentMethodEmployees] DROP CONSTRAINT [FK_PaymentMethodPaymentMethodEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_ShiftShiftCoding]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shifts] DROP CONSTRAINT [FK_ShiftShiftCoding];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeShift]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Shifts] DROP CONSTRAINT [FK_EmployeeShift];
GO
IF OBJECT_ID(N'[dbo].[FK_ShiftCodingVacationWeekly]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VacationWeeklies] DROP CONSTRAINT [FK_ShiftCodingVacationWeekly];
GO
IF OBJECT_ID(N'[dbo].[FK_VacationWeeklyVacationWeeklyCoding]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[VacationWeeklies] DROP CONSTRAINT [FK_VacationWeeklyVacationWeeklyCoding];
GO
IF OBJECT_ID(N'[dbo].[FK_AttendingAndLeavingEmployee]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AttendingAndLeavings] DROP CONSTRAINT [FK_AttendingAndLeavingEmployee];
GO
IF OBJECT_ID(N'[dbo].[FK_AttendingAndLeavingShiftCoding]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[AttendingAndLeavings] DROP CONSTRAINT [FK_AttendingAndLeavingShiftCoding];
GO
IF OBJECT_ID(N'[dbo].[FK_RewardsAndPenaltiesCodingRewardsAndPenaltiesCodingCategory]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RewardsAndPenaltiesCodings] DROP CONSTRAINT [FK_RewardsAndPenaltiesCodingRewardsAndPenaltiesCodingCategory];
GO
IF OBJECT_ID(N'[dbo].[FK_RewardsAndPenaltiesAttendingAndLeaving]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RewardsAndPenalties] DROP CONSTRAINT [FK_RewardsAndPenaltiesAttendingAndLeaving];
GO
IF OBJECT_ID(N'[dbo].[FK_RewardsAndPenaltiesCodingRewardsAndPenalties]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RewardsAndPenalties] DROP CONSTRAINT [FK_RewardsAndPenaltiesCodingRewardsAndPenalties];
GO
IF OBJECT_ID(N'[dbo].[FK_TypeOfDetailSalaryRewardsAndPenaltiesCoding]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RewardsAndPenaltiesCodings] DROP CONSTRAINT [FK_TypeOfDetailSalaryRewardsAndPenaltiesCoding];
GO
IF OBJECT_ID(N'[dbo].[FK_EmployeeSalaryMonthRewardsAndPenalties]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[RewardsAndPenalties] DROP CONSTRAINT [FK_EmployeeSalaryMonthRewardsAndPenalties];
GO
IF OBJECT_ID(N'[dbo].[FK_QuestionAnswer]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Answers] DROP CONSTRAINT [FK_QuestionAnswer];
GO
IF OBJECT_ID(N'[dbo].[FK_SewingDetailQuestion]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SewingDetails] DROP CONSTRAINT [FK_SewingDetailQuestion];
GO
IF OBJECT_ID(N'[dbo].[FK_SewingDetailSewing]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SewingDetails] DROP CONSTRAINT [FK_SewingDetailSewing];
GO
IF OBJECT_ID(N'[dbo].[FK_AnswerSewingDetail]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[SewingDetails] DROP CONSTRAINT [FK_AnswerSewingDetail];
GO
IF OBJECT_ID(N'[dbo].[FK_MasterPOSInvoiceSewing]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sewings] DROP CONSTRAINT [FK_MasterPOSInvoiceSewing];
GO
IF OBJECT_ID(N'[dbo].[FK_SewingSewingStatus]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Sewings] DROP CONSTRAINT [FK_SewingSewingStatus];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Products]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Products];
GO
IF OBJECT_ID(N'[dbo].[Stores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Stores];
GO
IF OBJECT_ID(N'[dbo].[MeasuringUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MeasuringUnits];
GO
IF OBJECT_ID(N'[dbo].[Categories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Categories];
GO
IF OBJECT_ID(N'[dbo].[Branches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Branches];
GO
IF OBJECT_ID(N'[dbo].[Companies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Companies];
GO
IF OBJECT_ID(N'[dbo].[BranchDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BranchDepartments];
GO
IF OBJECT_ID(N'[dbo].[Partners]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Partners];
GO
IF OBJECT_ID(N'[dbo].[Currencies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Currencies];
GO
IF OBJECT_ID(N'[dbo].[CompanyDepartments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyDepartments];
GO
IF OBJECT_ID(N'[dbo].[CompanyConfigrations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyConfigrations];
GO
IF OBJECT_ID(N'[dbo].[Countries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Countries];
GO
IF OBJECT_ID(N'[dbo].[Provinces]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Provinces];
GO
IF OBJECT_ID(N'[dbo].[Cities]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Cities];
GO
IF OBJECT_ID(N'[dbo].[BussinesUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[BussinesUnits];
GO
IF OBJECT_ID(N'[dbo].[Expenses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Expenses];
GO
IF OBJECT_ID(N'[dbo].[GLMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GLMasters];
GO
IF OBJECT_ID(N'[dbo].[GLDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GLDetails];
GO
IF OBJECT_ID(N'[dbo].[ChartOfAccountings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChartOfAccountings];
GO
IF OBJECT_ID(N'[dbo].[GL_Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GL_Type];
GO
IF OBJECT_ID(N'[dbo].[Customers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Customers];
GO
IF OBJECT_ID(N'[dbo].[InvoiceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceMasters];
GO
IF OBJECT_ID(N'[dbo].[InvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[Employees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employees];
GO
IF OBJECT_ID(N'[dbo].[Treasuries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Treasuries];
GO
IF OBJECT_ID(N'[dbo].[EmployeeFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeFileDetails];
GO
IF OBJECT_ID(N'[dbo].[Suppliers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Suppliers];
GO
IF OBJECT_ID(N'[dbo].[PurInvoiceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurInvoiceMasters];
GO
IF OBJECT_ID(N'[dbo].[PurchaseDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseDetails];
GO
IF OBJECT_ID(N'[dbo].[AnotherAssets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AnotherAssets];
GO
IF OBJECT_ID(N'[dbo].[CurrentAssetTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CurrentAssetTypes];
GO
IF OBJECT_ID(N'[dbo].[FixedAssetImages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssetImages];
GO
IF OBJECT_ID(N'[dbo].[DepreciationTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepreciationTypes];
GO
IF OBJECT_ID(N'[dbo].[RecalPurchInvoiceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecalPurchInvoiceMasters];
GO
IF OBJECT_ID(N'[dbo].[RecalPurchInvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecalPurchInvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[InventoryTransfer_Hd]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryTransfer_Hd];
GO
IF OBJECT_ID(N'[dbo].[InventoryTransfer_Detail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryTransfer_Detail];
GO
IF OBJECT_ID(N'[dbo].[Matchings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Matchings];
GO
IF OBJECT_ID(N'[dbo].[Receipts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Receipts];
GO
IF OBJECT_ID(N'[dbo].[MatchingPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MatchingPayments];
GO
IF OBJECT_ID(N'[dbo].[Payments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Payments];
GO
IF OBJECT_ID(N'[dbo].[ReveInvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReveInvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesInvoiceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesInvoiceMasters];
GO
IF OBJECT_ID(N'[dbo].[Banks]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Banks];
GO
IF OBJECT_ID(N'[dbo].[Taxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Taxes];
GO
IF OBJECT_ID(N'[dbo].[TransactionBatches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransactionBatches];
GO
IF OBJECT_ID(N'[dbo].[Projects]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Projects];
GO
IF OBJECT_ID(N'[dbo].[OfferPriceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfferPriceDetails];
GO
IF OBJECT_ID(N'[dbo].[OfferPriceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfferPriceMasters];
GO
IF OBJECT_ID(N'[dbo].[DeprTaskStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DeprTaskStatus];
GO
IF OBJECT_ID(N'[dbo].[ProductBatches]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductBatches];
GO
IF OBJECT_ID(N'[dbo].[CostDistributions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CostDistributions];
GO
IF OBJECT_ID(N'[dbo].[CostDistributionDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CostDistributionDetails];
GO
IF OBJECT_ID(N'[dbo].[ReceiptFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceiptFileDetails];
GO
IF OBJECT_ID(N'[dbo].[PaymentFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentFileDetails];
GO
IF OBJECT_ID(N'[dbo].[PartnerFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PartnerFileDetails];
GO
IF OBJECT_ID(N'[dbo].[ExpenseFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenseFileDetails];
GO
IF OBJECT_ID(N'[dbo].[FixedAssetFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssetFileDetails];
GO
IF OBJECT_ID(N'[dbo].[SupplierFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SupplierFileDetails];
GO
IF OBJECT_ID(N'[dbo].[CustomerFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerFileDetails];
GO
IF OBJECT_ID(N'[dbo].[ProjectFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectFileDetails];
GO
IF OBJECT_ID(N'[dbo].[InvoiceDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[InvoiceMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[SalesFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesFileDetails];
GO
IF OBJECT_ID(N'[dbo].[PurchaseDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[PurInvoiceMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurInvoiceMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[PurchaseFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseFileDetails];
GO
IF OBJECT_ID(N'[dbo].[RecallPurchInvDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecallPurchInvDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[RecallPurchInvoicMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecallPurchInvoicMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[RecallPurchFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RecallPurchFileDetails];
GO
IF OBJECT_ID(N'[dbo].[ReversSalesFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReversSalesFileDetails];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesInvoiceMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesInvoiceMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[ReveInvoiceDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReveInvoiceDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[InventoryTransfer_HdTemp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryTransfer_HdTemp];
GO
IF OBJECT_ID(N'[dbo].[InventoryTransfer_DetailTemp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InventoryTransfer_DetailTemp];
GO
IF OBJECT_ID(N'[dbo].[Contacts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Contacts];
GO
IF OBJECT_ID(N'[dbo].[PaymentMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentMethods];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptFiles];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptMasters];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptDetails];
GO
IF OBJECT_ID(N'[dbo].[FixedAssets]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssets];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[PurchaseFileOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseFileOrders];
GO
IF OBJECT_ID(N'[dbo].[PurchaseOrderMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PurchaseOrderMasters];
GO
IF OBJECT_ID(N'[dbo].[EmployeeTeamTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeTeamTemps];
GO
IF OBJECT_ID(N'[dbo].[EmployeeTeams]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeTeams];
GO
IF OBJECT_ID(N'[dbo].[Subscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscriptions];
GO
IF OBJECT_ID(N'[dbo].[Subscription_Type]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Subscription_Type];
GO
IF OBJECT_ID(N'[dbo].[ProductBatchesTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductBatchesTemps];
GO
IF OBJECT_ID(N'[dbo].[TransferToprojectFile1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToprojectFile1];
GO
IF OBJECT_ID(N'[dbo].[TransferToProjectMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToProjectMasters];
GO
IF OBJECT_ID(N'[dbo].[TransferToProjectDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToProjectDetails];
GO
IF OBJECT_ID(N'[dbo].[TransferToProjectTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToProjectTemps];
GO
IF OBJECT_ID(N'[dbo].[TransferToProjectDetailTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToProjectDetailTemps];
GO
IF OBJECT_ID(N'[dbo].[TransfareFromPorjectToStoretemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransfareFromPorjectToStoretemps];
GO
IF OBJECT_ID(N'[dbo].[Vacations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vacations];
GO
IF OBJECT_ID(N'[dbo].[Vacation_Types]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Vacation_Types];
GO
IF OBJECT_ID(N'[dbo].[Salary_Components]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Salary_Components];
GO
IF OBJECT_ID(N'[dbo].[Emp_Evaluation]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Emp_Evaluation];
GO
IF OBJECT_ID(N'[dbo].[EmployeTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeTemps];
GO
IF OBJECT_ID(N'[dbo].[Salary_ComponentsTemp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Salary_ComponentsTemp];
GO
IF OBJECT_ID(N'[dbo].[ProjectPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectPayments];
GO
IF OBJECT_ID(N'[dbo].[ProjectInvoiceMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectInvoiceMasters];
GO
IF OBJECT_ID(N'[dbo].[ProjectInvoiceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectInvoiceDetails];
GO
IF OBJECT_ID(N'[dbo].[ProjectInvoiceDetailTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectInvoiceDetailTemps];
GO
IF OBJECT_ID(N'[dbo].[ProjectInvoiceMasterTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectInvoiceMasterTemps];
GO
IF OBJECT_ID(N'[dbo].[SubscriptionInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubscriptionInvoices];
GO
IF OBJECT_ID(N'[dbo].[Maintenance_Invoice_Detail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maintenance_Invoice_Detail];
GO
IF OBJECT_ID(N'[dbo].[Maintenance_Invoice_Master]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maintenance_Invoice_Master];
GO
IF OBJECT_ID(N'[dbo].[Maintenance_Invoice_Master_Temp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maintenance_Invoice_Master_Temp];
GO
IF OBJECT_ID(N'[dbo].[Maintenance_Invoice_Detail_Temp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Maintenance_Invoice_Detail_Temp];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesReceiptMasterTemp1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesReceiptMasterTemp1];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesReceiptDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesReceiptDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesReceiptDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesReceiptDetails];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesReceiptFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesReceiptFiles];
GO
IF OBJECT_ID(N'[dbo].[ReverseSalesReceiptMasters]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseSalesReceiptMasters];
GO
IF OBJECT_ID(N'[dbo].[Accounts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Accounts];
GO
IF OBJECT_ID(N'[dbo].[SlicesProducts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SlicesProducts];
GO
IF OBJECT_ID(N'[dbo].[SlicesTems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SlicesTems];
GO
IF OBJECT_ID(N'[dbo].[ProductTems]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductTems];
GO
IF OBJECT_ID(N'[dbo].[Slices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Slices];
GO
IF OBJECT_ID(N'[dbo].[SubLedgerFileDetail1]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubLedgerFileDetail1];
GO
IF OBJECT_ID(N'[dbo].[SubLedgers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubLedgers];
GO
IF OBJECT_ID(N'[dbo].[DepartmentBussinesUnits]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DepartmentBussinesUnits];
GO
IF OBJECT_ID(N'[dbo].[TransferToMaintenanceDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToMaintenanceDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[TransferToMaintenanceTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToMaintenanceTemps];
GO
IF OBJECT_ID(N'[dbo].[TransferToMaintenanceDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToMaintenanceDetails];
GO
IF OBJECT_ID(N'[dbo].[TransferToMaintenances]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TransferToMaintenances];
GO
IF OBJECT_ID(N'[dbo].[AssetDepDates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AssetDepDates];
GO
IF OBJECT_ID(N'[dbo].[CompanyFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyFileDetails];
GO
IF OBJECT_ID(N'[dbo].[CompanyPhotoDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CompanyPhotoDetails];
GO
IF OBJECT_ID(N'[dbo].[CatchReceipts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CatchReceipts];
GO
IF OBJECT_ID(N'[dbo].[CatchReceiptFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CatchReceiptFileDetails];
GO
IF OBJECT_ID(N'[dbo].[ContentPersonDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentPersonDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[ContentPersonDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ContentPersonDetails];
GO
IF OBJECT_ID(N'[dbo].[CustomerTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[CustomerTemps];
GO
IF OBJECT_ID(N'[dbo].[SpentRecipts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpentRecipts];
GO
IF OBJECT_ID(N'[dbo].[SpentReciptFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpentReciptFileDetails];
GO
IF OBJECT_ID(N'[dbo].[Sales_Settings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sales_Settings];
GO
IF OBJECT_ID(N'[dbo].[Sales_Setting_Slice]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sales_Setting_Slice];
GO
IF OBJECT_ID(N'[dbo].[Sales_Setting_SliceTemp]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sales_Setting_SliceTemp];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalaryMonths]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalaryMonths];
GO
IF OBJECT_ID(N'[dbo].[Employee_Sales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Employee_Sales];
GO
IF OBJECT_ID(N'[dbo].[EmpSal_Detail]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpSal_Detail];
GO
IF OBJECT_ID(N'[dbo].[FixedAssetFileDetailInfoes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssetFileDetailInfoes];
GO
IF OBJECT_ID(N'[dbo].[FixedAssetInformations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssetInformations];
GO
IF OBJECT_ID(N'[dbo].[TypeOfDetailSalaries]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TypeOfDetailSalaries];
GO
IF OBJECT_ID(N'[dbo].[Clients]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Clients];
GO
IF OBJECT_ID(N'[dbo].[Charts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Charts];
GO
IF OBJECT_ID(N'[dbo].[SettlementOfStores]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SettlementOfStores];
GO
IF OBJECT_ID(N'[dbo].[SettlementOfStoresDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SettlementOfStoresDetails];
GO
IF OBJECT_ID(N'[dbo].[SettlementOfStoresTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SettlementOfStoresTemps];
GO
IF OBJECT_ID(N'[dbo].[SettlementOfStoresDetailsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SettlementOfStoresDetailsTemps];
GO
IF OBJECT_ID(N'[dbo].[ProjectRecipts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectRecipts];
GO
IF OBJECT_ID(N'[dbo].[POSDetailPayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[POSDetailPayments];
GO
IF OBJECT_ID(N'[dbo].[PointOfSales]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PointOfSales];
GO
IF OBJECT_ID(N'[dbo].[MasterPOSInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MasterPOSInvoices];
GO
IF OBJECT_ID(N'[dbo].[DetailPOSInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetailPOSInvoices];
GO
IF OBJECT_ID(N'[dbo].[PosPaymentMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PosPaymentMethods];
GO
IF OBJECT_ID(N'[dbo].[ReversePOSInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReversePOSInvoices];
GO
IF OBJECT_ID(N'[dbo].[ReversePOSDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReversePOSDetails];
GO
IF OBJECT_ID(N'[dbo].[POSDetailPaymentTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[POSDetailPaymentTemps];
GO
IF OBJECT_ID(N'[dbo].[PointOfSaleTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PointOfSaleTemps];
GO
IF OBJECT_ID(N'[dbo].[MasterPOSInvoiceTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MasterPOSInvoiceTemps];
GO
IF OBJECT_ID(N'[dbo].[PosPaymentMethodTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PosPaymentMethodTemps];
GO
IF OBJECT_ID(N'[dbo].[DetailPOSInvoiceTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetailPOSInvoiceTemps];
GO
IF OBJECT_ID(N'[dbo].[ReversePOSInvoiceTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReversePOSInvoiceTemps];
GO
IF OBJECT_ID(N'[dbo].[ReversePOSDetailTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReversePOSDetailTemps];
GO
IF OBJECT_ID(N'[dbo].[SubscriptionItemsTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubscriptionItemsTemps];
GO
IF OBJECT_ID(N'[dbo].[SubscriptionTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubscriptionTemps];
GO
IF OBJECT_ID(N'[dbo].[ExpenseAccountTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExpenseAccountTemps];
GO
IF OBJECT_ID(N'[dbo].[ProjectDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectDetails];
GO
IF OBJECT_ID(N'[dbo].[ProjectTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectTemps];
GO
IF OBJECT_ID(N'[dbo].[ProjectDetailTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectDetailTemps];
GO
IF OBJECT_ID(N'[dbo].[StoreCards]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StoreCards];
GO
IF OBJECT_ID(N'[dbo].[LockCodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[LockCodes];
GO
IF OBJECT_ID(N'[dbo].[ProjectPaymentFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectPaymentFiles];
GO
IF OBJECT_ID(N'[dbo].[ProjectReceiptFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectReceiptFiles];
GO
IF OBJECT_ID(N'[dbo].[SubscriptionInvoiceFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubscriptionInvoiceFiles];
GO
IF OBJECT_ID(N'[dbo].[GLMasterFiles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[GLMasterFiles];
GO
IF OBJECT_ID(N'[dbo].[Packages]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Packages];
GO
IF OBJECT_ID(N'[dbo].[OperationRuns]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OperationRuns];
GO
IF OBJECT_ID(N'[dbo].[EmpVacations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpVacations];
GO
IF OBJECT_ID(N'[dbo].[EmpVacationTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmpVacationTemps];
GO
IF OBJECT_ID(N'[dbo].[VacationEmgrations]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VacationEmgrations];
GO
IF OBJECT_ID(N'[dbo].[ChartDates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ChartDates];
GO
IF OBJECT_ID(N'[dbo].[InvoiceTaxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceTaxes];
GO
IF OBJECT_ID(N'[dbo].[SalesReceiptTaxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SalesReceiptTaxes];
GO
IF OBJECT_ID(N'[dbo].[TaxPOSInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxPOSInvoices];
GO
IF OBJECT_ID(N'[dbo].[OfferTaxes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfferTaxes];
GO
IF OBJECT_ID(N'[dbo].[ProductFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductFileDetails];
GO
IF OBJECT_ID(N'[dbo].[UserConnections]', 'U') IS NOT NULL
    DROP TABLE [dbo].[UserConnections];
GO
IF OBJECT_ID(N'[dbo].[PrintBarcodes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PrintBarcodes];
GO
IF OBJECT_ID(N'[dbo].[ProductCompounds]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCompounds];
GO
IF OBJECT_ID(N'[dbo].[ProductCompoundTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProductCompoundTemps];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderOperationals]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderOperationals];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderOperationalDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderOperationalDetails];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderOperationalStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderOperationalStatus];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceStatus];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderOperationalDetailTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderOperationalDetailTemps];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderOperationalTemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderOperationalTemps];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrders];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderStatus];
GO
IF OBJECT_ID(N'[dbo].[MaintenanceOrderCodings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MaintenanceOrderCodings];
GO
IF OBJECT_ID(N'[dbo].[Orders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Orders];
GO
IF OBJECT_ID(N'[dbo].[OrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderDetails];
GO
IF OBJECT_ID(N'[dbo].[TempOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TempOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[TempOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TempOrders];
GO
IF OBJECT_ID(N'[dbo].[HoldedOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoldedOrders];
GO
IF OBJECT_ID(N'[dbo].[HoldedOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HoldedOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[ReverseOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseOrders];
GO
IF OBJECT_ID(N'[dbo].[ReverseOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[DetailQRInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[DetailQRInvoices];
GO
IF OBJECT_ID(N'[dbo].[QRPaymentMethods]', 'U') IS NOT NULL
    DROP TABLE [dbo].[QRPaymentMethods];
GO
IF OBJECT_ID(N'[dbo].[MasterQRInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MasterQRInvoices];
GO
IF OBJECT_ID(N'[dbo].[TaxQRInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[TaxQRInvoices];
GO
IF OBJECT_ID(N'[dbo].[StoreEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StoreEmployees];
GO
IF OBJECT_ID(N'[dbo].[ExtrnalSalesEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExtrnalSalesEmployees];
GO
IF OBJECT_ID(N'[dbo].[EmployeeSalesDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[EmployeeSalesDetails];
GO
IF OBJECT_ID(N'[dbo].[ExternalEmployeePayments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ExternalEmployeePayments];
GO
IF OBJECT_ID(N'[dbo].[RoleFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RoleFunctions];
GO
IF OBJECT_ID(N'[dbo].[SystemFunctions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemFunctions];
GO
IF OBJECT_ID(N'[dbo].[SystemRoles]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SystemRoles];
GO
IF OBJECT_ID(N'[dbo].[ProjectClauses]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectClauses];
GO
IF OBJECT_ID(N'[dbo].[SubLedgertemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SubLedgertemps];
GO
IF OBJECT_ID(N'[dbo].[ProjectClausetemps]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ProjectClausetemps];
GO
IF OBJECT_ID(N'[dbo].[SpentReciptOrders]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpentReciptOrders];
GO
IF OBJECT_ID(N'[dbo].[SpentReciptOrderTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SpentReciptOrderTypes];
GO
IF OBJECT_ID(N'[dbo].[FixedAssetDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[FixedAssetDetails];
GO
IF OBJECT_ID(N'[dbo].[ReceiptPOSFileDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReceiptPOSFileDetails];
GO
IF OBJECT_ID(N'[dbo].[POSReceipts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[POSReceipts];
GO
IF OBJECT_ID(N'[dbo].[ReverseQRDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseQRDetails];
GO
IF OBJECT_ID(N'[dbo].[ReverseQRInvoices]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReverseQRInvoices];
GO
IF OBJECT_ID(N'[dbo].[PaymentMethodEmployees]', 'U') IS NOT NULL
    DROP TABLE [dbo].[PaymentMethodEmployees];
GO
IF OBJECT_ID(N'[dbo].[InvoiceSerials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceSerials];
GO
IF OBJECT_ID(N'[dbo].[ReveseInvoiceSerials]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ReveseInvoiceSerials];
GO
IF OBJECT_ID(N'[dbo].[MedianSubscriptions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[MedianSubscriptions];
GO
IF OBJECT_ID(N'[dbo].[HRNotifications]', 'U') IS NOT NULL
    DROP TABLE [dbo].[HRNotifications];
GO
IF OBJECT_ID(N'[dbo].[Shifts]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Shifts];
GO
IF OBJECT_ID(N'[dbo].[ShiftCodings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[ShiftCodings];
GO
IF OBJECT_ID(N'[dbo].[VacationWeeklies]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VacationWeeklies];
GO
IF OBJECT_ID(N'[dbo].[VacationWeeklyCodings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[VacationWeeklyCodings];
GO
IF OBJECT_ID(N'[dbo].[AttendingAndLeavings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AttendingAndLeavings];
GO
IF OBJECT_ID(N'[dbo].[RewardsAndPenalties]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RewardsAndPenalties];
GO
IF OBJECT_ID(N'[dbo].[RewardsAndPenaltiesCodingCategories]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RewardsAndPenaltiesCodingCategories];
GO
IF OBJECT_ID(N'[dbo].[RewardsAndPenaltiesCodings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[RewardsAndPenaltiesCodings];
GO
IF OBJECT_ID(N'[dbo].[OfficialHolidays]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OfficialHolidays];
GO
IF OBJECT_ID(N'[dbo].[InvoicePrintSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoicePrintSettings];
GO
IF OBJECT_ID(N'[dbo].[InvoiceWordPrintSettings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[InvoiceWordPrintSettings];
GO
IF OBJECT_ID(N'[dbo].[Questions]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Questions];
GO
IF OBJECT_ID(N'[dbo].[Answers]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Answers];
GO
IF OBJECT_ID(N'[dbo].[Sewings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Sewings];
GO
IF OBJECT_ID(N'[dbo].[SewingDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SewingDetails];
GO
IF OBJECT_ID(N'[dbo].[SewingStatus]', 'U') IS NOT NULL
    DROP TABLE [dbo].[SewingStatus];
GO
IF OBJECT_ID(N'[dbo].[OrderTypes]', 'U') IS NOT NULL
    DROP TABLE [dbo].[OrderTypes];
GO
IF OBJECT_ID(N'[dbo].[AdditionalOrderDetails]', 'U') IS NOT NULL
    DROP TABLE [dbo].[AdditionalOrderDetails];
GO
IF OBJECT_ID(N'[dbo].[StartAndEndSellings]', 'U') IS NOT NULL
    DROP TABLE [dbo].[StartAndEndSellings];
GO

-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'Products'
CREATE TABLE [dbo].[Products] (
    [Prod_ID] int IDENTITY(1,1) NOT NULL,
    [Prod_Name] nvarchar(max)  NOT NULL,
    [Prod_Code] nvarchar(max)  NOT NULL,
    [Prod_Descrip] nvarchar(max)  NULL,
    [Category_ID] int  NULL,
    [MeasuringUnit_ID] int  NULL,
    [Prod_Mark] nvarchar(max)  NULL,
    [Sale_Price] decimal(32,12)  NULL,
    [Demand_Limit] int  NULL,
    [Store_Id] int  NULL,
    [Prod_Stored] bit  NOT NULL,
    [Bar_Code] nvarchar(max)  NULL,
    [Bar_Code_one] varbinary(max)  NULL,
    [Pro_Img] varbinary(max)  NULL,
    [Tax_Id] int  NULL,
    [Prod_Cost_Price] decimal(32,12)  NULL,
    [Prod_Type] int  NULL,
    [OpenBalance_Code] nvarchar(max)  NULL,
    [Prod_Category] nvarchar(max)  NULL,
    [Prod_Unet] nvarchar(max)  NULL,
    [Total] decimal(18,2)  NULL,
    [Tax_Id_Purch] int  NULL,
    [Quantity] decimal(32,12)  NULL,
    [Validity] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'Stores'
CREATE TABLE [dbo].[Stores] (
    [Store_ID] int IDENTITY(1,1) NOT NULL,
    [Store_Code] nvarchar(max)  NOT NULL,
    [Store_Name] nvarchar(max)  NOT NULL,
    [Store_Address] nvarchar(max)  NULL,
    [Emp_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Total_Batch_Cost] decimal(32,12)  NULL,
    [isActive] bit  NULL
);
GO

-- Creating table 'MeasuringUnits'
CREATE TABLE [dbo].[MeasuringUnits] (
    [MUnit_ID] int IDENTITY(1,1) NOT NULL,
    [MUnit_Code] nvarchar(max)  NOT NULL,
    [MUnit_Descrip] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Categories'
CREATE TABLE [dbo].[Categories] (
    [Cat_ID] int IDENTITY(1,1) NOT NULL,
    [Cat_Code] nvarchar(max)  NOT NULL,
    [Cat_Img] varbinary(max)  NULL,
    [Cat_Descrip] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Branches'
CREATE TABLE [dbo].[Branches] (
    [Branch_Id] int IDENTITY(1,1) NOT NULL,
    [Branch_Name] nvarchar(max)  NOT NULL,
    [Branch_Address] nvarchar(max)  NULL,
    [Branch_Phone] nvarchar(max)  NULL,
    [CompanyComp_Id] int  NULL,
    [Branch_Email] nvarchar(max)  NULL,
    [Branch_Country] nvarchar(max)  NULL,
    [Branch_Province] nvarchar(max)  NULL,
    [Branch_City] nvarchar(max)  NULL,
    [Branch_Fax] nvarchar(max)  NULL,
    [Branch_Code] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL,
    [BranchLogo] varbinary(max)  NULL,
    [ImageHeader] varbinary(max)  NULL,
    [ImageFooter] varbinary(max)  NULL,
    [ImageWaterMark] varbinary(max)  NULL
);
GO

-- Creating table 'Companies'
CREATE TABLE [dbo].[Companies] (
    [Comp_Id] int IDENTITY(1,1) NOT NULL,
    [Comp_Name] nvarchar(max)  NOT NULL,
    [Comp_Code] nvarchar(max)  NOT NULL,
    [Comp_Address] nvarchar(max)  NULL,
    [Comp_phone] nvarchar(max)  NULL,
    [Comp_City] nvarchar(50)  NULL,
    [Comp_Email] nvarchar(50)  NULL,
    [Comp_PortalCode] nvarchar(max)  NULL,
    [Comp_Fax] nvarchar(max)  NULL,
    [Comp_Field] nvarchar(max)  NULL,
    [Company_Country] nvarchar(max)  NULL,
    [Comp_Province] nvarchar(max)  NULL,
    [Comp_Currency_Id] int  NULL,
    [Tax_Number] nvarchar(max)  NULL,
    [StartFinancialDate] datetime  NULL,
    [EndFinancialDate] datetime  NULL,
    [Number] int  NULL,
    [Country] nvarchar(max)  NULL,
    [Province] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [ImageFooter] varbinary(max)  NULL,
    [ImageHeader] varbinary(max)  NULL,
    [ImageWaterMark] varbinary(max)  NULL,
    [CRN] nvarchar(max)  NULL,
    [InvoicePrintSimple] int  NULL,
    [InvoicePrintOrdanary] int  NULL,
    [InvoicePrintSpecial] int  NULL,
    [InvoicePrintOfferPrice] int  NULL
);
GO

-- Creating table 'BranchDepartments'
CREATE TABLE [dbo].[BranchDepartments] (
    [Dept_Id] int IDENTITY(1,1) NOT NULL,
    [Dept_Name] nvarchar(max)  NOT NULL,
    [Dept_Phone] nvarchar(max)  NULL,
    [Dept_Description] nvarchar(50)  NULL,
    [BranchBranch_Id] int  NOT NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL
);
GO

-- Creating table 'Partners'
CREATE TABLE [dbo].[Partners] (
    [Part_Id] int IDENTITY(1,1) NOT NULL,
    [Part_Name] nvarchar(max)  NOT NULL,
    [Part_Phone] nvarchar(max)  NULL,
    [Part_Email] nvarchar(max)  NULL,
    [Part_Birthday] nvarchar(max)  NULL,
    [Part_AddedDate] nvarchar(max)  NULL,
    [Part_RemovedDate] nvarchar(max)  NULL,
    [Part_Nationality] nvarchar(max)  NULL,
    [CompanyComp_Id] int  NOT NULL,
    [Partner_Code] nvarchar(max)  NOT NULL,
    [Balance_Amount] decimal(18,2)  NULL
);
GO

-- Creating table 'Currencies'
CREATE TABLE [dbo].[Currencies] (
    [Currency_Id] int IDENTITY(1,1) NOT NULL,
    [Currency_Type] nvarchar(max)  NOT NULL,
    [Currency_Country] nvarchar(max)  NOT NULL,
    [Currency_ISO] nvarchar(max)  NOT NULL,
    [Currency_Name] nvarchar(max)  NULL,
    [Currency_Iso2] nvarchar(max)  NULL,
    [Break_Coin] nvarchar(max)  NULL,
    [Break_Coin2] nvarchar(max)  NULL,
    [Current_Price] decimal(32,12)  NULL
);
GO

-- Creating table 'CompanyDepartments'
CREATE TABLE [dbo].[CompanyDepartments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [CompanyComp_Id] int  NOT NULL,
    [CompDept_Code] nvarchar(max)  NULL,
    [CompDept_Name] nvarchar(max)  NOT NULL,
    [CompDept_Phone] nvarchar(max)  NULL,
    [CompDept_Description] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL
);
GO

-- Creating table 'CompanyConfigrations'
CREATE TABLE [dbo].[CompanyConfigrations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TypeOfConfigration] nvarchar(max)  NOT NULL,
    [ChosenBoolean] bit  NOT NULL
);
GO

-- Creating table 'Countries'
CREATE TABLE [dbo].[Countries] (
    [Country_Id] int  NOT NULL,
    [Country_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Provinces'
CREATE TABLE [dbo].[Provinces] (
    [Province_Id] int  NOT NULL,
    [CountryId] int  NOT NULL,
    [Province_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Cities'
CREATE TABLE [dbo].[Cities] (
    [City_Id] int  NOT NULL,
    [ProvinceId] int  NOT NULL,
    [City_Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'BussinesUnits'
CREATE TABLE [dbo].[BussinesUnits] (
    [BU_Id] int IDENTITY(1,1) NOT NULL,
    [BU_Code] nvarchar(max)  NOT NULL,
    [BU_Name] nvarchar(max)  NOT NULL,
    [BU_Description] nvarchar(max)  NOT NULL,
    [BU_Type] nvarchar(max)  NOT NULL,
    [CompanyComp_Id] int  NOT NULL
);
GO

-- Creating table 'Expenses'
CREATE TABLE [dbo].[Expenses] (
    [Expense_Id] int IDENTITY(1,1) NOT NULL,
    [AccountExpense_Id] int  NOT NULL,
    [AccountTreasure_Id] int  NOT NULL,
    [BU_Id] int  NOT NULL,
    [Expense_Value] decimal(32,12)  NOT NULL,
    [Expense_Description] nvarchar(max)  NULL,
    [Expense_date] datetime  NOT NULL,
    [Company_Id] int  NOT NULL,
    [Currency_Id] int  NOT NULL,
    [Expense_State] int  NOT NULL,
    [Subledger_Id] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Asset_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [Expense_Code] nvarchar(max)  NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL,
    [Tax_ID] int  NULL,
    [RandomValue] int  NULL
);
GO

-- Creating table 'GLMasters'
CREATE TABLE [dbo].[GLMasters] (
    [GLMaster_Id] int IDENTITY(1,1) NOT NULL,
    [GL_Date] datetime  NOT NULL,
    [GL_Desc] nvarchar(max)  NULL,
    [Company_Id] int  NOT NULL,
    [GLType_Id] nvarchar(max)  NOT NULL,
    [Currency_Id] nvarchar(max)  NOT NULL,
    [GLState] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [GLCode] nvarchar(max)  NULL,
    [Auto] bit  NULL,
    [Account_Child_ID] int  NULL
);
GO

-- Creating table 'GLDetails'
CREATE TABLE [dbo].[GLDetails] (
    [GLDetail_Id] int IDENTITY(1,1) NOT NULL,
    [GLDetail_Debit] decimal(32,12)  NOT NULL,
    [GLDetail_Credit] decimal(32,12)  NOT NULL,
    [Bu_Id] int  NOT NULL,
    [GLMasterGLMaster_Id] int  NOT NULL,
    [SubLedger_Id] int  NULL,
    [Asset_Id] int  NULL,
    [Description_Detail] nvarchar(max)  NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Child_ID] int  NULL,
    [Account_CHild_Name] nvarchar(max)  NULL,
    [Account_Id] int  NOT NULL,
    [CostDistrbutionSubldger] bit  NULL,
    [ProjectClause_Id] int  NULL
);
GO

-- Creating table 'ChartOfAccountings'
CREATE TABLE [dbo].[ChartOfAccountings] (
    [Account_Id] int IDENTITY(1,1) NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Code] nvarchar(max)  NOT NULL,
    [Account_Level] int  NOT NULL,
    [ChartOfAccountingAccount_Id] int  NULL,
    [Account_Balance] decimal(32,12)  NULL,
    [Currency_Name] nvarchar(max)  NULL,
    [Starting_Balance] decimal(32,12)  NULL,
    [BalanceSheetValue] decimal(32,12)  NULL,
    [ISFinalChild] bit  NULL,
    [isActive] bit  NULL
);
GO

-- Creating table 'GL_Type'
CREATE TABLE [dbo].[GL_Type] (
    [GL_Type_Id] int IDENTITY(1,1) NOT NULL,
    [GL_Type_Name] nvarchar(max)  NOT NULL,
    [GL_Type_Description] nvarchar(max)  NULL
);
GO

-- Creating table 'Customers'
CREATE TABLE [dbo].[Customers] (
    [Customer_Id] int IDENTITY(1,1) NOT NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Customer_Address] nvarchar(max)  NULL,
    [Customer_Phone] nvarchar(max)  NOT NULL,
    [Customer_Mail] nvarchar(max)  NULL,
    [Customer_Province] nvarchar(max)  NULL,
    [Customer_Country] nvarchar(max)  NULL,
    [Customer_City] nvarchar(max)  NULL,
    [Customer_Fax] nvarchar(max)  NULL,
    [Customer_Code] nvarchar(max)  NULL,
    [Branch_Id] int  NULL,
    [Customer_Mobile] nvarchar(max)  NULL,
    [Purchase_Price] decimal(18,0)  NULL,
    [Payment_Policy_Id] int  NULL,
    [Customer_Website] nvarchar(max)  NULL,
    [SlicesSlice_Id] int  NOT NULL,
    [Customer_Balance] decimal(32,12)  NULL,
    [Customer_State] int  NULL,
    [Customer_Bal] bit  NOT NULL,
    [Customer_Type] int  NULL,
    [Customer_TaxNumber] nvarchar(max)  NULL,
    [Country] nvarchar(max)  NULL,
    [Province] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL,
    [Comment] nvarchar(max)  NULL,
    [Employee_Id] int  NULL,
    [CommercialRegistrationNo] nvarchar(max)  NULL,
    [IdentificationNumber] nvarchar(max)  NULL
);
GO

-- Creating table 'InvoiceMasters'
CREATE TABLE [dbo].[InvoiceMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Document_Type] int  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Store_ID] int  NOT NULL,
    [TotalAmountAfMat] decimal(32,12)  NOT NULL,
    [Tax_ID] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [RevenueAccount_Id] int  NOT NULL,
    [Payment_Policy] int  NULL,
    [Emp_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Offer_Price_Code] nvarchar(max)  NULL,
    [SubLedger_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [Department_ID] int  NULL,
    [Company_Id] int  NOT NULL,
    [TotaL_After_Fixed] decimal(32,12)  NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [HasOff] bit  NOT NULL,
    [Notes] nvarchar(max)  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'InvoiceDetails'
CREATE TABLE [dbo].[InvoiceDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL,
    [InvoiceMaster_Id] int  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Reverse_Total_Amount] decimal(32,12)  NOT NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'Employees'
CREATE TABLE [dbo].[Employees] (
    [Emp_Id] int IDENTITY(1,1) NOT NULL,
    [Emp_Code] nvarchar(max)  NOT NULL,
    [Emp_Name] nvarchar(max)  NOT NULL,
    [Emp_Address] nvarchar(max)  NULL,
    [Emp_Mail] nvarchar(max)  NULL,
    [Emp_Phone] nvarchar(max)  NOT NULL,
    [Emp_Title] nvarchar(max)  NULL,
    [Emp_DOB] nvarchar(max)  NULL,
    [BU_Id] int  NULL,
    [Subldger_Id] int  NULL,
    [Residency_number] nvarchar(max)  NULL,
    [Passport_number] nvarchar(max)  NULL,
    [Start_Date_Passp] nvarchar(max)  NULL,
    [End_Date_Passp] nvarchar(max)  NULL,
    [Start_Date_Residency] nvarchar(max)  NULL,
    [End_Date_Residency] nvarchar(max)  NULL,
    [Team_ID] int  NULL,
    [Team_Name] nvarchar(max)  NULL,
    [Fixed_Salary] decimal(32,12)  NULL,
    [Total_Salary] decimal(32,12)  NULL,
    [Vacations_Number] int  NULL,
    [Vacation_Used] int  NULL,
    [Status] int  NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [Emp_SubLedger] bit  NULL,
    [User] bit  NOT NULL,
    [Active] bit  NOT NULL,
    [Company_ID] int  NULL,
    [Social_status] int  NULL,
    [Nationality] nvarchar(max)  NULL,
    [Contract_Begin] nvarchar(max)  NULL,
    [Contract_End] nvarchar(max)  NULL,
    [Work_Actual_Start] nvarchar(max)  NULL,
    [Type_OF_Work] int  NULL,
    [Social_insurance_number] nvarchar(max)  NULL,
    [Medical_Insurance_Number] nvarchar(max)  NULL,
    [End_medical_Insurance] nvarchar(max)  NULL,
    [Sex] int  NULL,
    [Reg_ID] nvarchar(max)  NULL,
    [Client_Id] int  NULL,
    [Emp_Image] varbinary(max)  NULL,
    [Vacation_Pervious] int  NULL,
    [TotalVacation] int  NULL,
    [TotalSalaryBeforPenality] decimal(32,12)  NULL,
    [BankName] nvarchar(max)  NULL,
    [BankCode] nvarchar(max)  NULL,
    [AccountNum] nvarchar(max)  NULL,
    [EbanNumb] nvarchar(max)  NULL,
    [SystemRoleId] int  NULL,
    [ViewAllCustomer] bit  NULL,
    [NumberOfHour] int  NULL
);
GO

-- Creating table 'Treasuries'
CREATE TABLE [dbo].[Treasuries] (
    [Trea_Id] int IDENTITY(1,1) NOT NULL,
    [Trea_Name] nvarchar(max)  NOT NULL,
    [Trea_Code] nvarchar(max)  NOT NULL,
    [Dep_Id] int  NULL,
    [Emp_Id] int  NULL,
    [Trea_Desc] nvarchar(max)  NULL,
    [EmpName] nvarchar(max)  NULL,
    [Balance_Amount] decimal(32,12)  NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL
);
GO

-- Creating table 'EmployeeFileDetails'
CREATE TABLE [dbo].[EmployeeFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [EmployeeEmp_Id] int  NOT NULL
);
GO

-- Creating table 'Suppliers'
CREATE TABLE [dbo].[Suppliers] (
    [Supp_Id] int IDENTITY(1,1) NOT NULL,
    [Supp_Name] nvarchar(max)  NOT NULL,
    [Supp_Address] nvarchar(max)  NULL,
    [Supp_Phone] nvarchar(max)  NOT NULL,
    [Supp_Mail] nvarchar(max)  NULL,
    [Supp_Country] nvarchar(max)  NULL,
    [Supp_Province] nvarchar(max)  NULL,
    [Supp_City] nvarchar(max)  NULL,
    [Supp_Fax] nvarchar(max)  NULL,
    [Supp_Code] nvarchar(max)  NULL,
    [Payment_Policy_Id] int  NULL,
    [Purchase_Price] decimal(32,12)  NULL,
    [Supp_Mobile] nvarchar(max)  NULL,
    [Branch_Id] int  NULL,
    [Supp_Website] nvarchar(max)  NULL,
    [Tax_Number] nvarchar(max)  NULL,
    [Supp_Balance] decimal(32,12)  NULL,
    [Supplier_type] int  NULL,
    [Supplier_Bal] bit  NOT NULL,
    [Supplier_state] int  NULL,
    [Country] nvarchar(max)  NULL,
    [Province] nvarchar(max)  NULL,
    [City] nvarchar(max)  NULL
);
GO

-- Creating table 'PurInvoiceMasters'
CREATE TABLE [dbo].[PurInvoiceMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Document_Type] int  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Supplier_Id] int  NOT NULL,
    [Store_ID] int  NULL,
    [TotalAmountAfMat] decimal(32,12)  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Subledger_Id] int  NULL,
    [BU_Id] int  NULL,
    [Account_Id] int  NULL,
    [Tax_Type] bit  NOT NULL,
    [Reference] nvarchar(max)  NULL,
    [Payment_Policy] int  NULL,
    [Emp_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Purch_Order_Code] nvarchar(max)  NULL,
    [Store_Name] nvarchar(max)  NULL,
    [Payed_Amount] decimal(32,12)  NULL,
    [Supp_Inv_Serial] nvarchar(max)  NULL,
    [Company_Id] int  NOT NULL,
    [Cost_distribute] bit  NOT NULL,
    [HasOff] bit  NOT NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [TotaL_After_Fixed] decimal(32,12)  NULL,
    [PaymentMethod_Id] int  NULL
);
GO

-- Creating table 'PurchaseDetails'
CREATE TABLE [dbo].[PurchaseDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [InvoiceMaster_Id] int  NOT NULL,
    [Product_Tax] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NULL,
    [Asset_Id] int  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [Tax_Name] nvarchar(max)  NULL,
    [Item_Code] nvarchar(max)  NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Reverse_Tota_Cost] decimal(32,12)  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'AnotherAssets'
CREATE TABLE [dbo].[AnotherAssets] (
    [AnothrAsst_ID] int IDENTITY(1,1) NOT NULL,
    [AnothrAsst_Desc] nvarchar(max)  NOT NULL,
    [AnothrAsst_Price] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CurrentAssetTypes'
CREATE TABLE [dbo].[CurrentAssetTypes] (
    [Casset_ID] int IDENTITY(1,1) NOT NULL,
    [CAsst_Name] nvarchar(max)  NULL
);
GO

-- Creating table 'FixedAssetImages'
CREATE TABLE [dbo].[FixedAssetImages] (
    [FasstImage_ID] int IDENTITY(1,1) NOT NULL,
    [FAsset_Image] nvarchar(max)  NULL,
    [FixedAssetID] int  NOT NULL
);
GO

-- Creating table 'DepreciationTypes'
CREATE TABLE [dbo].[DepreciationTypes] (
    [DepType_ID] int IDENTITY(1,1) NOT NULL,
    [DepType_Name] nvarchar(max)  NOT NULL,
    [Dep_Choosed] bit  NOT NULL
);
GO

-- Creating table 'RecalPurchInvoiceMasters'
CREATE TABLE [dbo].[RecalPurchInvoiceMasters] (
    [Recall_MPurch_Serial] nvarchar(max)  NOT NULL,
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Register_Date] datetime  NOT NULL,
    [Due_Date] datetime  NULL,
    [Inv_TotalAmount] decimal(32,12)  NOT NULL,
    [Inv_TotalTax] decimal(32,12)  NOT NULL,
    [Inv_TotalCost] decimal(32,12)  NOT NULL,
    [Inv_TotalDiscount] decimal(32,12)  NOT NULL,
    [PI_Currency] nvarchar(max)  NOT NULL,
    [Doc_Type] nvarchar(max)  NOT NULL,
    [PI_State] int  NULL,
    [SupplierSuppId] int  NOT NULL,
    [StoreStore_ID] int  NOT NULL,
    [CompanyComp_Id] int  NOT NULL,
    [PurInvoiceMasterMaster_Id] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [HasOff] bit  NOT NULL,
    [PaymentMethod_Id] int  NULL
);
GO

-- Creating table 'RecalPurchInvoiceDetails'
CREATE TABLE [dbo].[RecalPurchInvoiceDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Asset_Id] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [RecalPurchInvoiceMasterMaster_Id] int  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Tax] decimal(32,12)  NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL,
    [Tax_Name] nvarchar(max)  NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Product_Code] nvarchar(max)  NULL
);
GO

-- Creating table 'InventoryTransfer_Hd'
CREATE TABLE [dbo].[InventoryTransfer_Hd] (
    [TransferHd_Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Business_UnitId] int  NULL,
    [InvTransfere_Notes] nvarchar(max)  NULL,
    [SourceStore_Id] int  NOT NULL,
    [DestStore_Id] int  NOT NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Tranfer_State] int  NOT NULL,
    [Comop_ID] int  NOT NULL,
    [Inv_Currency] nvarchar(max)  NULL,
    [Inv_Code] nvarchar(max)  NOT NULL,
    [HasOff] bit  NOT NULL
);
GO

-- Creating table 'InventoryTransfer_Detail'
CREATE TABLE [dbo].[InventoryTransfer_Detail] (
    [TransferDetail_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Qty] decimal(32,12)  NOT NULL,
    [InventoryTransfer_Id] int  NOT NULL,
    [Detail_TotalCoct] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL
);
GO

-- Creating table 'Matchings'
CREATE TABLE [dbo].[Matchings] (
    [Match_Id] int IDENTITY(1,1) NOT NULL,
    [ReceiptRec_ID] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Match_currency] nvarchar(max)  NOT NULL,
    [Comp_ID] int  NOT NULL,
    [Match_BU] int  NULL,
    [Tota_Amount] decimal(32,12)  NOT NULL,
    [InvoiceMasterMaster_Id] int  NOT NULL,
    [Match_state] int  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Matching_Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Receipts'
CREATE TABLE [dbo].[Receipts] (
    [Rec_ID] int IDENTITY(1,1) NOT NULL,
    [RecComp_ID] int  NULL,
    [Rec_Amount] decimal(32,12)  NOT NULL,
    [RecCurrency_ID] nvarchar(max)  NOT NULL,
    [Rec_Number] nvarchar(max)  NULL,
    [RecBank_Account] int  NOT NULL,
    [Rec_Description] nvarchar(max)  NULL,
    [RecGl_Date] datetime  NULL,
    [Rec_Date] datetime  NOT NULL,
    [Rec_BU] int  NOT NULL,
    [TotalAmountAfMat] decimal(32,12)  NOT NULL,
    [Rec_state] int  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Branch_Id] int  NULL,
    [PaymentMethod_Id] int  NULL,
    [Invoice_ID] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL
);
GO

-- Creating table 'MatchingPayments'
CREATE TABLE [dbo].[MatchingPayments] (
    [Match_Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Match_currency] int  NOT NULL,
    [Comp_ID] int  NOT NULL,
    [Match_BU] int  NULL,
    [Tota_Amount] decimal(32,12)  NOT NULL,
    [PaymentPay_ID] int  NOT NULL,
    [PurInvoiceMaster_Id] int  NOT NULL,
    [Match_state] int  NOT NULL,
    [Supp_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Match_Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Payments'
CREATE TABLE [dbo].[Payments] (
    [Pay_ID] int IDENTITY(1,1) NOT NULL,
    [Pay_Currency_ID] nvarchar(max)  NOT NULL,
    [PayComp_ID] int  NOT NULL,
    [Pay_Amount] decimal(32,12)  NOT NULL,
    [Pay_Number] nvarchar(max)  NOT NULL,
    [PayBank_Account] int  NOT NULL,
    [Rec_Description] nvarchar(max)  NULL,
    [PayGl_Date] datetime  NULL,
    [Pay_Date] datetime  NOT NULL,
    [Pay_State] int  NOT NULL,
    [Pay_BU] int  NOT NULL,
    [TotalAmountAfMat] decimal(32,12)  NOT NULL,
    [Supplier_Id] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [RecCheck_Number] nvarchar(max)  NULL,
    [Reference] nvarchar(max)  NULL,
    [Branch_Id] int  NULL,
    [PurchaseInv_ID] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL
);
GO

-- Creating table 'ReveInvoiceDetails'
CREATE TABLE [dbo].[ReveInvoiceDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL,
    [InvoiceMaster_Id] int  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'ReverseSalesInvoiceMasters'
CREATE TABLE [dbo].[ReverseSalesInvoiceMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Company_Id] int  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Document_Type] int  NULL,
    [Invoice_State] int  NOT NULL,
    [SubLedger] nvarchar(max)  NULL,
    [InvoiceMaster_Id] int  NULL,
    [Branch_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [Department_ID] int  NULL,
    [Store_ID] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'Banks'
CREATE TABLE [dbo].[Banks] (
    [Bank_ID] int IDENTITY(1,1) NOT NULL,
    [Bank_Name] nvarchar(max)  NOT NULL,
    [Bank_Address] nvarchar(max)  NULL,
    [Bank_Code] nvarchar(max)  NOT NULL,
    [Balance_Code] nvarchar(max)  NULL,
    [Bank_Number] nvarchar(max)  NOT NULL,
    [Bank_Eban_Number] nvarchar(max)  NOT NULL,
    [Balance_Amount] decimal(32,12)  NULL,
    [Bank_Currency] nvarchar(max)  NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL,
    [Emp_Id] int  NULL
);
GO

-- Creating table 'Taxes'
CREATE TABLE [dbo].[Taxes] (
    [Tax_ID] int IDENTITY(1,1) NOT NULL,
    [Tax_Name] nvarchar(max)  NOT NULL,
    [Tax_Type] nvarchar(max)  NOT NULL,
    [Tax_AddedDate] nvarchar(max)  NULL,
    [Tax_Percentage] decimal(18,2)  NOT NULL,
    [Tax_Code] nvarchar(max)  NOT NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL,
    [IsSmokeTax] bit  NULL
);
GO

-- Creating table 'TransactionBatches'
CREATE TABLE [dbo].[TransactionBatches] (
    [Batch_Id] int IDENTITY(1,1) NOT NULL,
    [OperationName] nvarchar(max)  NOT NULL,
    [OperationId] int  NOT NULL,
    [Date] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [UserId] nvarchar(max)  NOT NULL,
    [Operation_Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Projects'
CREATE TABLE [dbo].[Projects] (
    [Project_Id] int IDENTITY(1,1) NOT NULL,
    [Project_Code] nvarchar(max)  NOT NULL,
    [Project_Name] nvarchar(max)  NOT NULL,
    [Project_Description] nvarchar(max)  NULL,
    [Project_Location] nvarchar(max)  NULL,
    [DeadLine] datetime  NULL,
    [StartTime] datetime  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [CustomerCustomer_Id] int  NOT NULL,
    [Emp_Id] int  NULL,
    [Payment_Method] int  NULL,
    [Revenu_Id] int  NULL,
    [Project_State] int  NOT NULL,
    [SubLedger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BranchSubledger_Id] int  NULL,
    [Company_ID] nvarchar(max)  NULL,
    [Emp_Team_Id] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [DetailOFPro] nvarchar(max)  NULL
);
GO

-- Creating table 'OfferPriceDetails'
CREATE TABLE [dbo].[OfferPriceDetails] (
    [OfferDet_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [OfferPriceMasterMaster_Id] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Product_Tax] decimal(32,12)  NOT NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'OfferPriceMasters'
CREATE TABLE [dbo].[OfferPriceMasters] (
    [OfferMas_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Offer_Date] datetime  NOT NULL,
    [Currency_Id] int  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Company_Id] int  NOT NULL,
    [OfferDeadLine] datetime  NOT NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Store_ID] int  NULL,
    [Tax_ID] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [RevenueAccount_Id] int  NOT NULL,
    [Payment_Policy] int  NULL,
    [Emp_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Offer_State] bit  NOT NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [TotaL_After_Fixed] decimal(32,12)  NULL
);
GO

-- Creating table 'DeprTaskStatus'
CREATE TABLE [dbo].[DeprTaskStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TaskStatus] nvarchar(max)  NOT NULL,
    [TaskDate] datetime  NOT NULL,
    [Asset_id] int  NOT NULL,
    [Count] int  NULL
);
GO

-- Creating table 'ProductBatches'
CREATE TABLE [dbo].[ProductBatches] (
    [Batch_ID] int IDENTITY(1,1) NOT NULL,
    [Product_ID] int  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NOT NULL,
    [Batch_Date] datetime  NULL,
    [Batch_ExpireDate] datetime  NULL,
    [Batch_Cost] decimal(32,12)  NOT NULL,
    [PurInvoice_ID] int  NULL,
    [Store_ID] int  NULL,
    [SalesPrice] decimal(32,12)  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'CostDistributions'
CREATE TABLE [dbo].[CostDistributions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Invoice_Serial] nvarchar(max)  NOT NULL,
    [Subledger_ID] int  NOT NULL,
    [Cost_Distr_Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CostDistributionDetails'
CREATE TABLE [dbo].[CostDistributionDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_AdditionalCost] decimal(32,12)  NOT NULL,
    [CostDistribution_ID] int  NOT NULL,
    [Product_ID] int  NOT NULL
);
GO

-- Creating table 'ReceiptFileDetails'
CREATE TABLE [dbo].[ReceiptFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ReceiptRec_ID] int  NOT NULL
);
GO

-- Creating table 'PaymentFileDetails'
CREATE TABLE [dbo].[PaymentFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [PaymentPay_ID] int  NOT NULL
);
GO

-- Creating table 'PartnerFileDetails'
CREATE TABLE [dbo].[PartnerFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [PartnerPart_Id] int  NOT NULL
);
GO

-- Creating table 'ExpenseFileDetails'
CREATE TABLE [dbo].[ExpenseFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ExpenseExpense_Id] int  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FixedAssetFileDetails'
CREATE TABLE [dbo].[FixedAssetFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL,
    [FixedAssetFasset_ID] int  NOT NULL
);
GO

-- Creating table 'SupplierFileDetails'
CREATE TABLE [dbo].[SupplierFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [SupplierSupp_Id] int  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'CustomerFileDetails'
CREATE TABLE [dbo].[CustomerFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [CustomerCustomer_Id] int  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProjectFileDetails'
CREATE TABLE [dbo].[ProjectFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL,
    [ProjectProject_Id] int  NOT NULL
);
GO

-- Creating table 'InvoiceDetailsTemps'
CREATE TABLE [dbo].[InvoiceDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [InvoiceMaster_Id] int  NOT NULL,
    [Product_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Product_Code] nvarchar(max)  NULL,
    [TotalDiscount] decimal(32,10)  NULL,
    [TotalBeforeDisc] decimal(32,10)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL,
    [Detail_Discount] decimal(32,12)  NULL
);
GO

-- Creating table 'InvoiceMasterTemps'
CREATE TABLE [dbo].[InvoiceMasterTemps] (
    [Master_Id] int  NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Customer_Id] int  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Document_Type] int  NULL,
    [Invoice_State] int  NULL,
    [Company_Id] int  NULL,
    [Store_ID] int  NULL,
    [TotalAmountAfMat] decimal(32,12)  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Reference] nvarchar(max)  NULL,
    [RevenueAccount_Id] int  NULL,
    [Payment_Policy] int  NULL,
    [Emp_Id] int  NULL,
    [Branch_Id] int  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL
);
GO

-- Creating table 'SalesFileDetails'
CREATE TABLE [dbo].[SalesFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [InvoiceMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'PurchaseDetailsTemps'
CREATE TABLE [dbo].[PurchaseDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NULL,
    [Asset_Id] int  NULL,
    [ItemDesc] nvarchar(max)  NULL,
    [PurInvoiceMasterTempMaster_Id] int  NOT NULL,
    [Product_Code] nvarchar(max)  NULL,
    [Detail_ID_Edit] int  NULL,
    [TotalDiscount] decimal(32,10)  NULL,
    [TotalBeforeDisc] decimal(32,10)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'PurInvoiceMasterTemps'
CREATE TABLE [dbo].[PurInvoiceMasterTemps] (
    [Master_Id] int  NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Document_Type] int  NULL,
    [Invoice_State] int  NULL,
    [Supplier_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [TotalAmountAfMat] decimal(32,12)  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Subledger_Id] int  NULL,
    [BU_Id] int  NULL,
    [Account_Id] int  NULL,
    [Tax_Type] bit  NULL,
    [Branch_Id] int  NULL,
    [Emp_Id] int  NULL,
    [Payment_Policy] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [AllowDiscount] decimal(32,12)  NULL
);
GO

-- Creating table 'PurchaseFileDetails'
CREATE TABLE [dbo].[PurchaseFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [PurInvoiceMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'RecallPurchInvDetailsTemps'
CREATE TABLE [dbo].[RecallPurchInvDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Asset_Id] int  NULL,
    [Detail_total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Tax] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Btach_ID] int  NOT NULL,
    [RecallPurchInvoicMasterTempMaster_Id] int  NOT NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Product_Code] nvarchar(max)  NULL
);
GO

-- Creating table 'RecallPurchInvoicMasterTemps'
CREATE TABLE [dbo].[RecallPurchInvoicMasterTemps] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Recall_MPurch_Serial] nvarchar(max)  NULL,
    [Register_Date] datetime  NULL,
    [Due_Date] datetime  NULL,
    [Inv_TotalAmount] decimal(32,12)  NULL,
    [Inv_TotalTax] decimal(32,12)  NULL,
    [Inv_TotalCost] decimal(32,12)  NULL,
    [Inv_TotalDiscount] decimal(32,12)  NULL,
    [PI_Currency] nvarchar(max)  NULL,
    [Doc_Type] nvarchar(max)  NULL,
    [Pi_State] int  NULL,
    [SupplierSuppId] int  NULL,
    [StoreStore_ID] int  NULL,
    [CompanyComp_Id] int  NULL,
    [PurInvoiceMasterMaster_Id] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Master_Discount] decimal(32,12)  NULL
);
GO

-- Creating table 'RecallPurchFileDetails'
CREATE TABLE [dbo].[RecallPurchFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLenght] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL,
    [RecalPurchInvoiceMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'ReversSalesFileDetails'
CREATE TABLE [dbo].[ReversSalesFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [ReverseSalesInvoiceMaster_Id] int  NOT NULL
);
GO

-- Creating table 'ReverseSalesInvoiceMasterTemps'
CREATE TABLE [dbo].[ReverseSalesInvoiceMasterTemps] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Due_Date] datetime  NULL,
    [GL_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Customer_Id] int  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Company_Id] int  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Document_Type] int  NULL,
    [Invoice_State] int  NULL,
    [Store_ID] int  NULL,
    [SubLedger] nvarchar(max)  NULL
);
GO

-- Creating table 'ReveInvoiceDetailsTemps'
CREATE TABLE [dbo].[ReveInvoiceDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL,
    [ReverseSalesInvoiceMasterTempMaster_Id] int  NOT NULL,
    [Product_Code] nvarchar(max)  NULL,
    [Product_Name] nvarchar(max)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'InventoryTransfer_HdTemp'
CREATE TABLE [dbo].[InventoryTransfer_HdTemp] (
    [TransferHd_Id] int  NOT NULL,
    [Date] datetime  NULL,
    [Business_UnitId] int  NULL,
    [InvTransfere_Notes] nvarchar(max)  NULL,
    [SourceStore_Id] int  NULL,
    [DestStore_Id] int  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Tranfer_State] int  NULL,
    [Comop_ID] int  NULL,
    [Inv_Currency] nvarchar(max)  NULL
);
GO

-- Creating table 'InventoryTransfer_DetailTemp'
CREATE TABLE [dbo].[InventoryTransfer_DetailTemp] (
    [TransferDetail_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Qty] decimal(32,12)  NOT NULL,
    [Detail_TotalCoct] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [Batch_ID] int  NULL,
    [InventoryTransfer_Temp_Id] int  NOT NULL
);
GO

-- Creating table 'Contacts'
CREATE TABLE [dbo].[Contacts] (
    [Contact_Id] int IDENTITY(1,1) NOT NULL,
    [Contact_Name] nvarchar(max)  NOT NULL,
    [Contact_Company] nvarchar(max)  NULL,
    [Contact_Email] nvarchar(max)  NOT NULL,
    [Customer_Id] int  NULL
);
GO

-- Creating table 'PaymentMethods'
CREATE TABLE [dbo].[PaymentMethods] (
    [PaymentMethod_Id] int IDENTITY(1,1) NOT NULL,
    [Payment_Type] nvarchar(max)  NOT NULL,
    [Bank_Account_Id] int  NULL,
    [Percentage] decimal(32,12)  NULL,
    [Expense_Account_Id] int  NULL,
    [Bank_Account_Name] nvarchar(max)  NULL,
    [Operatin_Cost] decimal(32,12)  NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL,
    [Emp_Id] int  NULL
);
GO

-- Creating table 'SalesReceiptDetailsTemps'
CREATE TABLE [dbo].[SalesReceiptDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [SalesReceiptMasterTempMaster_Id] int  NOT NULL,
    [comition] decimal(18,2)  NULL,
    [Comition_Id] int  NULL,
    [TotalDiscount] decimal(32,10)  NULL,
    [TotalBeforeDisc] decimal(32,10)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'SalesReceiptMasterTemps'
CREATE TABLE [dbo].[SalesReceiptMasterTemps] (
    [Master_Id] int  NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Invoice_State] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [PaymentMethod_Id] int  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [Emp_Id] int  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL
);
GO

-- Creating table 'SalesReceiptFiles'
CREATE TABLE [dbo].[SalesReceiptFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [Master_Id] int  NOT NULL
);
GO

-- Creating table 'SalesReceiptMasters'
CREATE TABLE [dbo].[SalesReceiptMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Company_Id] int  NOT NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NOT NULL,
    [SubLedger_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [Department_ID] int  NULL,
    [Store_ID] int  NOT NULL,
    [TotaL_After_Fixed] decimal(32,12)  NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL,
    [Notes] nvarchar(max)  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'SalesReceiptDetails'
CREATE TABLE [dbo].[SalesReceiptDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Master_Id] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Reverse_Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'FixedAssets'
CREATE TABLE [dbo].[FixedAssets] (
    [Fasset_ID] int IDENTITY(1,1) NOT NULL,
    [FAsset_BuyDate] datetime  NULL,
    [FAsset_SellDate] datetime  NULL,
    [FAsset_WorkingDate] datetime  NOT NULL,
    [FAsset_PurchasePrice] float  NULL,
    [FAsset_AgeMonth] int  NOT NULL,
    [FAsset_Description] nvarchar(max)  NOT NULL,
    [FAsset_Dispose] bit  NULL,
    [FAsset_Code] nvarchar(max)  NOT NULL,
    [FAsset_Type] nvarchar(max)  NULL,
    [FAssetCat_name] nvarchar(max)  NULL,
    [BookValue] float  NULL,
    [FAsset_Employee_ID] int  NULL,
    [FixedAssetTypeFAsset_TypeID] int  NOT NULL,
    [FixedAssetSecCategoryFAsset_CatID] int  NULL,
    [Complex] float  NULL,
    [Currency] nvarchar(max)  NULL,
    [BussinesUnitBU_Id] int  NULL,
    [JunckValue] float  NULL,
    [CompanyComp_Id] int  NOT NULL,
    [OpenBalanceWorkDate] datetime  NULL,
    [OpenBalance_Code] nvarchar(max)  NULL,
    [SubLedger_ID] int  NOT NULL,
    [FAsset_AgeDay] int  NULL,
    [PriceOfAssetDaily] decimal(32,12)  NULL,
    [PurchaseId] int  NULL
);
GO

-- Creating table 'PurchaseOrderDetails'
CREATE TABLE [dbo].[PurchaseOrderDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_ID] int  NULL,
    [Asset_Id] int  NULL,
    [InvoiceMaster_Id] int  NOT NULL
);
GO

-- Creating table 'PurchaseFileOrders'
CREATE TABLE [dbo].[PurchaseFileOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [PurchaseOrderMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'PurchaseOrderMasters'
CREATE TABLE [dbo].[PurchaseOrderMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Document_Type] int  NOT NULL,
    [Supplier_Id] int  NOT NULL,
    [Store_Id] int  NOT NULL,
    [Company_Id] int  NOT NULL,
    [ExchangeRate] decimal(18,0)  NULL,
    [Subledger_Id] int  NULL,
    [BU_Id] int  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Emp_Id] int  NULL,
    [Payment_Policy] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [OrderDate] datetime  NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [TotaL_After_Fixed] decimal(32,12)  NULL,
    [PaymentMethod_Id] int  NULL
);
GO

-- Creating table 'EmployeeTeamTemps'
CREATE TABLE [dbo].[EmployeeTeamTemps] (
    [ID] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NULL,
    [Employee_Name] nvarchar(max)  NULL,
    [TeamID] int  NULL,
    [Team_Name] nvarchar(max)  NULL
);
GO

-- Creating table 'EmployeeTeams'
CREATE TABLE [dbo].[EmployeeTeams] (
    [Team_ID] int IDENTITY(1,1) NOT NULL,
    [Team_Name] nvarchar(max)  NOT NULL,
    [Team_Leader_Name] nvarchar(max)  NULL,
    [Team_Leader_ID] int  NULL
);
GO

-- Creating table 'Subscriptions'
CREATE TABLE [dbo].[Subscriptions] (
    [Subscrip_Id] int IDENTITY(1,1) NOT NULL,
    [Subscrip_Code] nvarchar(max)  NOT NULL,
    [Subscrip_Description] nvarchar(max)  NULL,
    [DeadLine] datetime  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [Customer_Id] int  NULL,
    [Subscrip_Comp] int  NOT NULL,
    [Subscrip_BU] int  NULL,
    [Subscrip_State] int  NOT NULL,
    [Subscrip_Type] int  NOT NULL,
    [Subscrip_Pranch] int  NULL,
    [Emp_Team] int  NULL,
    [SubType_State] int  NOT NULL,
    [Payment_Method] int  NULL,
    [tel_customer] nvarchar(max)  NULL,
    [Subscrip_Stop] bit  NOT NULL,
    [Subscrip_DateEnd] datetime  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Sub_Type_ID] int  NOT NULL,
    [Check_Number] nvarchar(max)  NULL,
    [Sub_Pay_State] int  NULL,
    [Tax_ID] int  NOT NULL,
    [AmountAfterTax] decimal(32,12)  NOT NULL,
    [Subledger_ID] int  NULL,
    [Department_Id] int  NULL,
    [Payed_Amount] decimal(32,12)  NULL,
    [Discount] decimal(32,12)  NULL,
    [DiscountAllow] decimal(32,12)  NULL,
    [NewSubCode] nvarchar(max)  NULL,
    [HasOff] bit  NULL,
    [Emp_Id] int  NULL,
    [IsCanceled] bit  NULL,
    [Sub_Date] datetime  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'Subscription_Type'
CREATE TABLE [dbo].[Subscription_Type] (
    [Subscription_Type_ID] int IDENTITY(1,1) NOT NULL,
    [Subscrip_Code] nvarchar(max)  NOT NULL,
    [Subscrip_Name] nvarchar(max)  NOT NULL,
    [Subscrip_Description] nvarchar(max)  NULL,
    [Durationsubscription] int  NOT NULL,
    [Subscrip_BU] int  NULL,
    [Subscrip_Company] int  NOT NULL,
    [Rvenuo_ID] int  NOT NULL,
    [Bussines_Unit_Name] nvarchar(max)  NULL,
    [Account_Name] nvarchar(max)  NULL
);
GO

-- Creating table 'ProductBatchesTemps'
CREATE TABLE [dbo].[ProductBatchesTemps] (
    [Batch_ID] int IDENTITY(1,1) NOT NULL,
    [Product_ID] int  NULL,
    [Store_ID] int  NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Batch_Cost] decimal(32,12)  NULL
);
GO

-- Creating table 'TransferToprojectFile1'
CREATE TABLE [dbo].[TransferToprojectFile1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [TransferToProjectMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'TransferToProjectMasters'
CREATE TABLE [dbo].[TransferToProjectMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Trans_Code] nvarchar(max)  NOT NULL,
    [Describtion] nvarchar(max)  NULL,
    [Project_Id] int  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [Company_Id] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Trans_Date] datetime  NOT NULL,
    [Store_ID] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [HasTransfered] bit  NULL,
    [SubLedger_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [Department_ID] int  NULL
);
GO

-- Creating table 'TransferToProjectDetails'
CREATE TABLE [dbo].[TransferToProjectDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Product_ID] int  NOT NULL,
    [Master_ID] int  NOT NULL,
    [Reverse_quantity] decimal(32,12)  NULL,
    [Reverse_Total] decimal(32,12)  NULL
);
GO

-- Creating table 'TransferToProjectTemps'
CREATE TABLE [dbo].[TransferToProjectTemps] (
    [Master_Id] int  NOT NULL,
    [Trans_Code] nvarchar(max)  NULL,
    [Describtion] nvarchar(max)  NULL,
    [Project_Id] int  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Company_Id] int  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Store_Id] int  NULL,
    [Trans_Date] datetime  NULL,
    [Invoice_State] int  NULL
);
GO

-- Creating table 'TransferToProjectDetailTemps'
CREATE TABLE [dbo].[TransferToProjectDetailTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Product_Id] int  NOT NULL,
    [Master_ID] int  NOT NULL
);
GO

-- Creating table 'TransfareFromPorjectToStoretemps'
CREATE TABLE [dbo].[TransfareFromPorjectToStoretemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Detail_ID] int  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Product_Tax] decimal(32,12)  NULL,
    [Detail_total_tax] decimal(32,12)  NULL,
    [Batch_ID] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [Master_ID] int  NOT NULL,
    [State] int  NULL
);
GO

-- Creating table 'Vacations'
CREATE TABLE [dbo].[Vacations] (
    [Vacation_Id] int IDENTITY(1,1) NOT NULL,
    [Start_Date_Vacations] datetime  NOT NULL,
    [End_Date_Vacations] datetime  NOT NULL,
    [Employee_Id] int  NOT NULL,
    [Vacation_Type_Id] int  NOT NULL,
    [NumberOfDay] int  NOT NULL,
    [Status] bit  NULL,
    [Reason] nvarchar(max)  NULL,
    [MangerDescription] nvarchar(max)  NULL,
    [Attachment] varbinary(max)  NULL,
    [Manager_Id] int  NULL,
    [Attachname] nvarchar(max)  NULL
);
GO

-- Creating table 'Vacation_Types'
CREATE TABLE [dbo].[Vacation_Types] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Vacation_Type] nvarchar(max)  NOT NULL,
    [penalityPercentage] decimal(32,12)  NOT NULL,
    [PenalityPercentageAfter] decimal(32,12)  NOT NULL,
    [PenalityDay] decimal(18,0)  NOT NULL,
    [PenalityDayAfter] decimal(18,0)  NOT NULL,
    [TransferToNewYear] bit  NOT NULL
);
GO

-- Creating table 'Salary_Components'
CREATE TABLE [dbo].[Salary_Components] (
    [Salary_Components_Id] int IDENTITY(1,1) NOT NULL,
    [Account_Id] int  NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Amount] decimal(32,12)  NOT NULL,
    [Employee_Id] int  NOT NULL,
    [VacationId] int  NULL,
    [Default] bit  NULL
);
GO

-- Creating table 'Emp_Evaluation'
CREATE TABLE [dbo].[Emp_Evaluation] (
    [Evaluation_Id] int IDENTITY(1,1) NOT NULL,
    [Evaluation_Days] int  NULL,
    [Evaluation_Amount] decimal(32,12)  NULL,
    [Employee_Id] int  NOT NULL,
    [Evaluation_Date] nvarchar(max)  NULL,
    [Reference] nvarchar(max)  NULL,
    [Evaluation_State] int  NULL
);
GO

-- Creating table 'EmployeTemps'
CREATE TABLE [dbo].[EmployeTemps] (
    [EmpTemp_Id] int  NOT NULL,
    [Emp_Code] nvarchar(max)  NULL,
    [Emp_Name] nvarchar(max)  NULL,
    [Emp_Address] nvarchar(max)  NULL,
    [Emp_Mail] nvarchar(max)  NULL,
    [Emp_Phone] nvarchar(max)  NULL,
    [Emp_Title] nvarchar(max)  NULL,
    [Emp_DOB] nvarchar(max)  NULL,
    [BU_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Residency_number] nvarchar(max)  NULL,
    [Passport_number] nvarchar(max)  NULL,
    [Start_Date_Passp] datetime  NULL,
    [End_Date_Passp] datetime  NULL,
    [Start_Date_Residency] datetime  NULL,
    [End_Date_Residency] datetime  NULL,
    [Team_ID] int  NULL,
    [Team_Name] nvarchar(max)  NULL,
    [Fixed_Salary] decimal(32,12)  NULL,
    [Total_Salary] decimal(32,12)  NULL,
    [Vacations_Number] int  NULL,
    [Vacation_Used] int  NULL,
    [TotalVactionUsed] int  NULL
);
GO

-- Creating table 'Salary_ComponentsTemp'
CREATE TABLE [dbo].[Salary_ComponentsTemp] (
    [Salary_ComponentsTemp_Id] int IDENTITY(1,1) NOT NULL,
    [Account_Id] int  NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Amount] decimal(32,12)  NOT NULL,
    [EmployeeTemp_Id] int  NOT NULL
);
GO

-- Creating table 'ProjectPayments'
CREATE TABLE [dbo].[ProjectPayments] (
    [ProjectPaymentId] int IDENTITY(1,1) NOT NULL,
    [Expense_Id] int  NOT NULL,
    [Expense_Name] nvarchar(max)  NULL,
    [Pay_Amount] decimal(32,12)  NOT NULL,
    [ProjectPay_Code] nvarchar(max)  NOT NULL,
    [ProjectPay_State] int  NOT NULL,
    [Project_Id] int  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Revenue_Id] int  NOT NULL,
    [Revenue_Name] nvarchar(max)  NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [SubLedger_Id] int  NULL,
    [Tax_Id] int  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [HasOff] bit  NOT NULL,
    [Supplier_ID] int  NULL,
    [Emp_Id] int  NULL,
    [ProjectSub_Id] int  NULL,
    [ProjectSubSub_Id] int  NULL,
    [ProjectClause_Id] int  NULL
);
GO

-- Creating table 'ProjectInvoiceMasters'
CREATE TABLE [dbo].[ProjectInvoiceMasters] (
    [PInvoiceMaster_ID] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Project_Id] int  NOT NULL,
    [Project_Name] nvarchar(max)  NOT NULL,
    [Project_Description] nvarchar(max)  NULL,
    [Project_Code] nvarchar(max)  NOT NULL,
    [Invoice_Description] nvarchar(max)  NOT NULL,
    [Project_Location] nvarchar(max)  NULL,
    [DeadLine] datetime  NULL,
    [StartTime] datetime  NOT NULL,
    [CustomerCustomer_Id] int  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [Emp_Id] int  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Payment_Method] int  NOT NULL,
    [Payment_Method_Name] nvarchar(max)  NULL,
    [Revenu_Id] int  NULL,
    [Project_State] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [ProjectInv_Date] datetime  NULL,
    [MasterDiscount] decimal(32,12)  NULL,
    [FixedDiscount] decimal(32,12)  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'ProjectInvoiceDetails'
CREATE TABLE [dbo].[ProjectInvoiceDetails] (
    [Detail_ID] int IDENTITY(1,1) NOT NULL,
    [Product_Name] nvarchar(max)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Tax_ID] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [ProjectInvoiceMasterPInvoiceMaster_ID] int  NOT NULL
);
GO

-- Creating table 'ProjectInvoiceDetailTemps'
CREATE TABLE [dbo].[ProjectInvoiceDetailTemps] (
    [Product_Name] nvarchar(max)  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Tax_ID] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Detail_ID] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [ProjectInvoiceMasterTempId] int  NOT NULL,
    [MasterID] int  NULL,
    [fixeddiscount] decimal(32,12)  NULL
);
GO

-- Creating table 'ProjectInvoiceMasterTemps'
CREATE TABLE [dbo].[ProjectInvoiceMasterTemps] (
    [Id] int  NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Project_Id] int  NULL,
    [Project_Name] nvarchar(max)  NULL,
    [Project_Description] nvarchar(max)  NULL,
    [DeadLine] datetime  NULL,
    [Project_Code] nvarchar(max)  NULL,
    [Invoice_Description] nvarchar(max)  NULL,
    [Project_Location] nvarchar(max)  NULL,
    [StartTime] datetime  NULL,
    [Estimate_Amount] decimal(32,12)  NULL,
    [CustomerCustomer_Id] int  NULL,
    [Customer_Name] nvarchar(max)  NULL,
    [Payment_Method] int  NULL,
    [Emp_Id] int  NULL,
    [Payment_Method_Name] nvarchar(max)  NULL,
    [Revenu_Id] int  NULL,
    [Project_State] int  NULL,
    [master_ID] int  NULL
);
GO

-- Creating table 'SubscriptionInvoices'
CREATE TABLE [dbo].[SubscriptionInvoices] (
    [Inv_ID] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Subscrip_Code] nvarchar(max)  NOT NULL,
    [Subscrip_Description] nvarchar(max)  NULL,
    [DeadLine] datetime  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [Customer_Id] int  NULL,
    [Subscrip_BU] int  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Subscrip_Pranch] int  NULL,
    [Payment_Method] int  NOT NULL,
    [Pay_Amount] decimal(32,12)  NOT NULL,
    [InvoiceDate] datetime  NULL,
    [Transaction_State] int  NOT NULL,
    [Account_ID] int  NOT NULL,
    [Subledger_ID] int  NULL,
    [Department_Id] int  NULL,
    [Note] nvarchar(max)  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'Maintenance_Invoice_Detail'
CREATE TABLE [dbo].[Maintenance_Invoice_Detail] (
    [Main_Detail_Id] int IDENTITY(1,1) NOT NULL,
    [Subscription_Name] nvarchar(max)  NOT NULL,
    [Subscription_Cost] decimal(32,12)  NOT NULL,
    [Maintenance_Invoice_MasterMain_Invoice_Id] int  NOT NULL
);
GO

-- Creating table 'Maintenance_Invoice_Master'
CREATE TABLE [dbo].[Maintenance_Invoice_Master] (
    [Main_Invoice_Id] int IDENTITY(1,1) NOT NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Starting_Date] datetime  NOT NULL,
    [Ending_Date] datetime  NOT NULL,
    [Subscription_Code] nvarchar(max)  NOT NULL,
    [Main_Invoice_Code] nvarchar(max)  NOT NULL,
    [Describtion] nvarchar(max)  NULL
);
GO

-- Creating table 'Maintenance_Invoice_Master_Temp'
CREATE TABLE [dbo].[Maintenance_Invoice_Master_Temp] (
    [Main_Invoice_Id] int IDENTITY(1,1) NOT NULL,
    [Customer_Name] nvarchar(max)  NULL,
    [Starting_Date] datetime  NULL,
    [Ending_Date] datetime  NULL,
    [Subscription_Code] nvarchar(max)  NULL,
    [Describtion] nvarchar(max)  NULL,
    [Main_Invoice_Code] nvarchar(max)  NULL
);
GO

-- Creating table 'Maintenance_Invoice_Detail_Temp'
CREATE TABLE [dbo].[Maintenance_Invoice_Detail_Temp] (
    [Main_Detail_Id] int IDENTITY(1,1) NOT NULL,
    [Subscription_Name] nvarchar(max)  NOT NULL,
    [Subscription_Cost] decimal(32,12)  NOT NULL,
    [Maintenance_Invoice_Master_TempMain_Invoice_Id] int  NOT NULL
);
GO

-- Creating table 'ReverseSalesReceiptMasterTemp1'
CREATE TABLE [dbo].[ReverseSalesReceiptMasterTemp1] (
    [Master_Id] int  NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Invoice_State] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [PaymentMethod_Id] int  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [SalesRecipt_Id] int  NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NULL
);
GO

-- Creating table 'ReverseSalesReceiptDetailsTemps'
CREATE TABLE [dbo].[ReverseSalesReceiptDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [SalesReceiptMasterTempMaster_Id] int  NOT NULL,
    [Commition_Id] int  NULL,
    [Commition] decimal(18,2)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount1] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'ReverseSalesReceiptDetails'
CREATE TABLE [dbo].[ReverseSalesReceiptDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ReverseSalesReceiptMasterMaster_Id] int  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'ReverseSalesReceiptFiles'
CREATE TABLE [dbo].[ReverseSalesReceiptFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [FileContent] varbinary(max)  NOT NULL,
    [FileType] nvarchar(max)  NOT NULL,
    [ReverseSalesReceiptMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'ReverseSalesReceiptMasters'
CREATE TABLE [dbo].[ReverseSalesReceiptMasters] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NOT NULL,
    [Company_Id] int  NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [SalesRecipt_Id] int  NULL,
    [Total_Payment_Method_Cost] decimal(18,2)  NOT NULL,
    [Department_ID] int  NULL,
    [SubLedger_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [HasOff] bit  NOT NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'Accounts'
CREATE TABLE [dbo].[Accounts] (
    [Accounts_Id] int IDENTITY(1,1) NOT NULL,
    [Account_Code] nvarchar(max)  NOT NULL,
    [Account_Name] nvarchar(max)  NOT NULL,
    [Account_Date] datetime  NOT NULL,
    [Account_Id] nvarchar(max)  NOT NULL,
    [Account_Type] nvarchar(max)  NOT NULL,
    [Account_Desc] nvarchar(max)  NULL,
    [Account_Balance] decimal(32,12)  NULL,
    [OpenBalance_Code] nvarchar(max)  NULL
);
GO

-- Creating table 'SlicesProducts'
CREATE TABLE [dbo].[SlicesProducts] (
    [SlicesProduct_Id] int IDENTITY(1,1) NOT NULL,
    [Slice_Price] decimal(32,12)  NOT NULL,
    [Product_ID] int  NOT NULL,
    [SlicesSlice_Id] int  NOT NULL
);
GO

-- Creating table 'SlicesTems'
CREATE TABLE [dbo].[SlicesTems] (
    [Slice_Id] int IDENTITY(1,1) NOT NULL,
    [ProductTemProd_ID] int  NOT NULL,
    [Slice_Price] decimal(32,12)  NOT NULL,
    [SlicesSlice_Id] int  NOT NULL,
    [Slice_Name] nvarchar(max)  NULL
);
GO

-- Creating table 'ProductTems'
CREATE TABLE [dbo].[ProductTems] (
    [Prod_ID] int  NOT NULL,
    [Prod_Name] nvarchar(max)  NULL,
    [Prod_Code] nvarchar(max)  NULL,
    [Prod_Descrip] nvarchar(max)  NULL,
    [Category_ID] int  NULL,
    [MeasuringUnit_ID] int  NULL,
    [Prod_Mark] nvarchar(max)  NULL,
    [Sale_Price] decimal(18,2)  NULL,
    [Demand_Limit] int  NULL,
    [Store_Id] int  NULL,
    [Prod_Stored] bit  NULL,
    [Bar_Code] nvarchar(max)  NULL,
    [Tax_Id] int  NULL,
    [Prod_Cost_Price] decimal(32,12)  NULL,
    [Prod_Type] int  NULL,
    [OpenBalance_Code] nvarchar(max)  NULL,
    [Prod_Category] nvarchar(max)  NULL,
    [Prod_Unet] nvarchar(max)  NULL
);
GO

-- Creating table 'Slices'
CREATE TABLE [dbo].[Slices] (
    [Slice_Id] int IDENTITY(1,1) NOT NULL,
    [Slice_Name] nvarchar(max)  NOT NULL,
    [Slice_Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubLedgerFileDetail1'
CREATE TABLE [dbo].[SubLedgerFileDetail1] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL,
    [SubLedgerSubLed_Id] int  NOT NULL
);
GO

-- Creating table 'SubLedgers'
CREATE TABLE [dbo].[SubLedgers] (
    [SubLed_Id] int IDENTITY(1,1) NOT NULL,
    [SubLed_Code] nvarchar(max)  NOT NULL,
    [SubLed_Name] nvarchar(max)  NOT NULL,
    [SubLed_Date] datetime  NOT NULL,
    [SubLed_Type] nvarchar(max)  NOT NULL,
    [SubLed_Desc] nvarchar(max)  NULL,
    [SubLedger_Balance] decimal(32,12)  NULL,
    [OpenBalance_Code] nvarchar(max)  NULL,
    [Subled_Level] int  NOT NULL,
    [Account_Id] int  NULL,
    [Subledger_Expense] decimal(32,12)  NULL,
    [Subledger_Revenu] decimal(32,12)  NULL,
    [SubLedgerSubLed_Id] int  NULL,
    [isActive] bit  NULL,
    [InvoiceDesc] nvarchar(max)  NULL,
    [InvoiceNotes] nvarchar(max)  NULL,
    [OfferNotes] nvarchar(max)  NULL,
    [OfferDesc] nvarchar(max)  NULL
);
GO

-- Creating table 'DepartmentBussinesUnits'
CREATE TABLE [dbo].[DepartmentBussinesUnits] (
    [BussinessUnit_Id] int IDENTITY(1,1) NOT NULL,
    [BussinesUnit_Name] nvarchar(max)  NOT NULL,
    [BussinesUnit_Description] nvarchar(max)  NULL,
    [BranchDepartmentDept_Id] int  NOT NULL,
    [IsActive] bit  NULL,
    [DateOfDisActive] datetime  NULL
);
GO

-- Creating table 'TransferToMaintenanceDetailsTemps'
CREATE TABLE [dbo].[TransferToMaintenanceDetailsTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Product_Id] int  NOT NULL,
    [TransferToMaintenanceTempMaster_Id] int  NOT NULL
);
GO

-- Creating table 'TransferToMaintenanceTemps'
CREATE TABLE [dbo].[TransferToMaintenanceTemps] (
    [Master_Id] int  NOT NULL,
    [Trans_Code] nvarchar(max)  NULL,
    [Describtion] nvarchar(max)  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Company_Id] int  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Store_Id] int  NULL,
    [Trans_Date] datetime  NULL,
    [Invoice_State] int  NULL,
    [Sup_ID] int  NULL
);
GO

-- Creating table 'TransferToMaintenanceDetails'
CREATE TABLE [dbo].[TransferToMaintenanceDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NULL,
    [Detail_Total_Tax] decimal(32,12)  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [TransferToMaintenanceMaster_Id] int  NOT NULL,
    [Product_Id] int  NOT NULL
);
GO

-- Creating table 'TransferToMaintenances'
CREATE TABLE [dbo].[TransferToMaintenances] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Trans_Code] nvarchar(max)  NOT NULL,
    [Describtion] nvarchar(max)  NULL,
    [Invoice_State] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [Company_Id] int  NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Trans_Date] datetime  NOT NULL,
    [Department_ID] int  NULL,
    [Subledger_ID] int  NULL,
    [Transfer_BU] int  NULL,
    [Store_ID] int  NOT NULL,
    [HasOff] bit  NOT NULL
);
GO

-- Creating table 'AssetDepDates'
CREATE TABLE [dbo].[AssetDepDates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Asset_ID] int  NULL,
    [Date] datetime  NULL,
    [InstallMent_Amount] decimal(18,0)  NULL,
    [Percentage] float  NULL
);
GO

-- Creating table 'CompanyFileDetails'
CREATE TABLE [dbo].[CompanyFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL,
    [CompanyComp_Id] int  NOT NULL
);
GO

-- Creating table 'CompanyPhotoDetails'
CREATE TABLE [dbo].[CompanyPhotoDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL,
    [CompanyComp_Id] int  NOT NULL
);
GO

-- Creating table 'CatchReceipts'
CREATE TABLE [dbo].[CatchReceipts] (
    [CatchReceipt_Id] int IDENTITY(1,1) NOT NULL,
    [AccountCredit_Id] int  NOT NULL,
    [AccountDebit_Id] int  NOT NULL,
    [BU_Id] int  NULL,
    [CatchReceipt_Value] decimal(32,12)  NOT NULL,
    [CatchReceipt_Description] nvarchar(max)  NULL,
    [CatchReceipt_date] datetime  NOT NULL,
    [Company_Id] int  NULL,
    [Currency_Id] int  NOT NULL,
    [CatchReceipt_State] int  NOT NULL,
    [Subledger_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [CatchReceipt_Code] nvarchar(max)  NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [Child_AccountCredit_Id] int  NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL
);
GO

-- Creating table 'CatchReceiptFileDetails'
CREATE TABLE [dbo].[CatchReceiptFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL,
    [CatchReceipt_Id] int  NOT NULL
);
GO

-- Creating table 'ContentPersonDetailsTemps'
CREATE TABLE [dbo].[ContentPersonDetailsTemps] (
    [ContentPerson_ID] int IDENTITY(1,1) NOT NULL,
    [Content_Name] nvarchar(max)  NULL,
    [Content_Number] nvarchar(max)  NULL,
    [Content_Mail] nvarchar(max)  NULL,
    [Content_Job] nvarchar(max)  NULL,
    [CustomerTemp_Id] int  NOT NULL
);
GO

-- Creating table 'ContentPersonDetails'
CREATE TABLE [dbo].[ContentPersonDetails] (
    [ContentPerson_ID] int IDENTITY(1,1) NOT NULL,
    [Content_Name] nvarchar(max)  NULL,
    [Content_Number] nvarchar(max)  NULL,
    [Content_Mail] nvarchar(max)  NULL,
    [Content_Job] nvarchar(max)  NULL,
    [CustomerCustomer_Id] int  NOT NULL
);
GO

-- Creating table 'CustomerTemps'
CREATE TABLE [dbo].[CustomerTemps] (
    [Customer_Id] int  NOT NULL,
    [Customer_Name] nvarchar(max)  NULL,
    [Customer_Address] nvarchar(max)  NULL,
    [Customer_Phone] nvarchar(max)  NULL,
    [Customer_Mail] nvarchar(max)  NULL,
    [Customer_Province] nvarchar(max)  NULL,
    [Customer_Country] nvarchar(max)  NULL,
    [Customer_City] nvarchar(max)  NULL,
    [Customer_Fax] nvarchar(max)  NULL,
    [Customer_Code] nvarchar(max)  NULL,
    [Branch_Id] int  NULL,
    [Customer_Mobile] nvarchar(max)  NULL,
    [Purchase_Price] decimal(18,0)  NULL,
    [Payment_Policy_Id] int  NULL,
    [Customer_Website] nvarchar(max)  NULL,
    [SlicesSlice_Id] int  NULL,
    [Customer_Balance] decimal(32,12)  NULL,
    [Customer_State] int  NULL,
    [Customer_Bal] bit  NULL,
    [Customer_Type] int  NULL
);
GO

-- Creating table 'SpentRecipts'
CREATE TABLE [dbo].[SpentRecipts] (
    [SpentReceipt_Id] int IDENTITY(1,1) NOT NULL,
    [AccountCredit_Id] int  NOT NULL,
    [AccountDebit_Id] int  NOT NULL,
    [BU_Id] int  NULL,
    [SpentReceipt_Value] decimal(32,12)  NOT NULL,
    [SpentReceipt_Description] nvarchar(max)  NULL,
    [CatchReceipt_date] datetime  NOT NULL,
    [Company_Id] int  NULL,
    [Currency_Id] int  NOT NULL,
    [SpentReceipt_State] int  NOT NULL,
    [Subledger_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [SpentReceipt_Code] nvarchar(max)  NULL,
    [BranchSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [Child_AccountDebit_Id] int  NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL,
    [SpentReciptOrderId] int  NULL
);
GO

-- Creating table 'SpentReciptFileDetails'
CREATE TABLE [dbo].[SpentReciptFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Extention] nvarchar(max)  NOT NULL,
    [CatchReceipt_Id] int  NOT NULL,
    [SpentReciptSpentReceipt_Id] int  NOT NULL
);
GO

-- Creating table 'Sales_Settings'
CREATE TABLE [dbo].[Sales_Settings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Employee_ID] int  NOT NULL,
    [SalesMan] bit  NOT NULL,
    [Collector] bit  NOT NULL,
    [Cant_Controle] bit  NOT NULL,
    [Can_Increase_Price] bit  NOT NULL,
    [Can_Decrease_Price] bit  NOT NULL,
    [Can_Sell_InLow] bit  NOT NULL
);
GO

-- Creating table 'Sales_Setting_Slice'
CREATE TABLE [dbo].[Sales_Setting_Slice] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sales_SettingsId] int  NOT NULL,
    [Slice_Name] nvarchar(max)  NULL,
    [Slice_ID] int  NULL
);
GO

-- Creating table 'Sales_Setting_SliceTemp'
CREATE TABLE [dbo].[Sales_Setting_SliceTemp] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Sales_SettingsId] int  NOT NULL,
    [Slice_Name] nvarchar(max)  NULL,
    [Slice_ID] int  NULL
);
GO

-- Creating table 'EmployeeSalaryMonths'
CREATE TABLE [dbo].[EmployeeSalaryMonths] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Emp_Code] nvarchar(max)  NOT NULL,
    [Emp_Name] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [TotalSalary] decimal(32,12)  NOT NULL,
    [AcountSal] int  NULL,
    [Status] int  NOT NULL,
    [Trims_Pay] decimal(18,2)  NULL,
    [Date_Take_Sal] datetime  NULL,
    [HourValueForEmployee] decimal(32,12)  NULL
);
GO

-- Creating table 'Employee_Sales'
CREATE TABLE [dbo].[Employee_Sales] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Employee_ID] int  NULL,
    [Net_Sales] decimal(32,12)  NULL,
    [Collected] decimal(32,12)  NULL,
    [Target_Sales] decimal(32,12)  NULL,
    [Target_Collected] decimal(32,12)  NULL
);
GO

-- Creating table 'EmpSal_Detail'
CREATE TABLE [dbo].[EmpSal_Detail] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Account_Id] int  NOT NULL,
    [AcountValue] decimal(32,12)  NOT NULL,
    [EmployeeSalaryMonthId] int  NOT NULL,
    [VacationId] int  NULL
);
GO

-- Creating table 'FixedAssetFileDetailInfoes'
CREATE TABLE [dbo].[FixedAssetFileDetailInfoes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL,
    [FixedAssetInformationId1] int  NOT NULL
);
GO

-- Creating table 'FixedAssetInformations'
CREATE TABLE [dbo].[FixedAssetInformations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [FAsset_Code] nvarchar(max)  NOT NULL,
    [CompanyComp_Id] int  NOT NULL,
    [SubLedger_ID] int  NOT NULL,
    [FAsset_Description] nvarchar(max)  NOT NULL,
    [FixedAssetTypeFAsset_TypeID] int  NOT NULL,
    [FixedAssetSecCategoryFAsset_CatID] int  NULL,
    [FAsset_AgeMonth] int  NOT NULL,
    [FAsset_WorkingDate] datetime  NOT NULL,
    [FAssetCat_name] nvarchar(max)  NULL,
    [FAsset_Type] nvarchar(max)  NULL,
    [JunckValue] float  NULL,
    [FAsset_Employee_ID] int  NULL,
    [IsActive] bit  NULL
);
GO

-- Creating table 'TypeOfDetailSalaries'
CREATE TABLE [dbo].[TypeOfDetailSalaries] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SalaryDetail_Name] nvarchar(max)  NOT NULL,
    [Sal_Type] int  NOT NULL,
    [ComeFromVacation] bit  NULL
);
GO

-- Creating table 'Clients'
CREATE TABLE [dbo].[Clients] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [User_Id] nvarchar(max)  NOT NULL,
    [Start_sub] datetime  NULL,
    [End_sub] datetime  NULL,
    [status] int  NOT NULL,
    [E_Mail] nvarchar(max)  NOT NULL,
    [PackageId] int  NULL
);
GO

-- Creating table 'Charts'
CREATE TABLE [dbo].[Charts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [revenu] decimal(32,12)  NOT NULL,
    [Expences] decimal(32,12)  NOT NULL,
    [Purchase] decimal(32,12)  NOT NULL,
    [CostOfSales] decimal(32,12)  NOT NULL,
    [ProfitAndLoss] decimal(32,12)  NOT NULL,
    [Supplier] decimal(32,12)  NOT NULL,
    [Customer] decimal(32,12)  NOT NULL,
    [Treasury] decimal(32,12)  NOT NULL,
    [Banks] decimal(32,12)  NOT NULL,
    [date] int  NOT NULL
);
GO

-- Creating table 'SettlementOfStores'
CREATE TABLE [dbo].[SettlementOfStores] (
    [SettlementId] int IDENTITY(1,1) NOT NULL,
    [StoreId] int  NOT NULL,
    [Settlement_Code] nvarchar(max)  NOT NULL,
    [Total_Cost_Credit] decimal(32,12)  NOT NULL,
    [Settlement_State] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [Total_Cost_Debit] decimal(32,12)  NOT NULL,
    [Date] datetime  NULL,
    [BranchSubledger_Id] int  NULL,
    [Subledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [Expense_Id] int  NULL
);
GO

-- Creating table 'SettlementOfStoresDetails'
CREATE TABLE [dbo].[SettlementOfStoresDetails] (
    [Settlement_DetailsId] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NOT NULL,
    [SettlementOfStoresId] int  NOT NULL,
    [Total_Cost_Detail] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Real_Quantity] decimal(32,12)  NOT NULL
);
GO

-- Creating table 'SettlementOfStoresTemps'
CREATE TABLE [dbo].[SettlementOfStoresTemps] (
    [SettlementId] int  NOT NULL,
    [StoreId] int  NULL,
    [Settlement_Code] nvarchar(max)  NULL,
    [Total_Cost] decimal(32,12)  NULL
);
GO

-- Creating table 'SettlementOfStoresDetailsTemps'
CREATE TABLE [dbo].[SettlementOfStoresDetailsTemps] (
    [Settlement_DetailsId] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NOT NULL,
    [Total_Cost_Detail] decimal(32,12)  NOT NULL,
    [SettlementOfStoresTempId] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Real_Quantity] decimal(32,12)  NOT NULL
);
GO

-- Creating table 'ProjectRecipts'
CREATE TABLE [dbo].[ProjectRecipts] (
    [ProjectRect_ID] int IDENTITY(1,1) NOT NULL,
    [ProjectCode] nvarchar(max)  NOT NULL,
    [ReciptCode] nvarchar(max)  NOT NULL,
    [Recipt_Date] datetime  NOT NULL,
    [Recipt_Description] nvarchar(max)  NOT NULL,
    [Project_start] datetime  NOT NULL,
    [Project_End] datetime  NULL,
    [Project_Amount] decimal(32,12)  NOT NULL,
    [Pay_Amount] decimal(32,12)  NOT NULL,
    [Customer_ID] int  NOT NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Payment_Method] int  NOT NULL,
    [transActionState] int  NOT NULL,
    [Proj_inv_ID] int  NOT NULL,
    [Emp_Id] int  NULL,
    [HasOff] bit  NULL
);
GO

-- Creating table 'POSDetailPayments'
CREATE TABLE [dbo].[POSDetailPayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [PaymentMethod_Name] nvarchar(max)  NOT NULL,
    [PointOfSalePOS_Id] int  NOT NULL
);
GO

-- Creating table 'PointOfSales'
CREATE TABLE [dbo].[PointOfSales] (
    [POS_Id] int IDENTITY(1,1) NOT NULL,
    [POS_Name] nvarchar(max)  NOT NULL,
    [Company_Id] int  NOT NULL,
    [Currency_Id] int  NOT NULL,
    [Adress] nvarchar(max)  NOT NULL,
    [Store_Id] int  NOT NULL,
    [Subledger_Id] int  NOT NULL,
    [Employee_Id] int  NOT NULL,
    [DepartSubledger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [BranchSubledger_Id] int  NULL
);
GO

-- Creating table 'MasterPOSInvoices'
CREATE TABLE [dbo].[MasterPOSInvoices] (
    [MasterPOSInvoiceId] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Invoice_State] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [Account_Id] int  NULL,
    [Subledger_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NULL,
    [PointOfSalePOS_Id] int  NOT NULL,
    [Employee_Id] int  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [HasOff] bit  NOT NULL,
    [TotalNewTaxs] decimal(32,12)  NULL,
    [MainrenanceOrder_Id] int  NULL,
    [QRNumber] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'DetailPOSInvoices'
CREATE TABLE [dbo].[DetailPOSInvoices] (
    [Detail_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Reverse_Total_Amount] decimal(32,12)  NOT NULL,
    [MasterPOSInvoiceId] int  NOT NULL,
    [ProductPriceCost] decimal(32,12)  NOT NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL,
    [Detail_Discount] decimal(32,12)  NULL
);
GO

-- Creating table 'PosPaymentMethods'
CREATE TABLE [dbo].[PosPaymentMethods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MasterPOSInvoiceId] int  NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [Commution] decimal(32,12)  NOT NULL
);
GO

-- Creating table 'ReversePOSInvoices'
CREATE TABLE [dbo].[ReversePOSInvoices] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NOT NULL,
    [Company_Id] int  NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(18,2)  NOT NULL,
    [SubLedger_ID] int  NULL,
    [HasOff] bit  NOT NULL,
    [MasterPOSInvoiceMasterPOSInvoiceId] int  NOT NULL,
    [POS_ID] int  NOT NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'ReversePOSDetails'
CREATE TABLE [dbo].[ReversePOSDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ReversePOSInvoiceMaster_Id] int  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'POSDetailPaymentTemps'
CREATE TABLE [dbo].[POSDetailPaymentTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PaymentMethod_Id] int  NULL,
    [PaymentMethod_Name] nvarchar(max)  NULL,
    [PointOfSaleTempPOS_Id] int  NOT NULL
);
GO

-- Creating table 'PointOfSaleTemps'
CREATE TABLE [dbo].[PointOfSaleTemps] (
    [POS_Id] int  NOT NULL,
    [POS_Name] nvarchar(max)  NULL,
    [Company_Id] int  NULL,
    [Currency_Id] int  NULL,
    [Adress] nvarchar(max)  NULL,
    [Store_Id] int  NULL,
    [Subledger_Id] int  NULL,
    [Employee_Id] int  NULL
);
GO

-- Creating table 'MasterPOSInvoiceTemps'
CREATE TABLE [dbo].[MasterPOSInvoiceTemps] (
    [MasterPOSInvoiceId] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Invoice_State] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [Account_Id] int  NULL,
    [Branch_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NULL,
    [PointOfSalePOS_Id] int  NOT NULL,
    [Employee_Id] int  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [TotalNewTaxs] decimal(32,12)  NULL
);
GO

-- Creating table 'PosPaymentMethodTemps'
CREATE TABLE [dbo].[PosPaymentMethodTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Commution] decimal(32,12)  NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [MasterPOSInvoiceTempMasterPOSInvoiceId] int  NOT NULL
);
GO

-- Creating table 'DetailPOSInvoiceTemps'
CREATE TABLE [dbo].[DetailPOSInvoiceTemps] (
    [Detail_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Reverse_Total_Amount] decimal(32,12)  NOT NULL,
    [MasterPOSInvoiceTempMasterPOSInvoiceId] int  NOT NULL,
    [TotalBeforeDisc] decimal(32,10)  NULL,
    [TotalDiscount] decimal(32,10)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL,
    [Detail_Discount] decimal(32,12)  NULL
);
GO

-- Creating table 'ReversePOSInvoiceTemps'
CREATE TABLE [dbo].[ReversePOSInvoiceTemps] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [Invoice_State] int  NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [PaymentMethod_Id] int  NULL,
    [Account_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(18,2)  NULL,
    [SubLedger_ID] int  NULL,
    [HasOff] bit  NULL,
    [POS_ID] int  NULL
);
GO

-- Creating table 'ReversePOSDetailTemps'
CREATE TABLE [dbo].[ReversePOSDetailTemps] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ReversePOSInvoiceTempMaster_Id] int  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'SubscriptionItemsTemps'
CREATE TABLE [dbo].[SubscriptionItemsTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SubTempID] int  NOT NULL,
    [Item_Name] nvarchar(max)  NOT NULL,
    [Item_Quantity] decimal(32,12)  NOT NULL,
    [Item_price] decimal(32,12)  NOT NULL
);
GO

-- Creating table 'SubscriptionTemps'
CREATE TABLE [dbo].[SubscriptionTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subscrip_Id] int  NOT NULL,
    [Subscrip_Code] nvarchar(max)  NOT NULL,
    [Subscrip_Description] nvarchar(max)  NULL,
    [DeadLine] datetime  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [Customer_Id] int  NULL,
    [Subscrip_Comp] int  NOT NULL,
    [Subscrip_BU] int  NULL,
    [Subscrip_State] int  NOT NULL,
    [Subscrip_Pranch] int  NULL,
    [Subscrip_Type] int  NOT NULL,
    [Emp_Team] int  NULL,
    [SubType_State] int  NOT NULL,
    [Payment_Method] int  NULL,
    [tel_customer] nvarchar(max)  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Subscrip_DateEnd] datetime  NULL,
    [Subscrip_Stop] bit  NOT NULL,
    [Sub_Type_ID] int  NOT NULL,
    [Check_Number] nvarchar(max)  NULL,
    [Sub_Pay_State] int  NULL,
    [Tax_ID] int  NOT NULL,
    [AmountAfterTax] decimal(32,12)  NOT NULL,
    [Subledger_ID] int  NULL,
    [Payed_Amount] decimal(32,12)  NULL,
    [Department_Id] int  NULL,
    [Discount] decimal(32,12)  NULL,
    [DiscountAllow] decimal(32,12)  NULL,
    [Emp_Id] int  NULL,
    [IsCanceled] bit  NULL,
    [Sub_Date] datetime  NULL
);
GO

-- Creating table 'ExpenseAccountTemps'
CREATE TABLE [dbo].[ExpenseAccountTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Expense_ID] int  NOT NULL,
    [Tax_Bool] bit  NOT NULL,
    [AccountExp_ID] int  NULL,
    [AccountTreasury_ID] int  NULL,
    [SubLedger_ID] int  NULL,
    [ExpDesc] nvarchar(max)  NOT NULL,
    [AccountExp_Debit] decimal(32,12)  NOT NULL,
    [AccountTreat_Credit] decimal(32,12)  NOT NULL,
    [RandoumVAlue] int  NULL,
    [ChildAccount_Id] int  NULL
);
GO

-- Creating table 'ProjectDetails'
CREATE TABLE [dbo].[ProjectDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProjectProject_Id] int  NOT NULL,
    [Description_Detail] nvarchar(max)  NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [Date_Amount] datetime  NULL,
    [Payed] bit  NOT NULL
);
GO

-- Creating table 'ProjectTemps'
CREATE TABLE [dbo].[ProjectTemps] (
    [Project_Id] int  NOT NULL,
    [Project_Code] nvarchar(max)  NULL,
    [Project_Name] nvarchar(max)  NULL,
    [Project_Description] nvarchar(max)  NULL,
    [Project_Location] nvarchar(max)  NULL,
    [DeadLine] datetime  NULL,
    [StartTime] datetime  NULL,
    [Estimate_Amount] decimal(32,12)  NULL,
    [CustomerCustomer_Id] int  NULL,
    [Emp_Id] int  NULL,
    [Payment_Method] int  NULL,
    [Revenu_Id] int  NULL,
    [Project_State] int  NULL,
    [SubLedger_Id] int  NULL,
    [BussinesSubledger_Id] int  NULL,
    [DepartSubledger_Id] int  NULL,
    [BranchSubledger_Id] int  NULL,
    [Company_ID] nvarchar(max)  NULL,
    [Emp_Team_Id] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [DetailOFPro] nvarchar(max)  NULL
);
GO

-- Creating table 'ProjectDetailTemps'
CREATE TABLE [dbo].[ProjectDetailTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Description_Detail] nvarchar(max)  NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [Date_Amount] datetime  NULL,
    [Payed] bit  NOT NULL,
    [ProjectTempProject_Id] int  NOT NULL
);
GO

-- Creating table 'StoreCards'
CREATE TABLE [dbo].[StoreCards] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Store_Id] int  NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [CostPrice] decimal(32,12)  NOT NULL,
    [SalePrice] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [IncreaseQuantity] bit  NOT NULL,
    [TypeOfDocument] nvarchar(max)  NOT NULL,
    [Document_Serial] nvarchar(max)  NOT NULL,
    [TotalCost] decimal(32,12)  NULL,
    [TotalAmount] decimal(32,12)  NULL,
    [Account_Id] int  NULL,
    [Subledger_Id] int  NULL
);
GO

-- Creating table 'LockCodes'
CREATE TABLE [dbo].[LockCodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProjectPaymentFiles'
CREATE TABLE [dbo].[ProjectPaymentFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ProjectPaymentId] int  NOT NULL
);
GO

-- Creating table 'ProjectReceiptFiles'
CREATE TABLE [dbo].[ProjectReceiptFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ProjectRecipt_ID] int  NOT NULL
);
GO

-- Creating table 'SubscriptionInvoiceFiles'
CREATE TABLE [dbo].[SubscriptionInvoiceFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [SubscriptionInvoice_ID] int  NOT NULL
);
GO

-- Creating table 'GLMasterFiles'
CREATE TABLE [dbo].[GLMasterFiles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [GLMaster_Id] int  NOT NULL
);
GO

-- Creating table 'Packages'
CREATE TABLE [dbo].[Packages] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number_Branch] int  NOT NULL,
    [Number_POS] int  NOT NULL,
    [Number_User] int  NOT NULL,
    [Subscribtion] bit  NOT NULL,
    [Asset] bit  NOT NULL,
    [Package_Name] nvarchar(max)  NOT NULL,
    [Package_duration] int  NOT NULL,
    [Project] bit  NOT NULL,
    [OffGL] bit  NOT NULL,
    [Number_store] int  NOT NULL,
    [Number_subLedgger] int  NOT NULL,
    [HR] bit  NOT NULL,
    [Expenses] bit  NULL,
    [Number_Employee] int  NOT NULL,
    [ExtraTaxes] bit  NULL,
    [CompoundProduct] bit  NULL,
    [Accounting] bit  NULL,
    [Suppliers] bit  NULL,
    [ReportsAndPending] bit  NULL,
    [ChartOfAccount] bit  NULL,
    [Maintenance] bit  NULL,
    [Purchase] bit  NULL,
    [Casher] bit  NULL,
    [Sales] bit  NULL,
    [Commission] bit  NULL,
    [NumberOfInvoice] int  NULL,
    [StartOfInvoice] nvarchar(max)  NULL,
    [StartOfInReverse] nvarchar(max)  NULL,
    [NumberOfReverse] int  NULL,
    [Attendingandleaving] bit  NULL,
    [PrintSpecial] bit  NULL,
    [Sewing] bit  NULL,
    [SewingArab] nvarchar(max)  NULL,
    [SewingEng] nvarchar(max)  NULL
);
GO

-- Creating table 'OperationRuns'
CREATE TABLE [dbo].[OperationRuns] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'EmpVacations'
CREATE TABLE [dbo].[EmpVacations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Vacation_TypesId] int  NOT NULL,
    [EmployeeEmp_Id] int  NOT NULL,
    [DaysInyear] int  NOT NULL,
    [TotalAfterAbsent] int  NOT NULL
);
GO

-- Creating table 'EmpVacationTemps'
CREATE TABLE [dbo].[EmpVacationTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Vacation_TypesId] int  NOT NULL,
    [DaysInyear] int  NOT NULL,
    [TotalDay] int  NULL,
    [TotalAfterAbsent] int  NULL,
    [EmpTemp_Id] int  NOT NULL,
    [VacationTypeName] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'VacationEmgrations'
CREATE TABLE [dbo].[VacationEmgrations] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Employee_Id] int  NOT NULL,
    [Vacation_Type_id] int  NOT NULL,
    [DateOfTransaction] datetime  NOT NULL,
    [NumberOfDay] int  NOT NULL
);
GO

-- Creating table 'ChartDates'
CREATE TABLE [dbo].[ChartDates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL
);
GO

-- Creating table 'InvoiceTaxes'
CREATE TABLE [dbo].[InvoiceTaxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tax_Id] nvarchar(max)  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [InvoiceMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'SalesReceiptTaxes'
CREATE TABLE [dbo].[SalesReceiptTaxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tax_Id] nvarchar(max)  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [SalesReceiptMasterMaster_Id] int  NOT NULL
);
GO

-- Creating table 'TaxPOSInvoices'
CREATE TABLE [dbo].[TaxPOSInvoices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tax_Id] nvarchar(max)  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [MasterPOSInvoiceMasterPOSInvoiceId] int  NOT NULL
);
GO

-- Creating table 'OfferTaxes'
CREATE TABLE [dbo].[OfferTaxes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tax_Id] nvarchar(max)  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [OfferPriceMasterOfferMas_Id] int  NOT NULL
);
GO

-- Creating table 'ProductFileDetails'
CREATE TABLE [dbo].[ProductFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ProductProd_ID] int  NOT NULL,
    [File_Type] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'UserConnections'
CREATE TABLE [dbo].[UserConnections] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Email] nvarchar(max)  NOT NULL,
    [Connect] bit  NOT NULL,
    [DateLogin] datetime  NOT NULL,
    [DateLogout] datetime  NULL,
    [SessionId] nvarchar(max)  NULL,
    [Expires] datetime  NULL,
    [Token] nvarchar(max)  NULL
);
GO

-- Creating table 'PrintBarcodes'
CREATE TABLE [dbo].[PrintBarcodes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PriceWithTax] bit  NULL,
    [Tax] bit  NULL,
    [DateOfExpire] bit  NULL,
    [Price] bit  NULL,
    [Notes] bit  NULL,
    [Name] bit  NULL
);
GO

-- Creating table 'ProductCompounds'
CREATE TABLE [dbo].[ProductCompounds] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Master_Product_Id] int  NOT NULL
);
GO

-- Creating table 'ProductCompoundTemps'
CREATE TABLE [dbo].[ProductCompoundTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [Master_Product_Id] int  NOT NULL
);
GO

-- Creating table 'MaintenanceOrderOperationals'
CREATE TABLE [dbo].[MaintenanceOrderOperationals] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Customer_Id] int  NOT NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [Model] nvarchar(max)  NOT NULL,
    [Describtion] nvarchar(max)  NULL,
    [Status] int  NOT NULL,
    [guaranteeDuration] int  NULL,
    [Order_Code] nvarchar(max)  NOT NULL,
    [RequiredDescribtion] nvarchar(max)  NOT NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [Emplyee_id] int  NOT NULL,
    [Checked] bit  NOT NULL,
    [Date] datetime  NOT NULL,
    [EndGuaranteeDate] datetime  NULL,
    [ExpectedAmount] decimal(32,12)  NULL
);
GO

-- Creating table 'MaintenanceOrderOperationalDetails'
CREATE TABLE [dbo].[MaintenanceOrderOperationalDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ProductPriceCost] decimal(32,12)  NOT NULL,
    [MaintenanceOrderOperationalId] int  NOT NULL
);
GO

-- Creating table 'MaintenanceOrderOperationalStatus'
CREATE TABLE [dbo].[MaintenanceOrderOperationalStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaintenanceOrderOperationalId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [DateOfState] datetime  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'MaintenanceStatus'
CREATE TABLE [dbo].[MaintenanceStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Auto] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'MaintenanceOrderOperationalDetailTemps'
CREATE TABLE [dbo].[MaintenanceOrderOperationalDetailTemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ProductPriceCost] decimal(32,12)  NOT NULL,
    [MaintenanceOrderOperationalId] int  NOT NULL,
    [TotalBeforeDisc] decimal(32,10)  NULL,
    [TotalDiscount] decimal(32,10)  NULL
);
GO

-- Creating table 'MaintenanceOrderOperationalTemps'
CREATE TABLE [dbo].[MaintenanceOrderOperationalTemps] (
    [Id] int  NOT NULL,
    [Customer_Id] int  NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [Model] nvarchar(max)  NULL,
    [Describtion] nvarchar(max)  NULL,
    [Status] int  NULL,
    [guaranteeDuration] int  NULL,
    [Order_Code] nvarchar(max)  NULL,
    [RequiredDescribtion] nvarchar(max)  NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [Emplyee_id] int  NULL,
    [ExpectedAmount] decimal(32,12)  NULL
);
GO

-- Creating table 'MaintenanceOrderDetails'
CREATE TABLE [dbo].[MaintenanceOrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ProductPriceCost] decimal(32,12)  NOT NULL,
    [MaintenanceOrderOperationalId] int  NOT NULL
);
GO

-- Creating table 'MaintenanceOrders'
CREATE TABLE [dbo].[MaintenanceOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Customer_Id] int  NOT NULL,
    [SerialNumber] nvarchar(max)  NULL,
    [Model] nvarchar(max)  NOT NULL,
    [Describtion] nvarchar(max)  NULL,
    [Status] int  NOT NULL,
    [guaranteeDuration] int  NULL,
    [Order_Code] nvarchar(max)  NOT NULL,
    [RequiredDescribtion] nvarchar(max)  NOT NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Total_Amount] decimal(32,12)  NULL,
    [Total_Tax] decimal(32,12)  NULL,
    [Total_Cost] decimal(32,12)  NULL,
    [AllowDiscount] decimal(32,12)  NULL,
    [Emplyee_id] int  NOT NULL,
    [Checked] bit  NOT NULL,
    [Date] datetime  NOT NULL,
    [EndGuaranteeDate] datetime  NULL,
    [ExpectedAmount] decimal(32,12)  NULL
);
GO

-- Creating table 'MaintenanceOrderStatus'
CREATE TABLE [dbo].[MaintenanceOrderStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [MaintenanceOrderOperationalId] int  NOT NULL,
    [EmployeeId] int  NOT NULL,
    [DateOfState] datetime  NOT NULL,
    [Status] int  NOT NULL
);
GO

-- Creating table 'MaintenanceOrderCodings'
CREATE TABLE [dbo].[MaintenanceOrderCodings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'Orders'
CREATE TABLE [dbo].[Orders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderNumber] nvarchar(max)  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [OrderCountNumber] int  NULL,
    [DicountInPrecentage] decimal(32,12)  NOT NULL,
    [Compleeted] bit  NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [CustomerID] int  NULL,
    [PaymentMethodeID] int  NOT NULL,
    [SelectedExtraTaxId] int  NULL,
    [Comment] nvarchar(max)  NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [TotalNewTaxs] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [QRNumber] int  NULL,
    [StoreId] int  NULL,
    [Branch_Id] int  NULL,
    [OrderTypeId] int  NULL,
    [OrderType] nvarchar(max)  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'OrderDetails'
CREATE TABLE [dbo].[OrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PatchId] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [TaxId] int  NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NOT NULL,
    [Reverse_Quantity] decimal(32,12)  NOT NULL,
    [Reverse_Total_Tax] decimal(32,12)  NOT NULL,
    [Reverse_Total_Amount] decimal(32,12)  NOT NULL,
    [OrderId] int  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [AdditionalDetail] nvarchar(max)  NULL
);
GO

-- Creating table 'TempOrderDetails'
CREATE TABLE [dbo].[TempOrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PatchId] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [ProductName] nvarchar(max)  NOT NULL,
    [ProductTaxAmount] decimal(32,12)  NOT NULL,
    [ProductPrice] decimal(32,12)  NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [TempOrderId] int  NOT NULL,
    [TaxID] int  NULL,
    [AdditionalDetail] nvarchar(max)  NULL
);
GO

-- Creating table 'TempOrders'
CREATE TABLE [dbo].[TempOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderNumber] nvarchar(max)  NOT NULL,
    [DicountInPrecentage] decimal(32,12)  NOT NULL,
    [SelectedExtraTaxId] int  NULL,
    [CustomerID] int  NULL,
    [OrderTypeId] int  NULL,
    [OrderType] nvarchar(max)  NULL
);
GO

-- Creating table 'HoldedOrders'
CREATE TABLE [dbo].[HoldedOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeID] int  NOT NULL,
    [DicountInPrecentage] decimal(18,2)  NOT NULL,
    [OrderDate] datetime  NOT NULL,
    [CustomerID] int  NULL,
    [Comment] nvarchar(max)  NULL,
    [SelectedPaymentMethodeId] int  NULL,
    [SelectedExtraTaxId] int  NULL
);
GO

-- Creating table 'HoldedOrderDetails'
CREATE TABLE [dbo].[HoldedOrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PatchId] int  NOT NULL,
    [ProductID] int  NOT NULL,
    [Quantity] decimal(18,2)  NOT NULL,
    [ProductPrice] decimal(18,2)  NOT NULL,
    [ProductTaxAmount] decimal(18,2)  NOT NULL,
    [TaxID] int  NULL,
    [HoldedOrderId] int  NOT NULL
);
GO

-- Creating table 'ReverseOrders'
CREATE TABLE [dbo].[ReverseOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderNumber] nvarchar(max)  NOT NULL,
    [EmployeeID] int  NOT NULL,
    [DicountInPrecentage] decimal(18,2)  NOT NULL,
    [ReverseOrderDate] datetime  NOT NULL,
    [CustomerID] int  NULL,
    [PaymentMethodeID] int  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [OrderId] int  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [HasOff] bit  NOT NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'ReverseOrderDetails'
CREATE TABLE [dbo].[ReverseOrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductID] int  NOT NULL,
    [TaxId] int  NOT NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Product_Cost] decimal(32,12)  NOT NULL,
    [ReverseOrderId] int  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [PatchId] int  NOT NULL
);
GO

-- Creating table 'DetailQRInvoices'
CREATE TABLE [dbo].[DetailQRInvoices] (
    [Detail_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ProductPriceCost] decimal(32,12)  NOT NULL,
    [MasterQRInvoiceId] int  NOT NULL,
    [ProductProJ_Name] nvarchar(max)  NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL
);
GO

-- Creating table 'QRPaymentMethods'
CREATE TABLE [dbo].[QRPaymentMethods] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [Commution] decimal(32,12)  NOT NULL,
    [MasterQRInvoiceId] int  NOT NULL
);
GO

-- Creating table 'MasterQRInvoices'
CREATE TABLE [dbo].[MasterQRInvoices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NULL,
    [Master_Discount] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NULL,
    [Company_Id] int  NULL,
    [Account_Id] int  NULL,
    [Subledger_Id] int  NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(32,12)  NULL,
    [PointOfSalePOS_Id] int  NULL,
    [Employee_Id] int  NOT NULL,
    [AllowDiscount] decimal(32,12)  NOT NULL,
    [HasOff] bit  NOT NULL,
    [TotalNewTaxs] decimal(32,12)  NOT NULL,
    [MainrenanceOrder_Id] int  NULL,
    [InvoiceType] nvarchar(max)  NOT NULL,
    [Department_ID] int  NULL,
    [BussinesUnit_ID] int  NULL,
    [Branch_Id] int  NULL,
    [Notes] nvarchar(max)  NULL,
    [SubScribtionType_Id] int  NULL,
    [StartDate] datetime  NULL,
    [EndDate] datetime  NULL,
    [Duration] int  NULL,
    [Project_Id] int  NULL,
    [InvoiceType_Id] int  NOT NULL,
    [TotalBeforeDiscount] decimal(32,12)  NULL,
    [QrGuid] uniqueidentifier  NULL,
    [Number] nvarchar(max)  NULL,
    [DateOfCreate] datetime  NULL
);
GO

-- Creating table 'TaxQRInvoices'
CREATE TABLE [dbo].[TaxQRInvoices] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Tax_Id] nvarchar(max)  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [MasterQRInvoiceId] int  NOT NULL
);
GO

-- Creating table 'StoreEmployees'
CREATE TABLE [dbo].[StoreEmployees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Employee_Id] int  NOT NULL,
    [Store_ID] int  NOT NULL
);
GO

-- Creating table 'ExtrnalSalesEmployees'
CREATE TABLE [dbo].[ExtrnalSalesEmployees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ParentName] nvarchar(max)  NULL,
    [Email] nvarchar(max)  NOT NULL,
    [MobileNumber] nvarchar(max)  NOT NULL,
    [City] nvarchar(max)  NOT NULL,
    [BankName] nvarchar(max)  NOT NULL,
    [BankAccountNumber] nvarchar(max)  NOT NULL,
    [IBAN] nvarchar(max)  NOT NULL,
    [PromoCode] nvarchar(max)  NOT NULL,
    [RegistedDate] datetime  NOT NULL,
    [EmailConfirmed] bit  NOT NULL,
    [ParentID] int  NULL,
    [ParentPromoCode] nvarchar(max)  NULL,
    [TotalAmountBalance] decimal(18,2)  NOT NULL,
    [TotalChildsSalesAmountBalance] decimal(18,2)  NOT NULL,
    [TotalChildsSalesCount] decimal(18,2)  NOT NULL,
    [TotalCommissionAmount] decimal(18,2)  NOT NULL,
    [TotalReceivedAmount] decimal(18,2)  NOT NULL,
    [TotalCommissionAmountFromChild] decimal(18,2)  NULL,
    [TotalCustommersCount] int  NOT NULL,
    [Comment] nvarchar(max)  NULL
);
GO

-- Creating table 'EmployeeSalesDetails'
CREATE TABLE [dbo].[EmployeeSalesDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Amount] decimal(18,2)  NOT NULL,
    [CommissionAmount] decimal(18,2)  NOT NULL,
    [ParentCommissionAmount] decimal(18,2)  NULL,
    [SalesDate] datetime  NOT NULL,
    [CustommerID] int  NULL,
    [CustommerName] nvarchar(max)  NULL,
    [CustommerEmail] nvarchar(max)  NULL,
    [CustommerMobileNumber] nvarchar(max)  NULL,
    [Comment] nvarchar(max)  NULL,
    [ExtrnalSalesEmployeeId] int  NOT NULL
);
GO

-- Creating table 'ExternalEmployeePayments'
CREATE TABLE [dbo].[ExternalEmployeePayments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PaymentAmount] decimal(18,2)  NOT NULL,
    [Comment] nvarchar(max)  NULL,
    [PaymentDate] datetime  NOT NULL,
    [ExtrnalSalesEmployeeId] int  NOT NULL
);
GO

-- Creating table 'RoleFunctions'
CREATE TABLE [dbo].[RoleFunctions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [SystemRoleId] int  NOT NULL,
    [SystemFunctionId] int  NOT NULL,
    [isExist] bit  NULL
);
GO

-- Creating table 'SystemFunctions'
CREATE TABLE [dbo].[SystemFunctions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ArabName] nvarchar(max)  NOT NULL,
    [EnglishName] nvarchar(max)  NOT NULL,
    [Level] int  NOT NULL,
    [SystemFunctionId] int  NULL,
    [Code] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SystemRoles'
CREATE TABLE [dbo].[SystemRoles] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'ProjectClauses'
CREATE TABLE [dbo].[ProjectClauses] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Project_Id] int  NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'SubLedgertemps'
CREATE TABLE [dbo].[SubLedgertemps] (
    [SubLed_Id] int IDENTITY(1,1) NOT NULL,
    [SubLed_Name] nvarchar(max)  NOT NULL,
    [Subled_Level] int  NOT NULL,
    [SubLedgerSubLed_Id] int  NULL,
    [ProjectTempProject_Id] int  NOT NULL
);
GO

-- Creating table 'ProjectClausetemps'
CREATE TABLE [dbo].[ProjectClausetemps] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [ProjectTempProject_Id] int  NOT NULL
);
GO

-- Creating table 'SpentReciptOrders'
CREATE TABLE [dbo].[SpentReciptOrders] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Value] decimal(32,18)  NOT NULL,
    [SpentReciptOrderTypeId] int  NOT NULL,
    [DateAcceptOrRefused] datetime  NULL,
    [Employee_Id] int  NOT NULL
);
GO

-- Creating table 'SpentReciptOrderTypes'
CREATE TABLE [dbo].[SpentReciptOrderTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'FixedAssetDetails'
CREATE TABLE [dbo].[FixedAssetDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Date] datetime  NOT NULL,
    [DestructionValue] decimal(32,12)  NOT NULL,
    [DestructionDay] int  NOT NULL,
    [FixedAssetFasset_ID] int  NOT NULL,
    [GLMasterGLMaster_Id] int  NOT NULL
);
GO

-- Creating table 'ReceiptPOSFileDetails'
CREATE TABLE [dbo].[ReceiptPOSFileDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ContentLength] int  NOT NULL,
    [FileName] nvarchar(max)  NOT NULL,
    [ProImg] varbinary(max)  NOT NULL,
    [ReceiptRec_ID] int  NOT NULL
);
GO

-- Creating table 'POSReceipts'
CREATE TABLE [dbo].[POSReceipts] (
    [Rec_ID] int IDENTITY(1,1) NOT NULL,
    [RecComp_ID] int  NULL,
    [Rec_Amount] decimal(32,12)  NOT NULL,
    [RecCurrency_ID] nvarchar(max)  NOT NULL,
    [Rec_Number] nvarchar(max)  NULL,
    [RecBank_Account] int  NOT NULL,
    [Rec_Description] nvarchar(max)  NULL,
    [RecGl_Date] datetime  NULL,
    [Rec_Date] datetime  NOT NULL,
    [Rec_BU] int  NOT NULL,
    [TotalAmountAfMat] decimal(32,12)  NOT NULL,
    [Rec_state] int  NOT NULL,
    [Customer_Id] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Branch_Id] int  NULL,
    [PaymentMethod_Id] int  NULL,
    [HasOff] bit  NOT NULL,
    [Emp_Id] int  NULL,
    [MasterPOSInvoiceMasterPOSInvoiceId] int  NOT NULL
);
GO

-- Creating table 'ReverseQRDetails'
CREATE TABLE [dbo].[ReverseQRDetails] (
    [Details_Id] int IDENTITY(1,1) NOT NULL,
    [Product_Quantity] decimal(32,12)  NOT NULL,
    [Product_Price] decimal(32,12)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Product_Tax] int  NOT NULL,
    [Detail_Total_Tax] decimal(32,12)  NOT NULL,
    [Detail_Total_Cost] decimal(32,12)  NOT NULL,
    [Batch_ID] int  NOT NULL,
    [Product_Id] int  NOT NULL,
    [ReverseQRInvoiceMaster_Id] int  NOT NULL,
    [Detail_Discount] decimal(32,12)  NULL,
    [Detail_Discount_Amount] decimal(32,12)  NULL,
    [DetailQRInvoiceDetail_Id] int  NOT NULL
);
GO

-- Creating table 'ReverseQRInvoices'
CREATE TABLE [dbo].[ReverseQRInvoices] (
    [Master_Id] int IDENTITY(1,1) NOT NULL,
    [Serial_Number] nvarchar(max)  NOT NULL,
    [Description] nvarchar(max)  NULL,
    [Invoice_Date] datetime  NOT NULL,
    [Master_Discount] decimal(32,12)  NULL,
    [Invoice_Currency] nvarchar(max)  NOT NULL,
    [Total_Amount] decimal(32,12)  NOT NULL,
    [Total_Tax] decimal(32,12)  NOT NULL,
    [Total_Cost] decimal(32,12)  NOT NULL,
    [Invoice_State] int  NOT NULL,
    [Exchange_Rate] decimal(18,0)  NULL,
    [Customer_Id] int  NULL,
    [Store_ID] int  NOT NULL,
    [Company_Id] int  NOT NULL,
    [PaymentMethod_Id] int  NOT NULL,
    [Account_Id] int  NOT NULL,
    [Reference] nvarchar(max)  NULL,
    [ApprovalCode] nvarchar(max)  NULL,
    [Total_Payment_Method_Cost] decimal(18,2)  NOT NULL,
    [SubLedger_ID] int  NULL,
    [HasOff] bit  NOT NULL,
    [MasterQRInvoiceId] int  NOT NULL,
    [Number] nvarchar(max)  NULL,
    [DateOfCreate] datetime  NULL
);
GO

-- Creating table 'PaymentMethodEmployees'
CREATE TABLE [dbo].[PaymentMethodEmployees] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Employee_Id] int  NOT NULL,
    [PaymentMethod_Id] int  NULL
);
GO

-- Creating table 'InvoiceSerials'
CREATE TABLE [dbo].[InvoiceSerials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL
);
GO

-- Creating table 'ReveseInvoiceSerials'
CREATE TABLE [dbo].[ReveseInvoiceSerials] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Number] int  NOT NULL
);
GO

-- Creating table 'MedianSubscriptions'
CREATE TABLE [dbo].[MedianSubscriptions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Subscrip_Code] nvarchar(max)  NOT NULL,
    [Subscrip_Description] nvarchar(max)  NULL,
    [DeadLine] datetime  NOT NULL,
    [StartTime] datetime  NOT NULL,
    [Estimate_Amount] decimal(32,12)  NOT NULL,
    [Customer_Id] int  NULL,
    [Subscrip_Comp] int  NOT NULL,
    [Subscrip_BU] int  NULL,
    [Subscrip_State] int  NOT NULL,
    [Subscrip_Type] int  NOT NULL,
    [Subscrip_Pranch] int  NULL,
    [Emp_Team] int  NULL,
    [SubType_State] int  NOT NULL,
    [Payment_Method] int  NULL,
    [tel_customer] nvarchar(max)  NULL,
    [Subscrip_Stop] bit  NOT NULL,
    [Subscrip_DateEnd] datetime  NULL,
    [Customer_Name] nvarchar(max)  NOT NULL,
    [Sub_Type_ID] int  NOT NULL,
    [Check_Number] nvarchar(max)  NULL,
    [Sub_Pay_State] int  NULL,
    [Tax_ID] int  NOT NULL,
    [AmountAfterTax] decimal(32,12)  NOT NULL,
    [Subledger_ID] int  NULL,
    [Department_Id] int  NULL,
    [Payed_Amount] decimal(32,12)  NULL,
    [Discount] decimal(32,12)  NULL,
    [DiscountAllow] decimal(32,12)  NULL,
    [NewSubCode] nvarchar(max)  NULL,
    [HasOff] bit  NULL,
    [Emp_Id] int  NULL,
    [IsCanceled] bit  NULL,
    [Sub_Date] datetime  NULL,
    [QRNumber] int  NULL,
    [Number] nvarchar(max)  NULL
);
GO

-- Creating table 'HRNotifications'
CREATE TABLE [dbo].[HRNotifications] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [NoOfResidence] int  NOT NULL,
    [NoVacationEmployee] int  NULL,
    [NOContractEmployee] int  NULL,
    [NoOfhealtyInsurance] int  NOT NULL
);
GO

-- Creating table 'Shifts'
CREATE TABLE [dbo].[Shifts] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShiftCodingId] int  NOT NULL,
    [EmployeeEmp_Id] int  NOT NULL
);
GO

-- Creating table 'ShiftCodings'
CREATE TABLE [dbo].[ShiftCodings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [From] time  NOT NULL,
    [To] time  NOT NULL,
    [Hours] int  NOT NULL,
    [Minute] int  NOT NULL,
    [PercentageOfAdditionHours] decimal(32,12)  NOT NULL,
    [Active] bit  NOT NULL
);
GO

-- Creating table 'VacationWeeklies'
CREATE TABLE [dbo].[VacationWeeklies] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ShiftCodingId] int  NOT NULL,
    [VacationWeeklyCodingId] int  NOT NULL
);
GO

-- Creating table 'VacationWeeklyCodings'
CREATE TABLE [dbo].[VacationWeeklyCodings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AttendingAndLeavings'
CREATE TABLE [dbo].[AttendingAndLeavings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [EmployeeEmp_Id] int  NOT NULL,
    [Date] datetime  NULL,
    [From] time  NULL,
    [To] time  NULL,
    [ShiftCodingId] int  NOT NULL,
    [Hours] int  NULL,
    [Minute] int  NULL,
    [StatisticFrom] int  NULL,
    [StatisticTo] int  NULL,
    [Absent] bit  NULL
);
GO

-- Creating table 'RewardsAndPenalties'
CREATE TABLE [dbo].[RewardsAndPenalties] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AttendingAndLeavingId] int  NOT NULL,
    [RewardsAndPenaltiesCodingId] int  NOT NULL,
    [NumberOfHour] int  NOT NULL,
    [IsTaken] bit  NULL,
    [DateOfTaken] datetime  NULL,
    [Description] nvarchar(max)  NULL,
    [EmployeeSalaryMonthId] int  NULL
);
GO

-- Creating table 'RewardsAndPenaltiesCodingCategories'
CREATE TABLE [dbo].[RewardsAndPenaltiesCodingCategories] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [effectSalary] int  NOT NULL,
    [RewardsAndPenaltiesCodingId] int  NOT NULL
);
GO

-- Creating table 'RewardsAndPenaltiesCodings'
CREATE TABLE [dbo].[RewardsAndPenaltiesCodings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [RewardsAndPenaltiesCodingCategoryId] int  NOT NULL,
    [DurationInDay] int  NOT NULL,
    [DurationInhour] int  NOT NULL,
    [DurationInMinute] int  NOT NULL,
    [RewordInDay] int  NOT NULL,
    [RewordinHour] int  NOT NULL,
    [TypeOfDetailSalaryId] int  NOT NULL,
    [Active] bit  NOT NULL,
    [TransformToMinute] int  NULL,
    [InCreaseSalary] bit  NOT NULL,
    [IsMax] bit  NOT NULL
);
GO

-- Creating table 'OfficialHolidays'
CREATE TABLE [dbo].[OfficialHolidays] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [From] datetime  NOT NULL,
    [To] datetime  NOT NULL
);
GO

-- Creating table 'InvoicePrintSettings'
CREATE TABLE [dbo].[InvoicePrintSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [printWhite] nvarchar(max)  NOT NULL,
    [PrintLogo] nvarchar(max)  NOT NULL,
    [TypeId] int  NOT NULL,
    [Image] varbinary(max)  NOT NULL,
    [Active] bit  NOT NULL,
    [Package] bit  NOT NULL,
    [Description] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'InvoiceWordPrintSettings'
CREATE TABLE [dbo].[InvoiceWordPrintSettings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [TotalExcludingVat] nvarchar(max)  NULL,
    [Discount] nvarchar(max)  NULL,
    [TotalTaxableAmount] nvarchar(max)  NULL,
    [ToatlAmountDue] nvarchar(max)  NULL,
    [ToatalAmountPaid] nvarchar(max)  NULL,
    [ToatalRamainingAmount] nvarchar(max)  NULL,
    [TotalExcludingVat1] nvarchar(max)  NULL,
    [Discount1] nvarchar(max)  NULL,
    [TotalTaxableAmount1] nvarchar(max)  NULL,
    [ToatlAmountDue1] nvarchar(max)  NULL,
    [ToatalAmountPaid1] nvarchar(max)  NULL,
    [ToatalRamainingAmount1] nvarchar(max)  NULL,
    [DiscountInDetail] bit  NULL,
    [TotalVat] nvarchar(max)  NULL,
    [TotalVat1] nvarchar(max)  NULL,
    [Time] bit  NULL,
    [BaptismNumber] bit  NULL
);
GO

-- Creating table 'Questions'
CREATE TABLE [dbo].[Questions] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [Sort] int  NOT NULL,
    [IsRequired] bit  NOT NULL
);
GO

-- Creating table 'Answers'
CREATE TABLE [dbo].[Answers] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [PathImage] nvarchar(max)  NOT NULL,
    [Desc] nvarchar(max)  NOT NULL,
    [IsActive] bit  NOT NULL,
    [QuestionId] int  NOT NULL,
    [ProductId] int  NULL
);
GO

-- Creating table 'Sewings'
CREATE TABLE [dbo].[Sewings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [ProductId] int  NOT NULL,
    [Desc] nvarchar(max)  NULL,
    [Notes] nvarchar(max)  NULL,
    [DueDate] datetime  NOT NULL,
    [Date] datetime  NOT NULL,
    [CustomerId] int  NOT NULL,
    [Amount] decimal(32,12)  NOT NULL,
    [NumebrOfDesign] int  NOT NULL,
    [MasterPOSInvoiceMasterPOSInvoiceId] int  NULL,
    [Quantity] decimal(32,12)  NOT NULL,
    [SewingStatusId] int  NOT NULL,
    [OrderNumber] int  NOT NULL
);
GO

-- Creating table 'SewingDetails'
CREATE TABLE [dbo].[SewingDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [QuestionId] int  NOT NULL,
    [SewingId] int  NOT NULL,
    [Description] nvarchar(max)  NOT NULL,
    [AnswerId] int  NULL
);
GO

-- Creating table 'SewingStatus'
CREATE TABLE [dbo].[SewingStatus] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'OrderTypes'
CREATE TABLE [dbo].[OrderTypes] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'AdditionalOrderDetails'
CREATE TABLE [dbo].[AdditionalOrderDetails] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AdditionalDetail] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'StartAndEndSellings'
CREATE TABLE [dbo].[StartAndEndSellings] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [OrderNumber] int  NOT NULL,
    [StartingDateAndTime] datetime  NOT NULL,
    [EndDateAndTime] datetime  NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [Prod_ID] in table 'Products'
ALTER TABLE [dbo].[Products]
ADD CONSTRAINT [PK_Products]
    PRIMARY KEY CLUSTERED ([Prod_ID] ASC);
GO

-- Creating primary key on [Store_ID] in table 'Stores'
ALTER TABLE [dbo].[Stores]
ADD CONSTRAINT [PK_Stores]
    PRIMARY KEY CLUSTERED ([Store_ID] ASC);
GO

-- Creating primary key on [MUnit_ID] in table 'MeasuringUnits'
ALTER TABLE [dbo].[MeasuringUnits]
ADD CONSTRAINT [PK_MeasuringUnits]
    PRIMARY KEY CLUSTERED ([MUnit_ID] ASC);
GO

-- Creating primary key on [Cat_ID] in table 'Categories'
ALTER TABLE [dbo].[Categories]
ADD CONSTRAINT [PK_Categories]
    PRIMARY KEY CLUSTERED ([Cat_ID] ASC);
GO

-- Creating primary key on [Branch_Id] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [PK_Branches]
    PRIMARY KEY CLUSTERED ([Branch_Id] ASC);
GO

-- Creating primary key on [Comp_Id] in table 'Companies'
ALTER TABLE [dbo].[Companies]
ADD CONSTRAINT [PK_Companies]
    PRIMARY KEY CLUSTERED ([Comp_Id] ASC);
GO

-- Creating primary key on [Dept_Id] in table 'BranchDepartments'
ALTER TABLE [dbo].[BranchDepartments]
ADD CONSTRAINT [PK_BranchDepartments]
    PRIMARY KEY CLUSTERED ([Dept_Id] ASC);
GO

-- Creating primary key on [Part_Id] in table 'Partners'
ALTER TABLE [dbo].[Partners]
ADD CONSTRAINT [PK_Partners]
    PRIMARY KEY CLUSTERED ([Part_Id] ASC);
GO

-- Creating primary key on [Currency_Id] in table 'Currencies'
ALTER TABLE [dbo].[Currencies]
ADD CONSTRAINT [PK_Currencies]
    PRIMARY KEY CLUSTERED ([Currency_Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyDepartments'
ALTER TABLE [dbo].[CompanyDepartments]
ADD CONSTRAINT [PK_CompanyDepartments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyConfigrations'
ALTER TABLE [dbo].[CompanyConfigrations]
ADD CONSTRAINT [PK_CompanyConfigrations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Country_Id] in table 'Countries'
ALTER TABLE [dbo].[Countries]
ADD CONSTRAINT [PK_Countries]
    PRIMARY KEY CLUSTERED ([Country_Id] ASC);
GO

-- Creating primary key on [Province_Id] in table 'Provinces'
ALTER TABLE [dbo].[Provinces]
ADD CONSTRAINT [PK_Provinces]
    PRIMARY KEY CLUSTERED ([Province_Id] ASC);
GO

-- Creating primary key on [City_Id] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [PK_Cities]
    PRIMARY KEY CLUSTERED ([City_Id] ASC);
GO

-- Creating primary key on [BU_Id] in table 'BussinesUnits'
ALTER TABLE [dbo].[BussinesUnits]
ADD CONSTRAINT [PK_BussinesUnits]
    PRIMARY KEY CLUSTERED ([BU_Id] ASC);
GO

-- Creating primary key on [Expense_Id] in table 'Expenses'
ALTER TABLE [dbo].[Expenses]
ADD CONSTRAINT [PK_Expenses]
    PRIMARY KEY CLUSTERED ([Expense_Id] ASC);
GO

-- Creating primary key on [GLMaster_Id] in table 'GLMasters'
ALTER TABLE [dbo].[GLMasters]
ADD CONSTRAINT [PK_GLMasters]
    PRIMARY KEY CLUSTERED ([GLMaster_Id] ASC);
GO

-- Creating primary key on [GLDetail_Id] in table 'GLDetails'
ALTER TABLE [dbo].[GLDetails]
ADD CONSTRAINT [PK_GLDetails]
    PRIMARY KEY CLUSTERED ([GLDetail_Id] ASC);
GO

-- Creating primary key on [Account_Id] in table 'ChartOfAccountings'
ALTER TABLE [dbo].[ChartOfAccountings]
ADD CONSTRAINT [PK_ChartOfAccountings]
    PRIMARY KEY CLUSTERED ([Account_Id] ASC);
GO

-- Creating primary key on [GL_Type_Id] in table 'GL_Type'
ALTER TABLE [dbo].[GL_Type]
ADD CONSTRAINT [PK_GL_Type]
    PRIMARY KEY CLUSTERED ([GL_Type_Id] ASC);
GO

-- Creating primary key on [Customer_Id] in table 'Customers'
ALTER TABLE [dbo].[Customers]
ADD CONSTRAINT [PK_Customers]
    PRIMARY KEY CLUSTERED ([Customer_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'InvoiceMasters'
ALTER TABLE [dbo].[InvoiceMasters]
ADD CONSTRAINT [PK_InvoiceMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'InvoiceDetails'
ALTER TABLE [dbo].[InvoiceDetails]
ADD CONSTRAINT [PK_InvoiceDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Emp_Id] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [PK_Employees]
    PRIMARY KEY CLUSTERED ([Emp_Id] ASC);
GO

-- Creating primary key on [Trea_Id] in table 'Treasuries'
ALTER TABLE [dbo].[Treasuries]
ADD CONSTRAINT [PK_Treasuries]
    PRIMARY KEY CLUSTERED ([Trea_Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeFileDetails'
ALTER TABLE [dbo].[EmployeeFileDetails]
ADD CONSTRAINT [PK_EmployeeFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Supp_Id] in table 'Suppliers'
ALTER TABLE [dbo].[Suppliers]
ADD CONSTRAINT [PK_Suppliers]
    PRIMARY KEY CLUSTERED ([Supp_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'PurInvoiceMasters'
ALTER TABLE [dbo].[PurInvoiceMasters]
ADD CONSTRAINT [PK_PurInvoiceMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'PurchaseDetails'
ALTER TABLE [dbo].[PurchaseDetails]
ADD CONSTRAINT [PK_PurchaseDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [AnothrAsst_ID] in table 'AnotherAssets'
ALTER TABLE [dbo].[AnotherAssets]
ADD CONSTRAINT [PK_AnotherAssets]
    PRIMARY KEY CLUSTERED ([AnothrAsst_ID] ASC);
GO

-- Creating primary key on [Casset_ID] in table 'CurrentAssetTypes'
ALTER TABLE [dbo].[CurrentAssetTypes]
ADD CONSTRAINT [PK_CurrentAssetTypes]
    PRIMARY KEY CLUSTERED ([Casset_ID] ASC);
GO

-- Creating primary key on [FasstImage_ID] in table 'FixedAssetImages'
ALTER TABLE [dbo].[FixedAssetImages]
ADD CONSTRAINT [PK_FixedAssetImages]
    PRIMARY KEY CLUSTERED ([FasstImage_ID] ASC);
GO

-- Creating primary key on [DepType_ID] in table 'DepreciationTypes'
ALTER TABLE [dbo].[DepreciationTypes]
ADD CONSTRAINT [PK_DepreciationTypes]
    PRIMARY KEY CLUSTERED ([DepType_ID] ASC);
GO

-- Creating primary key on [Master_Id] in table 'RecalPurchInvoiceMasters'
ALTER TABLE [dbo].[RecalPurchInvoiceMasters]
ADD CONSTRAINT [PK_RecalPurchInvoiceMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'RecalPurchInvoiceDetails'
ALTER TABLE [dbo].[RecalPurchInvoiceDetails]
ADD CONSTRAINT [PK_RecalPurchInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [TransferHd_Id] in table 'InventoryTransfer_Hd'
ALTER TABLE [dbo].[InventoryTransfer_Hd]
ADD CONSTRAINT [PK_InventoryTransfer_Hd]
    PRIMARY KEY CLUSTERED ([TransferHd_Id] ASC);
GO

-- Creating primary key on [TransferDetail_Id] in table 'InventoryTransfer_Detail'
ALTER TABLE [dbo].[InventoryTransfer_Detail]
ADD CONSTRAINT [PK_InventoryTransfer_Detail]
    PRIMARY KEY CLUSTERED ([TransferDetail_Id] ASC);
GO

-- Creating primary key on [Match_Id] in table 'Matchings'
ALTER TABLE [dbo].[Matchings]
ADD CONSTRAINT [PK_Matchings]
    PRIMARY KEY CLUSTERED ([Match_Id] ASC);
GO

-- Creating primary key on [Rec_ID] in table 'Receipts'
ALTER TABLE [dbo].[Receipts]
ADD CONSTRAINT [PK_Receipts]
    PRIMARY KEY CLUSTERED ([Rec_ID] ASC);
GO

-- Creating primary key on [Match_Id] in table 'MatchingPayments'
ALTER TABLE [dbo].[MatchingPayments]
ADD CONSTRAINT [PK_MatchingPayments]
    PRIMARY KEY CLUSTERED ([Match_Id] ASC);
GO

-- Creating primary key on [Pay_ID] in table 'Payments'
ALTER TABLE [dbo].[Payments]
ADD CONSTRAINT [PK_Payments]
    PRIMARY KEY CLUSTERED ([Pay_ID] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReveInvoiceDetails'
ALTER TABLE [dbo].[ReveInvoiceDetails]
ADD CONSTRAINT [PK_ReveInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReverseSalesInvoiceMasters'
ALTER TABLE [dbo].[ReverseSalesInvoiceMasters]
ADD CONSTRAINT [PK_ReverseSalesInvoiceMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Bank_ID] in table 'Banks'
ALTER TABLE [dbo].[Banks]
ADD CONSTRAINT [PK_Banks]
    PRIMARY KEY CLUSTERED ([Bank_ID] ASC);
GO

-- Creating primary key on [Tax_ID] in table 'Taxes'
ALTER TABLE [dbo].[Taxes]
ADD CONSTRAINT [PK_Taxes]
    PRIMARY KEY CLUSTERED ([Tax_ID] ASC);
GO

-- Creating primary key on [Batch_Id] in table 'TransactionBatches'
ALTER TABLE [dbo].[TransactionBatches]
ADD CONSTRAINT [PK_TransactionBatches]
    PRIMARY KEY CLUSTERED ([Batch_Id] ASC);
GO

-- Creating primary key on [Project_Id] in table 'Projects'
ALTER TABLE [dbo].[Projects]
ADD CONSTRAINT [PK_Projects]
    PRIMARY KEY CLUSTERED ([Project_Id] ASC);
GO

-- Creating primary key on [OfferDet_Id] in table 'OfferPriceDetails'
ALTER TABLE [dbo].[OfferPriceDetails]
ADD CONSTRAINT [PK_OfferPriceDetails]
    PRIMARY KEY CLUSTERED ([OfferDet_Id] ASC);
GO

-- Creating primary key on [OfferMas_Id] in table 'OfferPriceMasters'
ALTER TABLE [dbo].[OfferPriceMasters]
ADD CONSTRAINT [PK_OfferPriceMasters]
    PRIMARY KEY CLUSTERED ([OfferMas_Id] ASC);
GO

-- Creating primary key on [Id] in table 'DeprTaskStatus'
ALTER TABLE [dbo].[DeprTaskStatus]
ADD CONSTRAINT [PK_DeprTaskStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Batch_ID] in table 'ProductBatches'
ALTER TABLE [dbo].[ProductBatches]
ADD CONSTRAINT [PK_ProductBatches]
    PRIMARY KEY CLUSTERED ([Batch_ID] ASC);
GO

-- Creating primary key on [Id] in table 'CostDistributions'
ALTER TABLE [dbo].[CostDistributions]
ADD CONSTRAINT [PK_CostDistributions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CostDistributionDetails'
ALTER TABLE [dbo].[CostDistributionDetails]
ADD CONSTRAINT [PK_CostDistributionDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceiptFileDetails'
ALTER TABLE [dbo].[ReceiptFileDetails]
ADD CONSTRAINT [PK_ReceiptFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentFileDetails'
ALTER TABLE [dbo].[PaymentFileDetails]
ADD CONSTRAINT [PK_PaymentFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PartnerFileDetails'
ALTER TABLE [dbo].[PartnerFileDetails]
ADD CONSTRAINT [PK_PartnerFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpenseFileDetails'
ALTER TABLE [dbo].[ExpenseFileDetails]
ADD CONSTRAINT [PK_ExpenseFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FixedAssetFileDetails'
ALTER TABLE [dbo].[FixedAssetFileDetails]
ADD CONSTRAINT [PK_FixedAssetFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SupplierFileDetails'
ALTER TABLE [dbo].[SupplierFileDetails]
ADD CONSTRAINT [PK_SupplierFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CustomerFileDetails'
ALTER TABLE [dbo].[CustomerFileDetails]
ADD CONSTRAINT [PK_CustomerFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectFileDetails'
ALTER TABLE [dbo].[ProjectFileDetails]
ADD CONSTRAINT [PK_ProjectFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'InvoiceDetailsTemps'
ALTER TABLE [dbo].[InvoiceDetailsTemps]
ADD CONSTRAINT [PK_InvoiceDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'InvoiceMasterTemps'
ALTER TABLE [dbo].[InvoiceMasterTemps]
ADD CONSTRAINT [PK_InvoiceMasterTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesFileDetails'
ALTER TABLE [dbo].[SalesFileDetails]
ADD CONSTRAINT [PK_SalesFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'PurchaseDetailsTemps'
ALTER TABLE [dbo].[PurchaseDetailsTemps]
ADD CONSTRAINT [PK_PurchaseDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'PurInvoiceMasterTemps'
ALTER TABLE [dbo].[PurInvoiceMasterTemps]
ADD CONSTRAINT [PK_PurInvoiceMasterTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseFileDetails'
ALTER TABLE [dbo].[PurchaseFileDetails]
ADD CONSTRAINT [PK_PurchaseFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'RecallPurchInvDetailsTemps'
ALTER TABLE [dbo].[RecallPurchInvDetailsTemps]
ADD CONSTRAINT [PK_RecallPurchInvDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'RecallPurchInvoicMasterTemps'
ALTER TABLE [dbo].[RecallPurchInvoicMasterTemps]
ADD CONSTRAINT [PK_RecallPurchInvoicMasterTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'RecallPurchFileDetails'
ALTER TABLE [dbo].[RecallPurchFileDetails]
ADD CONSTRAINT [PK_RecallPurchFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReversSalesFileDetails'
ALTER TABLE [dbo].[ReversSalesFileDetails]
ADD CONSTRAINT [PK_ReversSalesFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReverseSalesInvoiceMasterTemps'
ALTER TABLE [dbo].[ReverseSalesInvoiceMasterTemps]
ADD CONSTRAINT [PK_ReverseSalesInvoiceMasterTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReveInvoiceDetailsTemps'
ALTER TABLE [dbo].[ReveInvoiceDetailsTemps]
ADD CONSTRAINT [PK_ReveInvoiceDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [TransferHd_Id] in table 'InventoryTransfer_HdTemp'
ALTER TABLE [dbo].[InventoryTransfer_HdTemp]
ADD CONSTRAINT [PK_InventoryTransfer_HdTemp]
    PRIMARY KEY CLUSTERED ([TransferHd_Id] ASC);
GO

-- Creating primary key on [TransferDetail_Id] in table 'InventoryTransfer_DetailTemp'
ALTER TABLE [dbo].[InventoryTransfer_DetailTemp]
ADD CONSTRAINT [PK_InventoryTransfer_DetailTemp]
    PRIMARY KEY CLUSTERED ([TransferDetail_Id] ASC);
GO

-- Creating primary key on [Contact_Id] in table 'Contacts'
ALTER TABLE [dbo].[Contacts]
ADD CONSTRAINT [PK_Contacts]
    PRIMARY KEY CLUSTERED ([Contact_Id] ASC);
GO

-- Creating primary key on [PaymentMethod_Id] in table 'PaymentMethods'
ALTER TABLE [dbo].[PaymentMethods]
ADD CONSTRAINT [PK_PaymentMethods]
    PRIMARY KEY CLUSTERED ([PaymentMethod_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'SalesReceiptDetailsTemps'
ALTER TABLE [dbo].[SalesReceiptDetailsTemps]
ADD CONSTRAINT [PK_SalesReceiptDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'SalesReceiptMasterTemps'
ALTER TABLE [dbo].[SalesReceiptMasterTemps]
ADD CONSTRAINT [PK_SalesReceiptMasterTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesReceiptFiles'
ALTER TABLE [dbo].[SalesReceiptFiles]
ADD CONSTRAINT [PK_SalesReceiptFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'SalesReceiptMasters'
ALTER TABLE [dbo].[SalesReceiptMasters]
ADD CONSTRAINT [PK_SalesReceiptMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'SalesReceiptDetails'
ALTER TABLE [dbo].[SalesReceiptDetails]
ADD CONSTRAINT [PK_SalesReceiptDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Fasset_ID] in table 'FixedAssets'
ALTER TABLE [dbo].[FixedAssets]
ADD CONSTRAINT [PK_FixedAssets]
    PRIMARY KEY CLUSTERED ([Fasset_ID] ASC);
GO

-- Creating primary key on [Details_Id] in table 'PurchaseOrderDetails'
ALTER TABLE [dbo].[PurchaseOrderDetails]
ADD CONSTRAINT [PK_PurchaseOrderDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Id] in table 'PurchaseFileOrders'
ALTER TABLE [dbo].[PurchaseFileOrders]
ADD CONSTRAINT [PK_PurchaseFileOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'PurchaseOrderMasters'
ALTER TABLE [dbo].[PurchaseOrderMasters]
ADD CONSTRAINT [PK_PurchaseOrderMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [ID] in table 'EmployeeTeamTemps'
ALTER TABLE [dbo].[EmployeeTeamTemps]
ADD CONSTRAINT [PK_EmployeeTeamTemps]
    PRIMARY KEY CLUSTERED ([ID] ASC);
GO

-- Creating primary key on [Team_ID] in table 'EmployeeTeams'
ALTER TABLE [dbo].[EmployeeTeams]
ADD CONSTRAINT [PK_EmployeeTeams]
    PRIMARY KEY CLUSTERED ([Team_ID] ASC);
GO

-- Creating primary key on [Subscrip_Id] in table 'Subscriptions'
ALTER TABLE [dbo].[Subscriptions]
ADD CONSTRAINT [PK_Subscriptions]
    PRIMARY KEY CLUSTERED ([Subscrip_Id] ASC);
GO

-- Creating primary key on [Subscription_Type_ID] in table 'Subscription_Type'
ALTER TABLE [dbo].[Subscription_Type]
ADD CONSTRAINT [PK_Subscription_Type]
    PRIMARY KEY CLUSTERED ([Subscription_Type_ID] ASC);
GO

-- Creating primary key on [Batch_ID] in table 'ProductBatchesTemps'
ALTER TABLE [dbo].[ProductBatchesTemps]
ADD CONSTRAINT [PK_ProductBatchesTemps]
    PRIMARY KEY CLUSTERED ([Batch_ID] ASC);
GO

-- Creating primary key on [Id] in table 'TransferToprojectFile1'
ALTER TABLE [dbo].[TransferToprojectFile1]
ADD CONSTRAINT [PK_TransferToprojectFile1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'TransferToProjectMasters'
ALTER TABLE [dbo].[TransferToProjectMasters]
ADD CONSTRAINT [PK_TransferToProjectMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'TransferToProjectDetails'
ALTER TABLE [dbo].[TransferToProjectDetails]
ADD CONSTRAINT [PK_TransferToProjectDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'TransferToProjectTemps'
ALTER TABLE [dbo].[TransferToProjectTemps]
ADD CONSTRAINT [PK_TransferToProjectTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'TransferToProjectDetailTemps'
ALTER TABLE [dbo].[TransferToProjectDetailTemps]
ADD CONSTRAINT [PK_TransferToProjectDetailTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Id] in table 'TransfareFromPorjectToStoretemps'
ALTER TABLE [dbo].[TransfareFromPorjectToStoretemps]
ADD CONSTRAINT [PK_TransfareFromPorjectToStoretemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Vacation_Id] in table 'Vacations'
ALTER TABLE [dbo].[Vacations]
ADD CONSTRAINT [PK_Vacations]
    PRIMARY KEY CLUSTERED ([Vacation_Id] ASC);
GO

-- Creating primary key on [Id] in table 'Vacation_Types'
ALTER TABLE [dbo].[Vacation_Types]
ADD CONSTRAINT [PK_Vacation_Types]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Salary_Components_Id] in table 'Salary_Components'
ALTER TABLE [dbo].[Salary_Components]
ADD CONSTRAINT [PK_Salary_Components]
    PRIMARY KEY CLUSTERED ([Salary_Components_Id] ASC);
GO

-- Creating primary key on [Evaluation_Id] in table 'Emp_Evaluation'
ALTER TABLE [dbo].[Emp_Evaluation]
ADD CONSTRAINT [PK_Emp_Evaluation]
    PRIMARY KEY CLUSTERED ([Evaluation_Id] ASC);
GO

-- Creating primary key on [EmpTemp_Id] in table 'EmployeTemps'
ALTER TABLE [dbo].[EmployeTemps]
ADD CONSTRAINT [PK_EmployeTemps]
    PRIMARY KEY CLUSTERED ([EmpTemp_Id] ASC);
GO

-- Creating primary key on [Salary_ComponentsTemp_Id] in table 'Salary_ComponentsTemp'
ALTER TABLE [dbo].[Salary_ComponentsTemp]
ADD CONSTRAINT [PK_Salary_ComponentsTemp]
    PRIMARY KEY CLUSTERED ([Salary_ComponentsTemp_Id] ASC);
GO

-- Creating primary key on [ProjectPaymentId] in table 'ProjectPayments'
ALTER TABLE [dbo].[ProjectPayments]
ADD CONSTRAINT [PK_ProjectPayments]
    PRIMARY KEY CLUSTERED ([ProjectPaymentId] ASC);
GO

-- Creating primary key on [PInvoiceMaster_ID] in table 'ProjectInvoiceMasters'
ALTER TABLE [dbo].[ProjectInvoiceMasters]
ADD CONSTRAINT [PK_ProjectInvoiceMasters]
    PRIMARY KEY CLUSTERED ([PInvoiceMaster_ID] ASC);
GO

-- Creating primary key on [Detail_ID] in table 'ProjectInvoiceDetails'
ALTER TABLE [dbo].[ProjectInvoiceDetails]
ADD CONSTRAINT [PK_ProjectInvoiceDetails]
    PRIMARY KEY CLUSTERED ([Detail_ID] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectInvoiceDetailTemps'
ALTER TABLE [dbo].[ProjectInvoiceDetailTemps]
ADD CONSTRAINT [PK_ProjectInvoiceDetailTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectInvoiceMasterTemps'
ALTER TABLE [dbo].[ProjectInvoiceMasterTemps]
ADD CONSTRAINT [PK_ProjectInvoiceMasterTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Inv_ID] in table 'SubscriptionInvoices'
ALTER TABLE [dbo].[SubscriptionInvoices]
ADD CONSTRAINT [PK_SubscriptionInvoices]
    PRIMARY KEY CLUSTERED ([Inv_ID] ASC);
GO

-- Creating primary key on [Main_Detail_Id] in table 'Maintenance_Invoice_Detail'
ALTER TABLE [dbo].[Maintenance_Invoice_Detail]
ADD CONSTRAINT [PK_Maintenance_Invoice_Detail]
    PRIMARY KEY CLUSTERED ([Main_Detail_Id] ASC);
GO

-- Creating primary key on [Main_Invoice_Id] in table 'Maintenance_Invoice_Master'
ALTER TABLE [dbo].[Maintenance_Invoice_Master]
ADD CONSTRAINT [PK_Maintenance_Invoice_Master]
    PRIMARY KEY CLUSTERED ([Main_Invoice_Id] ASC);
GO

-- Creating primary key on [Main_Invoice_Id] in table 'Maintenance_Invoice_Master_Temp'
ALTER TABLE [dbo].[Maintenance_Invoice_Master_Temp]
ADD CONSTRAINT [PK_Maintenance_Invoice_Master_Temp]
    PRIMARY KEY CLUSTERED ([Main_Invoice_Id] ASC);
GO

-- Creating primary key on [Main_Detail_Id] in table 'Maintenance_Invoice_Detail_Temp'
ALTER TABLE [dbo].[Maintenance_Invoice_Detail_Temp]
ADD CONSTRAINT [PK_Maintenance_Invoice_Detail_Temp]
    PRIMARY KEY CLUSTERED ([Main_Detail_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReverseSalesReceiptMasterTemp1'
ALTER TABLE [dbo].[ReverseSalesReceiptMasterTemp1]
ADD CONSTRAINT [PK_ReverseSalesReceiptMasterTemp1]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReverseSalesReceiptDetailsTemps'
ALTER TABLE [dbo].[ReverseSalesReceiptDetailsTemps]
ADD CONSTRAINT [PK_ReverseSalesReceiptDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReverseSalesReceiptDetails'
ALTER TABLE [dbo].[ReverseSalesReceiptDetails]
ADD CONSTRAINT [PK_ReverseSalesReceiptDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReverseSalesReceiptFiles'
ALTER TABLE [dbo].[ReverseSalesReceiptFiles]
ADD CONSTRAINT [PK_ReverseSalesReceiptFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReverseSalesReceiptMasters'
ALTER TABLE [dbo].[ReverseSalesReceiptMasters]
ADD CONSTRAINT [PK_ReverseSalesReceiptMasters]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Accounts_Id] in table 'Accounts'
ALTER TABLE [dbo].[Accounts]
ADD CONSTRAINT [PK_Accounts]
    PRIMARY KEY CLUSTERED ([Accounts_Id] ASC);
GO

-- Creating primary key on [SlicesProduct_Id] in table 'SlicesProducts'
ALTER TABLE [dbo].[SlicesProducts]
ADD CONSTRAINT [PK_SlicesProducts]
    PRIMARY KEY CLUSTERED ([SlicesProduct_Id] ASC);
GO

-- Creating primary key on [Slice_Id] in table 'SlicesTems'
ALTER TABLE [dbo].[SlicesTems]
ADD CONSTRAINT [PK_SlicesTems]
    PRIMARY KEY CLUSTERED ([Slice_Id] ASC);
GO

-- Creating primary key on [Prod_ID] in table 'ProductTems'
ALTER TABLE [dbo].[ProductTems]
ADD CONSTRAINT [PK_ProductTems]
    PRIMARY KEY CLUSTERED ([Prod_ID] ASC);
GO

-- Creating primary key on [Slice_Id] in table 'Slices'
ALTER TABLE [dbo].[Slices]
ADD CONSTRAINT [PK_Slices]
    PRIMARY KEY CLUSTERED ([Slice_Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubLedgerFileDetail1'
ALTER TABLE [dbo].[SubLedgerFileDetail1]
ADD CONSTRAINT [PK_SubLedgerFileDetail1]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SubLed_Id] in table 'SubLedgers'
ALTER TABLE [dbo].[SubLedgers]
ADD CONSTRAINT [PK_SubLedgers]
    PRIMARY KEY CLUSTERED ([SubLed_Id] ASC);
GO

-- Creating primary key on [BussinessUnit_Id] in table 'DepartmentBussinesUnits'
ALTER TABLE [dbo].[DepartmentBussinesUnits]
ADD CONSTRAINT [PK_DepartmentBussinesUnits]
    PRIMARY KEY CLUSTERED ([BussinessUnit_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'TransferToMaintenanceDetailsTemps'
ALTER TABLE [dbo].[TransferToMaintenanceDetailsTemps]
ADD CONSTRAINT [PK_TransferToMaintenanceDetailsTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'TransferToMaintenanceTemps'
ALTER TABLE [dbo].[TransferToMaintenanceTemps]
ADD CONSTRAINT [PK_TransferToMaintenanceTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'TransferToMaintenanceDetails'
ALTER TABLE [dbo].[TransferToMaintenanceDetails]
ADD CONSTRAINT [PK_TransferToMaintenanceDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'TransferToMaintenances'
ALTER TABLE [dbo].[TransferToMaintenances]
ADD CONSTRAINT [PK_TransferToMaintenances]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'AssetDepDates'
ALTER TABLE [dbo].[AssetDepDates]
ADD CONSTRAINT [PK_AssetDepDates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyFileDetails'
ALTER TABLE [dbo].[CompanyFileDetails]
ADD CONSTRAINT [PK_CompanyFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'CompanyPhotoDetails'
ALTER TABLE [dbo].[CompanyPhotoDetails]
ADD CONSTRAINT [PK_CompanyPhotoDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [CatchReceipt_Id] in table 'CatchReceipts'
ALTER TABLE [dbo].[CatchReceipts]
ADD CONSTRAINT [PK_CatchReceipts]
    PRIMARY KEY CLUSTERED ([CatchReceipt_Id] ASC);
GO

-- Creating primary key on [Id] in table 'CatchReceiptFileDetails'
ALTER TABLE [dbo].[CatchReceiptFileDetails]
ADD CONSTRAINT [PK_CatchReceiptFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [ContentPerson_ID] in table 'ContentPersonDetailsTemps'
ALTER TABLE [dbo].[ContentPersonDetailsTemps]
ADD CONSTRAINT [PK_ContentPersonDetailsTemps]
    PRIMARY KEY CLUSTERED ([ContentPerson_ID] ASC);
GO

-- Creating primary key on [ContentPerson_ID] in table 'ContentPersonDetails'
ALTER TABLE [dbo].[ContentPersonDetails]
ADD CONSTRAINT [PK_ContentPersonDetails]
    PRIMARY KEY CLUSTERED ([ContentPerson_ID] ASC);
GO

-- Creating primary key on [Customer_Id] in table 'CustomerTemps'
ALTER TABLE [dbo].[CustomerTemps]
ADD CONSTRAINT [PK_CustomerTemps]
    PRIMARY KEY CLUSTERED ([Customer_Id] ASC);
GO

-- Creating primary key on [SpentReceipt_Id] in table 'SpentRecipts'
ALTER TABLE [dbo].[SpentRecipts]
ADD CONSTRAINT [PK_SpentRecipts]
    PRIMARY KEY CLUSTERED ([SpentReceipt_Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpentReciptFileDetails'
ALTER TABLE [dbo].[SpentReciptFileDetails]
ADD CONSTRAINT [PK_SpentReciptFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sales_Settings'
ALTER TABLE [dbo].[Sales_Settings]
ADD CONSTRAINT [PK_Sales_Settings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sales_Setting_Slice'
ALTER TABLE [dbo].[Sales_Setting_Slice]
ADD CONSTRAINT [PK_Sales_Setting_Slice]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sales_Setting_SliceTemp'
ALTER TABLE [dbo].[Sales_Setting_SliceTemp]
ADD CONSTRAINT [PK_Sales_Setting_SliceTemp]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalaryMonths'
ALTER TABLE [dbo].[EmployeeSalaryMonths]
ADD CONSTRAINT [PK_EmployeeSalaryMonths]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Employee_Sales'
ALTER TABLE [dbo].[Employee_Sales]
ADD CONSTRAINT [PK_Employee_Sales]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmpSal_Detail'
ALTER TABLE [dbo].[EmpSal_Detail]
ADD CONSTRAINT [PK_EmpSal_Detail]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FixedAssetFileDetailInfoes'
ALTER TABLE [dbo].[FixedAssetFileDetailInfoes]
ADD CONSTRAINT [PK_FixedAssetFileDetailInfoes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FixedAssetInformations'
ALTER TABLE [dbo].[FixedAssetInformations]
ADD CONSTRAINT [PK_FixedAssetInformations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TypeOfDetailSalaries'
ALTER TABLE [dbo].[TypeOfDetailSalaries]
ADD CONSTRAINT [PK_TypeOfDetailSalaries]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [PK_Clients]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Charts'
ALTER TABLE [dbo].[Charts]
ADD CONSTRAINT [PK_Charts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SettlementId] in table 'SettlementOfStores'
ALTER TABLE [dbo].[SettlementOfStores]
ADD CONSTRAINT [PK_SettlementOfStores]
    PRIMARY KEY CLUSTERED ([SettlementId] ASC);
GO

-- Creating primary key on [Settlement_DetailsId] in table 'SettlementOfStoresDetails'
ALTER TABLE [dbo].[SettlementOfStoresDetails]
ADD CONSTRAINT [PK_SettlementOfStoresDetails]
    PRIMARY KEY CLUSTERED ([Settlement_DetailsId] ASC);
GO

-- Creating primary key on [SettlementId] in table 'SettlementOfStoresTemps'
ALTER TABLE [dbo].[SettlementOfStoresTemps]
ADD CONSTRAINT [PK_SettlementOfStoresTemps]
    PRIMARY KEY CLUSTERED ([SettlementId] ASC);
GO

-- Creating primary key on [Settlement_DetailsId] in table 'SettlementOfStoresDetailsTemps'
ALTER TABLE [dbo].[SettlementOfStoresDetailsTemps]
ADD CONSTRAINT [PK_SettlementOfStoresDetailsTemps]
    PRIMARY KEY CLUSTERED ([Settlement_DetailsId] ASC);
GO

-- Creating primary key on [ProjectRect_ID] in table 'ProjectRecipts'
ALTER TABLE [dbo].[ProjectRecipts]
ADD CONSTRAINT [PK_ProjectRecipts]
    PRIMARY KEY CLUSTERED ([ProjectRect_ID] ASC);
GO

-- Creating primary key on [Id] in table 'POSDetailPayments'
ALTER TABLE [dbo].[POSDetailPayments]
ADD CONSTRAINT [PK_POSDetailPayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [POS_Id] in table 'PointOfSales'
ALTER TABLE [dbo].[PointOfSales]
ADD CONSTRAINT [PK_PointOfSales]
    PRIMARY KEY CLUSTERED ([POS_Id] ASC);
GO

-- Creating primary key on [MasterPOSInvoiceId] in table 'MasterPOSInvoices'
ALTER TABLE [dbo].[MasterPOSInvoices]
ADD CONSTRAINT [PK_MasterPOSInvoices]
    PRIMARY KEY CLUSTERED ([MasterPOSInvoiceId] ASC);
GO

-- Creating primary key on [Detail_Id] in table 'DetailPOSInvoices'
ALTER TABLE [dbo].[DetailPOSInvoices]
ADD CONSTRAINT [PK_DetailPOSInvoices]
    PRIMARY KEY CLUSTERED ([Detail_Id] ASC);
GO

-- Creating primary key on [Id] in table 'PosPaymentMethods'
ALTER TABLE [dbo].[PosPaymentMethods]
ADD CONSTRAINT [PK_PosPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReversePOSInvoices'
ALTER TABLE [dbo].[ReversePOSInvoices]
ADD CONSTRAINT [PK_ReversePOSInvoices]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReversePOSDetails'
ALTER TABLE [dbo].[ReversePOSDetails]
ADD CONSTRAINT [PK_ReversePOSDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Id] in table 'POSDetailPaymentTemps'
ALTER TABLE [dbo].[POSDetailPaymentTemps]
ADD CONSTRAINT [PK_POSDetailPaymentTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [POS_Id] in table 'PointOfSaleTemps'
ALTER TABLE [dbo].[PointOfSaleTemps]
ADD CONSTRAINT [PK_PointOfSaleTemps]
    PRIMARY KEY CLUSTERED ([POS_Id] ASC);
GO

-- Creating primary key on [MasterPOSInvoiceId] in table 'MasterPOSInvoiceTemps'
ALTER TABLE [dbo].[MasterPOSInvoiceTemps]
ADD CONSTRAINT [PK_MasterPOSInvoiceTemps]
    PRIMARY KEY CLUSTERED ([MasterPOSInvoiceId] ASC);
GO

-- Creating primary key on [Id] in table 'PosPaymentMethodTemps'
ALTER TABLE [dbo].[PosPaymentMethodTemps]
ADD CONSTRAINT [PK_PosPaymentMethodTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Detail_Id] in table 'DetailPOSInvoiceTemps'
ALTER TABLE [dbo].[DetailPOSInvoiceTemps]
ADD CONSTRAINT [PK_DetailPOSInvoiceTemps]
    PRIMARY KEY CLUSTERED ([Detail_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReversePOSInvoiceTemps'
ALTER TABLE [dbo].[ReversePOSInvoiceTemps]
ADD CONSTRAINT [PK_ReversePOSInvoiceTemps]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReversePOSDetailTemps'
ALTER TABLE [dbo].[ReversePOSDetailTemps]
ADD CONSTRAINT [PK_ReversePOSDetailTemps]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubscriptionItemsTemps'
ALTER TABLE [dbo].[SubscriptionItemsTemps]
ADD CONSTRAINT [PK_SubscriptionItemsTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubscriptionTemps'
ALTER TABLE [dbo].[SubscriptionTemps]
ADD CONSTRAINT [PK_SubscriptionTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExpenseAccountTemps'
ALTER TABLE [dbo].[ExpenseAccountTemps]
ADD CONSTRAINT [PK_ExpenseAccountTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectDetails'
ALTER TABLE [dbo].[ProjectDetails]
ADD CONSTRAINT [PK_ProjectDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Project_Id] in table 'ProjectTemps'
ALTER TABLE [dbo].[ProjectTemps]
ADD CONSTRAINT [PK_ProjectTemps]
    PRIMARY KEY CLUSTERED ([Project_Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectDetailTemps'
ALTER TABLE [dbo].[ProjectDetailTemps]
ADD CONSTRAINT [PK_ProjectDetailTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StoreCards'
ALTER TABLE [dbo].[StoreCards]
ADD CONSTRAINT [PK_StoreCards]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'LockCodes'
ALTER TABLE [dbo].[LockCodes]
ADD CONSTRAINT [PK_LockCodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectPaymentFiles'
ALTER TABLE [dbo].[ProjectPaymentFiles]
ADD CONSTRAINT [PK_ProjectPaymentFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectReceiptFiles'
ALTER TABLE [dbo].[ProjectReceiptFiles]
ADD CONSTRAINT [PK_ProjectReceiptFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SubscriptionInvoiceFiles'
ALTER TABLE [dbo].[SubscriptionInvoiceFiles]
ADD CONSTRAINT [PK_SubscriptionInvoiceFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'GLMasterFiles'
ALTER TABLE [dbo].[GLMasterFiles]
ADD CONSTRAINT [PK_GLMasterFiles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Packages'
ALTER TABLE [dbo].[Packages]
ADD CONSTRAINT [PK_Packages]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OperationRuns'
ALTER TABLE [dbo].[OperationRuns]
ADD CONSTRAINT [PK_OperationRuns]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmpVacations'
ALTER TABLE [dbo].[EmpVacations]
ADD CONSTRAINT [PK_EmpVacations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmpVacationTemps'
ALTER TABLE [dbo].[EmpVacationTemps]
ADD CONSTRAINT [PK_EmpVacationTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VacationEmgrations'
ALTER TABLE [dbo].[VacationEmgrations]
ADD CONSTRAINT [PK_VacationEmgrations]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ChartDates'
ALTER TABLE [dbo].[ChartDates]
ADD CONSTRAINT [PK_ChartDates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceTaxes'
ALTER TABLE [dbo].[InvoiceTaxes]
ADD CONSTRAINT [PK_InvoiceTaxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SalesReceiptTaxes'
ALTER TABLE [dbo].[SalesReceiptTaxes]
ADD CONSTRAINT [PK_SalesReceiptTaxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaxPOSInvoices'
ALTER TABLE [dbo].[TaxPOSInvoices]
ADD CONSTRAINT [PK_TaxPOSInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OfferTaxes'
ALTER TABLE [dbo].[OfferTaxes]
ADD CONSTRAINT [PK_OfferTaxes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductFileDetails'
ALTER TABLE [dbo].[ProductFileDetails]
ADD CONSTRAINT [PK_ProductFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'UserConnections'
ALTER TABLE [dbo].[UserConnections]
ADD CONSTRAINT [PK_UserConnections]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'PrintBarcodes'
ALTER TABLE [dbo].[PrintBarcodes]
ADD CONSTRAINT [PK_PrintBarcodes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductCompounds'
ALTER TABLE [dbo].[ProductCompounds]
ADD CONSTRAINT [PK_ProductCompounds]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProductCompoundTemps'
ALTER TABLE [dbo].[ProductCompoundTemps]
ADD CONSTRAINT [PK_ProductCompoundTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderOperationals'
ALTER TABLE [dbo].[MaintenanceOrderOperationals]
ADD CONSTRAINT [PK_MaintenanceOrderOperationals]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderOperationalDetails'
ALTER TABLE [dbo].[MaintenanceOrderOperationalDetails]
ADD CONSTRAINT [PK_MaintenanceOrderOperationalDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderOperationalStatus'
ALTER TABLE [dbo].[MaintenanceOrderOperationalStatus]
ADD CONSTRAINT [PK_MaintenanceOrderOperationalStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceStatus'
ALTER TABLE [dbo].[MaintenanceStatus]
ADD CONSTRAINT [PK_MaintenanceStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderOperationalDetailTemps'
ALTER TABLE [dbo].[MaintenanceOrderOperationalDetailTemps]
ADD CONSTRAINT [PK_MaintenanceOrderOperationalDetailTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderOperationalTemps'
ALTER TABLE [dbo].[MaintenanceOrderOperationalTemps]
ADD CONSTRAINT [PK_MaintenanceOrderOperationalTemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderDetails'
ALTER TABLE [dbo].[MaintenanceOrderDetails]
ADD CONSTRAINT [PK_MaintenanceOrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrders'
ALTER TABLE [dbo].[MaintenanceOrders]
ADD CONSTRAINT [PK_MaintenanceOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderStatus'
ALTER TABLE [dbo].[MaintenanceOrderStatus]
ADD CONSTRAINT [PK_MaintenanceOrderStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MaintenanceOrderCodings'
ALTER TABLE [dbo].[MaintenanceOrderCodings]
ADD CONSTRAINT [PK_MaintenanceOrderCodings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Orders'
ALTER TABLE [dbo].[Orders]
ADD CONSTRAINT [PK_Orders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [PK_OrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TempOrderDetails'
ALTER TABLE [dbo].[TempOrderDetails]
ADD CONSTRAINT [PK_TempOrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TempOrders'
ALTER TABLE [dbo].[TempOrders]
ADD CONSTRAINT [PK_TempOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoldedOrders'
ALTER TABLE [dbo].[HoldedOrders]
ADD CONSTRAINT [PK_HoldedOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HoldedOrderDetails'
ALTER TABLE [dbo].[HoldedOrderDetails]
ADD CONSTRAINT [PK_HoldedOrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReverseOrders'
ALTER TABLE [dbo].[ReverseOrders]
ADD CONSTRAINT [PK_ReverseOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReverseOrderDetails'
ALTER TABLE [dbo].[ReverseOrderDetails]
ADD CONSTRAINT [PK_ReverseOrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Detail_Id] in table 'DetailQRInvoices'
ALTER TABLE [dbo].[DetailQRInvoices]
ADD CONSTRAINT [PK_DetailQRInvoices]
    PRIMARY KEY CLUSTERED ([Detail_Id] ASC);
GO

-- Creating primary key on [Id] in table 'QRPaymentMethods'
ALTER TABLE [dbo].[QRPaymentMethods]
ADD CONSTRAINT [PK_QRPaymentMethods]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MasterQRInvoices'
ALTER TABLE [dbo].[MasterQRInvoices]
ADD CONSTRAINT [PK_MasterQRInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'TaxQRInvoices'
ALTER TABLE [dbo].[TaxQRInvoices]
ADD CONSTRAINT [PK_TaxQRInvoices]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StoreEmployees'
ALTER TABLE [dbo].[StoreEmployees]
ADD CONSTRAINT [PK_StoreEmployees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExtrnalSalesEmployees'
ALTER TABLE [dbo].[ExtrnalSalesEmployees]
ADD CONSTRAINT [PK_ExtrnalSalesEmployees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'EmployeeSalesDetails'
ALTER TABLE [dbo].[EmployeeSalesDetails]
ADD CONSTRAINT [PK_EmployeeSalesDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ExternalEmployeePayments'
ALTER TABLE [dbo].[ExternalEmployeePayments]
ADD CONSTRAINT [PK_ExternalEmployeePayments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [PK_RoleFunctions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemFunctions'
ALTER TABLE [dbo].[SystemFunctions]
ADD CONSTRAINT [PK_SystemFunctions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SystemRoles'
ALTER TABLE [dbo].[SystemRoles]
ADD CONSTRAINT [PK_SystemRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectClauses'
ALTER TABLE [dbo].[ProjectClauses]
ADD CONSTRAINT [PK_ProjectClauses]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [SubLed_Id] in table 'SubLedgertemps'
ALTER TABLE [dbo].[SubLedgertemps]
ADD CONSTRAINT [PK_SubLedgertemps]
    PRIMARY KEY CLUSTERED ([SubLed_Id] ASC);
GO

-- Creating primary key on [Id] in table 'ProjectClausetemps'
ALTER TABLE [dbo].[ProjectClausetemps]
ADD CONSTRAINT [PK_ProjectClausetemps]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpentReciptOrders'
ALTER TABLE [dbo].[SpentReciptOrders]
ADD CONSTRAINT [PK_SpentReciptOrders]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SpentReciptOrderTypes'
ALTER TABLE [dbo].[SpentReciptOrderTypes]
ADD CONSTRAINT [PK_SpentReciptOrderTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'FixedAssetDetails'
ALTER TABLE [dbo].[FixedAssetDetails]
ADD CONSTRAINT [PK_FixedAssetDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReceiptPOSFileDetails'
ALTER TABLE [dbo].[ReceiptPOSFileDetails]
ADD CONSTRAINT [PK_ReceiptPOSFileDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Rec_ID] in table 'POSReceipts'
ALTER TABLE [dbo].[POSReceipts]
ADD CONSTRAINT [PK_POSReceipts]
    PRIMARY KEY CLUSTERED ([Rec_ID] ASC);
GO

-- Creating primary key on [Details_Id] in table 'ReverseQRDetails'
ALTER TABLE [dbo].[ReverseQRDetails]
ADD CONSTRAINT [PK_ReverseQRDetails]
    PRIMARY KEY CLUSTERED ([Details_Id] ASC);
GO

-- Creating primary key on [Master_Id] in table 'ReverseQRInvoices'
ALTER TABLE [dbo].[ReverseQRInvoices]
ADD CONSTRAINT [PK_ReverseQRInvoices]
    PRIMARY KEY CLUSTERED ([Master_Id] ASC);
GO

-- Creating primary key on [Id] in table 'PaymentMethodEmployees'
ALTER TABLE [dbo].[PaymentMethodEmployees]
ADD CONSTRAINT [PK_PaymentMethodEmployees]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceSerials'
ALTER TABLE [dbo].[InvoiceSerials]
ADD CONSTRAINT [PK_InvoiceSerials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ReveseInvoiceSerials'
ALTER TABLE [dbo].[ReveseInvoiceSerials]
ADD CONSTRAINT [PK_ReveseInvoiceSerials]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'MedianSubscriptions'
ALTER TABLE [dbo].[MedianSubscriptions]
ADD CONSTRAINT [PK_MedianSubscriptions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'HRNotifications'
ALTER TABLE [dbo].[HRNotifications]
ADD CONSTRAINT [PK_HRNotifications]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [PK_Shifts]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'ShiftCodings'
ALTER TABLE [dbo].[ShiftCodings]
ADD CONSTRAINT [PK_ShiftCodings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VacationWeeklies'
ALTER TABLE [dbo].[VacationWeeklies]
ADD CONSTRAINT [PK_VacationWeeklies]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'VacationWeeklyCodings'
ALTER TABLE [dbo].[VacationWeeklyCodings]
ADD CONSTRAINT [PK_VacationWeeklyCodings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AttendingAndLeavings'
ALTER TABLE [dbo].[AttendingAndLeavings]
ADD CONSTRAINT [PK_AttendingAndLeavings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RewardsAndPenalties'
ALTER TABLE [dbo].[RewardsAndPenalties]
ADD CONSTRAINT [PK_RewardsAndPenalties]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RewardsAndPenaltiesCodingCategories'
ALTER TABLE [dbo].[RewardsAndPenaltiesCodingCategories]
ADD CONSTRAINT [PK_RewardsAndPenaltiesCodingCategories]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'RewardsAndPenaltiesCodings'
ALTER TABLE [dbo].[RewardsAndPenaltiesCodings]
ADD CONSTRAINT [PK_RewardsAndPenaltiesCodings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OfficialHolidays'
ALTER TABLE [dbo].[OfficialHolidays]
ADD CONSTRAINT [PK_OfficialHolidays]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoicePrintSettings'
ALTER TABLE [dbo].[InvoicePrintSettings]
ADD CONSTRAINT [PK_InvoicePrintSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'InvoiceWordPrintSettings'
ALTER TABLE [dbo].[InvoiceWordPrintSettings]
ADD CONSTRAINT [PK_InvoiceWordPrintSettings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Questions'
ALTER TABLE [dbo].[Questions]
ADD CONSTRAINT [PK_Questions]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [PK_Answers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Sewings'
ALTER TABLE [dbo].[Sewings]
ADD CONSTRAINT [PK_Sewings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SewingDetails'
ALTER TABLE [dbo].[SewingDetails]
ADD CONSTRAINT [PK_SewingDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'SewingStatus'
ALTER TABLE [dbo].[SewingStatus]
ADD CONSTRAINT [PK_SewingStatus]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'OrderTypes'
ALTER TABLE [dbo].[OrderTypes]
ADD CONSTRAINT [PK_OrderTypes]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AdditionalOrderDetails'
ALTER TABLE [dbo].[AdditionalOrderDetails]
ADD CONSTRAINT [PK_AdditionalOrderDetails]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'StartAndEndSellings'
ALTER TABLE [dbo].[StartAndEndSellings]
ADD CONSTRAINT [PK_StartAndEndSellings]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [Product_ID] in table 'ProductBatches'
ALTER TABLE [dbo].[ProductBatches]
ADD CONSTRAINT [FK_ProductProductBatches]
    FOREIGN KEY ([Product_ID])
    REFERENCES [dbo].[Products]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductBatches'
CREATE INDEX [IX_FK_ProductProductBatches]
ON [dbo].[ProductBatches]
    ([Product_ID]);
GO

-- Creating foreign key on [Product_ID] in table 'SlicesProducts'
ALTER TABLE [dbo].[SlicesProducts]
ADD CONSTRAINT [FK_ProductSlicesProduct]
    FOREIGN KEY ([Product_ID])
    REFERENCES [dbo].[Products]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductSlicesProduct'
CREATE INDEX [IX_FK_ProductSlicesProduct]
ON [dbo].[SlicesProducts]
    ([Product_ID]);
GO

-- Creating foreign key on [BranchBranch_Id] in table 'BranchDepartments'
ALTER TABLE [dbo].[BranchDepartments]
ADD CONSTRAINT [FK_BranchBranchDepartment]
    FOREIGN KEY ([BranchBranch_Id])
    REFERENCES [dbo].[Branches]
        ([Branch_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BranchBranchDepartment'
CREATE INDEX [IX_FK_BranchBranchDepartment]
ON [dbo].[BranchDepartments]
    ([BranchBranch_Id]);
GO

-- Creating foreign key on [CompanyComp_Id] in table 'Branches'
ALTER TABLE [dbo].[Branches]
ADD CONSTRAINT [FK_CompanyBranch]
    FOREIGN KEY ([CompanyComp_Id])
    REFERENCES [dbo].[Companies]
        ([Comp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyBranch'
CREATE INDEX [IX_FK_CompanyBranch]
ON [dbo].[Branches]
    ([CompanyComp_Id]);
GO

-- Creating foreign key on [PartnerPart_Id] in table 'PartnerFileDetails'
ALTER TABLE [dbo].[PartnerFileDetails]
ADD CONSTRAINT [FK_PartnerPartnerFileDetail]
    FOREIGN KEY ([PartnerPart_Id])
    REFERENCES [dbo].[Partners]
        ([Part_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PartnerPartnerFileDetail'
CREATE INDEX [IX_FK_PartnerPartnerFileDetail]
ON [dbo].[PartnerFileDetails]
    ([PartnerPart_Id]);
GO

-- Creating foreign key on [CompanyComp_Id] in table 'CompanyDepartments'
ALTER TABLE [dbo].[CompanyDepartments]
ADD CONSTRAINT [FK_CompanyCompanyDepartments]
    FOREIGN KEY ([CompanyComp_Id])
    REFERENCES [dbo].[Companies]
        ([Comp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompanyDepartments'
CREATE INDEX [IX_FK_CompanyCompanyDepartments]
ON [dbo].[CompanyDepartments]
    ([CompanyComp_Id]);
GO

-- Creating foreign key on [CountryId] in table 'Provinces'
ALTER TABLE [dbo].[Provinces]
ADD CONSTRAINT [FK_CountryProvince]
    FOREIGN KEY ([CountryId])
    REFERENCES [dbo].[Countries]
        ([Country_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CountryProvince'
CREATE INDEX [IX_FK_CountryProvince]
ON [dbo].[Provinces]
    ([CountryId]);
GO

-- Creating foreign key on [ProvinceId] in table 'Cities'
ALTER TABLE [dbo].[Cities]
ADD CONSTRAINT [FK_ProvinceCities]
    FOREIGN KEY ([ProvinceId])
    REFERENCES [dbo].[Provinces]
        ([Province_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProvinceCities'
CREATE INDEX [IX_FK_ProvinceCities]
ON [dbo].[Cities]
    ([ProvinceId]);
GO

-- Creating foreign key on [ExpenseExpense_Id] in table 'ExpenseFileDetails'
ALTER TABLE [dbo].[ExpenseFileDetails]
ADD CONSTRAINT [FK_ExpenseExpenseFileDetail]
    FOREIGN KEY ([ExpenseExpense_Id])
    REFERENCES [dbo].[Expenses]
        ([Expense_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExpenseExpenseFileDetail'
CREATE INDEX [IX_FK_ExpenseExpenseFileDetail]
ON [dbo].[ExpenseFileDetails]
    ([ExpenseExpense_Id]);
GO

-- Creating foreign key on [GLMasterGLMaster_Id] in table 'GLDetails'
ALTER TABLE [dbo].[GLDetails]
ADD CONSTRAINT [FK_GLMasterGLDetail]
    FOREIGN KEY ([GLMasterGLMaster_Id])
    REFERENCES [dbo].[GLMasters]
        ([GLMaster_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GLMasterGLDetail'
CREATE INDEX [IX_FK_GLMasterGLDetail]
ON [dbo].[GLDetails]
    ([GLMasterGLMaster_Id]);
GO

-- Creating foreign key on [ChartOfAccountingAccount_Id] in table 'ChartOfAccountings'
ALTER TABLE [dbo].[ChartOfAccountings]
ADD CONSTRAINT [FK_ChartOfAccountingChartOfAccounting]
    FOREIGN KEY ([ChartOfAccountingAccount_Id])
    REFERENCES [dbo].[ChartOfAccountings]
        ([Account_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChartOfAccountingChartOfAccounting'
CREATE INDEX [IX_FK_ChartOfAccountingChartOfAccounting]
ON [dbo].[ChartOfAccountings]
    ([ChartOfAccountingAccount_Id]);
GO

-- Creating foreign key on [CustomerCustomer_Id] in table 'CustomerFileDetails'
ALTER TABLE [dbo].[CustomerFileDetails]
ADD CONSTRAINT [FK_CustomerCustomerFileDetail]
    FOREIGN KEY ([CustomerCustomer_Id])
    REFERENCES [dbo].[Customers]
        ([Customer_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerCustomerFileDetail'
CREATE INDEX [IX_FK_CustomerCustomerFileDetail]
ON [dbo].[CustomerFileDetails]
    ([CustomerCustomer_Id]);
GO

-- Creating foreign key on [CustomerCustomer_Id] in table 'ContentPersonDetails'
ALTER TABLE [dbo].[ContentPersonDetails]
ADD CONSTRAINT [FK_CustomerContentPersonDetails]
    FOREIGN KEY ([CustomerCustomer_Id])
    REFERENCES [dbo].[Customers]
        ([Customer_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerContentPersonDetails'
CREATE INDEX [IX_FK_CustomerContentPersonDetails]
ON [dbo].[ContentPersonDetails]
    ([CustomerCustomer_Id]);
GO

-- Creating foreign key on [EmployeeEmp_Id] in table 'EmployeeFileDetails'
ALTER TABLE [dbo].[EmployeeFileDetails]
ADD CONSTRAINT [FK_EmployeeEmployeeFileDetail]
    FOREIGN KEY ([EmployeeEmp_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeEmployeeFileDetail'
CREATE INDEX [IX_FK_EmployeeEmployeeFileDetail]
ON [dbo].[EmployeeFileDetails]
    ([EmployeeEmp_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Salary_Components'
ALTER TABLE [dbo].[Salary_Components]
ADD CONSTRAINT [FK_EmployeeSalary_Components]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalary_Components'
CREATE INDEX [IX_FK_EmployeeSalary_Components]
ON [dbo].[Salary_Components]
    ([Employee_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Vacations'
ALTER TABLE [dbo].[Vacations]
ADD CONSTRAINT [FK_EmployeeVacations]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeVacations'
CREATE INDEX [IX_FK_EmployeeVacations]
ON [dbo].[Vacations]
    ([Employee_Id]);
GO

-- Creating foreign key on [Employee_Id] in table 'Emp_Evaluation'
ALTER TABLE [dbo].[Emp_Evaluation]
ADD CONSTRAINT [FK_EmployeeEmp_Evaluation]
    FOREIGN KEY ([Employee_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeEmp_Evaluation'
CREATE INDEX [IX_FK_EmployeeEmp_Evaluation]
ON [dbo].[Emp_Evaluation]
    ([Employee_Id]);
GO

-- Creating foreign key on [SupplierSupp_Id] in table 'SupplierFileDetails'
ALTER TABLE [dbo].[SupplierFileDetails]
ADD CONSTRAINT [FK_SupplierSupplierFileDetail]
    FOREIGN KEY ([SupplierSupp_Id])
    REFERENCES [dbo].[Suppliers]
        ([Supp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SupplierSupplierFileDetail'
CREATE INDEX [IX_FK_SupplierSupplierFileDetail]
ON [dbo].[SupplierFileDetails]
    ([SupplierSupp_Id]);
GO

-- Creating foreign key on [InvoiceMaster_Id] in table 'PurchaseDetails'
ALTER TABLE [dbo].[PurchaseDetails]
ADD CONSTRAINT [FK_InvoiceMasterInvoiceDetails1]
    FOREIGN KEY ([InvoiceMaster_Id])
    REFERENCES [dbo].[PurInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceMasterInvoiceDetails1'
CREATE INDEX [IX_FK_InvoiceMasterInvoiceDetails1]
ON [dbo].[PurchaseDetails]
    ([InvoiceMaster_Id]);
GO

-- Creating foreign key on [FixedAssetID] in table 'FixedAssetImages'
ALTER TABLE [dbo].[FixedAssetImages]
ADD CONSTRAINT [FK_FixedAssetFixedAssetImage]
    FOREIGN KEY ([FixedAssetID])
    REFERENCES [dbo].[FixedAssets]
        ([Fasset_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FixedAssetFixedAssetImage'
CREATE INDEX [IX_FK_FixedAssetFixedAssetImage]
ON [dbo].[FixedAssetImages]
    ([FixedAssetID]);
GO

-- Creating foreign key on [RecalPurchInvoiceMasterMaster_Id] in table 'RecalPurchInvoiceDetails'
ALTER TABLE [dbo].[RecalPurchInvoiceDetails]
ADD CONSTRAINT [FK_RecalPurchInvoiceMasterRecalPurchInvoiceDetails]
    FOREIGN KEY ([RecalPurchInvoiceMasterMaster_Id])
    REFERENCES [dbo].[RecalPurchInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecalPurchInvoiceMasterRecalPurchInvoiceDetails'
CREATE INDEX [IX_FK_RecalPurchInvoiceMasterRecalPurchInvoiceDetails]
ON [dbo].[RecalPurchInvoiceDetails]
    ([RecalPurchInvoiceMasterMaster_Id]);
GO

-- Creating foreign key on [InventoryTransfer_Id] in table 'InventoryTransfer_Detail'
ALTER TABLE [dbo].[InventoryTransfer_Detail]
ADD CONSTRAINT [FK_InventoryTransfer_HdInventoryTransfer_Detail]
    FOREIGN KEY ([InventoryTransfer_Id])
    REFERENCES [dbo].[InventoryTransfer_Hd]
        ([TransferHd_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryTransfer_HdInventoryTransfer_Detail'
CREATE INDEX [IX_FK_InventoryTransfer_HdInventoryTransfer_Detail]
ON [dbo].[InventoryTransfer_Detail]
    ([InventoryTransfer_Id]);
GO

-- Creating foreign key on [ReceiptRec_ID] in table 'Matchings'
ALTER TABLE [dbo].[Matchings]
ADD CONSTRAINT [FK_ReceiptMatching]
    FOREIGN KEY ([ReceiptRec_ID])
    REFERENCES [dbo].[Receipts]
        ([Rec_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReceiptMatching'
CREATE INDEX [IX_FK_ReceiptMatching]
ON [dbo].[Matchings]
    ([ReceiptRec_ID]);
GO

-- Creating foreign key on [PaymentPay_ID] in table 'MatchingPayments'
ALTER TABLE [dbo].[MatchingPayments]
ADD CONSTRAINT [FK_PaymentMatchingPayment]
    FOREIGN KEY ([PaymentPay_ID])
    REFERENCES [dbo].[Payments]
        ([Pay_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentMatchingPayment'
CREATE INDEX [IX_FK_PaymentMatchingPayment]
ON [dbo].[MatchingPayments]
    ([PaymentPay_ID]);
GO

-- Creating foreign key on [OfferPriceMasterMaster_Id] in table 'OfferPriceDetails'
ALTER TABLE [dbo].[OfferPriceDetails]
ADD CONSTRAINT [FK_OfferPriceMasterOfferPriceDetail]
    FOREIGN KEY ([OfferPriceMasterMaster_Id])
    REFERENCES [dbo].[OfferPriceMasters]
        ([OfferMas_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferPriceMasterOfferPriceDetail'
CREATE INDEX [IX_FK_OfferPriceMasterOfferPriceDetail]
ON [dbo].[OfferPriceDetails]
    ([OfferPriceMasterMaster_Id]);
GO

-- Creating foreign key on [CostDistribution_ID] in table 'CostDistributionDetails'
ALTER TABLE [dbo].[CostDistributionDetails]
ADD CONSTRAINT [FK_CostDistributionCostDistributionDetails]
    FOREIGN KEY ([CostDistribution_ID])
    REFERENCES [dbo].[CostDistributions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CostDistributionCostDistributionDetails'
CREATE INDEX [IX_FK_CostDistributionCostDistributionDetails]
ON [dbo].[CostDistributionDetails]
    ([CostDistribution_ID]);
GO

-- Creating foreign key on [ReceiptRec_ID] in table 'ReceiptFileDetails'
ALTER TABLE [dbo].[ReceiptFileDetails]
ADD CONSTRAINT [FK_ReceiptReceiptFileDetail]
    FOREIGN KEY ([ReceiptRec_ID])
    REFERENCES [dbo].[Receipts]
        ([Rec_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReceiptReceiptFileDetail'
CREATE INDEX [IX_FK_ReceiptReceiptFileDetail]
ON [dbo].[ReceiptFileDetails]
    ([ReceiptRec_ID]);
GO

-- Creating foreign key on [PaymentPay_ID] in table 'PaymentFileDetails'
ALTER TABLE [dbo].[PaymentFileDetails]
ADD CONSTRAINT [FK_PaymentPaymentFileDetail]
    FOREIGN KEY ([PaymentPay_ID])
    REFERENCES [dbo].[Payments]
        ([Pay_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentPaymentFileDetail'
CREATE INDEX [IX_FK_PaymentPaymentFileDetail]
ON [dbo].[PaymentFileDetails]
    ([PaymentPay_ID]);
GO

-- Creating foreign key on [FixedAssetFasset_ID] in table 'FixedAssetFileDetails'
ALTER TABLE [dbo].[FixedAssetFileDetails]
ADD CONSTRAINT [FK_FixedAssetFixedAssetFileDetail]
    FOREIGN KEY ([FixedAssetFasset_ID])
    REFERENCES [dbo].[FixedAssets]
        ([Fasset_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FixedAssetFixedAssetFileDetail'
CREATE INDEX [IX_FK_FixedAssetFixedAssetFileDetail]
ON [dbo].[FixedAssetFileDetails]
    ([FixedAssetFasset_ID]);
GO

-- Creating foreign key on [ProjectProject_Id] in table 'ProjectFileDetails'
ALTER TABLE [dbo].[ProjectFileDetails]
ADD CONSTRAINT [FK_ProjectProjectFileDetail]
    FOREIGN KEY ([ProjectProject_Id])
    REFERENCES [dbo].[Projects]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectProjectFileDetail'
CREATE INDEX [IX_FK_ProjectProjectFileDetail]
ON [dbo].[ProjectFileDetails]
    ([ProjectProject_Id]);
GO

-- Creating foreign key on [InvoiceMaster_Id] in table 'InvoiceDetailsTemps'
ALTER TABLE [dbo].[InvoiceDetailsTemps]
ADD CONSTRAINT [FK_InvoiceMasterInvoiceDetails3]
    FOREIGN KEY ([InvoiceMaster_Id])
    REFERENCES [dbo].[InvoiceMasterTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceMasterInvoiceDetails3'
CREATE INDEX [IX_FK_InvoiceMasterInvoiceDetails3]
ON [dbo].[InvoiceDetailsTemps]
    ([InvoiceMaster_Id]);
GO

-- Creating foreign key on [InvoiceMasterMaster_Id] in table 'SalesFileDetails'
ALTER TABLE [dbo].[SalesFileDetails]
ADD CONSTRAINT [FK_InvoiceMasterSalesFileDetail]
    FOREIGN KEY ([InvoiceMasterMaster_Id])
    REFERENCES [dbo].[InvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceMasterSalesFileDetail'
CREATE INDEX [IX_FK_InvoiceMasterSalesFileDetail]
ON [dbo].[SalesFileDetails]
    ([InvoiceMasterMaster_Id]);
GO

-- Creating foreign key on [PurInvoiceMasterTempMaster_Id] in table 'PurchaseDetailsTemps'
ALTER TABLE [dbo].[PurchaseDetailsTemps]
ADD CONSTRAINT [FK_PurInvoiceMasterTempPurchaseDetailsTemp]
    FOREIGN KEY ([PurInvoiceMasterTempMaster_Id])
    REFERENCES [dbo].[PurInvoiceMasterTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurInvoiceMasterTempPurchaseDetailsTemp'
CREATE INDEX [IX_FK_PurInvoiceMasterTempPurchaseDetailsTemp]
ON [dbo].[PurchaseDetailsTemps]
    ([PurInvoiceMasterTempMaster_Id]);
GO

-- Creating foreign key on [PurInvoiceMasterMaster_Id] in table 'PurchaseFileDetails'
ALTER TABLE [dbo].[PurchaseFileDetails]
ADD CONSTRAINT [FK_PurInvoiceMasterPurchaseFileDetail]
    FOREIGN KEY ([PurInvoiceMasterMaster_Id])
    REFERENCES [dbo].[PurInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurInvoiceMasterPurchaseFileDetail'
CREATE INDEX [IX_FK_PurInvoiceMasterPurchaseFileDetail]
ON [dbo].[PurchaseFileDetails]
    ([PurInvoiceMasterMaster_Id]);
GO

-- Creating foreign key on [RecallPurchInvoicMasterTempMaster_Id] in table 'RecallPurchInvDetailsTemps'
ALTER TABLE [dbo].[RecallPurchInvDetailsTemps]
ADD CONSTRAINT [FK_RecallPurchInvoicMasterTempRecallPurchInvDetailsTemp]
    FOREIGN KEY ([RecallPurchInvoicMasterTempMaster_Id])
    REFERENCES [dbo].[RecallPurchInvoicMasterTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecallPurchInvoicMasterTempRecallPurchInvDetailsTemp'
CREATE INDEX [IX_FK_RecallPurchInvoicMasterTempRecallPurchInvDetailsTemp]
ON [dbo].[RecallPurchInvDetailsTemps]
    ([RecallPurchInvoicMasterTempMaster_Id]);
GO

-- Creating foreign key on [RecalPurchInvoiceMasterMaster_Id] in table 'RecallPurchFileDetails'
ALTER TABLE [dbo].[RecallPurchFileDetails]
ADD CONSTRAINT [FK_RecalPurchInvoiceMasterRecallPurchFileDetails]
    FOREIGN KEY ([RecalPurchInvoiceMasterMaster_Id])
    REFERENCES [dbo].[RecalPurchInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RecalPurchInvoiceMasterRecallPurchFileDetails'
CREATE INDEX [IX_FK_RecalPurchInvoiceMasterRecallPurchFileDetails]
ON [dbo].[RecallPurchFileDetails]
    ([RecalPurchInvoiceMasterMaster_Id]);
GO

-- Creating foreign key on [ReverseSalesInvoiceMaster_Id] in table 'ReversSalesFileDetails'
ALTER TABLE [dbo].[ReversSalesFileDetails]
ADD CONSTRAINT [FK_ReverseSalesInvoiceMasterReversSalesFileDetail]
    FOREIGN KEY ([ReverseSalesInvoiceMaster_Id])
    REFERENCES [dbo].[ReverseSalesInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseSalesInvoiceMasterReversSalesFileDetail'
CREATE INDEX [IX_FK_ReverseSalesInvoiceMasterReversSalesFileDetail]
ON [dbo].[ReversSalesFileDetails]
    ([ReverseSalesInvoiceMaster_Id]);
GO

-- Creating foreign key on [ReverseSalesInvoiceMasterTempMaster_Id] in table 'ReveInvoiceDetailsTemps'
ALTER TABLE [dbo].[ReveInvoiceDetailsTemps]
ADD CONSTRAINT [FK_ReverseSalesInvoiceMasterTempReveInvoiceDetailsTemp]
    FOREIGN KEY ([ReverseSalesInvoiceMasterTempMaster_Id])
    REFERENCES [dbo].[ReverseSalesInvoiceMasterTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseSalesInvoiceMasterTempReveInvoiceDetailsTemp'
CREATE INDEX [IX_FK_ReverseSalesInvoiceMasterTempReveInvoiceDetailsTemp]
ON [dbo].[ReveInvoiceDetailsTemps]
    ([ReverseSalesInvoiceMasterTempMaster_Id]);
GO

-- Creating foreign key on [InventoryTransfer_Temp_Id] in table 'InventoryTransfer_DetailTemp'
ALTER TABLE [dbo].[InventoryTransfer_DetailTemp]
ADD CONSTRAINT [FK_InventoryTransfer_HdTempInventoryTransfer_DetailTemp]
    FOREIGN KEY ([InventoryTransfer_Temp_Id])
    REFERENCES [dbo].[InventoryTransfer_HdTemp]
        ([TransferHd_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InventoryTransfer_HdTempInventoryTransfer_DetailTemp'
CREATE INDEX [IX_FK_InventoryTransfer_HdTempInventoryTransfer_DetailTemp]
ON [dbo].[InventoryTransfer_DetailTemp]
    ([InventoryTransfer_Temp_Id]);
GO

-- Creating foreign key on [SalesReceiptMasterTempMaster_Id] in table 'SalesReceiptDetailsTemps'
ALTER TABLE [dbo].[SalesReceiptDetailsTemps]
ADD CONSTRAINT [FK_SalesReceiptMasterTempSalesReceiptDetailsTemp]
    FOREIGN KEY ([SalesReceiptMasterTempMaster_Id])
    REFERENCES [dbo].[SalesReceiptMasterTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesReceiptMasterTempSalesReceiptDetailsTemp'
CREATE INDEX [IX_FK_SalesReceiptMasterTempSalesReceiptDetailsTemp]
ON [dbo].[SalesReceiptDetailsTemps]
    ([SalesReceiptMasterTempMaster_Id]);
GO

-- Creating foreign key on [Master_Id] in table 'SalesReceiptFiles'
ALTER TABLE [dbo].[SalesReceiptFiles]
ADD CONSTRAINT [FK_SalesReceiptMasterSalesReceiptFile]
    FOREIGN KEY ([Master_Id])
    REFERENCES [dbo].[SalesReceiptMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesReceiptMasterSalesReceiptFile'
CREATE INDEX [IX_FK_SalesReceiptMasterSalesReceiptFile]
ON [dbo].[SalesReceiptFiles]
    ([Master_Id]);
GO

-- Creating foreign key on [Master_Id] in table 'SalesReceiptDetails'
ALTER TABLE [dbo].[SalesReceiptDetails]
ADD CONSTRAINT [FK_SalesReceiptMasterSalesReceiptDetails]
    FOREIGN KEY ([Master_Id])
    REFERENCES [dbo].[SalesReceiptMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesReceiptMasterSalesReceiptDetails'
CREATE INDEX [IX_FK_SalesReceiptMasterSalesReceiptDetails]
ON [dbo].[SalesReceiptDetails]
    ([Master_Id]);
GO

-- Creating foreign key on [InvoiceMaster_Id] in table 'PurchaseOrderDetails'
ALTER TABLE [dbo].[PurchaseOrderDetails]
ADD CONSTRAINT [FK_PurchaseOrderMasterPurchaseOrderDetail]
    FOREIGN KEY ([InvoiceMaster_Id])
    REFERENCES [dbo].[PurchaseOrderMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrderMasterPurchaseOrderDetail'
CREATE INDEX [IX_FK_PurchaseOrderMasterPurchaseOrderDetail]
ON [dbo].[PurchaseOrderDetails]
    ([InvoiceMaster_Id]);
GO

-- Creating foreign key on [PurchaseOrderMasterMaster_Id] in table 'PurchaseFileOrders'
ALTER TABLE [dbo].[PurchaseFileOrders]
ADD CONSTRAINT [FK_PurchaseOrderMasterPurchaseFileOrder]
    FOREIGN KEY ([PurchaseOrderMasterMaster_Id])
    REFERENCES [dbo].[PurchaseOrderMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PurchaseOrderMasterPurchaseFileOrder'
CREATE INDEX [IX_FK_PurchaseOrderMasterPurchaseFileOrder]
ON [dbo].[PurchaseFileOrders]
    ([PurchaseOrderMasterMaster_Id]);
GO

-- Creating foreign key on [TransferToProjectMasterMaster_Id] in table 'TransferToprojectFile1'
ALTER TABLE [dbo].[TransferToprojectFile1]
ADD CONSTRAINT [FK_TransferToProjectMasterTransferToprojectFile1]
    FOREIGN KEY ([TransferToProjectMasterMaster_Id])
    REFERENCES [dbo].[TransferToProjectMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToProjectMasterTransferToprojectFile1'
CREATE INDEX [IX_FK_TransferToProjectMasterTransferToprojectFile1]
ON [dbo].[TransferToprojectFile1]
    ([TransferToProjectMasterMaster_Id]);
GO

-- Creating foreign key on [Master_ID] in table 'TransferToProjectDetailTemps'
ALTER TABLE [dbo].[TransferToProjectDetailTemps]
ADD CONSTRAINT [FK_TransferToProjectTempTransferToProjectDetailTemp]
    FOREIGN KEY ([Master_ID])
    REFERENCES [dbo].[TransferToProjectTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToProjectTempTransferToProjectDetailTemp'
CREATE INDEX [IX_FK_TransferToProjectTempTransferToProjectDetailTemp]
ON [dbo].[TransferToProjectDetailTemps]
    ([Master_ID]);
GO

-- Creating foreign key on [EmployeeTemp_Id] in table 'Salary_ComponentsTemp'
ALTER TABLE [dbo].[Salary_ComponentsTemp]
ADD CONSTRAINT [FK_EmployeTempSalary_ComponentsTemp]
    FOREIGN KEY ([EmployeeTemp_Id])
    REFERENCES [dbo].[EmployeTemps]
        ([EmpTemp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeTempSalary_ComponentsTemp'
CREATE INDEX [IX_FK_EmployeTempSalary_ComponentsTemp]
ON [dbo].[Salary_ComponentsTemp]
    ([EmployeeTemp_Id]);
GO

-- Creating foreign key on [ProjectInvoiceMasterPInvoiceMaster_ID] in table 'ProjectInvoiceDetails'
ALTER TABLE [dbo].[ProjectInvoiceDetails]
ADD CONSTRAINT [FK_ProjectInvoiceMasterProjectInvoiceDetails]
    FOREIGN KEY ([ProjectInvoiceMasterPInvoiceMaster_ID])
    REFERENCES [dbo].[ProjectInvoiceMasters]
        ([PInvoiceMaster_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectInvoiceMasterProjectInvoiceDetails'
CREATE INDEX [IX_FK_ProjectInvoiceMasterProjectInvoiceDetails]
ON [dbo].[ProjectInvoiceDetails]
    ([ProjectInvoiceMasterPInvoiceMaster_ID]);
GO

-- Creating foreign key on [Maintenance_Invoice_MasterMain_Invoice_Id] in table 'Maintenance_Invoice_Detail'
ALTER TABLE [dbo].[Maintenance_Invoice_Detail]
ADD CONSTRAINT [FK_Maintenance_Invoice_MasterMaintenance_Invoice_Detail]
    FOREIGN KEY ([Maintenance_Invoice_MasterMain_Invoice_Id])
    REFERENCES [dbo].[Maintenance_Invoice_Master]
        ([Main_Invoice_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Maintenance_Invoice_MasterMaintenance_Invoice_Detail'
CREATE INDEX [IX_FK_Maintenance_Invoice_MasterMaintenance_Invoice_Detail]
ON [dbo].[Maintenance_Invoice_Detail]
    ([Maintenance_Invoice_MasterMain_Invoice_Id]);
GO

-- Creating foreign key on [Maintenance_Invoice_Master_TempMain_Invoice_Id] in table 'Maintenance_Invoice_Detail_Temp'
ALTER TABLE [dbo].[Maintenance_Invoice_Detail_Temp]
ADD CONSTRAINT [FK_Maintenance_Invoice_Master_TempMaintenance_Invoice_Detail_Temp]
    FOREIGN KEY ([Maintenance_Invoice_Master_TempMain_Invoice_Id])
    REFERENCES [dbo].[Maintenance_Invoice_Master_Temp]
        ([Main_Invoice_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Maintenance_Invoice_Master_TempMaintenance_Invoice_Detail_Temp'
CREATE INDEX [IX_FK_Maintenance_Invoice_Master_TempMaintenance_Invoice_Detail_Temp]
ON [dbo].[Maintenance_Invoice_Detail_Temp]
    ([Maintenance_Invoice_Master_TempMain_Invoice_Id]);
GO

-- Creating foreign key on [SalesReceiptMasterTempMaster_Id] in table 'ReverseSalesReceiptDetailsTemps'
ALTER TABLE [dbo].[ReverseSalesReceiptDetailsTemps]
ADD CONSTRAINT [FK_SalesReceiptMasterTempSalesReceiptDetailsTemp2]
    FOREIGN KEY ([SalesReceiptMasterTempMaster_Id])
    REFERENCES [dbo].[ReverseSalesReceiptMasterTemp1]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesReceiptMasterTempSalesReceiptDetailsTemp2'
CREATE INDEX [IX_FK_SalesReceiptMasterTempSalesReceiptDetailsTemp2]
ON [dbo].[ReverseSalesReceiptDetailsTemps]
    ([SalesReceiptMasterTempMaster_Id]);
GO

-- Creating foreign key on [ReverseSalesReceiptMasterMaster_Id] in table 'ReverseSalesReceiptDetails'
ALTER TABLE [dbo].[ReverseSalesReceiptDetails]
ADD CONSTRAINT [FK_ReverseSalesReceiptMasterReverseSalesReceiptDetails]
    FOREIGN KEY ([ReverseSalesReceiptMasterMaster_Id])
    REFERENCES [dbo].[ReverseSalesReceiptMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseSalesReceiptMasterReverseSalesReceiptDetails'
CREATE INDEX [IX_FK_ReverseSalesReceiptMasterReverseSalesReceiptDetails]
ON [dbo].[ReverseSalesReceiptDetails]
    ([ReverseSalesReceiptMasterMaster_Id]);
GO

-- Creating foreign key on [ReverseSalesReceiptMasterMaster_Id] in table 'ReverseSalesReceiptFiles'
ALTER TABLE [dbo].[ReverseSalesReceiptFiles]
ADD CONSTRAINT [FK_ReverseSalesReceiptMasterReverseSalesReceiptFile]
    FOREIGN KEY ([ReverseSalesReceiptMasterMaster_Id])
    REFERENCES [dbo].[ReverseSalesReceiptMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseSalesReceiptMasterReverseSalesReceiptFile'
CREATE INDEX [IX_FK_ReverseSalesReceiptMasterReverseSalesReceiptFile]
ON [dbo].[ReverseSalesReceiptFiles]
    ([ReverseSalesReceiptMasterMaster_Id]);
GO

-- Creating foreign key on [ProductTemProd_ID] in table 'SlicesTems'
ALTER TABLE [dbo].[SlicesTems]
ADD CONSTRAINT [FK_ProductTemSlicesTem]
    FOREIGN KEY ([ProductTemProd_ID])
    REFERENCES [dbo].[ProductTems]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTemSlicesTem'
CREATE INDEX [IX_FK_ProductTemSlicesTem]
ON [dbo].[SlicesTems]
    ([ProductTemProd_ID]);
GO

-- Creating foreign key on [SubLedgerSubLed_Id] in table 'SubLedgerFileDetail1'
ALTER TABLE [dbo].[SubLedgerFileDetail1]
ADD CONSTRAINT [FK_SubLedgerSubLedgerFileDetail1]
    FOREIGN KEY ([SubLedgerSubLed_Id])
    REFERENCES [dbo].[SubLedgers]
        ([SubLed_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubLedgerSubLedgerFileDetail1'
CREATE INDEX [IX_FK_SubLedgerSubLedgerFileDetail1]
ON [dbo].[SubLedgerFileDetail1]
    ([SubLedgerSubLed_Id]);
GO

-- Creating foreign key on [BranchDepartmentDept_Id] in table 'DepartmentBussinesUnits'
ALTER TABLE [dbo].[DepartmentBussinesUnits]
ADD CONSTRAINT [FK_BranchDepartmentDepartmentBussinesUnit]
    FOREIGN KEY ([BranchDepartmentDept_Id])
    REFERENCES [dbo].[BranchDepartments]
        ([Dept_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_BranchDepartmentDepartmentBussinesUnit'
CREATE INDEX [IX_FK_BranchDepartmentDepartmentBussinesUnit]
ON [dbo].[DepartmentBussinesUnits]
    ([BranchDepartmentDept_Id]);
GO

-- Creating foreign key on [TransferToMaintenanceTempMaster_Id] in table 'TransferToMaintenanceDetailsTemps'
ALTER TABLE [dbo].[TransferToMaintenanceDetailsTemps]
ADD CONSTRAINT [FK_TransferToMaintenanceTempTransferToMaintenanceDetailsTemp]
    FOREIGN KEY ([TransferToMaintenanceTempMaster_Id])
    REFERENCES [dbo].[TransferToMaintenanceTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToMaintenanceTempTransferToMaintenanceDetailsTemp'
CREATE INDEX [IX_FK_TransferToMaintenanceTempTransferToMaintenanceDetailsTemp]
ON [dbo].[TransferToMaintenanceDetailsTemps]
    ([TransferToMaintenanceTempMaster_Id]);
GO

-- Creating foreign key on [TransferToMaintenanceMaster_Id] in table 'TransferToMaintenanceDetails'
ALTER TABLE [dbo].[TransferToMaintenanceDetails]
ADD CONSTRAINT [FK_TransferToMaintenanceTransferToMaintenanceDetails]
    FOREIGN KEY ([TransferToMaintenanceMaster_Id])
    REFERENCES [dbo].[TransferToMaintenances]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToMaintenanceTransferToMaintenanceDetails'
CREATE INDEX [IX_FK_TransferToMaintenanceTransferToMaintenanceDetails]
ON [dbo].[TransferToMaintenanceDetails]
    ([TransferToMaintenanceMaster_Id]);
GO

-- Creating foreign key on [CompanyComp_Id] in table 'CompanyFileDetails'
ALTER TABLE [dbo].[CompanyFileDetails]
ADD CONSTRAINT [FK_CompanyCompanyFileDetail]
    FOREIGN KEY ([CompanyComp_Id])
    REFERENCES [dbo].[Companies]
        ([Comp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompanyFileDetail'
CREATE INDEX [IX_FK_CompanyCompanyFileDetail]
ON [dbo].[CompanyFileDetails]
    ([CompanyComp_Id]);
GO

-- Creating foreign key on [CompanyComp_Id] in table 'CompanyPhotoDetails'
ALTER TABLE [dbo].[CompanyPhotoDetails]
ADD CONSTRAINT [FK_CompanyCompanyPhotoDetail]
    FOREIGN KEY ([CompanyComp_Id])
    REFERENCES [dbo].[Companies]
        ([Comp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CompanyCompanyPhotoDetail'
CREATE INDEX [IX_FK_CompanyCompanyPhotoDetail]
ON [dbo].[CompanyPhotoDetails]
    ([CompanyComp_Id]);
GO

-- Creating foreign key on [CatchReceipt_Id] in table 'CatchReceiptFileDetails'
ALTER TABLE [dbo].[CatchReceiptFileDetails]
ADD CONSTRAINT [FK_CatchReceiptCatchReceiptFileDetail]
    FOREIGN KEY ([CatchReceipt_Id])
    REFERENCES [dbo].[CatchReceipts]
        ([CatchReceipt_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CatchReceiptCatchReceiptFileDetail'
CREATE INDEX [IX_FK_CatchReceiptCatchReceiptFileDetail]
ON [dbo].[CatchReceiptFileDetails]
    ([CatchReceipt_Id]);
GO

-- Creating foreign key on [CustomerTemp_Id] in table 'ContentPersonDetailsTemps'
ALTER TABLE [dbo].[ContentPersonDetailsTemps]
ADD CONSTRAINT [FK_CustomerTempContentPersonTemp]
    FOREIGN KEY ([CustomerTemp_Id])
    REFERENCES [dbo].[CustomerTemps]
        ([Customer_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CustomerTempContentPersonTemp'
CREATE INDEX [IX_FK_CustomerTempContentPersonTemp]
ON [dbo].[ContentPersonDetailsTemps]
    ([CustomerTemp_Id]);
GO

-- Creating foreign key on [SpentReciptSpentReceipt_Id] in table 'SpentReciptFileDetails'
ALTER TABLE [dbo].[SpentReciptFileDetails]
ADD CONSTRAINT [FK_SpentReciptSpentReciptFileDetail]
    FOREIGN KEY ([SpentReciptSpentReceipt_Id])
    REFERENCES [dbo].[SpentRecipts]
        ([SpentReceipt_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SpentReciptSpentReciptFileDetail'
CREATE INDEX [IX_FK_SpentReciptSpentReciptFileDetail]
ON [dbo].[SpentReciptFileDetails]
    ([SpentReciptSpentReceipt_Id]);
GO

-- Creating foreign key on [Sales_SettingsId] in table 'Sales_Setting_Slice'
ALTER TABLE [dbo].[Sales_Setting_Slice]
ADD CONSTRAINT [FK_Sales_SettingsSales_Setting_Slice]
    FOREIGN KEY ([Sales_SettingsId])
    REFERENCES [dbo].[Sales_Settings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Sales_SettingsSales_Setting_Slice'
CREATE INDEX [IX_FK_Sales_SettingsSales_Setting_Slice]
ON [dbo].[Sales_Setting_Slice]
    ([Sales_SettingsId]);
GO

-- Creating foreign key on [SubLedgerSubLed_Id] in table 'SubLedgers'
ALTER TABLE [dbo].[SubLedgers]
ADD CONSTRAINT [FK_SubLedgerSubLedger]
    FOREIGN KEY ([SubLedgerSubLed_Id])
    REFERENCES [dbo].[SubLedgers]
        ([SubLed_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubLedgerSubLedger'
CREATE INDEX [IX_FK_SubLedgerSubLedger]
ON [dbo].[SubLedgers]
    ([SubLedgerSubLed_Id]);
GO

-- Creating foreign key on [InvoiceMaster_Id] in table 'ReveInvoiceDetails'
ALTER TABLE [dbo].[ReveInvoiceDetails]
ADD CONSTRAINT [FK_ReveInvoiceDetailsReverseSalesInvoiceMaster]
    FOREIGN KEY ([InvoiceMaster_Id])
    REFERENCES [dbo].[ReverseSalesInvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReveInvoiceDetailsReverseSalesInvoiceMaster'
CREATE INDEX [IX_FK_ReveInvoiceDetailsReverseSalesInvoiceMaster]
ON [dbo].[ReveInvoiceDetails]
    ([InvoiceMaster_Id]);
GO

-- Creating foreign key on [InvoiceMaster_Id] in table 'InvoiceDetails'
ALTER TABLE [dbo].[InvoiceDetails]
ADD CONSTRAINT [FK_InvoiceDetailsInvoiceMaster]
    FOREIGN KEY ([InvoiceMaster_Id])
    REFERENCES [dbo].[InvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceDetailsInvoiceMaster'
CREATE INDEX [IX_FK_InvoiceDetailsInvoiceMaster]
ON [dbo].[InvoiceDetails]
    ([InvoiceMaster_Id]);
GO

-- Creating foreign key on [Master_ID] in table 'TransferToProjectDetails'
ALTER TABLE [dbo].[TransferToProjectDetails]
ADD CONSTRAINT [FK_TransferToProjectDetailTransferToProjectMaster]
    FOREIGN KEY ([Master_ID])
    REFERENCES [dbo].[TransferToProjectMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TransferToProjectDetailTransferToProjectMaster'
CREATE INDEX [IX_FK_TransferToProjectDetailTransferToProjectMaster]
ON [dbo].[TransferToProjectDetails]
    ([Master_ID]);
GO

-- Creating foreign key on [SlicesSlice_Id] in table 'SlicesProducts'
ALTER TABLE [dbo].[SlicesProducts]
ADD CONSTRAINT [FK_SlicesProductSlices]
    FOREIGN KEY ([SlicesSlice_Id])
    REFERENCES [dbo].[Slices]
        ([Slice_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SlicesProductSlices'
CREATE INDEX [IX_FK_SlicesProductSlices]
ON [dbo].[SlicesProducts]
    ([SlicesSlice_Id]);
GO

-- Creating foreign key on [Account_Id] in table 'GLDetails'
ALTER TABLE [dbo].[GLDetails]
ADD CONSTRAINT [FK_ChartOfAccountingGLDetail]
    FOREIGN KEY ([Account_Id])
    REFERENCES [dbo].[ChartOfAccountings]
        ([Account_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ChartOfAccountingGLDetail'
CREATE INDEX [IX_FK_ChartOfAccountingGLDetail]
ON [dbo].[GLDetails]
    ([Account_Id]);
GO

-- Creating foreign key on [ProjectInvoiceMasterTempId] in table 'ProjectInvoiceDetailTemps'
ALTER TABLE [dbo].[ProjectInvoiceDetailTemps]
ADD CONSTRAINT [FK_ProjectInvoiceMasterTempProjectInvoiceDetailTemp]
    FOREIGN KEY ([ProjectInvoiceMasterTempId])
    REFERENCES [dbo].[ProjectInvoiceMasterTemps]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectInvoiceMasterTempProjectInvoiceDetailTemp'
CREATE INDEX [IX_FK_ProjectInvoiceMasterTempProjectInvoiceDetailTemp]
ON [dbo].[ProjectInvoiceDetailTemps]
    ([ProjectInvoiceMasterTempId]);
GO

-- Creating foreign key on [EmployeeSalaryMonthId] in table 'EmpSal_Detail'
ALTER TABLE [dbo].[EmpSal_Detail]
ADD CONSTRAINT [FK_EmployeeSalaryMonthEmpSal_Detail]
    FOREIGN KEY ([EmployeeSalaryMonthId])
    REFERENCES [dbo].[EmployeeSalaryMonths]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalaryMonthEmpSal_Detail'
CREATE INDEX [IX_FK_EmployeeSalaryMonthEmpSal_Detail]
ON [dbo].[EmpSal_Detail]
    ([EmployeeSalaryMonthId]);
GO

-- Creating foreign key on [FixedAssetInformationId1] in table 'FixedAssetFileDetailInfoes'
ALTER TABLE [dbo].[FixedAssetFileDetailInfoes]
ADD CONSTRAINT [FK_FixedAssetInformationFixedAssetFileDetailInfo]
    FOREIGN KEY ([FixedAssetInformationId1])
    REFERENCES [dbo].[FixedAssetInformations]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FixedAssetInformationFixedAssetFileDetailInfo'
CREATE INDEX [IX_FK_FixedAssetInformationFixedAssetFileDetailInfo]
ON [dbo].[FixedAssetFileDetailInfoes]
    ([FixedAssetInformationId1]);
GO

-- Creating foreign key on [Store_ID] in table 'ProductBatches'
ALTER TABLE [dbo].[ProductBatches]
ADD CONSTRAINT [FK_StoreProductBatches]
    FOREIGN KEY ([Store_ID])
    REFERENCES [dbo].[Stores]
        ([Store_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreProductBatches'
CREATE INDEX [IX_FK_StoreProductBatches]
ON [dbo].[ProductBatches]
    ([Store_ID]);
GO

-- Creating foreign key on [SettlementOfStoresId] in table 'SettlementOfStoresDetails'
ALTER TABLE [dbo].[SettlementOfStoresDetails]
ADD CONSTRAINT [FK_SettlementOfStoresSettlementOfStoresDetails]
    FOREIGN KEY ([SettlementOfStoresId])
    REFERENCES [dbo].[SettlementOfStores]
        ([SettlementId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SettlementOfStoresSettlementOfStoresDetails'
CREATE INDEX [IX_FK_SettlementOfStoresSettlementOfStoresDetails]
ON [dbo].[SettlementOfStoresDetails]
    ([SettlementOfStoresId]);
GO

-- Creating foreign key on [SettlementOfStoresTempId] in table 'SettlementOfStoresDetailsTemps'
ALTER TABLE [dbo].[SettlementOfStoresDetailsTemps]
ADD CONSTRAINT [FK_SettlementOfStoresTempSettlementOfStoresDetailsTemp]
    FOREIGN KEY ([SettlementOfStoresTempId])
    REFERENCES [dbo].[SettlementOfStoresTemps]
        ([SettlementId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SettlementOfStoresTempSettlementOfStoresDetailsTemp'
CREATE INDEX [IX_FK_SettlementOfStoresTempSettlementOfStoresDetailsTemp]
ON [dbo].[SettlementOfStoresDetailsTemps]
    ([SettlementOfStoresTempId]);
GO

-- Creating foreign key on [PointOfSalePOS_Id] in table 'POSDetailPayments'
ALTER TABLE [dbo].[POSDetailPayments]
ADD CONSTRAINT [FK_PointOfSalePOSDetailPayment]
    FOREIGN KEY ([PointOfSalePOS_Id])
    REFERENCES [dbo].[PointOfSales]
        ([POS_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PointOfSalePOSDetailPayment'
CREATE INDEX [IX_FK_PointOfSalePOSDetailPayment]
ON [dbo].[POSDetailPayments]
    ([PointOfSalePOS_Id]);
GO

-- Creating foreign key on [PointOfSalePOS_Id] in table 'MasterPOSInvoices'
ALTER TABLE [dbo].[MasterPOSInvoices]
ADD CONSTRAINT [FK_PointOfSaleMasterPOSInvoice]
    FOREIGN KEY ([PointOfSalePOS_Id])
    REFERENCES [dbo].[PointOfSales]
        ([POS_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PointOfSaleMasterPOSInvoice'
CREATE INDEX [IX_FK_PointOfSaleMasterPOSInvoice]
ON [dbo].[MasterPOSInvoices]
    ([PointOfSalePOS_Id]);
GO

-- Creating foreign key on [MasterPOSInvoiceId] in table 'DetailPOSInvoices'
ALTER TABLE [dbo].[DetailPOSInvoices]
ADD CONSTRAINT [FK_MasterPOSInvoiceDetailPOSInvoice]
    FOREIGN KEY ([MasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceDetailPOSInvoice'
CREATE INDEX [IX_FK_MasterPOSInvoiceDetailPOSInvoice]
ON [dbo].[DetailPOSInvoices]
    ([MasterPOSInvoiceId]);
GO

-- Creating foreign key on [MasterPOSInvoiceId] in table 'PosPaymentMethods'
ALTER TABLE [dbo].[PosPaymentMethods]
ADD CONSTRAINT [FK_MasterPOSInvoicePosPaymentMethod]
    FOREIGN KEY ([MasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoicePosPaymentMethod'
CREATE INDEX [IX_FK_MasterPOSInvoicePosPaymentMethod]
ON [dbo].[PosPaymentMethods]
    ([MasterPOSInvoiceId]);
GO

-- Creating foreign key on [ReversePOSInvoiceMaster_Id] in table 'ReversePOSDetails'
ALTER TABLE [dbo].[ReversePOSDetails]
ADD CONSTRAINT [FK_ReversePOSInvoiceReversePOSDetail]
    FOREIGN KEY ([ReversePOSInvoiceMaster_Id])
    REFERENCES [dbo].[ReversePOSInvoices]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReversePOSInvoiceReversePOSDetail'
CREATE INDEX [IX_FK_ReversePOSInvoiceReversePOSDetail]
ON [dbo].[ReversePOSDetails]
    ([ReversePOSInvoiceMaster_Id]);
GO

-- Creating foreign key on [MasterPOSInvoiceMasterPOSInvoiceId] in table 'ReversePOSInvoices'
ALTER TABLE [dbo].[ReversePOSInvoices]
ADD CONSTRAINT [FK_MasterPOSInvoiceReversePOSInvoice]
    FOREIGN KEY ([MasterPOSInvoiceMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceReversePOSInvoice'
CREATE INDEX [IX_FK_MasterPOSInvoiceReversePOSInvoice]
ON [dbo].[ReversePOSInvoices]
    ([MasterPOSInvoiceMasterPOSInvoiceId]);
GO

-- Creating foreign key on [PointOfSaleTempPOS_Id] in table 'POSDetailPaymentTemps'
ALTER TABLE [dbo].[POSDetailPaymentTemps]
ADD CONSTRAINT [FK_PointOfSaleTempPOSDetailPaymentTemp]
    FOREIGN KEY ([PointOfSaleTempPOS_Id])
    REFERENCES [dbo].[PointOfSaleTemps]
        ([POS_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PointOfSaleTempPOSDetailPaymentTemp'
CREATE INDEX [IX_FK_PointOfSaleTempPOSDetailPaymentTemp]
ON [dbo].[POSDetailPaymentTemps]
    ([PointOfSaleTempPOS_Id]);
GO

-- Creating foreign key on [MasterPOSInvoiceTempMasterPOSInvoiceId] in table 'DetailPOSInvoiceTemps'
ALTER TABLE [dbo].[DetailPOSInvoiceTemps]
ADD CONSTRAINT [FK_MasterPOSInvoiceTempDetailPOSInvoiceTemp]
    FOREIGN KEY ([MasterPOSInvoiceTempMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoiceTemps]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceTempDetailPOSInvoiceTemp'
CREATE INDEX [IX_FK_MasterPOSInvoiceTempDetailPOSInvoiceTemp]
ON [dbo].[DetailPOSInvoiceTemps]
    ([MasterPOSInvoiceTempMasterPOSInvoiceId]);
GO

-- Creating foreign key on [MasterPOSInvoiceTempMasterPOSInvoiceId] in table 'PosPaymentMethodTemps'
ALTER TABLE [dbo].[PosPaymentMethodTemps]
ADD CONSTRAINT [FK_MasterPOSInvoiceTempPosPaymentMethodTemp]
    FOREIGN KEY ([MasterPOSInvoiceTempMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoiceTemps]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceTempPosPaymentMethodTemp'
CREATE INDEX [IX_FK_MasterPOSInvoiceTempPosPaymentMethodTemp]
ON [dbo].[PosPaymentMethodTemps]
    ([MasterPOSInvoiceTempMasterPOSInvoiceId]);
GO

-- Creating foreign key on [ReversePOSInvoiceTempMaster_Id] in table 'ReversePOSDetailTemps'
ALTER TABLE [dbo].[ReversePOSDetailTemps]
ADD CONSTRAINT [FK_ReversePOSInvoiceTempReversePOSDetailTemp]
    FOREIGN KEY ([ReversePOSInvoiceTempMaster_Id])
    REFERENCES [dbo].[ReversePOSInvoiceTemps]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReversePOSInvoiceTempReversePOSDetailTemp'
CREATE INDEX [IX_FK_ReversePOSInvoiceTempReversePOSDetailTemp]
ON [dbo].[ReversePOSDetailTemps]
    ([ReversePOSInvoiceTempMaster_Id]);
GO

-- Creating foreign key on [ProjectProject_Id] in table 'ProjectDetails'
ALTER TABLE [dbo].[ProjectDetails]
ADD CONSTRAINT [FK_ProjectProject_Details]
    FOREIGN KEY ([ProjectProject_Id])
    REFERENCES [dbo].[Projects]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectProject_Details'
CREATE INDEX [IX_FK_ProjectProject_Details]
ON [dbo].[ProjectDetails]
    ([ProjectProject_Id]);
GO

-- Creating foreign key on [ProjectTempProject_Id] in table 'ProjectDetailTemps'
ALTER TABLE [dbo].[ProjectDetailTemps]
ADD CONSTRAINT [FK_ProjectTempProjectDetailTemp]
    FOREIGN KEY ([ProjectTempProject_Id])
    REFERENCES [dbo].[ProjectTemps]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectTempProjectDetailTemp'
CREATE INDEX [IX_FK_ProjectTempProjectDetailTemp]
ON [dbo].[ProjectDetailTemps]
    ([ProjectTempProject_Id]);
GO

-- Creating foreign key on [ProjectPaymentId] in table 'ProjectPaymentFiles'
ALTER TABLE [dbo].[ProjectPaymentFiles]
ADD CONSTRAINT [FK_ProjectPaymentProjectPaymentFile]
    FOREIGN KEY ([ProjectPaymentId])
    REFERENCES [dbo].[ProjectPayments]
        ([ProjectPaymentId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectPaymentProjectPaymentFile'
CREATE INDEX [IX_FK_ProjectPaymentProjectPaymentFile]
ON [dbo].[ProjectPaymentFiles]
    ([ProjectPaymentId]);
GO

-- Creating foreign key on [ProjectRecipt_ID] in table 'ProjectReceiptFiles'
ALTER TABLE [dbo].[ProjectReceiptFiles]
ADD CONSTRAINT [FK_ProjectReciptProjectReceiptFile]
    FOREIGN KEY ([ProjectRecipt_ID])
    REFERENCES [dbo].[ProjectRecipts]
        ([ProjectRect_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectReciptProjectReceiptFile'
CREATE INDEX [IX_FK_ProjectReciptProjectReceiptFile]
ON [dbo].[ProjectReceiptFiles]
    ([ProjectRecipt_ID]);
GO

-- Creating foreign key on [SubscriptionInvoice_ID] in table 'SubscriptionInvoiceFiles'
ALTER TABLE [dbo].[SubscriptionInvoiceFiles]
ADD CONSTRAINT [FK_SubscriptionInvoiceSubscriptionInvoiceFile]
    FOREIGN KEY ([SubscriptionInvoice_ID])
    REFERENCES [dbo].[SubscriptionInvoices]
        ([Inv_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubscriptionInvoiceSubscriptionInvoiceFile'
CREATE INDEX [IX_FK_SubscriptionInvoiceSubscriptionInvoiceFile]
ON [dbo].[SubscriptionInvoiceFiles]
    ([SubscriptionInvoice_ID]);
GO

-- Creating foreign key on [GLMaster_Id] in table 'GLMasterFiles'
ALTER TABLE [dbo].[GLMasterFiles]
ADD CONSTRAINT [FK_GLMasterGLMasterFile]
    FOREIGN KEY ([GLMaster_Id])
    REFERENCES [dbo].[GLMasters]
        ([GLMaster_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_GLMasterGLMasterFile'
CREATE INDEX [IX_FK_GLMasterGLMasterFile]
ON [dbo].[GLMasterFiles]
    ([GLMaster_Id]);
GO

-- Creating foreign key on [PackageId] in table 'Clients'
ALTER TABLE [dbo].[Clients]
ADD CONSTRAINT [FK_ClientPackage]
    FOREIGN KEY ([PackageId])
    REFERENCES [dbo].[Packages]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ClientPackage'
CREATE INDEX [IX_FK_ClientPackage]
ON [dbo].[Clients]
    ([PackageId]);
GO

-- Creating foreign key on [Vacation_Type_Id] in table 'Vacations'
ALTER TABLE [dbo].[Vacations]
ADD CONSTRAINT [FK_VacationsVacation_Types]
    FOREIGN KEY ([Vacation_Type_Id])
    REFERENCES [dbo].[Vacation_Types]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VacationsVacation_Types'
CREATE INDEX [IX_FK_VacationsVacation_Types]
ON [dbo].[Vacations]
    ([Vacation_Type_Id]);
GO

-- Creating foreign key on [Vacation_TypesId] in table 'EmpVacations'
ALTER TABLE [dbo].[EmpVacations]
ADD CONSTRAINT [FK_EmpVacationVacation_Types]
    FOREIGN KEY ([Vacation_TypesId])
    REFERENCES [dbo].[Vacation_Types]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpVacationVacation_Types'
CREATE INDEX [IX_FK_EmpVacationVacation_Types]
ON [dbo].[EmpVacations]
    ([Vacation_TypesId]);
GO

-- Creating foreign key on [EmployeeEmp_Id] in table 'EmpVacations'
ALTER TABLE [dbo].[EmpVacations]
ADD CONSTRAINT [FK_EmpVacationEmployee]
    FOREIGN KEY ([EmployeeEmp_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmpVacationEmployee'
CREATE INDEX [IX_FK_EmpVacationEmployee]
ON [dbo].[EmpVacations]
    ([EmployeeEmp_Id]);
GO

-- Creating foreign key on [EmpTemp_Id] in table 'EmpVacationTemps'
ALTER TABLE [dbo].[EmpVacationTemps]
ADD CONSTRAINT [FK_EmployeTempEmpVacationTemp]
    FOREIGN KEY ([EmpTemp_Id])
    REFERENCES [dbo].[EmployeTemps]
        ([EmpTemp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeTempEmpVacationTemp'
CREATE INDEX [IX_FK_EmployeTempEmpVacationTemp]
ON [dbo].[EmpVacationTemps]
    ([EmpTemp_Id]);
GO

-- Creating foreign key on [InvoiceMasterMaster_Id] in table 'InvoiceTaxes'
ALTER TABLE [dbo].[InvoiceTaxes]
ADD CONSTRAINT [FK_InvoiceMasterInvoiceTax]
    FOREIGN KEY ([InvoiceMasterMaster_Id])
    REFERENCES [dbo].[InvoiceMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_InvoiceMasterInvoiceTax'
CREATE INDEX [IX_FK_InvoiceMasterInvoiceTax]
ON [dbo].[InvoiceTaxes]
    ([InvoiceMasterMaster_Id]);
GO

-- Creating foreign key on [SalesReceiptMasterMaster_Id] in table 'SalesReceiptTaxes'
ALTER TABLE [dbo].[SalesReceiptTaxes]
ADD CONSTRAINT [FK_SalesReceiptMasterSalesReceiptTax]
    FOREIGN KEY ([SalesReceiptMasterMaster_Id])
    REFERENCES [dbo].[SalesReceiptMasters]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SalesReceiptMasterSalesReceiptTax'
CREATE INDEX [IX_FK_SalesReceiptMasterSalesReceiptTax]
ON [dbo].[SalesReceiptTaxes]
    ([SalesReceiptMasterMaster_Id]);
GO

-- Creating foreign key on [MasterPOSInvoiceMasterPOSInvoiceId] in table 'TaxPOSInvoices'
ALTER TABLE [dbo].[TaxPOSInvoices]
ADD CONSTRAINT [FK_MasterPOSInvoiceTaxPOSInvoice]
    FOREIGN KEY ([MasterPOSInvoiceMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceTaxPOSInvoice'
CREATE INDEX [IX_FK_MasterPOSInvoiceTaxPOSInvoice]
ON [dbo].[TaxPOSInvoices]
    ([MasterPOSInvoiceMasterPOSInvoiceId]);
GO

-- Creating foreign key on [OfferPriceMasterOfferMas_Id] in table 'OfferTaxes'
ALTER TABLE [dbo].[OfferTaxes]
ADD CONSTRAINT [FK_OfferPriceMasterOfferTax]
    FOREIGN KEY ([OfferPriceMasterOfferMas_Id])
    REFERENCES [dbo].[OfferPriceMasters]
        ([OfferMas_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OfferPriceMasterOfferTax'
CREATE INDEX [IX_FK_OfferPriceMasterOfferTax]
ON [dbo].[OfferTaxes]
    ([OfferPriceMasterOfferMas_Id]);
GO

-- Creating foreign key on [ProductProd_ID] in table 'ProductFileDetails'
ALTER TABLE [dbo].[ProductFileDetails]
ADD CONSTRAINT [FK_ProductProductFileDetails]
    FOREIGN KEY ([ProductProd_ID])
    REFERENCES [dbo].[Products]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductFileDetails'
CREATE INDEX [IX_FK_ProductProductFileDetails]
ON [dbo].[ProductFileDetails]
    ([ProductProd_ID]);
GO

-- Creating foreign key on [Master_Product_Id] in table 'ProductCompoundTemps'
ALTER TABLE [dbo].[ProductCompoundTemps]
ADD CONSTRAINT [FK_ProductTemProductCompoundTemp]
    FOREIGN KEY ([Master_Product_Id])
    REFERENCES [dbo].[ProductTems]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductTemProductCompoundTemp'
CREATE INDEX [IX_FK_ProductTemProductCompoundTemp]
ON [dbo].[ProductCompoundTemps]
    ([Master_Product_Id]);
GO

-- Creating foreign key on [Master_Product_Id] in table 'ProductCompounds'
ALTER TABLE [dbo].[ProductCompounds]
ADD CONSTRAINT [FK_ProductProductCompound]
    FOREIGN KEY ([Master_Product_Id])
    REFERENCES [dbo].[Products]
        ([Prod_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProductProductCompound'
CREATE INDEX [IX_FK_ProductProductCompound]
ON [dbo].[ProductCompounds]
    ([Master_Product_Id]);
GO

-- Creating foreign key on [MaintenanceOrderOperationalId] in table 'MaintenanceOrderOperationalDetails'
ALTER TABLE [dbo].[MaintenanceOrderOperationalDetails]
ADD CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail]
    FOREIGN KEY ([MaintenanceOrderOperationalId])
    REFERENCES [dbo].[MaintenanceOrderOperationals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail'
CREATE INDEX [IX_FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail]
ON [dbo].[MaintenanceOrderOperationalDetails]
    ([MaintenanceOrderOperationalId]);
GO

-- Creating foreign key on [MaintenanceOrderOperationalId] in table 'MaintenanceOrderOperationalStatus'
ALTER TABLE [dbo].[MaintenanceOrderOperationalStatus]
ADD CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus]
    FOREIGN KEY ([MaintenanceOrderOperationalId])
    REFERENCES [dbo].[MaintenanceOrderOperationals]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus'
CREATE INDEX [IX_FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus]
ON [dbo].[MaintenanceOrderOperationalStatus]
    ([MaintenanceOrderOperationalId]);
GO

-- Creating foreign key on [MaintenanceOrderOperationalId] in table 'MaintenanceOrderOperationalDetailTemps'
ALTER TABLE [dbo].[MaintenanceOrderOperationalDetailTemps]
ADD CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail1]
    FOREIGN KEY ([MaintenanceOrderOperationalId])
    REFERENCES [dbo].[MaintenanceOrderOperationalTemps]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail1'
CREATE INDEX [IX_FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail1]
ON [dbo].[MaintenanceOrderOperationalDetailTemps]
    ([MaintenanceOrderOperationalId]);
GO

-- Creating foreign key on [MaintenanceOrderOperationalId] in table 'MaintenanceOrderDetails'
ALTER TABLE [dbo].[MaintenanceOrderDetails]
ADD CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail2]
    FOREIGN KEY ([MaintenanceOrderOperationalId])
    REFERENCES [dbo].[MaintenanceOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail2'
CREATE INDEX [IX_FK_MaintenanceOrderOperationalMaintenanceOrderOperationalDetail2]
ON [dbo].[MaintenanceOrderDetails]
    ([MaintenanceOrderOperationalId]);
GO

-- Creating foreign key on [MaintenanceOrderOperationalId] in table 'MaintenanceOrderStatus'
ALTER TABLE [dbo].[MaintenanceOrderStatus]
ADD CONSTRAINT [FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus1]
    FOREIGN KEY ([MaintenanceOrderOperationalId])
    REFERENCES [dbo].[MaintenanceOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus1'
CREATE INDEX [IX_FK_MaintenanceOrderOperationalMaintenanceOrderOperationalStatus1]
ON [dbo].[MaintenanceOrderStatus]
    ([MaintenanceOrderOperationalId]);
GO

-- Creating foreign key on [OrderId] in table 'OrderDetails'
ALTER TABLE [dbo].[OrderDetails]
ADD CONSTRAINT [FK_OrderOrderDetail]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderOrderDetail'
CREATE INDEX [IX_FK_OrderOrderDetail]
ON [dbo].[OrderDetails]
    ([OrderId]);
GO

-- Creating foreign key on [TempOrderId] in table 'TempOrderDetails'
ALTER TABLE [dbo].[TempOrderDetails]
ADD CONSTRAINT [FK_TempOrderTempOrderDetail]
    FOREIGN KEY ([TempOrderId])
    REFERENCES [dbo].[TempOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TempOrderTempOrderDetail'
CREATE INDEX [IX_FK_TempOrderTempOrderDetail]
ON [dbo].[TempOrderDetails]
    ([TempOrderId]);
GO

-- Creating foreign key on [HoldedOrderId] in table 'HoldedOrderDetails'
ALTER TABLE [dbo].[HoldedOrderDetails]
ADD CONSTRAINT [FK_HoldedOrderHoldedOrderDetail]
    FOREIGN KEY ([HoldedOrderId])
    REFERENCES [dbo].[HoldedOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_HoldedOrderHoldedOrderDetail'
CREATE INDEX [IX_FK_HoldedOrderHoldedOrderDetail]
ON [dbo].[HoldedOrderDetails]
    ([HoldedOrderId]);
GO

-- Creating foreign key on [OrderId] in table 'ReverseOrders'
ALTER TABLE [dbo].[ReverseOrders]
ADD CONSTRAINT [FK_OrderReverseOrder]
    FOREIGN KEY ([OrderId])
    REFERENCES [dbo].[Orders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_OrderReverseOrder'
CREATE INDEX [IX_FK_OrderReverseOrder]
ON [dbo].[ReverseOrders]
    ([OrderId]);
GO

-- Creating foreign key on [ReverseOrderId] in table 'ReverseOrderDetails'
ALTER TABLE [dbo].[ReverseOrderDetails]
ADD CONSTRAINT [FK_ReverseOrderReverseOrderDetail]
    FOREIGN KEY ([ReverseOrderId])
    REFERENCES [dbo].[ReverseOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseOrderReverseOrderDetail'
CREATE INDEX [IX_FK_ReverseOrderReverseOrderDetail]
ON [dbo].[ReverseOrderDetails]
    ([ReverseOrderId]);
GO

-- Creating foreign key on [MasterQRInvoiceId] in table 'QRPaymentMethods'
ALTER TABLE [dbo].[QRPaymentMethods]
ADD CONSTRAINT [FK_MasterQRInvoiceQRPaymentMethod]
    FOREIGN KEY ([MasterQRInvoiceId])
    REFERENCES [dbo].[MasterQRInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterQRInvoiceQRPaymentMethod'
CREATE INDEX [IX_FK_MasterQRInvoiceQRPaymentMethod]
ON [dbo].[QRPaymentMethods]
    ([MasterQRInvoiceId]);
GO

-- Creating foreign key on [MasterQRInvoiceId] in table 'DetailQRInvoices'
ALTER TABLE [dbo].[DetailQRInvoices]
ADD CONSTRAINT [FK_MasterQRInvoiceDetailQRInvoice]
    FOREIGN KEY ([MasterQRInvoiceId])
    REFERENCES [dbo].[MasterQRInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterQRInvoiceDetailQRInvoice'
CREATE INDEX [IX_FK_MasterQRInvoiceDetailQRInvoice]
ON [dbo].[DetailQRInvoices]
    ([MasterQRInvoiceId]);
GO

-- Creating foreign key on [MasterQRInvoiceId] in table 'TaxQRInvoices'
ALTER TABLE [dbo].[TaxQRInvoices]
ADD CONSTRAINT [FK_MasterQRInvoiceTaxQRInvoice]
    FOREIGN KEY ([MasterQRInvoiceId])
    REFERENCES [dbo].[MasterQRInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterQRInvoiceTaxQRInvoice'
CREATE INDEX [IX_FK_MasterQRInvoiceTaxQRInvoice]
ON [dbo].[TaxQRInvoices]
    ([MasterQRInvoiceId]);
GO

-- Creating foreign key on [Store_ID] in table 'StoreEmployees'
ALTER TABLE [dbo].[StoreEmployees]
ADD CONSTRAINT [FK_StoreStoreEmployee]
    FOREIGN KEY ([Store_ID])
    REFERENCES [dbo].[Stores]
        ([Store_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_StoreStoreEmployee'
CREATE INDEX [IX_FK_StoreStoreEmployee]
ON [dbo].[StoreEmployees]
    ([Store_ID]);
GO

-- Creating foreign key on [ExtrnalSalesEmployeeId] in table 'EmployeeSalesDetails'
ALTER TABLE [dbo].[EmployeeSalesDetails]
ADD CONSTRAINT [FK_ExtrnalSalesEmployeeEmployeeSalesDetails]
    FOREIGN KEY ([ExtrnalSalesEmployeeId])
    REFERENCES [dbo].[ExtrnalSalesEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExtrnalSalesEmployeeEmployeeSalesDetails'
CREATE INDEX [IX_FK_ExtrnalSalesEmployeeEmployeeSalesDetails]
ON [dbo].[EmployeeSalesDetails]
    ([ExtrnalSalesEmployeeId]);
GO

-- Creating foreign key on [ExtrnalSalesEmployeeId] in table 'ExternalEmployeePayments'
ALTER TABLE [dbo].[ExternalEmployeePayments]
ADD CONSTRAINT [FK_ExtrnalSalesEmployeeExternalEmployeePayments]
    FOREIGN KEY ([ExtrnalSalesEmployeeId])
    REFERENCES [dbo].[ExtrnalSalesEmployees]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ExtrnalSalesEmployeeExternalEmployeePayments'
CREATE INDEX [IX_FK_ExtrnalSalesEmployeeExternalEmployeePayments]
ON [dbo].[ExternalEmployeePayments]
    ([ExtrnalSalesEmployeeId]);
GO

-- Creating foreign key on [SystemRoleId] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [FK_SystemRoleRoleFunction]
    FOREIGN KEY ([SystemRoleId])
    REFERENCES [dbo].[SystemRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemRoleRoleFunction'
CREATE INDEX [IX_FK_SystemRoleRoleFunction]
ON [dbo].[RoleFunctions]
    ([SystemRoleId]);
GO

-- Creating foreign key on [SystemFunctionId] in table 'RoleFunctions'
ALTER TABLE [dbo].[RoleFunctions]
ADD CONSTRAINT [FK_RoleFunctionSystemFunction]
    FOREIGN KEY ([SystemFunctionId])
    REFERENCES [dbo].[SystemFunctions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RoleFunctionSystemFunction'
CREATE INDEX [IX_FK_RoleFunctionSystemFunction]
ON [dbo].[RoleFunctions]
    ([SystemFunctionId]);
GO

-- Creating foreign key on [SystemFunctionId] in table 'SystemFunctions'
ALTER TABLE [dbo].[SystemFunctions]
ADD CONSTRAINT [FK_SystemFunctionSystemFunction]
    FOREIGN KEY ([SystemFunctionId])
    REFERENCES [dbo].[SystemFunctions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SystemFunctionSystemFunction'
CREATE INDEX [IX_FK_SystemFunctionSystemFunction]
ON [dbo].[SystemFunctions]
    ([SystemFunctionId]);
GO

-- Creating foreign key on [SystemRoleId] in table 'Employees'
ALTER TABLE [dbo].[Employees]
ADD CONSTRAINT [FK_EmployeeSystemRole]
    FOREIGN KEY ([SystemRoleId])
    REFERENCES [dbo].[SystemRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSystemRole'
CREATE INDEX [IX_FK_EmployeeSystemRole]
ON [dbo].[Employees]
    ([SystemRoleId]);
GO

-- Creating foreign key on [Project_Id] in table 'ProjectClauses'
ALTER TABLE [dbo].[ProjectClauses]
ADD CONSTRAINT [FK_ProjectProjectSub]
    FOREIGN KEY ([Project_Id])
    REFERENCES [dbo].[Projects]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectProjectSub'
CREATE INDEX [IX_FK_ProjectProjectSub]
ON [dbo].[ProjectClauses]
    ([Project_Id]);
GO

-- Creating foreign key on [SubLedgerSubLed_Id] in table 'SubLedgertemps'
ALTER TABLE [dbo].[SubLedgertemps]
ADD CONSTRAINT [FK_SubLedgerSubLedger1]
    FOREIGN KEY ([SubLedgerSubLed_Id])
    REFERENCES [dbo].[SubLedgertemps]
        ([SubLed_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SubLedgerSubLedger1'
CREATE INDEX [IX_FK_SubLedgerSubLedger1]
ON [dbo].[SubLedgertemps]
    ([SubLedgerSubLed_Id]);
GO

-- Creating foreign key on [ProjectTempProject_Id] in table 'ProjectClausetemps'
ALTER TABLE [dbo].[ProjectClausetemps]
ADD CONSTRAINT [FK_ProjectTempProjectClausetemp]
    FOREIGN KEY ([ProjectTempProject_Id])
    REFERENCES [dbo].[ProjectTemps]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectTempProjectClausetemp'
CREATE INDEX [IX_FK_ProjectTempProjectClausetemp]
ON [dbo].[ProjectClausetemps]
    ([ProjectTempProject_Id]);
GO

-- Creating foreign key on [ProjectTempProject_Id] in table 'SubLedgertemps'
ALTER TABLE [dbo].[SubLedgertemps]
ADD CONSTRAINT [FK_ProjectTempSubLedgertemp]
    FOREIGN KEY ([ProjectTempProject_Id])
    REFERENCES [dbo].[ProjectTemps]
        ([Project_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ProjectTempSubLedgertemp'
CREATE INDEX [IX_FK_ProjectTempSubLedgertemp]
ON [dbo].[SubLedgertemps]
    ([ProjectTempProject_Id]);
GO

-- Creating foreign key on [SpentReciptOrderTypeId] in table 'SpentReciptOrders'
ALTER TABLE [dbo].[SpentReciptOrders]
ADD CONSTRAINT [FK_SpentReciptOrderSpentReciptOrderType]
    FOREIGN KEY ([SpentReciptOrderTypeId])
    REFERENCES [dbo].[SpentReciptOrderTypes]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SpentReciptOrderSpentReciptOrderType'
CREATE INDEX [IX_FK_SpentReciptOrderSpentReciptOrderType]
ON [dbo].[SpentReciptOrders]
    ([SpentReciptOrderTypeId]);
GO

-- Creating foreign key on [SpentReciptOrderId] in table 'SpentRecipts'
ALTER TABLE [dbo].[SpentRecipts]
ADD CONSTRAINT [FK_SpentReciptOrderSpentRecipt]
    FOREIGN KEY ([SpentReciptOrderId])
    REFERENCES [dbo].[SpentReciptOrders]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SpentReciptOrderSpentRecipt'
CREATE INDEX [IX_FK_SpentReciptOrderSpentRecipt]
ON [dbo].[SpentRecipts]
    ([SpentReciptOrderId]);
GO

-- Creating foreign key on [FixedAssetFasset_ID] in table 'FixedAssetDetails'
ALTER TABLE [dbo].[FixedAssetDetails]
ADD CONSTRAINT [FK_FixedAssetFixedAssetDetails]
    FOREIGN KEY ([FixedAssetFasset_ID])
    REFERENCES [dbo].[FixedAssets]
        ([Fasset_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FixedAssetFixedAssetDetails'
CREATE INDEX [IX_FK_FixedAssetFixedAssetDetails]
ON [dbo].[FixedAssetDetails]
    ([FixedAssetFasset_ID]);
GO

-- Creating foreign key on [GLMasterGLMaster_Id] in table 'FixedAssetDetails'
ALTER TABLE [dbo].[FixedAssetDetails]
ADD CONSTRAINT [FK_FixedAssetDetailsGLMaster]
    FOREIGN KEY ([GLMasterGLMaster_Id])
    REFERENCES [dbo].[GLMasters]
        ([GLMaster_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_FixedAssetDetailsGLMaster'
CREATE INDEX [IX_FK_FixedAssetDetailsGLMaster]
ON [dbo].[FixedAssetDetails]
    ([GLMasterGLMaster_Id]);
GO

-- Creating foreign key on [ReceiptRec_ID] in table 'ReceiptPOSFileDetails'
ALTER TABLE [dbo].[ReceiptPOSFileDetails]
ADD CONSTRAINT [FK_ReceiptReceiptFileDetail1]
    FOREIGN KEY ([ReceiptRec_ID])
    REFERENCES [dbo].[POSReceipts]
        ([Rec_ID])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReceiptReceiptFileDetail1'
CREATE INDEX [IX_FK_ReceiptReceiptFileDetail1]
ON [dbo].[ReceiptPOSFileDetails]
    ([ReceiptRec_ID]);
GO

-- Creating foreign key on [MasterPOSInvoiceMasterPOSInvoiceId] in table 'POSReceipts'
ALTER TABLE [dbo].[POSReceipts]
ADD CONSTRAINT [FK_MasterPOSInvoicePOSReceipt]
    FOREIGN KEY ([MasterPOSInvoiceMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoicePOSReceipt'
CREATE INDEX [IX_FK_MasterPOSInvoicePOSReceipt]
ON [dbo].[POSReceipts]
    ([MasterPOSInvoiceMasterPOSInvoiceId]);
GO

-- Creating foreign key on [ReverseQRInvoiceMaster_Id] in table 'ReverseQRDetails'
ALTER TABLE [dbo].[ReverseQRDetails]
ADD CONSTRAINT [FK_ReversePOSInvoiceReversePOSDetail1]
    FOREIGN KEY ([ReverseQRInvoiceMaster_Id])
    REFERENCES [dbo].[ReverseQRInvoices]
        ([Master_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReversePOSInvoiceReversePOSDetail1'
CREATE INDEX [IX_FK_ReversePOSInvoiceReversePOSDetail1]
ON [dbo].[ReverseQRDetails]
    ([ReverseQRInvoiceMaster_Id]);
GO

-- Creating foreign key on [MasterQRInvoiceId] in table 'ReverseQRInvoices'
ALTER TABLE [dbo].[ReverseQRInvoices]
ADD CONSTRAINT [FK_ReverseQRInvoiceMasterQRInvoice]
    FOREIGN KEY ([MasterQRInvoiceId])
    REFERENCES [dbo].[MasterQRInvoices]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseQRInvoiceMasterQRInvoice'
CREATE INDEX [IX_FK_ReverseQRInvoiceMasterQRInvoice]
ON [dbo].[ReverseQRInvoices]
    ([MasterQRInvoiceId]);
GO

-- Creating foreign key on [DetailQRInvoiceDetail_Id] in table 'ReverseQRDetails'
ALTER TABLE [dbo].[ReverseQRDetails]
ADD CONSTRAINT [FK_ReverseQRDetailDetailQRInvoice]
    FOREIGN KEY ([DetailQRInvoiceDetail_Id])
    REFERENCES [dbo].[DetailQRInvoices]
        ([Detail_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ReverseQRDetailDetailQRInvoice'
CREATE INDEX [IX_FK_ReverseQRDetailDetailQRInvoice]
ON [dbo].[ReverseQRDetails]
    ([DetailQRInvoiceDetail_Id]);
GO

-- Creating foreign key on [PaymentMethod_Id] in table 'PaymentMethodEmployees'
ALTER TABLE [dbo].[PaymentMethodEmployees]
ADD CONSTRAINT [FK_PaymentMethodPaymentMethodEmployee]
    FOREIGN KEY ([PaymentMethod_Id])
    REFERENCES [dbo].[PaymentMethods]
        ([PaymentMethod_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_PaymentMethodPaymentMethodEmployee'
CREATE INDEX [IX_FK_PaymentMethodPaymentMethodEmployee]
ON [dbo].[PaymentMethodEmployees]
    ([PaymentMethod_Id]);
GO

-- Creating foreign key on [ShiftCodingId] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [FK_ShiftShiftCoding]
    FOREIGN KEY ([ShiftCodingId])
    REFERENCES [dbo].[ShiftCodings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ShiftShiftCoding'
CREATE INDEX [IX_FK_ShiftShiftCoding]
ON [dbo].[Shifts]
    ([ShiftCodingId]);
GO

-- Creating foreign key on [EmployeeEmp_Id] in table 'Shifts'
ALTER TABLE [dbo].[Shifts]
ADD CONSTRAINT [FK_EmployeeShift]
    FOREIGN KEY ([EmployeeEmp_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeShift'
CREATE INDEX [IX_FK_EmployeeShift]
ON [dbo].[Shifts]
    ([EmployeeEmp_Id]);
GO

-- Creating foreign key on [ShiftCodingId] in table 'VacationWeeklies'
ALTER TABLE [dbo].[VacationWeeklies]
ADD CONSTRAINT [FK_ShiftCodingVacationWeekly]
    FOREIGN KEY ([ShiftCodingId])
    REFERENCES [dbo].[ShiftCodings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_ShiftCodingVacationWeekly'
CREATE INDEX [IX_FK_ShiftCodingVacationWeekly]
ON [dbo].[VacationWeeklies]
    ([ShiftCodingId]);
GO

-- Creating foreign key on [VacationWeeklyCodingId] in table 'VacationWeeklies'
ALTER TABLE [dbo].[VacationWeeklies]
ADD CONSTRAINT [FK_VacationWeeklyVacationWeeklyCoding]
    FOREIGN KEY ([VacationWeeklyCodingId])
    REFERENCES [dbo].[VacationWeeklyCodings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_VacationWeeklyVacationWeeklyCoding'
CREATE INDEX [IX_FK_VacationWeeklyVacationWeeklyCoding]
ON [dbo].[VacationWeeklies]
    ([VacationWeeklyCodingId]);
GO

-- Creating foreign key on [EmployeeEmp_Id] in table 'AttendingAndLeavings'
ALTER TABLE [dbo].[AttendingAndLeavings]
ADD CONSTRAINT [FK_AttendingAndLeavingEmployee]
    FOREIGN KEY ([EmployeeEmp_Id])
    REFERENCES [dbo].[Employees]
        ([Emp_Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AttendingAndLeavingEmployee'
CREATE INDEX [IX_FK_AttendingAndLeavingEmployee]
ON [dbo].[AttendingAndLeavings]
    ([EmployeeEmp_Id]);
GO

-- Creating foreign key on [ShiftCodingId] in table 'AttendingAndLeavings'
ALTER TABLE [dbo].[AttendingAndLeavings]
ADD CONSTRAINT [FK_AttendingAndLeavingShiftCoding]
    FOREIGN KEY ([ShiftCodingId])
    REFERENCES [dbo].[ShiftCodings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AttendingAndLeavingShiftCoding'
CREATE INDEX [IX_FK_AttendingAndLeavingShiftCoding]
ON [dbo].[AttendingAndLeavings]
    ([ShiftCodingId]);
GO

-- Creating foreign key on [RewardsAndPenaltiesCodingCategoryId] in table 'RewardsAndPenaltiesCodings'
ALTER TABLE [dbo].[RewardsAndPenaltiesCodings]
ADD CONSTRAINT [FK_RewardsAndPenaltiesCodingRewardsAndPenaltiesCodingCategory]
    FOREIGN KEY ([RewardsAndPenaltiesCodingCategoryId])
    REFERENCES [dbo].[RewardsAndPenaltiesCodingCategories]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RewardsAndPenaltiesCodingRewardsAndPenaltiesCodingCategory'
CREATE INDEX [IX_FK_RewardsAndPenaltiesCodingRewardsAndPenaltiesCodingCategory]
ON [dbo].[RewardsAndPenaltiesCodings]
    ([RewardsAndPenaltiesCodingCategoryId]);
GO

-- Creating foreign key on [AttendingAndLeavingId] in table 'RewardsAndPenalties'
ALTER TABLE [dbo].[RewardsAndPenalties]
ADD CONSTRAINT [FK_RewardsAndPenaltiesAttendingAndLeaving]
    FOREIGN KEY ([AttendingAndLeavingId])
    REFERENCES [dbo].[AttendingAndLeavings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RewardsAndPenaltiesAttendingAndLeaving'
CREATE INDEX [IX_FK_RewardsAndPenaltiesAttendingAndLeaving]
ON [dbo].[RewardsAndPenalties]
    ([AttendingAndLeavingId]);
GO

-- Creating foreign key on [RewardsAndPenaltiesCodingId] in table 'RewardsAndPenalties'
ALTER TABLE [dbo].[RewardsAndPenalties]
ADD CONSTRAINT [FK_RewardsAndPenaltiesCodingRewardsAndPenalties]
    FOREIGN KEY ([RewardsAndPenaltiesCodingId])
    REFERENCES [dbo].[RewardsAndPenaltiesCodings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_RewardsAndPenaltiesCodingRewardsAndPenalties'
CREATE INDEX [IX_FK_RewardsAndPenaltiesCodingRewardsAndPenalties]
ON [dbo].[RewardsAndPenalties]
    ([RewardsAndPenaltiesCodingId]);
GO

-- Creating foreign key on [TypeOfDetailSalaryId] in table 'RewardsAndPenaltiesCodings'
ALTER TABLE [dbo].[RewardsAndPenaltiesCodings]
ADD CONSTRAINT [FK_TypeOfDetailSalaryRewardsAndPenaltiesCoding]
    FOREIGN KEY ([TypeOfDetailSalaryId])
    REFERENCES [dbo].[TypeOfDetailSalaries]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_TypeOfDetailSalaryRewardsAndPenaltiesCoding'
CREATE INDEX [IX_FK_TypeOfDetailSalaryRewardsAndPenaltiesCoding]
ON [dbo].[RewardsAndPenaltiesCodings]
    ([TypeOfDetailSalaryId]);
GO

-- Creating foreign key on [EmployeeSalaryMonthId] in table 'RewardsAndPenalties'
ALTER TABLE [dbo].[RewardsAndPenalties]
ADD CONSTRAINT [FK_EmployeeSalaryMonthRewardsAndPenalties]
    FOREIGN KEY ([EmployeeSalaryMonthId])
    REFERENCES [dbo].[EmployeeSalaryMonths]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_EmployeeSalaryMonthRewardsAndPenalties'
CREATE INDEX [IX_FK_EmployeeSalaryMonthRewardsAndPenalties]
ON [dbo].[RewardsAndPenalties]
    ([EmployeeSalaryMonthId]);
GO

-- Creating foreign key on [QuestionId] in table 'Answers'
ALTER TABLE [dbo].[Answers]
ADD CONSTRAINT [FK_QuestionAnswer]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_QuestionAnswer'
CREATE INDEX [IX_FK_QuestionAnswer]
ON [dbo].[Answers]
    ([QuestionId]);
GO

-- Creating foreign key on [QuestionId] in table 'SewingDetails'
ALTER TABLE [dbo].[SewingDetails]
ADD CONSTRAINT [FK_SewingDetailQuestion]
    FOREIGN KEY ([QuestionId])
    REFERENCES [dbo].[Questions]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SewingDetailQuestion'
CREATE INDEX [IX_FK_SewingDetailQuestion]
ON [dbo].[SewingDetails]
    ([QuestionId]);
GO

-- Creating foreign key on [SewingId] in table 'SewingDetails'
ALTER TABLE [dbo].[SewingDetails]
ADD CONSTRAINT [FK_SewingDetailSewing]
    FOREIGN KEY ([SewingId])
    REFERENCES [dbo].[Sewings]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SewingDetailSewing'
CREATE INDEX [IX_FK_SewingDetailSewing]
ON [dbo].[SewingDetails]
    ([SewingId]);
GO

-- Creating foreign key on [AnswerId] in table 'SewingDetails'
ALTER TABLE [dbo].[SewingDetails]
ADD CONSTRAINT [FK_AnswerSewingDetail]
    FOREIGN KEY ([AnswerId])
    REFERENCES [dbo].[Answers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AnswerSewingDetail'
CREATE INDEX [IX_FK_AnswerSewingDetail]
ON [dbo].[SewingDetails]
    ([AnswerId]);
GO

-- Creating foreign key on [MasterPOSInvoiceMasterPOSInvoiceId] in table 'Sewings'
ALTER TABLE [dbo].[Sewings]
ADD CONSTRAINT [FK_MasterPOSInvoiceSewing]
    FOREIGN KEY ([MasterPOSInvoiceMasterPOSInvoiceId])
    REFERENCES [dbo].[MasterPOSInvoices]
        ([MasterPOSInvoiceId])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_MasterPOSInvoiceSewing'
CREATE INDEX [IX_FK_MasterPOSInvoiceSewing]
ON [dbo].[Sewings]
    ([MasterPOSInvoiceMasterPOSInvoiceId]);
GO

-- Creating foreign key on [SewingStatusId] in table 'Sewings'
ALTER TABLE [dbo].[Sewings]
ADD CONSTRAINT [FK_SewingSewingStatus]
    FOREIGN KEY ([SewingStatusId])
    REFERENCES [dbo].[SewingStatus]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_SewingSewingStatus'
CREATE INDEX [IX_FK_SewingSewingStatus]
ON [dbo].[Sewings]
    ([SewingStatusId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------