export type LoginResponse = {
    tokenType: string,
    accessToken: string,
    expiresIn: number,
    refreshToken: string
}