export interface ApiListResult<R> {
  filteredCount?: number;
  totalCount?: number;
  entities?: R[] | undefined;
}
