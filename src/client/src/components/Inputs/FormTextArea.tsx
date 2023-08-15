import Textarea from "@mui/joy/Textarea";
import FormControl from "@mui/joy/FormControl";
import FormLabel from "@mui/joy/FormLabel";
import FormHelperText from "@mui/joy/FormHelperText";

export interface IFormInputProps {
  label?: string | null;
  minRows: number;
  name: string;
  placeholder?: string | null;
  value: string;
  onChange?: (event: React.ChangeEvent<HTMLTextAreaElement>) => void;
  onKeyUp?: (event: React.KeyboardEvent<HTMLTextAreaElement>) => void;
  hidden?: boolean;
  disabled?: boolean;
  error?: string;
}

const FormInput = ({
  label,
  minRows,
  name,
  placeholder,
  value,
  onChange,
  onKeyUp,
  hidden,
  disabled,
  error,
}: IFormInputProps) => {
  return (
    <>
      <FormControl sx={{ mb: 2 }} error={!!error && error!.length > 0}>
        {label && <FormLabel>{label}</FormLabel>}
        <Textarea
          // html textarea attribute
          minRows={minRows}
          name={name}
          placeholder={placeholder || ""}
          value={value}
          onChange={onChange}
          onKeyUp={onKeyUp}
          sx={{
            display: hidden ? "none" : "auto",
          }}
          disabled={disabled}
          error={!!error && error!.length > 0}
        />
        <FormHelperText>{error}</FormHelperText>
      </FormControl>
    </>
  );
};

export default FormInput;
