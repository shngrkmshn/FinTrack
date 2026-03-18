import { Routes, Route, Navigate, Link } from 'react-router-dom'
import { useAuth } from './contexts/AuthContext.tsx'
import LoginPage from './pages/LoginPage.tsx'
import RegisterPage from './pages/RegisterPage.tsx'
import AccountsPage from './pages/AccountsPage.tsx'

function ProtectedRoute({ children }: { children: React.ReactNode }) {
    const { isAuthenticated } = useAuth()
    if (!isAuthenticated) {
        return <Navigate to="/login" replace />
    }
    return children
}

function HomePage() {
    const { user, logout } = useAuth()

    return (
        <div className="flex flex-col items-center justify-center min-h-screen bg-gray-900 p-4">
            <div className="bg-gray-800 rounded-lg shadow-lg shadow-black/20 p-8 max-w-md w-full text-center">
                <h1 className="text-2xl font-bold text-gray-100 mb-2">
                    Welcome, {user?.firstName}!
                </h1>
                <p className="text-gray-400 mb-6">{user?.email}</p>
                <Link
                    to="/accounts"
                    className="block w-full py-2 px-4 bg-emerald-600 text-white font-medium rounded-md
                               hover:bg-emerald-700 transition-colors mb-3 text-center"
                >
                    View Accounts
                </Link>
                <button
                    onClick={logout}
                    className="w-full px-4 py-2 bg-gray-700 text-gray-300 rounded-md hover:bg-gray-600
                               transition-colors"
                >
                    Sign out
                </button>
            </div>
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
        </Routes>
    )
}
