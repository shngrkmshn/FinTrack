import { useState } from 'react'
import { useBudgets } from '../hooks/useBudgets.ts'
import { useCategories } from '../hooks/useCategories.ts'
import { formatCurrency } from '../types/account.ts'
import type { Currency } from '../types/account.ts'
import { BUDGET_PERIOD_LABELS } from '../types/budget.ts'
import type { BudgetPeriod } from '../types/budget.ts'
import CreateBudgetModal from '../components/CreateBudgetModal.tsx'
import { Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext.tsx'

export default function BudgetsPage() {
    const { user, logout } = useAuth()
    const { budgets, isLoading, error, createBudget } = useBudgets()
    const { categories } = useCategories()
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false)

    function getCategoryName(categoryId: string): string {
        return categories.find((category) => category.id === categoryId)?.name ?? 'Unknown'
    }

    function getProgressBarColor(percentage: number): string {
        if (percentage >= 90) return 'bg-red-500'
        if (percentage >= 75) return 'bg-yellow-500'
        return 'bg-emerald-500'
    }

    return (
        <div className="min-h-screen bg-gray-900">
            <header className="bg-gray-800 border-b border-gray-700">
                <div className="max-w-6xl mx-auto px-4 py-4 flex items-center justify-between">
                    <div className="flex items-center gap-3">
                        <Link
                            to="/"
                            className="text-gray-400 hover:text-gray-200 transition-colors"
                            title="Back to Dashboard"
                        >
                            <svg xmlns="http://www.w3.org/2000/svg" className="h-5 w-5" viewBox="0 0 20 20" fill="currentColor">
                                <path fillRule="evenodd" d="M9.707 16.707a1 1 0 01-1.414 0l-6-6a1 1 0 010-1.414l6-6a1 1 0 011.414 1.414L5.414 9H17a1 1 0 110 2H5.414l4.293 4.293a1 1 0 010 1.414z" clipRule="evenodd" />
                            </svg>
                        </Link>
                        <Link to="/" className="text-xl font-bold text-emerald-500">
                            FinTrack Pro
                        </Link>
                    </div>
                    <div className="flex items-center gap-4">
                        <span className="text-gray-400 text-sm">{user?.email}</span>
                        <button
                            onClick={logout}
                            className="px-3 py-1.5 text-sm text-gray-300 bg-gray-700 rounded-md
                                       hover:bg-gray-600 transition-colors"
                        >
                            Sign out
                        </button>
                    </div>
                </div>
            </header>

            <main className="max-w-6xl mx-auto px-4 py-8">
                <div className="flex items-center justify-between mb-6">
                    <h1 className="text-2xl font-bold text-gray-100">Budgets</h1>
                    <button
                        onClick={() => setIsCreateModalOpen(true)}
                        className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 transition-colors"
                    >
                        + New Budget
                    </button>
                </div>

                {error && (
                    <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-6">
                        {error}
                    </div>
                )}

                {isLoading ? (
                    <p className="text-gray-400">Loading budgets...</p>
                ) : budgets.length === 0 ? (
                    <div className="text-center py-16">
                        <p className="text-gray-400 mb-4">No budgets yet. Create your first budget to start tracking spending.</p>
                        <button
                            onClick={() => setIsCreateModalOpen(true)}
                            className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                       hover:bg-emerald-700 transition-colors"
                        >
                            + New Budget
                        </button>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {budgets.map((budget) => (
                            <div
                                key={budget.id}
                                className="bg-gray-800 rounded-lg p-5 shadow-lg shadow-black/20"
                            >
                                <h3 className="text-lg font-semibold text-gray-100 mb-1">{budget.name}</h3>
                                <div className="flex items-center gap-2">
                                    <span className="text-xs bg-gray-700 text-gray-300 rounded-full px-2 py-0.5">
                                        {getCategoryName(budget.categoryId)}
                                    </span>
                                    <span className="text-xs bg-emerald-900/50 text-emerald-400 rounded-full px-2 py-0.5">
                                        {BUDGET_PERIOD_LABELS[budget.period as BudgetPeriod]}
                                    </span>
                                </div>

                                <div className="mt-4 mb-2">
                                    <div className="flex justify-between text-sm mb-1">
                                        <span className="text-gray-400">
                                            {formatCurrency(budget.spentAmount, budget.currency as Currency)} spent
                                        </span>
                                        <span className="text-gray-400">
                                            {formatCurrency(budget.amount, budget.currency as Currency)}
                                        </span>
                                    </div>
                                    <div className="w-full bg-gray-700 rounded-full h-2.5">
                                        <div
                                            className={`h-2.5 rounded-full transition-all ${getProgressBarColor(budget.spentPercentage)}`}
                                            style={{ width: `${Math.min(budget.spentPercentage, 100)}%` }}
                                        />
                                    </div>
                                    <p className="text-xs text-gray-500 mt-1">
                                        {budget.spentPercentage.toFixed(1)}% used
                                    </p>
                                </div>

                                <p className="text-xs text-gray-500 mt-3">
                                    Current period: {new Date(budget.periodStartDate).toLocaleDateString()} — {new Date(budget.periodEndDate).toLocaleDateString()}
                                </p>
                            </div>
                        ))}
                    </div>
                )}
            </main>

            <CreateBudgetModal
                isOpen={isCreateModalOpen}
                onClose={() => setIsCreateModalOpen(false)}
                onCreateBudget={createBudget}
                categories={categories}
            />
        </div>
    )
}
