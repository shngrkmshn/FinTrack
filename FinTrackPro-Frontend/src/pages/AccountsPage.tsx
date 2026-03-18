import { useState } from 'react'
import { useAccounts } from '../hooks/useAccounts.ts'
import { ACCOUNT_TYPE_LABELS, formatCurrency, type AccountDto } from '../types/account.ts'
import CreateAccountModal from '../components/CreateAccountModal.tsx'
import ConfirmationDialog from '../components/ConfirmationDialog.tsx'
import { Link } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext.tsx'

export default function AccountsPage() {
    const { user, logout } = useAuth()
    const { accounts, isLoading, error, createAccount, deactivateAccount } = useAccounts()
    const [isCreateModalOpen, setIsCreateModalOpen] = useState(false)
    const [accountToDelete, setAccountToDelete] = useState<AccountDto | null>(null)

    async function handleDeactivate() {
        if (!accountToDelete) return

        try {
            await deactivateAccount(accountToDelete.id)
        } catch {
            // Error is handled by the hook
        } finally {
            setAccountToDelete(null)
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
                    <h1 className="text-2xl font-bold text-gray-100">Accounts</h1>
                    <button
                        onClick={() => setIsCreateModalOpen(true)}
                        className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 transition-colors"
                    >
                        + New Account
                    </button>
                </div>

                {error && (
                    <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-6">
                        {error}
                    </div>
                )}

                {isLoading ? (
                    <p className="text-gray-400">Loading accounts...</p>
                ) : accounts.length === 0 ? (
                    <div className="text-center py-16">
                        <p className="text-gray-400 mb-4">No accounts yet. Create your first account to get started.</p>
                        <button
                            onClick={() => setIsCreateModalOpen(true)}
                            className="px-4 py-2 bg-emerald-600 text-white font-medium rounded-md
                                       hover:bg-emerald-700 transition-colors"
                        >
                            + New Account
                        </button>
                    </div>
                ) : (
                    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-4">
                        {accounts.map((account) => (
                            <div
                                key={account.id}
                                className="bg-gray-800 rounded-lg p-5 shadow-lg shadow-black/20 relative"
                            >
                                <button
                                    onClick={() => setAccountToDelete(account)}
                                    className="absolute top-4 right-4 text-gray-500 hover:text-red-400
                                               transition-colors"
                                    title="Deactivate account"
                                >
                                    <svg
                                        xmlns="http://www.w3.org/2000/svg"
                                        className="h-5 w-5"
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
                                <div className="mb-3">
                                    <h3 className="text-lg font-semibold text-gray-100">{account.name}</h3>
                                    <span className="text-xs bg-gray-700 text-gray-300 rounded-full px-2 py-0.5">
                                        {ACCOUNT_TYPE_LABELS[account.accountType]}
                                    </span>
                                </div>
                                <p className="text-2xl font-bold text-emerald-400">
                                    {formatCurrency(account.balanceAmount, account.balanceCurrency)}
                                </p>
                            </div>
                        ))}
                    </div>
                )}
            </main>

            <CreateAccountModal
                isOpen={isCreateModalOpen}
                onClose={() => setIsCreateModalOpen(false)}
                onCreateAccount={createAccount}
            />

            <ConfirmationDialog
                isOpen={accountToDelete !== null}
                title="Deactivate Account"
                message={`Are you sure you want to deactivate "${accountToDelete?.name}"? This action cannot be undone.`}
                confirmLabel="Deactivate"
                onConfirm={handleDeactivate}
                onCancel={() => setAccountToDelete(null)}
                isDestructive
            />
        </div>
    )
}
