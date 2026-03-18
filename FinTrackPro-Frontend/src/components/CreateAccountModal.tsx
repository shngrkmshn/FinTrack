import { type FormEvent, useState } from 'react'
import axios from 'axios'
import Modal from './Modal.tsx'
import {
    AccountType,
    ACCOUNT_TYPE_LABELS,
    Currency,
    CURRENCY_LABELS,
    type CreateAccountRequest,
} from '../types/account.ts'

interface CreateAccountModalProperties {
    isOpen: boolean
    onClose: () => void
    onCreateAccount: (request: CreateAccountRequest) => Promise<void>
}

export default function CreateAccountModal({
    isOpen,
    onClose,
    onCreateAccount,
}: CreateAccountModalProperties) {
    const [name, setName] = useState('')
    const [accountType, setAccountType] = useState<AccountType>(AccountType.Checking)
    const [currency, setCurrency] = useState<Currency>(Currency.USD)
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    function resetForm() {
        setName('')
        setAccountType(AccountType.Checking)
        setCurrency(Currency.USD)
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
            await onCreateAccount({ name, accountType, currency })
            resetForm()
            onClose()
        } catch (err) {
            if (axios.isAxiosError(err)) {
                const data = err.response?.data
                if (data?.errors) {
                    const messages = Object.values(data.errors).flat()
                    setError(messages.join('\n'))
                } else {
                    setError(data?.detail ?? 'Failed to create account.')
                }
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <Modal isOpen={isOpen} onClose={handleClose} title="New Account">
            {error && (
                <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-4 whitespace-pre-line">
                    {error}
                </div>
            )}

            <form onSubmit={handleSubmit} className="space-y-4">
                <div>
                    <label htmlFor="accountName" className="block text-sm font-medium text-gray-300 mb-1">
                        Name
                    </label>
                    <input
                        id="accountName"
                        type="text"
                        required
                        maxLength={100}
                        value={name}
                        onChange={(event) => setName(event.target.value)}
                        className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                   focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                   placeholder-gray-400"
                        placeholder="e.g. Main Checking"
                    />
                </div>

                <div>
                    <label htmlFor="accountType" className="block text-sm font-medium text-gray-300 mb-1">
                        Account Type
                    </label>
                    <select
                        id="accountType"
                        value={accountType}
                        onChange={(event) => setAccountType(Number(event.target.value) as AccountType)}
                        className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                   focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent"
                    >
                        {Object.entries(ACCOUNT_TYPE_LABELS).map(([value, label]) => (
                            <option key={value} value={value}>
                                {label}
                            </option>
                        ))}
                    </select>
                </div>

                <div>
                    <label htmlFor="currency" className="block text-sm font-medium text-gray-300 mb-1">
                        Currency
                    </label>
                    <select
                        id="currency"
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
                        {isLoading ? 'Creating...' : 'Create Account'}
                    </button>
                </div>
            </form>
        </Modal>
    )
}
