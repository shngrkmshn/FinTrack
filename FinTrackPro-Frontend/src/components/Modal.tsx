import { type ReactNode, useEffect } from 'react'

interface ModalProperties {
    isOpen: boolean
    onClose: () => void
    title: string
    children: ReactNode
}

export default function Modal({ isOpen, onClose, title, children }: ModalProperties) {
    useEffect(() => {
        function handleKeyDown(event: KeyboardEvent) {
            if (event.key === 'Escape') {
                onClose()
            }
        }

        if (isOpen) {
            document.addEventListener('keydown', handleKeyDown)
        }

        return () => document.removeEventListener('keydown', handleKeyDown)
    }, [isOpen, onClose])

    if (!isOpen) return null

    return (
        <div
            className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4"
            onClick={onClose}
        >
            <div
                className="bg-gray-800 rounded-lg p-6 max-w-md w-full shadow-lg shadow-black/30"
                role="dialog"
                aria-modal="true"
                onClick={(event) => event.stopPropagation()}
            >
                <h2 className="text-xl font-bold text-gray-100 mb-4">{title}</h2>
                {children}
            </div>
        </div>
    )
}
