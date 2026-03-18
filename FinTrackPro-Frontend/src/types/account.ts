export const AccountType = {
    Checking: 0,
    Savings: 1,
    CreditCard: 2,
    Investment: 3,
    Cash: 4,
    Loan: 5,
} as const

export type AccountType = (typeof AccountType)[keyof typeof AccountType]

export const Currency = {
    USD: 0,
    EUR: 1,
    GBP: 2,
    JPY: 3,
    CAD: 4,
    AUD: 5,
    CHF: 6,
    CNY: 7,
    INR: 8,
    TRY: 9,
} as const

export type Currency = (typeof Currency)[keyof typeof Currency]

export interface AccountDto {
    id: string
    name: string
    accountType: AccountType
    balanceAmount: number
    balanceCurrency: Currency
    createdAt: string
    updatedAt: string | null
    isActive: boolean
}

export interface CreateAccountRequest {
    name: string
    accountType: AccountType
    currency: Currency
}

export const ACCOUNT_TYPE_LABELS: Record<AccountType, string> = {
    [AccountType.Checking]: 'Checking',
    [AccountType.Savings]: 'Savings',
    [AccountType.CreditCard]: 'Credit Card',
    [AccountType.Investment]: 'Investment',
    [AccountType.Cash]: 'Cash',
    [AccountType.Loan]: 'Loan',
}

const CURRENCY_CODES: Record<Currency, string> = {
    [Currency.USD]: 'USD',
    [Currency.EUR]: 'EUR',
    [Currency.GBP]: 'GBP',
    [Currency.JPY]: 'JPY',
    [Currency.CAD]: 'CAD',
    [Currency.AUD]: 'AUD',
    [Currency.CHF]: 'CHF',
    [Currency.CNY]: 'CNY',
    [Currency.INR]: 'INR',
    [Currency.TRY]: 'TRY',
}

export const CURRENCY_LABELS: Record<Currency, string> = {
    [Currency.USD]: 'USD – US Dollar',
    [Currency.EUR]: 'EUR – Euro',
    [Currency.GBP]: 'GBP – British Pound',
    [Currency.JPY]: 'JPY – Japanese Yen',
    [Currency.CAD]: 'CAD – Canadian Dollar',
    [Currency.AUD]: 'AUD – Australian Dollar',
    [Currency.CHF]: 'CHF – Swiss Franc',
    [Currency.CNY]: 'CNY – Chinese Yuan',
    [Currency.INR]: 'INR – Indian Rupee',
    [Currency.TRY]: 'TRY – Turkish Lira',
}

export function formatCurrency(amount: number, currency: Currency): string {
    return new Intl.NumberFormat('en-US', {
        style: 'currency',
        currency: CURRENCY_CODES[currency],
    }).format(amount)
}
