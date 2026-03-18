import { type FormEvent, useState } from 'react'
import { Link, useNavigate } from 'react-router-dom'
import { useAuth } from '../contexts/AuthContext.tsx'
import axios from 'axios'

export default function RegisterPage() {
    const navigate = useNavigate()
    const { register } = useAuth()

    const [email, setEmail] = useState('')
    const [password, setPassword] = useState('')
    const [firstName, setFirstName] = useState('')
    const [lastName, setLastName] = useState('')
    const [error, setError] = useState('')
    const [isLoading, setIsLoading] = useState(false)

    async function handleSubmit(event: FormEvent) {
        event.preventDefault()
        setError('')
        setIsLoading(true)

        try {
            await register(email, password, firstName, lastName)
            navigate('/')
        } catch (err) {
            if (axios.isAxiosError(err)) {
                // FluentValidation errors come as "errors" object, general errors as "detail"
                const data = err.response?.data
                if (data?.errors) {
                    const messages = Object.values(data.errors).flat()
                    setError(messages.join('\n'))
                } else {
                    setError(data?.detail ?? 'Registration failed. Please try again.')
                }
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
                    Create your account
                </h1>

                {error && (
                    <div className="bg-red-900/30 text-red-400 text-sm rounded-md p-3 mb-4 whitespace-pre-line">
                        {error}
                    </div>
                )}

                <form onSubmit={handleSubmit} className="space-y-4">
                    <div className="flex gap-4">
                        <div className="flex-1">
                            <label htmlFor="firstName" className="block text-sm font-medium text-gray-300 mb-1">
                                First name
                            </label>
                            <input
                                id="firstName"
                                type="text"
                                required
                                value={firstName}
                                onChange={(event) => setFirstName(event.target.value)}
                                className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                           focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                           placeholder-gray-400"
                            />
                        </div>
                        <div className="flex-1">
                            <label htmlFor="lastName" className="block text-sm font-medium text-gray-300 mb-1">
                                Last name
                            </label>
                            <input
                                id="lastName"
                                type="text"
                                required
                                value={lastName}
                                onChange={(event) => setLastName(event.target.value)}
                                className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                           focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                           placeholder-gray-400"
                            />
                        </div>
                    </div>

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
                            minLength={8}
                            value={password}
                            onChange={(event) => setPassword(event.target.value)}
                            className="w-full px-3 py-2 bg-gray-700 text-gray-100 border border-gray-600 rounded-md
                                       focus:outline-none focus:ring-2 focus:ring-emerald-500 focus:border-transparent
                                       placeholder-gray-400"
                            placeholder="Minimum 8 characters"
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
                        {isLoading ? 'Creating account...' : 'Create account'}
                    </button>
                </form>

                <p className="mt-6 text-center text-sm text-gray-400">
                    Already have an account?{' '}
                    <Link to="/login" className="text-emerald-500 hover:text-emerald-400 font-medium">
                        Sign in
                    </Link>
                </p>
            </div>
        </div>
    )
}
