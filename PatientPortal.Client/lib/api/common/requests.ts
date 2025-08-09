// simple enough for now to handle responses and throw enough info into a toast

const handleResponse = async <T = unknown>(
    res: Response,
    defaultErrorMessage: string
): Promise<T> => {
    if (res.status >= 200 && res.status < 300) {
        if (res.status === 204 || res.status === 205) {
            return undefined as T;
        }

        try {
            return (await res.json()) as T;
        } catch {
            return undefined as T;
        }
    }

    let message = defaultErrorMessage;
    try {
        const data = await res.json();
        message = data?.detail || data?.title || message;
    } catch {
        // ignore parse errors
    }

    throw new Error(message);
};

export { handleResponse };
