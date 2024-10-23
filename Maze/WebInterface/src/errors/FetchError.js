
class FetchError extends Error {
  constructor(message) {
    super(message);
    this.name = 'FetchError';
  }
}
export default FetchError;
