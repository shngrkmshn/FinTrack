export enum TransactionType {
    Income = 0,
    Expense = 1,
    Transfer = 2,
}

export const TRANSACTION_TYPE_LABELS: Record<TransactionType, string> = {
    [TransactionType.Income]: 'Income',
    [TransactionType.Expense]: 'Expense',
    [TransactionType.Transfer]: 'Transfer',
}

export interface TransactionDto {
    id: string
    transactionType: TransactionType
    amount: number
    currency: number
    date: string
    description: string
    accountId: string
    categoryId: string | null
    toAccountId: string | null
    recurrenceScheduleId: string | null
    createdAt: string
    updatedAt: string | null
    isDeleted: boolean
    deletedAt: string | null
}

export interface CreateTransactionRequest {
    transactionType: TransactionType
    amount: number
    currency: number
    date: string
    description: string
    accountId: string
    categoryId?: string
    toAccountId?: string
}

export interface PaginatedList<T> {
    items: T[]
    page: number
    pageSize: number
    totalCount: number
    totalPages: number
    hasPreviousPage: boolean
    hasNextPage: boolean
}
