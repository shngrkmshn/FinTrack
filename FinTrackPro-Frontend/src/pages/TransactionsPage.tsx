import { useEffect, useState } from 'react'
import { useTransactions, type TransactionFilters } from '../hooks/useTransactions.ts'
import { useAccounts } from '../hooks/useAccounts.ts'
import { useCategories } from '../hooks/useCategories.ts'
import { TransactionType, TRANSACTION_TYPE_LABELS } from '../types/transaction.ts'
import type { TransactionDto } from '../types/transaction.ts'
import { formatCurrency } from '../types/account.ts'
import type { Currency } from '../types/account.ts'
import CreateTransactionForm from '../components/CreateTransactionForm.tsx'
import ConfirmationDialog from '../components/ConfirmationDialog.tsx'
import { Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext.tsx'

export default function TransactionsPage() {
    const { user, logout } = useAuth()
    const { transactions, isLoading, error, fetchTransactions, createTransaction, deleteTransaction } = useTransactions()
    const { accounts } = useAccounts()
    const { categories } = useCategories()
    const [isFormOpen, setIsFormOpen] = useState(false)
    const [transactionToDelete, setTransactionToDelete] = useState<TransactionDto | null>(null)

    const [dateFrom, setDateFrom] = useState('')
    const [dateTo, setDateTo] = useState('')
    const [typeFilter, setTypeFilter] = useState<string>('')
    const [accountFilter, setAccountFilter] = useState('')
    const [currentPage, setCurrentPage] = useState(1)

    useEffect(() => {
        const filters: TransactionFilters = { page: 1, pageSize: 10 }
        fetchTransactions(filters)
    }, [fetchTransactions])

    function applyFilters(page: number = 1) {
        setCurrentPage(page)
        const filters: TransactionFilters = { page, pageSize: 10 }
        if (dateFrom) filters.dateFrom = dateFrom
        if (dateTo) filters.dateTo = dateTo
        if (typeFilter !== '') filters.type = Number(typeFilter)
        if (accountFilter) filters.accountId = accountFilter
        fetchTransactions(filters)
    }

    function handlePageChange(page: number) {
        applyFilters(page)
    }

    async function handleDelete() {
        if (!transactionToDelete) return

        try {
            await deleteTransaction(transactionToDelete.id)
        } catch {
            // Error is handled by the hook
        } finally {
            setTransactionToDelete(null)
        }
    }

    function getAccountName(accountId: string): string {
        return accounts.find((account) => account.id === accountId)?.name ?? 'Unknown'
    }

    function getTypeBadgeClasses(type: TransactionType): string {
        switch (type) {
            case TransactionType.Income:
                return 'bg-emerald-900/50 text-emerald-400'
            case TransactionType.Expense:
                return 'bg-red-900/50 text-red-400'
            case TransactionType.Transfer:
                return 'bg-blue-900/50 text-blue-400'
        }
    }

    function getAmountColor(type: TransactionType): string {
        switch (type) {
            case TransactionType.Income:
                return 'text-emerald-400'
            case TransactionType.Expense:
                return 'text-red-400'
            case TransactionType.Transfer:
                return 'text-blue-400'
        }
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
                    <h1 className="text-2xl font-bold text-gray-100">Transactions</h1>
                    <button
                        onClick={() => setIsFormOpen(!isFormOpen)}
                        className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 transition-colors"
                    >
                        {isFormOpen ? 'Cancel' : '+ New Transaction'}
                    </button>
                </div>

                {isFormOpen && (
                    <CreateTransactionForm
                        accounts={accounts}
                        categories={categories}
                        onCreateTransaction={createTransaction}
                        onCancel={() => setIsFormOpen(false)}
                    />
                )}

                <div className="bg-gray-800 rounded-lg p-4 mb-6 flex flex-wrap items-end gap-4">
                    <div>
                        <label htmlFor="filterDateFrom" className="block text-sm font-medium text-gray-300 mb-1">
                            From
                        </label>
                        <input
                            id="filterDateFrom"
                            type="date"
                            value={dateFrom}
                            onChange={(event) => setDateFrom(event.target.value)}
                            className="px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        />
                    </div>
                    <div>
                        <label htmlFor="filterDateTo" className="block text-sm font-medium text-gray-300 mb-1">
                            To
                        </label>
                        <input
                            id="filterDateTo"
                            type="date"
                            value={dateTo}
                            onChange={(event) => setDateTo(event.target.value)}
                            className="px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        />
                    </div>
                    <div>
                        <label htmlFor="filterType" className="block text-sm font-medium text-gray-300 mb-1">
                            Type
                        </label>
                        <select
                            id="filterType"
                            value={typeFilter}
                            onChange={(event) => setTypeFilter(event.target.value)}
                            className="px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        >
                            <option value="">All</option>
                            {Object.entries(TRANSACTION_TYPE_LABELS).map(([value, label]) => (
                                <option key={value} value={value}>
                                    {label}
                                </option>
                            ))}
                        </select>
                    </div>
                    <div>
                        <label htmlFor="filterAccount" className="block text-sm font-medium text-gray-300 mb-1">
                            Account
                        </label>
                        <select
                            id="filterAccount"
                            value={accountFilter}
                            onChange={(event) => setAccountFilter(event.target.value)}
                            className="px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                        >
                            <option value="">All</option>
                            {accounts.map((account) => (
                                <option key={account.id} value={account.id}>
                                    {account.name}
                                </option>
                            ))}
                        </select>
                    </div>
                    <button
                        onClick={() => applyFilters(1)}
                        className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 transition-colors"
                    >
                        Apply Filters
                    </button>
                </div>

                {error && (
                    <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-6">
                        {error}
                    </div>
                )}

                {isLoading ? (
                    <p className="text-gray-400">Loading transactions...</p>
                ) : transactions.items.length === 0 ? (
                    <div className="text-center py-16">
                        <p className="text-gray-400">No transactions found.</p>
                    </div>
                ) : (
                    <>
                        <div className="bg-gray-800 rounded-lg overflow-hidden shadow-lg shadow-black/20">
                            <table className="w-full">
                                <thead>
                                    <tr className="border-b border-gray-700">
                                        <th className="text-left px-4 py-3 text-sm font-medium text-gray-400">Date</th>
                                        <th className="text-left px-4 py-3 text-sm font-medium text-gray-400">Description</th>
                                        <th className="text-left px-4 py-3 text-sm font-medium text-gray-400">Type</th>
                                        <th className="text-right px-4 py-3 text-sm font-medium text-gray-400">Amount</th>
                                        <th className="text-left px-4 py-3 text-sm font-medium text-gray-400">Account</th>
                                        <th className="px-4 py-3"></th>
                                    </tr>
                                </thead>
                                <tbody>
                                    {transactions.items.map((transaction) => (
                                        <tr
                                            key={transaction.id}
                                            className="border-b border-gray-700/50 hover:bg-gray-700/30"
                                        >
                                            <td className="px-4 py-3 text-sm text-gray-300">
                                                {new Date(transaction.date).toLocaleDateString()}
                                            </td>
                                            <td className="px-4 py-3 text-sm text-gray-100">
                                                {transaction.description}
                                            </td>
                                            <td className="px-4 py-3">
                                                <span
                                                    className={`text-xs px-2 py-0.5 rounded-full ${getTypeBadgeClasses(transaction.transactionType)}`}
                                                >
                                                    {TRANSACTION_TYPE_LABELS[transaction.transactionType]}
                                                </span>
                                            </td>
                                            <td className={`px-4 py-3 text-sm text-right font-medium ${getAmountColor(transaction.transactionType)}`}>
                                                {formatCurrency(transaction.amount, transaction.currency as Currency)}
                                            </td>
                                            <td className="px-4 py-3 text-sm text-gray-300">
                                                {getAccountName(transaction.accountId)}
                                            </td>
                                            <td className="px-4 py-3">
                                                <button
                                                    onClick={() => setTransactionToDelete(transaction)}
                                                    className="text-gray-500 hover:text-red-400 transition-colors"
                                                    title="Delete transaction"
                                                >
                                                    <svg
                                                        xmlns="http://www.w3.org/2000/svg"
                                                        className="h-4 w-4"
                                                        viewBox="0 0 20 20"
                                                        fill="currentColor"
                                                    >
                                                        <path
                                                            fillRule="evenodd"
                                                            d="M9 2a1 1 0 00-.894.553L7.382 4H4a1 1 0 000 2v10a2 2 0 002 2h8a2 2 0 002-2V6a1 1 0 100-2h-3.382l-.724-1.447A1 1 0 0011 2H9zM7 8a1 1 0 012 0v6a1 1 0 11-2 0V8zm5-1a1 1 0 00-1 1v6a1 1 0 102 0V8a1 1 0 00-1-1z"
                                                            clipRule="evenodd"
                                                        />
                                                    </svg>
                                                </button>
                                            </td>
                                        </tr>
                                    ))}
                                </tbody>
                            </table>
                        </div>

                        <div className="flex items-center justify-between mt-4">
                            <button
                                onClick={() => handlePageChange(currentPage - 1)}
                                disabled={!transactions.hasPreviousPage}
                                className="px-4 py-2 text-gray-300 bg-gray-800 rounded-md hover:bg-gray-700
                                           disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                            >
                                Previous
                            </button>
                            <span className="text-sm text-gray-400">
                                Page {transactions.page} of {transactions.totalPages}
                            </span>
                            <button
                                onClick={() => handlePageChange(currentPage + 1)}
                                disabled={!transactions.hasNextPage}
                                className="px-4 py-2 text-gray-300 bg-gray-800 rounded-md hover:bg-gray-700
                                           disabled:opacity-50 disabled:cursor-not-allowed transition-colors"
                            >
                                Next
                            </button>
                        </div>
                    </>
                )}
            </main>

            <ConfirmationDialog
                isOpen={transactionToDelete !== null}
                title="Delete Transaction"
                message={`Are you sure you want to delete "${transactionToDelete?.description}"? This action cannot be undone.`}
                confirmLabel="Delete"
                onConfirm={handleDelete}
                onCancel={() => setTransactionToDelete(null)}
                isDestructive
            />
        </div>
    )
}
