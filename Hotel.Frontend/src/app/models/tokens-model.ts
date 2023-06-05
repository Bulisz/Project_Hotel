export interface TokensModel {
  accessToken: {
    value: string;
    expiration: string;
  },
  refreshToken: {
    value: string;
    expiration: string;
  }
}
