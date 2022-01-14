import {ApiValidationError} from 'src/app/api/common/api-errors';

export interface AdminEditContainerProps<TUpsertCommand> {
    id: string | undefined;
    command: TUpsertCommand;
    apiError: ApiValidationError | null;
    onSubmit: (values: TUpsertCommand, id: string | undefined) => void;
    isSubmitting: boolean;
    onCancel: () => void;
}

export interface AdminTableContainerProps<TModel> {
    entities: TModel[];
    filter?: (value: string | undefined) => void;
    buttonsColumn: any
}
