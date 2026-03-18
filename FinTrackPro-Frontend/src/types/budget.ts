export const BudgetPeriod = {
    Weekly: 0,
    Monthly: 1,
    Yearly: 2,
} as const

export type BudgetPeriod = (typeof BudgetPeriod)[keyof typeof BudgetPeriod]

export const BUDGET_PERIOD_LABELS: Record<BudgetPeriod, string> = {
    [BudgetPeriod.Weekly]: 'Weekly',
    [BudgetPeriod.Monthly]: 'Monthly',
    [BudgetPeriod.Yearly]: 'Yearly',
}

export interface BudgetDto {
    id: string
    name: string
    categoryId: string
    userId: string
    amount: number
    currency: number
    period: BudgetPeriod
    startDate: string | null
    periodStartDate: string
    periodEndDate: string
    spentAmount: number
    spentPercentage: number
    createdAt: string
    updatedAt: string | null
}

export interface CreateBudgetRequest {
    name: string
    categoryId: string
    amount: number
    currency: number
    period: BudgetPeriod
    startDate?: string
}
