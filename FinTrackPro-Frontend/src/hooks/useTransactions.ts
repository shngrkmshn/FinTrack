import { useCallback, useState } from 'react'
import apiClient from '../api/axios.ts'
import axios from 'axios'
import type { CreateTransactionRequest, PaginatedList, TransactionDto } from '../types/transaction.ts'

export interface TransactionFilters {
    page: number
    pageSize: number
    dateFrom?: string
    dateTo?: string
    type?: number
    categoryId?: string
    accountId?: string
}

export function useTransactions() {
    const [transactions, setTransactions] = useState<PaginatedList<TransactionDto>>({
        items: [],
        page: 1,
        pageSize: 10,
        totalCount: 0,
        totalPages: 0,
        hasPreviousPage: false,
        hasNextPage: false,
    })
    const [isLoading, setIsLoading] = useState(false)
    const [error, setError] = useState('')
    const [currentFilters, setCurrentFilters] = useState<TransactionFilters>({ page: 1, pageSize: 10 })

    const fetchTransactions = useCallback(async (filters: TransactionFilters) => {
        setIsLoading(true)
        setError('')
        setCurrentFilters(filters)

        try {
            const parameters = new URLSearchParams()
            parameters.append('page', filters.page.toString())
            parameters.append('pageSize', filters.pageSize.toString())
            if (filters.dateFrom) parameters.append('dateFrom', filters.dateFrom)
            if (filters.dateTo) parameters.append('dateTo', filters.dateTo)
            if (filters.type !== undefined) parameters.append('type', filters.type.toString())
            if (filters.categoryId) parameters.append('categoryId', filters.categoryId)
            if (filters.accountId) parameters.append('accountId', filters.accountId)

            const response = await apiClient.get<PaginatedList<TransactionDto>>(
                `/transactions?${parameters.toString()}`
            )
            setTransactions(response.data)
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.detail ?? 'Failed to load transactions.')
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }, [])

    async function createTransaction(request: CreateTransactionRequest): Promise<void> {
        await apiClient.post('/transactions', request)
        await fetchTransactions(currentFilters)
    }

    async function deleteTransaction(id: string): Promise<void> {
        await apiClient.delete(`/transactions/${id}`)
        await fetchTransactions(currentFilters)
    }

    return { transactions, isLoading, error, fetchTransactions, createTransaction, deleteTransaction }
}
