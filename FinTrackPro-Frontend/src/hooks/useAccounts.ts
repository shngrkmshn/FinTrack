import { useCallback, useEffect, useState } from 'react'
import apiClient from '../api/axios.ts'
import axios from 'axios'
import type { AccountDto, CreateAccountRequest } from '../types/account.ts'

export function useAccounts() {
    const [accounts, setAccounts] = useState<AccountDto[]>([])
    const [isLoading, setIsLoading] = useState(true)
    const [error, setError] = useState('')

    const fetchAccounts = useCallback(async () => {
        setIsLoading(true)
        setError('')

        try {
            const response = await apiClient.get<AccountDto[]>('/accounts')
            setAccounts(response.data.filter((account) => account.isActive))
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.detail ?? 'Failed to load accounts.')
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }, [])

    useEffect(() => {
        fetchAccounts()
    }, [fetchAccounts])

    async function createAccount(request: CreateAccountRequest): Promise<void> {
        const response = await apiClient.post<AccountDto>('/accounts', request)
        setAccounts((previous) => [...previous, response.data])
    }

    async function deactivateAccount(id: string): Promise<void> {
        await apiClient.delete(`/accounts/${id}`)
        setAccounts((previous) => previous.filter((account) => account.id !== id))
    }

    return { accounts, isLoading, error, createAccount, deactivateAccount, refreshAccounts: fetchAccounts }
}
