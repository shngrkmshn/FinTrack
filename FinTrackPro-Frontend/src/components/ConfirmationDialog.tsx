import { useEffect } from 'react'

interface ConfirmationDialogProperties {
    isOpen: boolean
    title: string
    message: string
    confirmLabel?: string
    cancelLabel?: string
    onConfirm: () => void
    onCancel: () => void
    isDestructive?: boolean
}

export default function ConfirmationDialog({
    isOpen,
    title,
    message,
    confirmLabel = 'Confirm',
    cancelLabel = 'Cancel',
    onConfirm,
    onCancel,
    isDestructive = false,
}: ConfirmationDialogProperties) {
    useEffect(() => {
        function handleKeyDown(event: KeyboardEvent) {
            if (event.key === 'Escape') {
                onCancel()
            }
        }

        if (isOpen) {
            document.addEventListener('keydown', handleKeyDown)
        }

        return () => document.removeEventListener('keydown', handleKeyDown)
    }, [isOpen, onCancel])

    if (!isOpen) return null

    return (
        <div
            className="fixed inset-0 bg-black/50 flex items-center justify-center z-50 p-4"
            onClick={onCancel}
        >
            <div
                className="bg-gray-800 rounded-lg p-6 max-w-sm w-full shadow-lg shadow-black/30"
                role="dialog"
                aria-modal="true"
                onClick={(event) => event.stopPropagation()}
            >
                <h2 className="text-lg font-bold text-gray-100 mb-2">{title}</h2>
                <p className="text-gray-400 mb-6">{message}</p>
                <div className="flex justify-end gap-3">
                    <button
                        onClick={onCancel}
                        className="px-4 py-2 text-gray-300 bg-gray-700 rounded-md hover:bg-gray-600
                                   transition-colors"
                    >
                        {cancelLabel}
                    </button>
                    <button
                        onClick={onConfirm}
                        className={`px-4 py-2 text-white rounded-md transition-colors ${
                            isDestructive
                                ? 'bg-red-600 hover:bg-red-700'
                                : 'bg-emerald-600 hover:bg-emerald-700'
                        }`}
                    >
                        {confirmLabel}
                    </button>
                </div>
            </div>
        </div>
    )
}
