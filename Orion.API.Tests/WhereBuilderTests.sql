CREATE TABLE [dbo].[INV_InvoiceIssue] (
    [InvoiceId]        INT            IDENTITY (1, 1) NOT NULL,
    [InvoicePrefix]    NVARCHAR (2)   NOT NULL,
    [InvoiceNum]       INT            NOT NULL,
    [InvoiceDate]      DATE       NOT NULL,
    [DeliveryCustCode]    NVARCHAR (24)   NOT NULL,
    [DeliveryCustName]    NVARCHAR (128)  NOT NULL,
    [Total]            MONEY          NOT NULL,
    [CreateBy]         INT            NOT NULL,
    [CreateDate]       DATETIME       NOT NULL,
    [ModifyBy]         INT            NOT NULL,
    [ModifyDate]       DATETIME       NOT NULL,
    CONSTRAINT [PK_INV_InvoiceIssue] PRIMARY KEY CLUSTERED ([InvoiceId] ASC),
    CONSTRAINT [UK_INV_InvoiceIssue_InvoiceNum] UNIQUE NONCLUSTERED ([InvoicePrefix] ASC, [InvoiceNum] ASC)
);

CREATE TABLE [dbo].[INV_InvoiceIssueItems] (
    [ItemId]      INT           IDENTITY (1, 1) NOT NULL,
    [InvoiceId]   INT           NOT NULL,
    [DeliveryNum] NVARCHAR (20) NOT NULL,
    [PurchaseNum] NVARCHAR (15) NULL,
    [Qty]         INT           NOT NULL,
    [Price]       MONEY         NOT NULL,
    [TotalPrice]  MONEY         NOT NULL,
    CONSTRAINT [PK_INV_InvoiceIssueItems] PRIMARY KEY CLUSTERED ([ItemId] ASC),
    CONSTRAINT [FK_INV_InvoiceIssueItems_INV_InvoiceIssue] FOREIGN KEY ([InvoiceId]) REFERENCES [dbo].[INV_InvoiceIssue] ([InvoiceId])
);

