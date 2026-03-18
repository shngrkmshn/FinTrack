import { type FormEvent, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext.tsx'
import axios from 'axios'

export default function LoginPage() {
    const navigate = useNavigate()
    const { login } = useAuth()

    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleSubmit(event: FormEvent) {
        event.preventDefault()
        setError('')
        setIsLoading(true)

        try {
            await login(email, password)
            navigate('/')
        } catch (err) {
            if (axios.isAxiosError(err)) {
                setError(err.response?.data?.detail ?? 'Invalid email or password.')
            } else {
                setError('Something went wrong. Please try again.')
            }
        } finally {
            setIsLoading(false)
        }
    }

    return (
        <div className="flex items-center justify-center min-h-screen bg-gray-900 p-4">
            <div className="w-full max-w-md bg-gray-800 rounded-lg shadow-lg shadow-black/20 p-8">
                <h1 className="text-2xl font-bold text-center text-gray-100 mb-6">
                    Sign in to FinTrack Pro
                </h1>

                {error && (
                    <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-4">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label htmlFor="email" className="block text-sm font-medium text-gray-300 mb-1">
                            Email
                        </label>
                        <input
                            id="email"
                            type="email"
                            required
                            value={email}
                            onChange={(event) => setEmail(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                       placeholder-gray-400"
                            placeholder="you@example.com"
                        />
                    </div>

                    <div>
                        <label htmlFor="password" className="block text-sm font-medium text-gray-300 mb-1">
                            Password
                        </label>
                        <input
                            id="password"
                            type="password"
                            required
                            value={password}
                            onChange={(event) => setPassword(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                       placeholder-gray-400"
                            placeholder="••••••••"
                        />
                    </div>

                    <button
                        type="submit"
                        disabled={isLoading}
                        className="w-full py-2 px-4 bg-emerald-600 text-white font-medium rounded-md
                                   hover:bg-emerald-700 focus:outline-none focus:ring-2 focus:ring-emerald-500
                                   focus:ring-offset-2 focus:ring-offset-gray-800 disabled:opacity-50
                                   disabled:cursor-not-allowed transition-colors"
                    >
                        {isLoading ? 'Signing in...' : 'Sign in'}
                    </button>
                </form>

                <p className="mt-6 text-center text-sm text-gray-400">
                    Don't have an account?{' '}
                    <Link to="/register" className="text-emerald-500 hover:text-emerald-400 font-medium">
                        Register
                    </Link>
                </p>
            </div>
        </div>
    )
}
