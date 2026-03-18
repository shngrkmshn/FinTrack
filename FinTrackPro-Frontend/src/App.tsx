import { Routes, Route, Navigate, Link, useNavigate } from 'react-router-dom'
import { useAuth } from './contexts/AuthContext.tsx'
import LoginPage from './pages/LoginPage.tsx'
import RegisterPage from './pages/RegisterPage.tsx'
import AccountsPage from './pages/AccountsPage.tsx'
import CategoriesPage from './pages/CategoriesPage.tsx'
import TransactionsPage from './pages/TransactionsPage.tsx'
import BudgetsPage from './pages/BudgetsPage.tsx'

function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated } = useAuth()
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }
    return children
}

const dashboardCards = [
    {
        title: 'Accounts',
        description: 'Manage your bank accounts, credit cards, and wallets.',
        path: '/accounts',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" viewBox="0 0 20 20" fill="currentColor">
                <path d="M4 4a2 2 0 00-2 2v1h16V6a2 2 0 00-2-2H4z" />
                <path fillRule="evenodd" d="M18 9H2v5a2 2 0 002 2h12a2 2 0 002-2V9zM4 13a1 1 0 011-1h1a1 1 0 110 2H5a1 1 0 01-1-1zm5-1a1 1 0 100 2h1a1 1 0 100-2H9z" clipRule="evenodd" />
            </svg>
        ),
    },
    {
        title: 'Categories',
        description: 'Organize your transactions with custom categories.',
        path: '/categories',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" viewBox="0 0 20 20" fill="currentColor">
                <path d="M7 3a1 1 0 000 2h6a1 1 0 100-2H7zM4 7a1 1 0 011-1h10a1 1 0 110 2H5a1 1 0 01-1-1zM2 11a2 2 0 012-2h12a2 2 0 012 2v4a2 2 0 01-2 2H4a2 2 0 01-2-2v-4z" />
            </svg>
        ),
    },
    {
        title: 'Transactions',
        description: 'Track income, expenses, and transfers across accounts.',
        path: '/transactions',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M4 2a1 1 0 011 1v2.101a7.002 7.002 0 0111.601 2.566 1 1 0 11-1.885.666A5.002 5.002 0 005.999 7H9a1 1 0 010 2H4a1 1 0 01-1-1V3a1 1 0 011-1zm.008 9.057a1 1 0 011.276.61A5.002 5.002 0 0014.001 13H11a1 1 0 110-2h5a1 1 0 011 1v5a1 1 0 11-2 0v-2.101a7.002 7.002 0 01-11.601-2.566 1 1 0 01.61-1.276z" clipRule="evenodd" />
            </svg>
        ),
    },
    {
        title: 'Budgets',
        description: 'Set spending limits and monitor your progress.',
        path: '/budgets',
        icon: (
            <svg xmlns="http://www.w3.org/2000/svg" className="h-8 w-8" viewBox="0 0 20 20" fill="currentColor">
                <path fillRule="evenodd" d="M3 3a1 1 0 000 2v8a2 2 0 002 2h2.586l-1.293 1.293a1 1 0 101.414 1.414L10 15.414l2.293 2.293a1 1 0 001.414-1.414L12.414 15H15a2 2 0 002-2V5a1 1 0 100-2H3zm11.707 4.707a1 1 0 00-1.414-1.414L10 9.586 8.707 8.293a1 1 0 00-1.414 0l-2 2a1 1 0 101.414 1.414L8 10.414l1.293 1.293a1 1 0 001.414 0l4-4z" clipRule="evenodd" />
            </svg>
        ),
    },
]

function HomePage() {
    const { user, logout } = useAuth()
    const navigate = useNavigate()

    return (
        <div className="min-h-screen bg-gray-900">
            <header className="bg-gray-800 border-b border-gray-700">
                <div className="max-w-6xl mx-auto px-4 py-4 flex items-center justify-between">
                    <Link to="/" className="text-xl font-bold text-emerald-500">
                        FinTrack Pro
                    </Link>
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
                <h1 className="text-2xl font-bold text-gray-100 mb-2">
                    Welcome back, {user?.firstName}!
                </h1>
                <p className="text-gray-400 mb-8">What would you like to manage today?</p>

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                    {dashboardCards.map((card) => (
                        <button
                            key={card.path}
                            onClick={() => navigate(card.path)}
                            className="bg-gray-800 rounded-lg p-6 shadow-lg shadow-black/20 text-left
                                       hover:bg-gray-750 hover:ring-1 hover:ring-emerald-500/50
                                       transition-all group"
                        >
                            <div className="text-emerald-500 mb-3 group-hover:text-emerald-400 transition-colors">
                                {card.icon}
                            </div>
                            <h2 className="text-lg font-semibold text-gray-100 mb-1">{card.title}</h2>
                            <p className="text-sm text-gray-400">{card.description}</p>
                        </button>
                    ))}
                </div>
            </main>
        </div>
    )
}

export default function App() {
    return (
        <Routes>
            <Route path="/login" element={<LoginPage />} />
            <Route path="/register" element={<RegisterPage />} />
            <Route
                path="/"
                element={
                    <ProtectedRoute>
                        <HomePage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/accounts"
                element={
                    <ProtectedRoute>
                        <AccountsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/categories"
                element={
                    <ProtectedRoute>
                        <CategoriesPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/transactions"
                element={
                    <ProtectedRoute>
                        <TransactionsPage />
                    </ProtectedRoute>
                }
            />
            <Route
                path="/budgets"
                element={
                    <ProtectedRoute>
                        <BudgetsPage />
                    </ProtectedRoute>
                }
            />
        </Routes>
    )
}
