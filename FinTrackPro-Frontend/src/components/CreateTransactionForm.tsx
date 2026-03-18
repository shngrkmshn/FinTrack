import { type FormEvent, useState } from 'react'
import axios from 'axios'
import { TransactionType, TRANSACTION_TYPE_LABELS, type CreateTransactionRequest } from '../types/transaction.ts'
import { Currency, CURRENCY_LABELS } from '../types/account.ts'
import type { AccountDto } from '../types/account.ts'
import type { CategoryDto } from '../types/category.ts'

interface CreateTransactionFormProperties {
    accounts: AccountDto[]
    categories: CategoryDto[]
    onCreateTransaction: (request: CreateTransactionRequest) => Promise<void>
    onCancel: () => void
}

export default function CreateTransactionForm({
    accounts,
    categories,
    onCreateTransaction,
    onCancel,
}: CreateTransactionFormProperties) {
    const [transactionType, setTransactionType] = useState<TransactionType>(TransactionType.Income)
    const [amount, setAmount] = useState('')
    const [currency, setCurrency] = useState<Currency>(Currency.USD)
    const [date, setDate] = useState(new Date().toISOString().split('T')[0])
    const [description, setDescription] = useState('')
    const [accountId, setAccountId] = useState(accounts[0]?.id ?? '')
    const [categoryId, setCategoryId] = useState('')
    const [toAccountId, setToAccountId] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    function resetForm() {
        setTransactionType(TransactionType.Income)
        setAmount('')
        setCurrency(Currency.USD)
        setDate(new Date().toISOString().split('T')[0])
        setDescription('')
        setAccountId(accounts[0]?.id ?? '')
        setCategoryId('')
        setToAccountId('')
        setError('')
    }

    async function handleSubmit(event: FormEvent) {
        event.preventDefault()
        setError('')
        setIsLoading(true)

        try {
            const request: CreateTransactionRequest = {
                transactionType,
                amount: parseFloat(amount),
                currency,
                date,
                description,
                accountId,
            }

            if (transactionType === TransactionType.Transfer) {
                request.toAccountId = toAccountId
            } else if (categoryId) {
                request.categoryId = categoryId
            }

            await onCreateTransaction(request)
            resetForm()
        } catch (err) {
            if (axios.isAxiosError(err)) {
                const data = err.response?.data
                if (data?.errors) {
                    const messages = Object.values(data.errors).flat()
                    setError(messages.join('\n'))
                } else {
                    setError(data?.detail ?? 'Failed to create transaction.')
                }
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    const availableDestinationAccounts = accounts.filter((account) => account.id !== accountId)

    return (
        <div className="bg-gray-800 rounded-lg p-6 mb-6">
            <h2 className="text-lg font-bold text-gray-100 mb-4">New Transaction</h2>

            {error && (
                <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-4 whitespace-pre-line">
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
                <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
                    <div>
                        <label htmlFor="transactionType" className="block text-sm font-medium text-gray-300 mb-1">
                            Type
                        </label>
                        <select
                            id="transactionType"
                            value={transactionType}
                            onChange={(event) => setTransactionType(Number(event.target.value) as TransactionType)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        >
                            {Object.entries(TRANSACTION_TYPE_LABELS).map(([value, label]) => (
                                <option key={value} value={value}>
                                    {label}
                                </option>
                            ))}
                        </select>
                    </div>

                    <div>
                        <label htmlFor="transactionAmount" className="block text-sm font-medium text-gray-300 mb-1">
                            Amount
                        </label>
                        <input
                            id="transactionAmount"
                            type="number"
                            required
                            min="0.01"
                            step="0.01"
                            value={amount}
                            onChange={(event) => setAmount(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                       placeholder-gray-400"
                            placeholder="0.00"
                        />
                    </div>

                    <div>
                        <label htmlFor="transactionCurrency" className="block text-sm font-medium text-gray-300 mb-1">
                            Currency
                        </label>
                        <select
                            id="transactionCurrency"
                            value={currency}
                            onChange={(event) => setCurrency(Number(event.target.value) as Currency)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        >
                            {Object.entries(CURRENCY_LABELS).map(([value, label]) => (
                                <option key={value} value={value}>
                                    {label}
                                </option>
                            ))}
                        </select>
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label htmlFor="transactionDate" className="block text-sm font-medium text-gray-300 mb-1">
                            Date
                        </label>
                        <input
                            id="transactionDate"
                            type="date"
                            required
                            value={date}
                            onChange={(event) => setDate(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        />
                    </div>

                    <div>
                        <label htmlFor="transactionDescription" className="block text-sm font-medium text-gray-300 mb-1">
                            Description
                        </label>
                        <input
                            id="transactionDescription"
                            type="text"
                            required
                            value={description}
                            onChange={(event) => setDescription(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                       placeholder-gray-400"
                            placeholder="e.g. Grocery shopping"
                        />
                    </div>
                </div>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    <div>
                        <label htmlFor="transactionAccount" className="block text-sm font-medium text-gray-300 mb-1">
                            Account
                        </label>
                        <select
                            id="transactionAccount"
                            required
                            value={accountId}
                            onChange={(event) => setAccountId(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        >
                            <option value="">Select account</option>
                            {accounts.map((account) => (
                                <option key={account.id} value={account.id}>
                                    {account.name}
                                </option>
                            ))}
                        </select>
                    </div>

                    {transactionType === TransactionType.Transfer ? (
                        <div>
                            <label htmlFor="transactionToAccount" className="block text-sm font-medium text-gray-300 mb-1">
                                Destination Account
                            </label>
                            <select
                                id="transactionToAccount"
                                required
                                value={toAccountId}
                                onChange={(event) => setToAccountId(event.target.value)}
                                className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                           focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                            >
                                <option value="">Select destination</option>
                                {availableDestinationAccounts.map((account) => (
                                    <option key={account.id} value={account.id}>
                                        {account.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                    ) : (
                        <div>
                            <label htmlFor="transactionCategory" className="block text-sm font-medium text-gray-300 mb-1">
                                Category
                            </label>
                            <select
                                id="transactionCategory"
                                value={categoryId}
                                onChange={(event) => setCategoryId(event.target.value)}
                                className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                           focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                            >
                                <option value="">Select category</option>
                                {categories.map((category) => (
                                    <option key={category.id} value={category.id}>
                                        {category.name}
                                    </option>
                                ))}
                            </select>
                        </div>
                    )}
                </div>

                <div className="flex justify-end gap-3 pt-2">
                    <button
                        type="button"
                        onClick={onCancel}
                        className="px-4 py-2 text-gray-300 bg-gray-700 rounded-md hover:bg-gray-600
                                   transition-colors"
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        disabled={isLoading}
                        className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 disabled:opacity-50 disabled:cursor-not-allowed
                                   transition-colors"
                    >
                        {isLoading ? 'Creating...' : 'Create Transaction'}
                    </button>
                </div>
            </form>
        </div>
    )
}
