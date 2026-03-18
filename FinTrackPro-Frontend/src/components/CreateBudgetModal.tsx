import { type FormEvent, useState } from 'react'
import axios from 'axios'
import Modal from './Modal.tsx'
import { Currency, CURRENCY_LABELS } from '../types/account.ts'
import type { CategoryDto } from '../types/category.ts'
import { BudgetPeriod, BUDGET_PERIOD_LABELS, type CreateBudgetRequest } from '../types/budget.ts'

interface CreateBudgetModalProperties {
    isOpen: boolean
    onClose: () => void
    onCreateBudget: (request: CreateBudgetRequest) => Promise<void>
    categories: CategoryDto[]
}

export default function CreateBudgetModal({
    isOpen,
    onClose,
    onCreateBudget,
    categories,
}: CreateBudgetModalProperties) {
    const [name, setName] = useState('')
    const [categoryId, setCategoryId] = useState('')
    const [amount, setAmount] = useState('')
    const [currency, setCurrency] = useState<Currency>(Currency.USD)
    const [period, setPeriod] = useState<BudgetPeriod>(BudgetPeriod.Monthly)
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    function resetForm() {
        setName('')
        setCategoryId('')
        setAmount('')
        setCurrency(Currency.USD)
        setPeriod(BudgetPeriod.Monthly)
        setError('')
    }

    function handleClose() {
        resetForm()
        onClose()
    }

    async function handleSubmit(event: FormEvent) {
        event.preventDefault()
        setError('')
        setIsLoading(true)

        try {
            await onCreateBudget({
                name,
                categoryId,
                amount: parseFloat(amount),
                currency,
                period,
            })
            resetForm()
            onClose()
        } catch (err) {
            if (axios.isAxiosError(err)) {
                const data = err.response?.data
                if (data?.errors) {
                    const messages = Object.values(data.errors).flat()
                    setError(messages.join('\n'))
                } else {
                    setError(data?.detail ?? 'Failed to create budget.')
                }
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <Modal isOpen={isOpen} onClose={handleClose} title="New Budget">
            {error && (
                <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-4 whitespace-pre-line">
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label htmlFor="budgetName" className="block text-sm font-medium text-gray-300 mb-1">
                        Name
                    </label>
                    <input
                        id="budgetName"
                        type="text"
                        required
                        maxLength={100}
                        value={name}
                        onChange={(event) => setName(event.target.value)}
                        className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                   focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                   placeholder-gray-400"
                        placeholder="e.g. Monthly Groceries"
                    />
                </div>

                <div>
                    <label htmlFor="budgetCategory" className="block text-sm font-medium text-gray-300 mb-1">
                        Category
                    </label>
                    <select
                        id="budgetCategory"
                        required
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

                <div className="grid grid-cols-2 gap-4">
                    <div>
                        <label htmlFor="budgetAmount" className="block text-sm font-medium text-gray-300 mb-1">
                            Amount
                        </label>
                        <input
                            id="budgetAmount"
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
                        <label htmlFor="budgetCurrency" className="block text-sm font-medium text-gray-300 mb-1">
                            Currency
                        </label>
                        <select
                            id="budgetCurrency"
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

                <div>
                    <label htmlFor="budgetPeriod" className="block text-sm font-medium text-gray-300 mb-1">
                        Recurrence
                    </label>
                    <select
                        id="budgetPeriod"
                        value={period}
                        onChange={(event) => setPeriod(Number(event.target.value) as BudgetPeriod)}
                        className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                   focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                    >
                        {Object.entries(BUDGET_PERIOD_LABELS).map(([value, label]) => (
                            <option key={value} value={value}>
                                {label}
                            </option>
                        ))}
                    </select>
                </div>

                <div className="flex justify-end gap-3 pt-2">
                    <button
                        type="button"
                        onClick={handleClose}
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
                        {isLoading ? 'Creating...' : 'Create Budget'}
                    </button>
                </div>
            </form>
        </Modal>
    )
}
